namespace Framework.UI.TestHarness.ViewModels
{
    using System.Threading.Tasks;
    using Framework.UI.Controls;
    using Framework.UI.Input;
    using Framework.UI.TestHarness.Models;

    public sealed class InputViewModel
    {
        private readonly FundCollection funds;
        private readonly AsyncDelegateCommand<string> showMessageBoxCommand;

        public InputViewModel()
        {
            this.funds = new FundCollection();
            this.showMessageBoxCommand = new AsyncDelegateCommand<string>(this.ShowMessageBox);
        }

        public FundCollection Funds
        {
            get { return this.funds; }
        }

        public AsyncDelegateCommand<string> ShowMessageBoxCommand
        {
            get { return this.showMessageBoxCommand; }
        }

        private async Task ShowMessageBox(string message)
        {
            await MessageDialog.ShowAsync("Show Message", message);
        }
    }
}
