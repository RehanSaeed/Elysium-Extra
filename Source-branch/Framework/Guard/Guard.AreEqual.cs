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
        /// Confirms that the parameter value and expected value are equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was not equal to the expected value.</exception>
        public static Parameter<T> AreEqual<T>(T value, T expected, string parameterName)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).AreEqual<T>(expected);
        }

        /// <summary>
        /// Confirms that the parameter value and expected value are equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was not equal to the expected value.</exception>
        public static Parameter<T> AreEqual<T>(T value, T expected, string parameterName, string exceptionMessage)
            where T : IComparable<T>
        {
            return new Parameter<T>(value, parameterName).AreEqual<T>(expected, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the parameter value and expected value are equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="expected">The expected value.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was not equal to the expected value.</exception>
        public static Parameter<T> AreEqual<T>(this Parameter<T> parameter, T expected)
            where T : IComparable<T>
        {
            return AreEqual(parameter, expected, null);
        }

        /// <summary>
        /// Confirms that the parameter value and expected value are equal, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="expected">The expected value.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>The input parameter.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter value was not equal to the expected value.</exception>
        public static Parameter<T> AreEqual<T>(this Parameter<T> parameter, T expected, string exceptionMessage) 
            where T : IComparable<T>
        {
            if (Comparer<T>.Default.Compare(parameter.Value, expected) == 0)
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "Value is not equal to the expected value. Expected:<{0}> Actual:<{1}>.",
                    expected,
                    parameter.Value);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
