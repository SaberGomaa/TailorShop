using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TailorShop
{
    public partial class RestoreWindow : Window
    {
        public string SelectedBackupPath { get; private set; }

        public RestoreWindow()
        {
            InitializeComponent();
            LoadBackups();
        }

        private void LoadBackups()
        {
            string backupFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            if (Directory.Exists(backupFolder))
            {
                var backupFiles = Directory.GetFiles(backupFolder, "TailorShop_Backup_*.db");
                var backupFileInfos = new List<FileInfo>();
                foreach (var file in backupFiles)
                {
                    backupFileInfos.Add(new FileInfo(file));
                }
                lstBackups.ItemsSource = backupFileInfos;
            }
        }

        private void btnRestore_Click(object sender, RoutedEventArgs e)
        {
            if (lstBackups.SelectedItem != null)
            {
                SelectedBackupPath = lstBackups.SelectedValue?.ToString();
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("يرجى تحديد نسخة احتياطية للاستعادة.", "خطأ");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}