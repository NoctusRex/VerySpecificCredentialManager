using System;
using System.Diagnostics;
using System.IO;

namespace VerySpecificCredentialManager.Services
{
    public class StartUpService
    {
        private string StartupPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "VerySpecificCredentialManager.lnk");

        public bool StartUpEnabled => File.Exists(StartupPath);

        public void CreateStartupShortcut()
        {
            if (StartUpEnabled) { return; }

            dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell"));
            dynamic link = shell.CreateShortcut(StartupPath);

            link.TargetPath = Process.GetCurrentProcess().MainModule.FileName;
            link.WindowStyle = 1;
            link.Save();
        }


        public void RemoveStartupShortcut()
        {
            if (!StartUpEnabled) { return; }

            File.Delete(StartupPath);
        }
    }
}
