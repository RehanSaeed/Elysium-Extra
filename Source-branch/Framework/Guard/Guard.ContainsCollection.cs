namespace Framework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the collection parameter value contains the expected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="value">The collection parameter value.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value does not contain the expected value.</exception>
        public static Parameter<IEnumerable<T>> Contains<T>(IEnumerable<T> value, T expectedValue, string parameterName)
        {
            return new Parameter<IEnumerable<T>>(value, parameterName).Contains(expectedValue);
        }

        /// <summary>
        /// Confirms that the collection parameter value contains the expected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="value">The collection parameter value.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value does not contain the expected value.</exception>
        public static Parameter<IEnumerable<T>> Contains<T>(IEnumerable<T> value, T expectedValue, string parameterName, string exceptionMessage)
        {
            return new Parameter<IEnumerable<T>>(value, parameterName).Contains(expectedValue, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the collection parameter value contains the expected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="parameter">The collection parameter.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <returns>
        /// The input collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value does not contain the expected value.</exception>
        public static Parameter<IEnumerable<T>> Contains<T>(this Parameter<IEnumerable<T>> parameter, T expectedValue)
        {
            return Contains(parameter, expectedValue, null);
        }

        /// <summary>
        /// Confirms that the collection parameter value contains the expected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="parameter">The collection parameter.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value does not contain the expected value.</exception>
        public static Parameter<IEnumerable<T>> Contains<T>(this Parameter<IEnumerable<T>> parameter, T expectedValue, string exceptionMessage)
        {
            parameter = parameter.IsNotNull(exceptionMessage);

            if (parameter.Value.Contains(expectedValue))
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "Collection does not contain the expected value. Expected:<{0}>.",
                    expectedValue);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
