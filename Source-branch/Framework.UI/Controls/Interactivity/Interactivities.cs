namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Interactivity;

    /// <summary>
    /// Allows you to set a <see cref="Behavior"/> or <see cref="Trigger"/> in a style.
    /// </summary>
    public static class SupplementaryInteraction
    {
        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
            "Behaviors",
            typeof(Behaviors),
            typeof(SupplementaryInteraction),
            new PropertyMetadata(null, OnPropertyBehaviorsChanged));

        public static readonly DependencyProperty TriggersProperty = DependencyProperty.RegisterAttached(
            "Triggers",
            typeof(Triggers),
            typeof(SupplementaryInteraction),
            new PropertyMetadata(null, OnPropertyTriggersChanged));

        /// <summary>
        /// The get behaviours.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <returns> The <see cref="Behaviors"/>. </returns>
        public static Behaviors GetBehaviors(DependencyObject obj)
        {
            return (Behaviors)obj.GetValue(BehaviorsProperty);
        }

        /// <summary>
        /// The set behaviours.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <param name="value"> The value. </param>
        public static void SetBehaviors(DependencyObject obj, Behaviors value)
        {
            obj.SetValue(BehaviorsProperty, value);
        }

        /// <summary>
        /// The get triggers.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <returns> The <see cref="Triggers"/>. </returns>
        public static Triggers GetTriggers(DependencyObject obj)
        {
            return (Triggers)obj.GetValue(TriggersProperty);
        }

        /// <summary>
        /// The set triggers.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <param name="value"> The value. </param>
        public static void SetTriggers(DependencyObject obj, Triggers value)
        {
            obj.SetValue(TriggersProperty, value);
        }

        /// <summary>
        /// The on property behaviours changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnPropertyBehaviorsChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var behaviors = Interaction.GetBehaviors(d);
                foreach (var behavior in (Behaviors)e.NewValue)
                {
                    behaviors.Add(behavior);
                }
            }
        }

        /// <summary>
        /// The on property triggers changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void OnPropertyTriggersChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var triggers = Interaction.GetTriggers(d);
            foreach (var trigger in e.NewValue as Triggers)
            {
                triggers.Add(trigger);
            }
        }
    }
}
