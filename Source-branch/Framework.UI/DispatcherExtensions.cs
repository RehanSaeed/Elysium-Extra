namespace Framework.UI
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Threading;

    /// <summary>
    /// <see cref="Dispatcher"/> extension methods.
    /// </summary>
    public static class DispatcherExtensions
    {
        /// <summary>
        /// Invokes the specified action asynchronously using the <see cref="Dispatcher"/> if required.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="action">The action.</param>
        /// <returns>A task containing the invoked action.</returns>
        public static async Task RunAsync(this Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                await dispatcher.InvokeAsync(() => RunAsync(dispatcher, action));
            }
        }

        /// <summary>
        /// Invokes the specified action asynchronously using the <see cref="Dispatcher"/> if required.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="action">The action.</param>
        /// <param name="dispatcherPriority">The dispatcher priority.</param>
        /// <returns>A task containing the invoked action.</returns>
        public static async Task RunAsync(
            this Dispatcher dispatcher,
            Action action,
            DispatcherPriority dispatcherPriority)
        {
            if (dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                await dispatcher.InvokeAsync(() => RunAsync(dispatcher, action), dispatcherPriority);
            }
        }
    }
}
