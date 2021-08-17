using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VerySpecificCredentialManager.Models;
using System;

namespace VerySpecificCredentialManager.Controls
{
    /// <summary>
    /// Interaction logic for CredentialControl.xaml
    /// </summary>
    public partial class CredentialControl : UserControl
    {
        public Credential Credential { get; set; }
        private Action Delete { get; set; }

        public CredentialControl(Credential credential, Action delete)
        {
            InitializeComponent();
            Credential = credential;
            Delete = delete;

            UserNameValueTextBlock.Text = Credential.UserName;
            FolderValueTextBlock.Text = Credential.ProcessName;
            PasswordValueTextBlock.Text = Credential.Password;
        }

        private void PasswordValueTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText(PasswordValueTextBlock.Text);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Delete?.Invoke();
        }
    }
}
