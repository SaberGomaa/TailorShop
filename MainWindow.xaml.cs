using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TailorShop
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseHelper _dbHelper;
        private ObservableCollection<Customer> _customers;

        public MainWindow()
        {
            InitializeComponent();
            _dbHelper = new DatabaseHelper();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            _customers = new ObservableCollection<Customer>(_dbHelper.GetAllCustomers());
            dgCustomers.ItemsSource = _customers;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("يرجى إدخال الاسم.", "خطأ في الإدخال");
                return;
            }

            if (!double.TryParse(txtLength.Text, out double length) ||
                !double.TryParse(txtSleeve.Text, out double sleeve) ||
                !double.TryParse(txtShoulder.Text, out double shoulder) ||
                !double.TryParse(txtWidth.Text, out double width) ||
                !double.TryParse(txtFitness.Text, out double fitness) ||
                !double.TryParse(txtCollar.Text, out double collar))
            {
                MessageBox.Show("يرجى إدخال قيم صحيحة للقياسات.", "خطأ في الإدخال");
                return;
            }

            var customer = new Customer
            {
                Name = txtName.Text,
                Length = length,
                Sleeve = sleeve,
                Shoulder = shoulder,
                Width = width,
                Fitness = fitness,
                Collar = collar,
                Embroidery = txtEmbroidery.Text,
                Phone = txtPhone.Text,
                Notes = txtNotes.Text
            };

            _dbHelper.InsertCustomer(customer);
            _customers.Add(customer);
            ClearInputs();
        }
        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var restoreWindow = new RestoreWindow();
                if (restoreWindow.ShowDialog() == true && !string.IsNullOrEmpty(restoreWindow.SelectedBackupPath))
                {
                    string sourcePath = restoreWindow.SelectedBackupPath;
                    string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TailorShop.db");

                    // Replace the current database with the selected backup
                    File.Copy(sourcePath, destinationPath, true);

                    // Reload the data from the restored database
                    LoadCustomers();

                    MessageBox.Show("تم استعادة قاعدة البيانات بنجاح.", "نجاح");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل استعادة قاعدة البيانات: {ex.Message}", "خطأ");
            }
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create Backups folder if it doesn't exist
                string backupFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "D:\\work\\Saber\\Backups");
                Directory.CreateDirectory(backupFolder);

                // Generate backup filename with current date and time
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TailorShop.db");
                string backupPath = Path.Combine(backupFolder, $"TailorShop_Backup_{timestamp}.db");

                // Copy the database file to the backup location
                File.Copy(sourcePath, backupPath, true);
                MessageBox.Show("تم إنشاء نسخة احتياطية بنجاح.", "نجاح");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"فشل إنشاء النسخة الاحتياطية: {ex.Message}", "خطأ");
            }
        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                var customer = _customers.FirstOrDefault(c => c.ID == id);
                if (customer != null)
                {
                    _dbHelper.DeleteCustomer(id);
                    _customers.Remove(customer);
                }
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int id)
            {
                var customer = _customers.FirstOrDefault(c => c.ID == id);
                if (customer != null)
                {
                    var updateWindow = new UpdateCustomerWindow(customer);
                    if (updateWindow.ShowDialog() == true)
                    {
                        _dbHelper.UpdateCustomer(customer);
                        dgCustomers.Items.Refresh();
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchText = txtSearch.Text.ToLower();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadCustomers();
            }
            else
            {
                _customers = new ObservableCollection<Customer>(_dbHelper.SearchCustomers(searchText));
                dgCustomers.ItemsSource = _customers;
            }
        }

        private void ClearInputs()
        {
            txtName.Text = "";
            txtLength.Text = "";
            txtSleeve.Text = "";
            txtShoulder.Text = "";
            txtWidth.Text = "";
            txtFitness.Text = "";
            txtCollar.Text = "";
            txtEmbroidery.Text = "";
            txtPhone.Text = "";
            txtNotes.Text = "";
        }
    }
}