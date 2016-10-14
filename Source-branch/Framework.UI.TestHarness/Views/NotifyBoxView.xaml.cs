namespace Framework.UI.TestHarness.Views
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using Framework.UI.Controls;

    /// <summary>
    /// Interaction logic for NotifyBoxView.xaml
    /// </summary>
    public partial class NotifyBoxView
    {
        public NotifyBoxView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The on show notify box.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnShowNotifyBox(object sender, RoutedEventArgs e)
        {
            NotifyBox.Show(
                (DrawingImage)this.FindResource(this.NotifyBoxIconTextBox.Text),
                this.NotifyBoxTitleTextBox.Text,
                this.NotifyBoxMessageTextBox.Text,
                this.IsDoubleHeightCheckBox.IsChecked.Value);
        } 
    }
}
