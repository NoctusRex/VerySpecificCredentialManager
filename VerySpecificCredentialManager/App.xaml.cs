using System;
using System.Data;
using System.Threading;
using System.Transactions;
using System.Windows;
using System.Windows.Threading;
using Unity;
using VerySpecificCredentialManager.Controls;
using VerySpecificCredentialManager.Services;

namespace VerySpecificCredentialManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex Mutex { get; set; }
        private bool HandleExceptions { get; set; }

        private UnityContainer UnityContainer { get; set; }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = HandleExceptions;

            MessageBox.Show(e.Exception.ToString());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                CheckAlreadyRunning();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Current.Shutdown();
            }
            finally
            {
                HandleExceptions = true;
            }

            UnityContainer = new UnityContainer();
            _ = UnityContainer.RegisterSingleton<MainWindow>()
                .RegisterType<EncryptionService>()
                .RegisterType<StartUpService>()
                .RegisterType<StorageService>()
                .RegisterType<AddCredentialControl>()
                .RegisterSingleton<HotkeyService>();

            UnityContainer.Resolve<MainWindow>().Show();
        }

        private void CheckAlreadyRunning()
        {
            Mutex = new Mutex(false, "VerySpecificCredentialManager - 133742069");

            if (Mutex.WaitOne(0, false)) { return; }

            throw new DuplicateNameException("The application is already running.");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Mutex?.Close();
            Mutex?.Dispose();
            Mutex = null;

            base.OnExit(e);
        }
    }
}
