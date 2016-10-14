namespace Framework.UI.TestHarness.ViewModels
{
    using System.Threading.Tasks;
    using Framework.UI.Controls;
    using Framework.UI.Input;

    public sealed class CommandsViewModel
    {
        private readonly AsyncDelegateCommand<string> showMessageBoxCommand;

        public CommandsViewModel()
        {
            this.showMessageBoxCommand = new AsyncDelegateCommand<string>(this.ShowMessageBox);
        }

        /// <summary>
        /// Gets the show message box command.
        /// </summary>
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
