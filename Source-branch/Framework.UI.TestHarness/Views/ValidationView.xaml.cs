namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for ValidationView.xaml
    /// </summary>
    public partial class ValidationView
    {
        public ValidationView()
        {
            InitializeComponent();
            this.DataContext = new ValidationViewModel();
        }
    }
}
