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
        /// Confirms that the collection parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="value">The collection value.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value contains the unexpected value.</exception>
        public static Parameter<IEnumerable<T>> DoesNotContain<T>(IEnumerable<T> value, T unexpectedValue, string parameterName)
        {
            return new Parameter<IEnumerable<T>>(value, parameterName).DoesNotContain(unexpectedValue);
        }

        /// <summary>
        /// Confirms that the collection parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="value">The collection value.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value contains the unexpected value.</exception>
        public static Parameter<IEnumerable<T>> DoesNotContain<T>(IEnumerable<T> value, T unexpectedValue, string parameterName, string exceptionMessage)
        {
            return new Parameter<IEnumerable<T>>(value, parameterName).DoesNotContain(unexpectedValue, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the collection parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="parameter">The collection parameter.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <returns>
        /// The input collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value contains the unexpected value.</exception>
        public static Parameter<IEnumerable<T>> DoesNotContain<T>(this Parameter<IEnumerable<T>> parameter, T unexpectedValue)
        {
            return DoesNotContain(parameter, unexpectedValue, null);
        }

        /// <summary>
        /// Confirms that the collection parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the parameter collection.</typeparam>
        /// <param name="parameter">The collection parameter.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter value contains the unexpected value.</exception>
        public static Parameter<IEnumerable<T>> DoesNotContain<T>(this Parameter<IEnumerable<T>> parameter, T unexpectedValue, string exceptionMessage)
        {
            parameter = parameter.IsNotNull(exceptionMessage);

            if (!parameter.Value.Contains(unexpectedValue))
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "Collection contains an unexpected value. Value:<{0}>.",
                    unexpectedValue);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
