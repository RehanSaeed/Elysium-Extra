namespace Framework.UI.TestHarness.Views
{
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for WindowView.xaml
    /// </summary>
    public partial class WindowView
    {
        public WindowView()
        {
            InitializeComponent();
        }

        private void OnShowBackgroundFadeWindow(object sender, System.Windows.RoutedEventArgs e)
        {
            Framework.UI.Controls.Window window = new Framework.UI.Controls.Window()
            {
                Height = 300,
                Icon = (DrawingImage)this.FindResource("FolderDrawingImage"),
                Style = (Style)this.FindResource("BackgroundFadeWindowStyle"),
                Title = "BackgroundFadeWindowStyle",
                Width = 500
            };
            window.Show();
        }

        private void OnShowAccentTitleBarWindow(object sender, RoutedEventArgs e)
        {
            Framework.UI.Controls.Window window = new Framework.UI.Controls.Window()
            {
                Content = "TODO Restyle the IsMouseOver for close/min/max buttons.",
                Height = 300,
                Icon = (DrawingImage)this.FindResource("LightFolderDrawingImage"),
                Style = (Style)this.FindResource("AccentTitleBarWindowStyle"),
                Title = "AccentTitleBarWindowStyle",
                Width = 500
            };
            window.Show();
        }
    }
}
