namespace Framework.UI
{
    using System.Windows;
    using System.Windows.Controls;
    using Elysium.Parameters;

    internal static class ElysiumParameterOverrides
    {
        public static void Apply()
        {
            // RadioButton
            Bullet.DecoratorSizeProperty.OverrideMetadata(
                typeof(RadioButton),
                new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsMeasure, null, new CoerceValueCallback(DoubleUtil.CoerceNonNegative)));
            Bullet.SizeProperty.OverrideMetadata(
                typeof(RadioButton),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure, null, new CoerceValueCallback(DoubleUtil.CoerceNonNegative)));

            // CheckBox
            Bullet.DecoratorSizeProperty.OverrideMetadata(
                typeof(System.Windows.Controls.CheckBox),
                new FrameworkPropertyMetadata(20.0, FrameworkPropertyMetadataOptions.AffectsMeasure, null, new CoerceValueCallback(DoubleUtil.CoerceNonNegative)));
            Bullet.SizeProperty.OverrideMetadata(
                typeof(System.Windows.Controls.CheckBox),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure, null, new CoerceValueCallback(DoubleUtil.CoerceNonNegative)));
            Elysium.Parameters.CheckBox.CheckSizeProperty.OverrideMetadata(
                typeof(System.Windows.Controls.CheckBox),
                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsMeasure, null, new CoerceValueCallback(DoubleUtil.CoerceNonNegative)));
        }

        internal static class DoubleUtil
        {
            internal static object CoerceNonNegative(DependencyObject obj, object basevalue)
            {
                double num = (double)basevalue;
                return (IsNonNegative(num) ? num : 0.0);
            }

            internal static bool IsNonNegative(double value)
            {
                return ((!double.IsNaN(value) && !double.IsInfinity(value)) && (value > 0.0));
            }
        }
    }
}
