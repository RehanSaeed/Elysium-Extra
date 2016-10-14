namespace Framework.UI.TestHarness.Views
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for PopupsView.xaml
    /// </summary>
    public partial class PopupsView
    {
        public PopupsView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The on open popup click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnOpenPopupClick(object sender, RoutedEventArgs e)
        {
            this.Popup.IsOpen = true;
        }
    }
}
