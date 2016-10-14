namespace Framework.UI.TestHarness.Views
{
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Framework.UI.TestHarness.ViewModels;

    /// <summary>
    /// Interaction logic for DataGridView.xaml
    /// </summary>
    public partial class DataGridView
    {
        public DataGridView()
        {
            InitializeComponent();
            this.DataContext = new DataGridViewModel();
        }

        private void OnGroupsTextChanged(object sender, TextChangedEventArgs e)
        {
            DataGridViewModel viewModel = (DataGridViewModel)this.DataContext;

            viewModel.FundsView.GroupDescriptions.Clear();

            foreach (string propertyName in this.GroupsTextBox.Text
                .Split(',')
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                viewModel.FundsView.GroupDescriptions.Add(new PropertyGroupDescription(propertyName));
            }
        }
    }
}
