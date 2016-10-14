namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for IconsView.xaml
    /// </summary>
    public partial class IconsView
    {
        public IconsView()
        {
            InitializeComponent();
            this.DataContext = new IconViewModel();
        }
    }
}
