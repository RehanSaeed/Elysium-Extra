namespace Framework.UI.Commands
{
    using System;

    /// <summary>
    /// Executes a command 
    /// </summary>
    internal class CommandExecutionStrategy : IExecutionStrategy
    {
        #region IExecutionStrategy Members
        /// <summary>
        /// Gets or sets the behaviour that we execute this strategy
        /// </summary>
        public CommandBehaviorBinding Behavior { get; set; }

        /// <summary>
        /// Executes the Command that is stored in the CommandProperty of the CommandExecution
        /// </summary>
        /// <param name="parameter">The parameter for the command</param>
        public void Execute(object parameter)
        {
            if (this.Behavior == null)
            {
                throw new InvalidOperationException("Behavior property cannot be null when executing a strategy");
            }

            if (this.Behavior.Command.CanExecute(this.Behavior.CommandParameter))
            {
                this.Behavior.Command.Execute(this.Behavior.CommandParameter);
            }
        }

        #endregion
    }
}
