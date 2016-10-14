namespace Framework.UI.Commands
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;

    /// <summary>
    /// Defines the attached properties to create a <see cref="CommandBehaviorBinding"/>.
    /// </summary>
    public partial class EventCommand
    {
        #region CommandBindings

        /// <summary>
        /// CommandBindings Read-Only Dependency Property
        /// As you can see the Attached readonly property has a name registered different (CommandBindingsInternal) than the property name, this is a trick that we can construct the collection as we want
        /// </summary>
        private static readonly DependencyPropertyKey CommandBindingsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "CommandBindingsInternal",
            typeof(CommandBindingCollection),
            typeof(EventCommand),
            new FrameworkPropertyMetadata((CommandBindingCollection)null));

        public static readonly DependencyProperty CommandBindingsProperty = CommandBindingsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the CommandBindings property. Here we initialize the collection and set the Owner property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <returns>
        /// The command bindings.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The dependency object trying to attach to is set to null</exception>
        public static CommandBindingCollection GetCommandBindings(DependencyObject d)
        {
            if (d == null)
            {
                throw new InvalidOperationException("The dependency object trying to attach to is set to null");
            }

            CommandBindingCollection collection = d.GetValue(EventCommand.CommandBindingsProperty) as CommandBindingCollection;
            if (collection == null)
            {
                collection = new CommandBindingCollection();
                collection.Owner = d;
                SetCommandBindings(d, collection);
            }

            return collection;
        }

        /// <summary>
        /// Provides a secure method for setting the CommandBindings property.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="value">The command binding collection value.</param>
        private static void SetCommandBindings(DependencyObject d, CommandBindingCollection value)
        {
            d.SetValue(CommandBindingsPropertyKey, value);
            INotifyCollectionChanged collection = (INotifyCollectionChanged)value;
            collection.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
        }

        /// <summary>
        /// Called when the command bindings collections has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private static void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CommandBindingCollection sourceCollection = (CommandBindingCollection)sender;
            switch (e.Action)
            {
                // When an item(s) is added we need to set the Owner property implicitly
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)
                    {
                        foreach (CommandBinding item in e.NewItems)
                        {
                            item.Owner = sourceCollection.Owner;
                        }
                    }

                    break;

                // When an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems != null)
                    {
                        foreach (CommandBinding item in e.OldItems)
                        {
                            item.Behavior.Dispose();
                        }
                    }

                    break;

                // Here we have to set the owner property to the new item and unregister the old item
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null)
                    {
                        foreach (CommandBinding item in e.NewItems)
                        {
                            item.Owner = sourceCollection.Owner;
                        }
                    }

                    if (e.OldItems != null)
                    {
                        foreach (CommandBinding item in e.OldItems)
                        {
                            item.Behavior.Dispose();
                        }
                    }

                    break;

                // When an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null)
                    {
                        foreach (CommandBinding item in e.OldItems)
                        {
                            item.Behavior.Dispose();
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Move:
                default:
                    break;
            }
        }

        #endregion
    }
}