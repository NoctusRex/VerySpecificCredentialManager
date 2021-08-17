using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace VerySpecificCredentialManager.Services
{
    public class HotkeyService : IDisposable
    {
        public static int NOMOD => 0x0000;
        public static int ALT => 0x0001;
        public static int CTRL => 0x0002;
        public static int SHIFT => 0x0004;
        public static int WIN => 0x0008;

        private List<Hotkey.Hotkey> Hotkeys { get; set; } = new();

        public void AddHotkey(Keys key, Action triggered)
        {
            Hotkey.Hotkey hotkey = new(NOMOD, key, Application.Current.MainWindow);
            hotkey.Triggered += (sender, e) => { triggered?.Invoke(); };
            hotkey.Register();

            Hotkeys.Add(hotkey);
        }

        public void Dispose()
        {
            foreach (Hotkey.Hotkey hotkey in Hotkeys)
            {
                hotkey.Dispose();
            }
        }
    }
}
