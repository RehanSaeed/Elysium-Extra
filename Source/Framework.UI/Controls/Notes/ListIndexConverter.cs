namespace Framework.UI.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Provides a OneWay converter that can provide a Rotation value for a give ListBoxItem, based on the source ListBoxItem index
    /// </summary>
    internal sealed class ListIndexConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Converts the specified <see cref="ListBoxItem"/> to a rotation angle based on its index.
        /// </summary>
        /// <param name="value">The <see cref="ListBoxItem"/>.</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>
        /// The rotation angle in degrees.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListBoxItem listBoxItem = (ListBoxItem)value;
            ListBox listBox = ItemsControl.ItemsControlFromItemContainer(listBoxItem) as ListBox;
            string paramValue = parameter.ToString();
            int index = listBox.ItemContainerGenerator.IndexFromContainer(listBoxItem);

            switch (paramValue)
            {
                case "Top":
                    return (double)index * (double)80;
                case "Left":
                    return (double)index * (double)80;
                case "ZIndex":
                    return index;
                case "Rotate":
                    return this.GetRotationAngle(index);
            }

            return value;
        }

        /// <summary>
        /// The method is not used.
        /// </summary>
        /// <param name="value">The parameter is not used.</param>
        /// <param name="targetType">The parameter is not used.</param>
        /// <param name="parameter">The parameter is not used.</param>
        /// <param name="culture">The parameter is not used.</param>
        /// <returns>
        /// Never returns.
        /// </returns>
        /// <exception cref="System.NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Cannot convert back.");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the rotation angle based on the index of an item.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The rotation angle in degrees.</returns>
        private int GetRotationAngle(int index)
        {
            if (index == 0)
            {
                return -5;
            }

            if (index % 3 == 0)
            {
                return -12;
            }

            if (index % 2 == 0)
            {
                return 10;
            }

            if (index % 1 == 0)
            {
                return 6;
            }
            else
            { 
                return 3;
            }
        }

        #endregion
    }
}
