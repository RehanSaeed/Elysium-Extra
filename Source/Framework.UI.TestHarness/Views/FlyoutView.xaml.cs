namespace Framework.UI.TestHarness.Views
{
    using System.Windows.Controls;
    using Framework.UI.Controls;

    /// <summary>
    /// Interaction logic for FlyoutView.xaml
    /// </summary>
    public partial class FlyoutView
    {
        public FlyoutView()
        {
            InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.ParentGrid.Children.Contains(this.ExpanderGrid))
            {
                this.ParentGrid.Children.Remove(this.ExpanderGrid);
                Window window = (Window)Window.GetWindow(this);
                window.ForegroundContent = this.ExpanderGrid;
            }
        }
    }
}
