namespace Framework.UI.TestHarness.Views
{
    using System.Collections.Generic;
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for ColoursView.xaml
    /// </summary>
    public partial class ColoursView
    {
        public ColoursView()
        {
            InitializeComponent();
            this.DataContext = new ColoursViewModel();
        }
    }
}
