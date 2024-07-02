using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace RevitAddin.WindowDarkMode.Example.Views
{
    public static class DesktopWindowManager
    {
        /// <summary>
        /// Check whether Windows build is 19041 or higher, that supports <see cref="SetImmersiveDarkMode(Window, bool)"/>.
        /// </summary>
        public static bool IsImmersiveDarkModeSupported { get; } =
            Environment.OSVersion.Version.Build >= 19041;

        /// <summary>
        /// Set the immersive dark mode for the window.
        /// </summary>
        public static bool SetImmersiveDarkMode(Window window, bool enable)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            return SetImmersiveDarkMode(hwnd, enable);
        }
        /// <summary>
        /// Set the immersive dark mode for the window.
        /// </summary>
        public static bool SetImmersiveDarkMode(IntPtr hWnd, bool enable)
        {
            if (IsImmersiveDarkModeSupported == false) 
                return false;

            int darkMode = enable ? 1 : 0;
            var result = DwmSetWindowAttribute(hWnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));
            return result > 0;
        }

        #region Dwm Window
        /// <summary>
        /// https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/nf-dwmapi-dwmsetwindowattribute
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwAttribute"></param>
        /// <param name="pvAttribute"></param>
        /// <param name="cbAttribute"></param>
        /// <returns></returns>
        [DllImport("dwmapi.dll", SetLastError = false, ExactSpelling = true)]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, uint dwAttribute, ref int pvAttribute, int cbAttribute);
        /// <summary>
        /// https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute
        /// </summary>
        const uint DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        #endregion
    }
}
