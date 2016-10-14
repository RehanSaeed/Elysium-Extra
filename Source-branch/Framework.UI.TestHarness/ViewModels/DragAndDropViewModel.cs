namespace Framework.UI.TestHarness.ViewModels
{
    using System.Threading.Tasks;
    using Framework.UI.Controls;
    using Framework.UI.Input;
    using Framework.UI.TestHarness.Models;

    public sealed class DragAndDropViewModel
    {
        private readonly AsyncDelegateCommand<string> showMessageBoxCommand;
        private FundCollection funds;

        #region Constructors

        public DragAndDropViewModel()
        {
            this.funds = new FundCollection();
            this.showMessageBoxCommand = new AsyncDelegateCommand<string>(this.ShowMessageBox);
        }

        #endregion

        public FundCollection Funds
        {
            get { return this.funds; }
        }

        public AsyncDelegateCommand<string> ShowMessageBoxCommand
        {
            get { return this.showMessageBoxCommand; }
        }

        /// <summary>
        /// The show message box.
        /// </summary>
        /// <param name="message"> The message. </param>
        private async Task ShowMessageBox(string message)
        {
            await MessageDialog.ShowAsync("Show Message", message);
        }
    }
}
