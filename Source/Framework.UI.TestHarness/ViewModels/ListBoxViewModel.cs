namespace Framework.UI.TestHarness.ViewModels
{
    using System.Threading.Tasks;
    using Framework.UI.Controls;
    using Framework.UI.Input;
    using Framework.UI.TestHarness.Models;

    public sealed class ListBoxViewModel
    {
        private readonly FundCollection funds;
        private readonly AsyncDelegateCommand pinCommand;

        public ListBoxViewModel()
        {
            this.funds = new FundCollection();
            this.pinCommand = new AsyncDelegateCommand(this.Pin);
        }

        public FundCollection Funds
        {
            get { return this.funds; }
        }

        public AsyncDelegateCommand PinCommand
        {
            get { return this.pinCommand; }
        }

        private async Task Pin()
        {
            await MessageDialog.ShowAsync("PinCommand", "PinCommand Fired.");
        }
    }
}
