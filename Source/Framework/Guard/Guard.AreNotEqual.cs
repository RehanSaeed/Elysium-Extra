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
        /// Confirms that the parameter value and expected value are not equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="unexpected">The unexpected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was equal to the expected value.</exception>
        public static Parameter<T> AreNotEqual<T>(T value, T unexpected, string parameterName)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).AreNotEqual<T>(unexpected);
        }

        /// <summary>
        /// Confirms that the parameter value and expected value are not equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="unexpected">The unexpected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was equal to the expected value.</exception>
        public static Parameter<T> AreNotEqual<T>(T value, T unexpected, string parameterName, string exceptionMessage)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).AreNotEqual<T>(unexpected, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the parameter value and expected value are not equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="unexpected">The unexpected value.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was equal to the expected value.</exception>
        public static Parameter<T> AreNotEqual<T>(this Parameter<T> parameter, T unexpected)
            where T : IComparable<T>
        {
            return AreNotEqual(parameter, unexpected, null);
        }

        /// <summary>
        /// Confirms that the parameter value and expected value are not equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="unexpected">The unexpected value.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was equal to the expected value.</exception>
        public static Parameter<T> AreNotEqual<T>(this Parameter<T> parameter, T unexpected, string exceptionMessage) 
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(parameter.Value, unexpected) != 0)
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "Value is equal to an ununexpected value. Value:<{0}>.",
                    unexpected);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
