namespace Framework.UI.TestHarness.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for OverlayWindowView.xaml
    /// </summary>
    public partial class OverlayWindowView
    {
        public OverlayWindowView()
        {
            InitializeComponent();
        }

        private void OnShowOverlayWindow(object sender, RoutedEventArgs e)
        {
            OverlayWindowExample window = new OverlayWindowExample();
            window.Show();
        }

        private void OnShowOverlayWindowOverThisWindow(object sender, RoutedEventArgs e)
        {
            OverlayWindowExample window = new OverlayWindowExample();
            window.Owner = Window.GetWindow(this);
            window.Show();
        }

        private void OnShowAccentOverlayWindowOverThisWindow(object sender, RoutedEventArgs e)
        {
            OverlayWindowExample window = new OverlayWindowExample()
            {
                Style = (Style)this.FindResource("AccentOverlayWindowStyle")
            };
            window.Owner = Window.GetWindow(this);
            window.Show();
        }

        private void OnShowDarkOverlayWindowOverThisWindow(object sender, RoutedEventArgs e)
        {
            OverlayWindowExample window = new OverlayWindowExample()
            {
                Style = (Style)this.FindResource("DarkOverlayWindowStyle")
            };
            window.Owner = Window.GetWindow(this);
            window.Show();
        }
    }
}
