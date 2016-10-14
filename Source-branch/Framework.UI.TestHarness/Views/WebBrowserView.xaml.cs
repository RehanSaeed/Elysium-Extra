namespace Framework.UI.TestHarness.Views
{
    /// <summary>
    /// Interaction logic for WebBrowserView.xaml
    /// </summary>
    public partial class WebBrowserView
    {
        public WebBrowserView()
        {
            InitializeComponent();
            this.DataContext = "http://www.bing.com";
        }
    }
}
