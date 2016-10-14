namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for CommandsView.xaml
    /// </summary>
    public partial class CommandsView
    {
        public CommandsView()
        {
            InitializeComponent();
            this.DataContext = new CommandsViewModel();
        }
    }
}
