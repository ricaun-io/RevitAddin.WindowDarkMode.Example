using System;
using System.Windows;

namespace RevitAddin.WindowDarkMode.Example.Views
{
    public partial class SimpleView : Window
    {
        public SimpleView()
        {
            InitializeComponent();
            InitializeWindow();

            Title = $"Build: {Environment.OSVersion.Version.Build}";
            this.Loaded += (s, e) =>
            {
                ChangeImmersiveDarkMode();
            };
            this.KeyDown += (s, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Escape) Close();
                else if (e.Key == System.Windows.Input.Key.A) ChangeImmersiveDarkMode2();
                else if (e.Key == System.Windows.Input.Key.S)
                {
                    DesktopWindowManager.SetCaptionColor(this, System.Drawing.Color.FromArgb(0x2e, 0x34, 0x40));
                }
                else if (e.Key == System.Windows.Input.Key.W)
                {
                    DesktopWindowManager.SetCaptionColor(this, System.Drawing.Color.FromArgb(0x3b, 0x44, 0x53));
                }
                else if (e.Key == System.Windows.Input.Key.Z)
                    DesktopWindowManager.SetCaptionColor(this, System.Drawing.Color.Red);
                else if (e.Key == System.Windows.Input.Key.X)
                    DesktopWindowManager.SetCaptionColor(this, System.Drawing.Color.Blue);
                else if (e.Key == System.Windows.Input.Key.C)
                    DesktopWindowManager.SetCaptionColor(this, System.Drawing.Color.Green);
                else if (e.Key == System.Windows.Input.Key.V)
                    DesktopWindowManager.SetCaptionColor(this, System.Drawing.Color.White);
                else if (e.Key == System.Windows.Input.Key.D)
                {
                    DesktopWindowManager.SetCaptionColor(this);
                }
                else ChangeImmersiveDarkMode();
            };
        }

        public void ChangeImmersiveDarkMode()
        {
            var darkBackground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(16, 16, 16));
            var lightBackground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);

            var darkForeground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
            var lightForeground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);

            var enableDarkMode = false;
            if (Background is System.Windows.Media.SolidColorBrush solidColor)
                enableDarkMode = solidColor.Color == lightBackground.Color;

            DesktopWindowManager.SetImmersiveDarkMode(this, enableDarkMode);
            Background = enableDarkMode ? darkBackground : lightBackground;
            Foreground = enableDarkMode ? darkForeground : lightForeground;

            //// Force to redraw the window border
            //WindowStyle = WindowStyle.None;
            //WindowStyle = WindowStyle.SingleBorderWindow;
            this.ShowInTaskbar = !this.ShowInTaskbar;
            this.ShowInTaskbar = !this.ShowInTaskbar;
        }
        bool enableDarkMode = false;
        public void ChangeImmersiveDarkMode2()
        {
            enableDarkMode = !enableDarkMode;
            DesktopWindowManager.SetImmersiveDarkMode(this, enableDarkMode);

            //// Force to redraw the window border
            this.ShowInTaskbar = !this.ShowInTaskbar;
            this.ShowInTaskbar = !this.ShowInTaskbar;
        }

        #region InitializeWindow
        private void InitializeWindow()
        {
            this.ShowInTaskbar = false;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            new System.Windows.Interop.WindowInteropHelper(this) { Owner = Autodesk.Windows.ComponentManager.ApplicationWindow };
        }
        #endregion
    }
}