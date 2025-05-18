using System.Collections.ObjectModel;
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