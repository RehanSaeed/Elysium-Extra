namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for MenuItemView.xaml
    /// </summary>
    public partial class MenuItemView
    {
        public MenuItemView()
        {
            InitializeComponent();
            this.DataContext = new MenuItemViewModel();
        }
    }
}
