using System;
using System.Runtime.InteropServices;

namespace RevitAddin.WindowDarkMode.Example;

/// <summary>
/// Provides helper methods to manage application modes such as light and dark themes.
/// </summary>
internal static class AppModeHelper
{
    /// <summary>
    /// Represents the preferred application mode.
    /// </summary>
    public enum PreferredAppMode
    {
        /// <summary>
        /// Default application mode.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Allow dark mode if supported.
        /// </summary>
        AllowDark = 1,

        /// <summary>
        /// Force dark mode.
        /// </summary>
        ForceDark = 2,

        /// <summary>
        /// Force light mode.
        /// </summary>
        ForceLight = 3,

        /// <summary>
        /// Maximum value for the enumeration.
        /// </summary>
        Max = 4
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr LoadLibrary(string lpFileName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetProcAddress(IntPtr hModule, IntPtr procName);

    private delegate int SetPreferredAppModeDelegate(PreferredAppMode appMode);
    private static readonly SetPreferredAppModeDelegate _setPreferredAppMode;

    private delegate void FlushMenuThemesDelegate();
    private static readonly FlushMenuThemesDelegate _flushMenuThemes;

    /// <summary>
    /// Static constructor to initialize delegates for setting application mode and flushing menu themes.
    /// </summary>
    static AppModeHelper()
    {
        IntPtr hUxTheme = GetModuleHandle("uxtheme.dll");

        if (hUxTheme == IntPtr.Zero)
            hUxTheme = LoadLibrary("uxtheme.dll");

        if (hUxTheme == IntPtr.Zero)
            return;

        // #135 corresponds to SetPreferredAppMode in uxtheme.dll
        IntPtr proc = GetProcAddress(hUxTheme, (IntPtr)135);
        if (proc != IntPtr.Zero)
        {
            _setPreferredAppMode =
                Marshal.GetDelegateForFunctionPointer<SetPreferredAppModeDelegate>(proc);
        }

        // #136 corresponds to FlushMenuThemes in uxtheme.dll
        IntPtr fnFlush = GetProcAddress(hUxTheme, (IntPtr)136);
        if (fnFlush != IntPtr.Zero)
        {
            _flushMenuThemes =
                Marshal.GetDelegateForFunctionPointer<FlushMenuThemesDelegate>(fnFlush);
        }
    }

    /// <summary>
    /// Sets the application mode to dark.
    /// </summary>
    /// <param name="application">The WPF application instance.</param>
    public static void SetAppModeDark(this System.Windows.Application application)
    {
        SetAppMode(PreferredAppMode.ForceDark);
    }

    /// <summary>
    /// Sets the application mode to light.
    /// </summary>
    /// <param name="application">The WPF application instance.</param>
    public static void SetAppModeLight(this System.Windows.Application application)
    {
        SetAppMode(PreferredAppMode.ForceLight);
    }

    /// <summary>
    /// Gets a value indicating whether setting the preferred application mode is supported.
    /// </summary>
    public static bool IsSupported => _setPreferredAppMode != null;

    /// <summary>
    /// Sets the application mode to the default mode.
    /// </summary>
    public static void SetAppModeDefault()
    {
        SetAppMode(PreferredAppMode.Default);
    }

    /// <summary>
    /// Sets the application mode to light.
    /// </summary>
    public static void SetAppModeLight()
    {
        SetAppMode(PreferredAppMode.ForceLight);
    }

    /// <summary>
    /// Sets the application mode to dark.
    /// </summary>
    public static void SetAppModeDark()
    {
        SetAppMode(PreferredAppMode.ForceDark);
    }

    /// <summary>
    /// Sets the application mode to the specified mode.
    /// </summary>
    /// <param name="mode">The preferred application mode.</param>
    public static void SetAppMode(PreferredAppMode mode)
    {
        _setPreferredAppMode?.Invoke(mode);
        FlushMenuThemes();
    }

    /// <summary>
    /// Flushes the menu themes to ensure that the changes take effect immediately.
    /// </summary>
    private static void FlushMenuThemes()
    {
        _flushMenuThemes?.Invoke();
    }
}
