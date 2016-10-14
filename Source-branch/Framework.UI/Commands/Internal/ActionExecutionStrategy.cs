namespace Framework.UI.Commands
{
    /// <summary>
    /// executes a delegate
    /// </summary>
    internal class ActionExecutionStrategy : IExecutionStrategy
    {
        #region IExecutionStrategy Members

        /// <summary>
        /// Gets or sets the behaviour that we execute this strategy
        /// </summary>
        public CommandBehaviorBinding Behavior { get; set; }

        /// <summary>
        /// Executes an Action delegate
        /// </summary>
        /// <param name="parameter">The parameter to pass to the Action</param>
        public void Execute(object parameter)
        {
            this.Behavior.Action(parameter);
        }

        #endregion
    }
}
