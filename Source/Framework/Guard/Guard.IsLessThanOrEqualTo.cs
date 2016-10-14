namespace Framework
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the parameter value is less than or equal to the maximum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is more than the maximum value.</exception>
        public static Parameter<T> IsLessThanOrEqualTo<T>(T value, T maximum, string parameterName)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).IsLessThanOrEqualTo<T>(maximum);
        }

        /// <summary>
        /// Confirms that the parameter value is less than or equal to the maximum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is more than the maximum value.</exception>
        public static Parameter<T> IsLessThanOrEqualTo<T>(T value, T maximum, string parameterName, string exceptionMessage)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).IsLessThanOrEqualTo<T>(maximum, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the parameter value is less than or equal to the maximum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is more than the maximum value.</exception>
        public static Parameter<T> IsLessThanOrEqualTo<T>(this Parameter<T> parameter, T maximum)
            where T : IComparable<T>
        {
            return IsLessThanOrEqualTo(parameter, maximum, null);
        }

        /// <summary>
        /// Confirms that the parameter value is less than or equal to the maximum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="maximum">The maximum.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is more than the maximum value.</exception>
        public static Parameter<T> IsLessThanOrEqualTo<T>(this Parameter<T> parameter, T maximum, string exceptionMessage) 
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(parameter.Value, maximum) <= 0)
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "Value is more than the maximum value. Value:<{0}> Maximum:<{1}>.",
                    parameter.Value,
                    maximum);
            }

            throw new ArgumentOutOfRangeException(parameter.ParameterName, exceptionMessage);
        }
    }
}
