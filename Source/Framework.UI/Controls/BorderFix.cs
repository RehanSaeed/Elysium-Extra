namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    public sealed class BorderFix : ContentControl
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="BorderFix"/> class.
        /// </summary>
        static BorderFix()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderFix), new FrameworkPropertyMetadata(typeof(BorderFix)));
        }

        #endregion
    }
}
