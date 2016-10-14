namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Represents a selectable item inside a <see cref="ExpanderMenu"/>.
    /// </summary>
    public class ExpanderMenuItem : MenuItem
    {
        public static readonly DependencyProperty IsColoursInvertedProperty = DependencyProperty.Register(
            "IsColoursInverted", 
            typeof(bool), 
            typeof(ExpanderMenuItem), 
            new PropertyMetadata(false));

        /// <summary>
        /// Gets or sets a value indicating whether the colours of this instance are inverted so that the 
        /// Foreground and Background are swapped. Useful for showing contrast.
        /// </summary>
        /// <value>
        /// <c>true</c> if the colours in this instance are inverted; otherwise, <c>false</c>.
        /// </value>
        public bool IsColoursInverted
        {
            get { return (bool)this.GetValue(IsColoursInvertedProperty); }
            set { this.SetValue(IsColoursInvertedProperty, value); }
        }
    }
}
