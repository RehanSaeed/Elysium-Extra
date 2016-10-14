namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class InputView
    {
        public InputView()
        {
            InitializeComponent();
            this.DataContext = new InputViewModel();
        }
    }
}
