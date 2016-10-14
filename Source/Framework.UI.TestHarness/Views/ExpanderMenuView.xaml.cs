namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for ExpanderMenuView.xaml
    /// </summary>
    public partial class ExpanderMenuView
    {
        public ExpanderMenuView()
        {
            InitializeComponent();
            this.DataContext = new ExpanderMenuViewModel();
        }
    }
}
