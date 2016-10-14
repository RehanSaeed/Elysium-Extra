namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;

    /// <summary>
    /// An expandable list box item.
    /// </summary>
    public sealed class NotesListBoxItem : ListBoxItem
    {
        #region Dependency Properties

        public static readonly DependencyProperty ExpandedHeightProperty = DependencyProperty.Register(
            "ExpandedHeight", 
            typeof(double), 
            typeof(NotesListBoxItem), 
            new PropertyMetadata(400D));

        public static readonly DependencyProperty ExpandedWidthProperty = DependencyProperty.Register(
            "ExpandedWidth", 
            typeof(double), 
            typeof(NotesListBoxItem), 
            new PropertyMetadata(600D));

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded",
            typeof(bool),
            typeof(NotesListBoxItem),
            new PropertyMetadata(false, OnIsExpandedChanged));

        public static readonly DependencyProperty PinCommandProperty = DependencyProperty.Register(
            "PinCommand",
            typeof(ICommand),
            typeof(NotesListBoxItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PinCommandParameterProperty = DependencyProperty.Register(
            "PinCommandParameter",
            typeof(object),
            typeof(NotesListBoxItem),
            new PropertyMetadata(null)); 

        #endregion

        private Storyboard storyboard;

        #region Public Properties

        /// <summary>
        /// Gets or sets the height of the expanded item.
        /// </summary>
        /// <value>
        /// The height of the expanded item.
        /// </value>
        public double ExpandedHeight
        {
            get { return (double)this.GetValue(ExpandedHeightProperty); }
            set { this.SetValue(ExpandedHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the expanded item.
        /// </summary>
        /// <value>
        /// The width of the expanded item.
        /// </value>
        public double ExpandedWidth
        {
            get { return (double)this.GetValue(ExpandedWidthProperty); }
            set { this.SetValue(ExpandedWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpanded
        {
            get { return (bool)this.GetValue(IsExpandedProperty); }
            set { this.SetValue(IsExpandedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the pin command.
        /// </summary>
        /// <value>
        /// The pin command.
        /// </value>
        public ICommand PinCommand
        {
            get { return (ICommand)this.GetValue(PinCommandProperty); }
            set { this.SetValue(PinCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the pin command parameter.
        /// </summary>
        /// <value>
        /// The pin command parameter.
        /// </value>
        public object PinCommandParameter
        {
            get { return (object)this.GetValue(PinCommandParameterProperty); }
            set { this.SetValue(PinCommandParameterProperty, value); }
        }
        
        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the expanded property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsExpandedChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs e)
        {
            NotesListBoxItem notesListBoxItem = (NotesListBoxItem)dependencyObject;

            if (notesListBoxItem.IsExpanded)
            {
                notesListBoxItem.Expand();
            }
            else
            {
                notesListBoxItem.Collapse();
            }
        }

        /// <summary>
        /// Expands this instance.
        /// </summary>
        private void Expand()
        {
            if (this.storyboard != null)
            {
                this.storyboard.Stop();
            }

            this.storyboard = new Storyboard();

            DoubleAnimation widthDoubleAnimation = new DoubleAnimation(this.ExpandedWidth, new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard.SetTarget(widthDoubleAnimation, this);
            Storyboard.SetTargetProperty(widthDoubleAnimation, new PropertyPath("Width"));
            this.storyboard.Children.Add(widthDoubleAnimation);

            DoubleAnimation heightDoubleAnimation = new DoubleAnimation(this.ExpandedHeight, new Duration(TimeSpan.FromMilliseconds(300)));
            Storyboard.SetTarget(heightDoubleAnimation, this);
            Storyboard.SetTargetProperty(heightDoubleAnimation, new PropertyPath("Height"));
            this.storyboard.Children.Add(heightDoubleAnimation);

            this.storyboard.Begin();
        }

        /// <summary>
        /// Collapses this instance.
        /// </summary>
        private void Collapse()
        {
            if (this.storyboard != null)
            {
                this.storyboard.Stop();
            }
        }

        #endregion
    }
}
