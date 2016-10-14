namespace Framework.UI.TestHarness.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for OverlayWindowExample.xaml
    /// </summary>
    public partial class OverlayWindowExample
    {
        public OverlayWindowExample()
        {
            InitializeComponent();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
