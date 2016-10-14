namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A content control with a triangle shaped arrow on one corner.
    /// </summary>
    public sealed class CalloutContentControl : ContentControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty ArrowAlignmentProperty = DependencyProperty.Register(
           "ArrowAlignment",
           typeof(ArrowAlignment),
           typeof(CalloutContentControl),
           new FrameworkPropertyMetadata(
               ArrowAlignment.Left,
               FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowHeightProperty = DependencyProperty.Register(
            "ArrowHeight",
            typeof(double),
            typeof(CalloutContentControl),
            new FrameworkPropertyMetadata(
                12.0D,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowPlacementProperty = DependencyProperty.Register(
            "ArrowPlacement",
            typeof(ArrowPlacement),
            typeof(CalloutContentControl),
            new FrameworkPropertyMetadata(
                ArrowPlacement.Top,
                FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ArrowWidthProperty = DependencyProperty.Register(
            "ArrowWidth",
            typeof(double),
            typeof(CalloutContentControl),
            new FrameworkPropertyMetadata(
                18.0D,
                FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the ArrowAlignment property.  This dependency property 
        /// indicates the offset of the arrow relative to its placement.
        /// <para/>
        /// If the arrow is placed along the top or bottom edge (ArrowPlacement = Top/Bottom), then 
        /// ArrowAlignment specifies whether the arrow is located on the left or right side of the
        /// edge.
        /// <para/>
        /// If the arrow is placed along the left or right edge (ArrowPlacement = Left/Right), then
        /// ArrowAlignment specifies whether the arrow is located on the top or bottom side of the
        /// edge.
        /// </summary>
        public ArrowAlignment ArrowAlignment
        {
            get { return (ArrowAlignment)this.GetValue(ArrowAlignmentProperty); }
            set { this.SetValue(ArrowAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ArrowHeight property.  This dependency property 
        /// indicates the Height of the arrow on the callout.
        /// </summary>
        public double ArrowHeight
        {
            get { return (double)this.GetValue(ArrowHeightProperty); }
            set { this.SetValue(ArrowHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ArrowWidth property.  This dependency property 
        /// indicates the width of the arrow on the callout.
        /// </summary>
        public double ArrowWidth
        {
            get { return (double)this.GetValue(ArrowWidthProperty); }
            set { this.SetValue(ArrowWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ArrowPlacement property.  This dependency property specifies which 
        /// edge the arrow is placed on (Top, Bottom, Left, Right).
        /// </summary>
        public ArrowPlacement ArrowPlacement
        {
            get { return (ArrowPlacement)this.GetValue(ArrowPlacementProperty); }
            set { this.SetValue(ArrowPlacementProperty, value); }
        }

        #endregion
    }
}
