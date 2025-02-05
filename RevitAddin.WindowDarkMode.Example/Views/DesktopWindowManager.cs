using System;
using System.Runtime.InteropServices;
using System.Drawing;
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

            var darkMode = enable ? 1u : 0;
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
        static extern int DwmSetWindowAttribute(IntPtr hwnd, uint dwAttribute, ref uint pvAttribute, int cbAttribute);
        /// <summary>
        /// https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute
        /// </summary>
        const uint DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        #endregion

        #region CaptionColor
        /// <summary>
        /// Check whether Windows 11 build is 22000 or higher, that supports <see cref="SetCaptionColor(Window, Color)"/>.
        /// </summary>
        public static bool IsCaptionColorSupported { get; } =
            Environment.OSVersion.Version.Build >= 22000;

        /// <summary>
        /// Sets the caption color for the specified window.
        /// </summary>
        /// <param name="window">The window for which to set the caption color.</param>
        /// <param name="color">The color to set for the window caption.</param>
        /// <returns>True if the caption color was set successfully; otherwise, false.</returns>
        public static bool SetCaptionColor(Window window, Color color)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            return SetCaptionColor(hwnd, color);
        }

        /// <summary>
        /// Sets the caption color for the specified window.
        /// </summary>
        /// <param name="window">The window for which to set the caption color.</param>
        /// <param name="colorRef">The color reference to set for the window caption. Defaults to DWMWA_COLOR_DEFAULT.</param>
        /// <returns>True if the caption color was set successfully; otherwise, false.</returns>
        public static bool SetCaptionColor(Window window, uint colorRef = DWMWA_COLOR_DEFAULT)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            return SetCaptionColor(hwnd, colorRef);
        }

        /// <summary>
        /// Sets the caption color for the specified window handle.
        /// </summary>
        /// <param name="hWnd">The handle of the window for which to set the caption color.</param>
        /// <param name="color">The color to set for the window caption.</param>
        /// <returns>True if the caption color was set successfully; otherwise, false.</returns>
        public static bool SetCaptionColor(IntPtr hWnd, Color color)
        {
            var colorRef = (uint)ColorTranslator.ToWin32(color);
            return SetCaptionColor(hWnd, colorRef);
        }

        /// <summary>
        /// Sets the caption color for the specified window handle.
        /// </summary>
        /// <param name="hWnd">The handle of the window for which to set the caption color.</param>
        /// <param name="colorRef">The color reference to set for the window caption. Defaults to DWMWA_COLOR_DEFAULT.</param>
        /// <returns>True if the caption color was set successfully; otherwise, false.</returns>
        public static bool SetCaptionColor(IntPtr hWnd, uint colorRef = DWMWA_COLOR_DEFAULT)
        {
            if (IsCaptionColorSupported == false)
                return false;

            var result = DwmSetWindowAttribute(hWnd, DWMWA_CAPTION_COLOR, ref colorRef, sizeof(int));
            return result > 0;
        }

        /// <summary>
        /// https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute
        /// </summary>
        const uint DWMWA_CAPTION_COLOR = 35;
        /// <summary>
        /// Specifying DWMWA_COLOR_DEFAULT (value 0xFFFFFFFF) for the color will reset the window back to using the system's default behavior for the caption color.
        /// </summary>
        public const uint DWMWA_COLOR_DEFAULT = 0xffffffff;
        #endregion
    }
}
