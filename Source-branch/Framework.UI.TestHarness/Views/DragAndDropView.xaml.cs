namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for DragAndDropView.xaml
    /// </summary>
    public partial class DragAndDropView
    {
        public DragAndDropView()
        {
            InitializeComponent();
            this.DataContext = new DragAndDropViewModel();
        }
    }
}
