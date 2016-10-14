namespace Framework.UI.TestHarness
{
    using System.Windows;
    using Framework.UI.TestHarness.Windows;

    /// <summary>
    /// Interaction logic for Main View XAML
    /// </summary>
    public partial class MainView
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="MainView"/> class.
        /// </summary>
        public MainView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The on Elysium click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event Arguments. </param>
        private void OnElysiumClick(object sender, RoutedEventArgs e)
        {
            new ElysiumView().Show();
        }

        /// <summary>
        /// The on styles click.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event Arguments. </param>
        private void OnStylesClick(object sender, RoutedEventArgs e)
        {
            new StylesView().Show();
        }
    }
}
