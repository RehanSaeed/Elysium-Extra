namespace Framework.UI.Commands
{
    using System.Windows;

    /// <summary>
    /// Collection to store the list of behaviours. This is done so that you can initiate it from XAML
    /// This inherits from <see cref="Freezable"/> so that it gets inheritance context for DataBinding to work
    /// </summary>
    public class CommandBindingCollection : FreezableCollection<CommandBinding>
    {
        /// <summary>
        /// Gets or sets the Owner of the binding
        /// </summary>
        public DependencyObject Owner { get; set; }
    }
}
