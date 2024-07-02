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
            WindowStyle = WindowStyle.None;
            WindowStyle = WindowStyle.SingleBorderWindow;
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