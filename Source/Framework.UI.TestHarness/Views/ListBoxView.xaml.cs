namespace Framework.UI.TestHarness.Views
{
    using System.Windows;
    using Framework.UI.Input;
    using Framework.UI.TestHarness.Models;
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for ListBoxView.xaml
    /// </summary>
    public partial class ListBoxView
    {
        public ListBoxView()
        {
            InitializeComponent();
            this.DataContext = new ListBoxViewModel();
        }
    }
}
