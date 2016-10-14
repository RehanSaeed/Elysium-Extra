namespace Framework.UI.TestHarness.Models
{
    using Framework.ComponentModel;
    using Framework.UI.Input;

    /// <summary>
    /// The wizard view model.
    /// </summary>
    public sealed class WizardViewModel : NotifyPropertyChanges
    {
        private readonly DelegateCommand enteringCommand;
        private readonly DelegateCommand enteringFirstTimeCommand;
        private readonly DelegateCommand leavingCommand;

        private bool isBackwardEnabled = true;
        private bool isForwardEnabled = true;
        private bool isNavigationCancelled;

        public WizardViewModel()
        {
            this.enteringCommand = new DelegateCommand(this.Enter, this.CanEnter);
            this.enteringFirstTimeCommand = new DelegateCommand(this.EnterFirstTime, this.CanEnterFirstTime);
            this.leavingCommand = new DelegateCommand(this.Leave, this.CanLeave);
        }

        /// <summary>
        /// Gets or sets a value indicating whether is backward enabled.
        /// </summary>
        public bool IsBackwardEnabled
        {
            get { return this.isBackwardEnabled; }
            set { this.SetProperty(ref this.isBackwardEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is forward enabled.
        /// </summary>
        public bool IsForwardEnabled
        {
            get { return this.isForwardEnabled; }
            set { this.SetProperty(ref this.isForwardEnabled, value); }
        }

        public bool IsNavigationCancelled
        {
            get { return this.isNavigationCancelled; }
            set { this.SetProperty(ref this.isNavigationCancelled, value); }
        }

        public DelegateCommand EnteringCommand
        {
            get { return this.enteringCommand; }
        }

        public DelegateCommand EnteringFirstTimeCommand
        {
            get { return this.enteringFirstTimeCommand; }
        }

        public DelegateCommand LeavingCommand
        {
            get { return this.leavingCommand; }
        }

        private bool CanEnter()
        {
            return true;
        }

        private void Enter()
        {
        }

        private bool CanEnterFirstTime()
        {
            return true;
        }

        private void EnterFirstTime()
        {
        }

        private bool CanLeave()
        {
            return true;
        }

        private void Leave()
        {
        }
    }
}
