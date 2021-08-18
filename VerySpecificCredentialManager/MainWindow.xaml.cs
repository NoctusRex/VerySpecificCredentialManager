using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using Unity;
using VerySpecificCredentialManager.Controls;
using VerySpecificCredentialManager.Models;
using VerySpecificCredentialManager.Services;

namespace VerySpecificCredentialManager
{
    public partial class MainWindow : Window
    {
        [Dependency]
        public StorageService StorageService { get; set; }
        [Dependency]
        public StartUpService StartUpService { get; set; }
        [Dependency]
        public HotkeyService HotkeyService { get; set; }
        [Dependency]
        public UnityContainer UnityContainer { get; set; }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        public MainWindow()
        {
            InitializeComponent();
            // TODO: Minimize
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCredentials();
            HotkeyService.AddHotkey(Keys.End, () =>
            {
                List<Credential> credentials = StorageService.Read<List<Credential>>("Credentials");
                string windowTitle = GetCurrentWindowTitle(GetForegroundWindow());

                Credential credential = credentials.FirstOrDefault(x => new Regex(x.ProcessName).IsMatch(windowTitle));

                if (credential is not null)
                {
                    System.Windows.Clipboard.SetText(credential.Password);
                    return;
                }
            });

            startUpCheckBox.IsChecked = StartUpService.StartUpEnabled;
        }

        public void LoadCredentials()
        {
            CredentialStackPanel.Children.Clear();
            CredentialStackPanel.Children.Add(new AddCredentialControl((credential) =>
            {
                List<Credential> credentials = StorageService.Read<List<Credential>>("Credentials");
                if (credentials.Any(x => x.Equals(credential))) { return; }

                credentials.Add(credential);
                StorageService.Write("Credentials", credentials);

                LoadCredentials();
            }));

            List<Credential> credentials = StorageService.Read<List<Credential>>("Credentials");
            if (credentials is null) return;

            foreach (Credential credential in credentials)
            {
                CredentialStackPanel.Children.Add(new CredentialControl(credential, () =>
                {

                    Dispatcher.Invoke(() =>
                    {
                        List<Credential> credentials = StorageService.Read<List<Credential>>("Credentials").Where(x => !x.Equals(credential)).ToList();
                        StorageService.Write("Credentials", credentials);
                        LoadCredentials();
                    });

                }));
            }
        }

        private static string GetCurrentWindowTitle(IntPtr handle)
        {
            const int nChars = 256;
            StringBuilder Buff = new(nChars);

            return GetWindowText(handle, Buff, nChars) > 0 ? Buff.ToString() : null;
        }

        private void startUpCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (startUpCheckBox.IsChecked.Value)
            {
                StartUpService.CreateStartupShortcut();
            }
            else
            {
                StartUpService.RemoveStartupShortcut();
            }
        }
    }
}
