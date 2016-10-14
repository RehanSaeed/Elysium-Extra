namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Expander Menu
    /// </summary>
    public class ExpanderMenu : Menu
    {
        #region Dependency Properties

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(
            "ExpandDirection",
            typeof(ExpandDirection),
            typeof(ExpanderMenu),
            new PropertyMetadata(ExpandDirection.Right, null));

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded",
            typeof(bool),
            typeof(ExpanderMenu),
            new PropertyMetadata(false));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the expand direction.
        /// </summary>
        /// <value>
        /// The expand direction.
        /// </value>
        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)this.GetValue(ExpandDirectionProperty); }
            set { this.SetValue(ExpandDirectionProperty, value); }
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

        #endregion
    }
}
