namespace Framework.UI.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// The less than equal to boolean converter.
    /// </summary>
    public sealed class LessThanEqualToBoolConverter : IValueConverter
    {
        #region Public Methods

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IComparable comparable1 = (IComparable)value;

            if (comparable1 == null)
            {
                return false;
            }

            IComparable comparable2 = (IComparable)System.Convert.ChangeType(parameter, comparable1.GetType(), culture);

            if (comparable1.CompareTo(comparable2) <= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The convert back.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The <see cref="object"/>. </returns>
        /// <exception cref="NotImplementedException">Any Exception </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        } 

        #endregion
    }
}
