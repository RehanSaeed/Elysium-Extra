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
        /// Confirms that the parameter value is more than the minimum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is less than or equal to the minimum value.</exception>
        public static Parameter<T> IsMoreThan<T>(T value, T minimum, string parameterName)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).IsMoreThan<T>(minimum);
        }

        /// <summary>
        /// Confirms that the parameter value is more than the minimum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is less than or equal to the minimum value.</exception>
        public static Parameter<T> IsMoreThan<T>(T value, T minimum, string parameterName, string exceptionMessage)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).IsMoreThan<T>(minimum, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the parameter value is more than the minimum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="minimum">The minimum.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is less than or equal to the minimum value.</exception>
        public static Parameter<T> IsMoreThan<T>(this Parameter<T> parameter, T minimum)
            where T : IComparable<T>
        {
            return IsMoreThan(parameter, minimum, null);
        }

        /// <summary>
        /// Confirms that the parameter value is more than the minimum value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if the parameter value is less than or equal to the minimum value.</exception>
        public static Parameter<T> IsMoreThan<T>(this Parameter<T> parameter, T minimum, string exceptionMessage) 
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(parameter.Value, minimum) > 0)
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "Value is less than or equal to the minimum value. Value:<{0}> Minimum:<{1}>.",
                    parameter.Value,
                    minimum);
            }

            throw new ArgumentOutOfRangeException(parameter.ParameterName, exceptionMessage);
        }
    }
}
