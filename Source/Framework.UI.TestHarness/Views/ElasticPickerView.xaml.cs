namespace Framework.UI.TestHarness.Views
{
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for ElasticPickerView.xaml
    /// </summary>
    public partial class ElasticPickerView
    {
        public ElasticPickerView()
        {
            InitializeComponent();
            this.DataContext = new ElasticPickerViewModel();
        }
    }
}
