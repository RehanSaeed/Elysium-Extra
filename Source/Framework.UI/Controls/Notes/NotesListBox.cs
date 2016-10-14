namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A post-it note style expandable list box.
    /// </summary>
    public sealed class NotesListBox : ListBox
    {
        #region Dependency Properties

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate", 
            typeof(DataTemplate), 
            typeof(NotesListBox), 
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
            "HeaderTemplateSelector", 
            typeof(DataTemplateSelector), 
            typeof(NotesListBox), 
            new PropertyMetadata(null));

        /// <summary>
        /// The orientation of the notes.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Orientation),
            typeof(NotesListBox),
            new UIPropertyMetadata(Orientation.Horizontal));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        /// <value>
        /// The header template.
        /// </value>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)this.GetValue(HeaderTemplateProperty); }
            set { this.SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header template selector.
        /// </summary>
        /// <value>
        /// The header template selector.
        /// </value>
        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(HeaderTemplateSelectorProperty); }
            set { this.SetValue(HeaderTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation of the notes.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get { return (Orientation)this.GetValue(OrientationProperty); }
            set { this.SetValue(OrientationProperty, value); }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The get container for item override.
        /// </summary>
        /// <returns>
        /// The <see cref="DependencyObject"/>.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new NotesListBoxItem();
        } 

        #endregion
    }
}
