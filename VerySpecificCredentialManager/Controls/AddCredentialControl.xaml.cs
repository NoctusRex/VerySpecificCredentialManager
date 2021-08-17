using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Unity;
using VerySpecificCredentialManager.Models;
using VerySpecificCredentialManager.Services;

namespace VerySpecificCredentialManager.Controls
{
    /// <summary>
    /// Interaction logic for AddCredentialControl.xaml
    /// </summary>
    public partial class AddCredentialControl : UserControl
    {
        private Action<Credential> AddCredential { get; set; }

        public AddCredentialControl(Action<Credential> addCredential)
        {
            InitializeComponent();

            AddCredential = addCredential;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FolderValueTextBox.Text)) { return; }
            if (string.IsNullOrEmpty(PasswordValueTextBox.Text)) { return; }
            if (string.IsNullOrEmpty(UserNameValueTextBox.Text)) { return; }

            AddCredential?.Invoke(new Credential() { ProcessName = FolderValueTextBox.Text, Password = PasswordValueTextBox.Text, UserName = UserNameValueTextBox.Text });

            UserNameValueTextBox.Text = "";
            PasswordValueTextBox.Text = "";
            FolderValueTextBox.Text = "";
        }

    }
}
