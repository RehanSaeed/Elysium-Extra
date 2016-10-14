namespace Framework.UI.TestHarness.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using Framework.UI.Controls;

    /// <summary>
    /// Interaction logic for PagingView.xaml
    /// </summary>
    public partial class PagingView
    {
        public PagingView()
        {
            InitializeComponent();
        }

        private void OnSetPageClick(object sender, RoutedEventArgs e)
        {
            if (this.PageIntegerUpDown.Value.HasValue)
            {
                this.SetPage(this.PagingItemsControl, this.PageIntegerUpDown.Value.Value);
            }
        }

        private void SetPage(ItemsControl pagingItemsControl, int page)
        {
            PagingDecorator pagingDecorator = pagingItemsControl.FindVisualChild<PagingDecorator>();
            if ((page >= 0) &&
                (page < pagingDecorator.Items.Count))
            {
                pagingDecorator.SelectedIndex = page;
            }
        }
    }
}
