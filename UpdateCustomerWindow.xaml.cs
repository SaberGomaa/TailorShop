using System.Windows;

namespace TailorShop
{
    public partial class UpdateCustomerWindow : Window
    {
        public Customer Customer { get; private set; }

        public UpdateCustomerWindow(Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            txtName.Text = Customer.Name;
            txtLength.Text = Customer.Length.ToString();
            txtSleeve.Text = Customer.Sleeve.ToString();
            txtShoulder.Text = Customer.Shoulder.ToString();
            txtWidth.Text = Customer.Width.ToString();
            txtFitness.Text = Customer.Fitness.ToString();
            txtCollar.Text = Customer.Collar.ToString();
            txtEmbroidery.Text = Customer.Embroidery;
            txtPhone.Text = Customer.Phone;
            txtNotes.Text = Customer.Notes;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
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

            Customer.Name = txtName.Text;
            Customer.Length = length;
            Customer.Sleeve = sleeve;
            Customer.Shoulder = shoulder;
            Customer.Width = width;
            Customer.Fitness = fitness;
            Customer.Collar = collar;
            Customer.Embroidery = txtEmbroidery.Text;
            Customer.Phone = txtPhone.Text;
            Customer.Notes = txtNotes.Text;

            DialogResult = true;
            Close();
        }
    }
}