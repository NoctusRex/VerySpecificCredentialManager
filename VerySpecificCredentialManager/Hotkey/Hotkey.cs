﻿using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace VerySpecificCredentialManager.Hotkey
{
    /// <summary>
    /// https://www.dreamincode.net/forums/topic/180436-global-hotkeys/
    /// https://stackoverflow.com/questions/11377977/global-hotkeys-in-wpf-working-from-every-window
    /// </summary>
    public class Hotkey : IDisposable
    {
        public event EventHandler Triggered;

        //windows message id for hotkey
        private static int WM_HOTKEY_MSG_ID => 0x0312;
        private int Modifier { get; set; }
        private int Key { get; set; }
        private IntPtr Hwnd { get; set; }
        private int Id { get; set; }
        private HwndSource Source { get; set; }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public Hotkey(int modifier, Keys key, Window window)
        {
            Modifier = modifier;
            Key = (int)key;
            Hwnd = new WindowInteropHelper(window).Handle;
            Source = HwndSource.FromHwnd(Hwnd);
            Source.AddHook(HwndHook);
            Id = GetHashCode();
        }

        public bool Register()
        {
            return RegisterHotKey(Hwnd, Id, Modifier, Key);
        }

        public bool Unregister()
        {
            Source.RemoveHook(HwndHook);
            Source = null;
            return UnregisterHotKey(Hwnd, Id);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg != WM_HOTKEY_MSG_ID) return IntPtr.Zero;
            if (wParam.ToInt32() != Id) return IntPtr.Zero;

            Triggered(this, new EventArgs());
            handled = true;

            return IntPtr.Zero;
        }

        public override int GetHashCode() => Modifier ^ Key ^ Hwnd.ToInt32();

        public void Dispose()
        {
            Unregister();
        }
    }
}
