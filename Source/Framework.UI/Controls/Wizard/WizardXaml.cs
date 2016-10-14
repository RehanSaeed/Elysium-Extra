namespace Framework.UI.Controls
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Code behind for the XAML
    /// </summary>
    public partial class WizardXaml
    {
        /// <summary>
        /// On Arrow Click Event
        /// </summary>
        /// <param name="sender">THe Sender </param>
        /// <param name="e">Event Arguments </param>
        private void OnArrowClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            StackPanel stackPanel = (StackPanel)button.Parent;
            Popup popup = stackPanel.Children.OfType<Popup>().First();
            popup.IsOpen = true;
        }

        /// <summary>
        /// On Arrow Item Click Event
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">Event Arguments</param>
        private void OnArrowItemClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Popup popup = button.FindVisualParent<Popup>();
            popup.IsOpen = false;
        }
    }
}
