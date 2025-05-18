using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace TailorShop
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Customer> _customers = new ObservableCollection<Customer>();

        public MainWindow()
        {
            InitializeComponent();
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
                ID = _customers.Count + 1,
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
                dgCustomers.ItemsSource = _customers;
            }
            else
            {
                dgCustomers.ItemsSource = new ObservableCollection<Customer>(
                    _customers.Where(c => c.Name.ToLower().Contains(searchText)));
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