namespace Framework.UI.TestHarness.Views
{
    using System.Windows;
    using Framework.UI.Controls;
    using Framework.UI.Input;

    /// <summary>
    /// Interaction logic for MessageDialogView.xaml
    /// </summary>
    public partial class MessageDialogView
    {
        public MessageDialogView()
        {
            InitializeComponent();
        }

        private async void OnShowMessageDialogView(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = await MessageDialog.ShowAsync(
                this.HeaderTextBox.Text,
                this.ContentTextBox.Text,
                (MessageBoxButton)(this.ButtonComboBox.SelectedItem),
                (MessageDialogType)this.TypeComboBox.SelectedItem);
        }

        private async void OnShowCustomMessageDialogView(object sender, RoutedEventArgs e)
        {
            await MessageDialog.ShowAsync(
                this.HeaderTextBox.Text,
                this.ContentTextBox.Text,
                new[] 
                {
                    new MessageDialogButton()
                    {
                        Command = new DelegateCommand(() => MessageBox.Show("Button 1 Clicked")),
                        Content = "Custom Button 1"
                    },
                    new MessageDialogButton()
                    {
                        Command = new DelegateCommand(() => MessageBox.Show("Button 2 Clicked")),
                        Content = "Custom Button 2"
                    }
                },
                (MessageDialogType)this.TypeComboBox.SelectedItem);
        } 
    }
}
