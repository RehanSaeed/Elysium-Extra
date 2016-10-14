namespace Framework.UI.Commands
{
    /// <summary>
    /// Defines the interface for a strategy of execution for the <see cref="CommandBehaviorBinding"/>.
    /// </summary>
    internal interface IExecutionStrategy
    {
        /// <summary>
        /// Gets or sets the behaviour that we execute this strategy
        /// </summary>
        CommandBehaviorBinding Behavior { get; set; }

        /// <summary>
        /// Executes according to the strategy type
        /// </summary>
        /// <param name="parameter">The parameter to be used in the execution</param>
        void Execute(object parameter);
    }
}
