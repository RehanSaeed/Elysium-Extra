namespace Framework.UI.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts the <see cref="CalloutContentControl"/> to a <see cref="Thickness"/> based on the controls ArrowPlacement and ArrowHeight.
    /// </summary>
    public sealed class CalloutContentControlToThicknessConverter : IValueConverter
    {
        #region Public Methods

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <exception cref="System.Exception">ArrowPlacement not recognized</exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CalloutContentControl c = (CalloutContentControl)value;

            switch (c.ArrowPlacement)
            {
                case ArrowPlacement.Bottom:
                    return new Thickness(0, 0, 0, c.ArrowHeight);
                case ArrowPlacement.None:
                    return new Thickness(0, 0, 0, 0);
                case ArrowPlacement.Top:
                    return new Thickness(0, c.ArrowHeight, 0, 0);
                default:
                    throw new Exception("ArrowPlacement not recognized");
            }
        }

        /// <summary>
        /// Throws NotImplementedException. Does nothing.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// Throws NotImplementedException.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Throws NotImplementedException.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
