namespace Framework
{
    using System;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the string parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value contains the unexpected value.</exception>
        public static Parameter<string> DoesNotContain(string value, string unexpectedValue, string parameterName)
        {
            return new Parameter<string>(value, parameterName).DoesNotContain(unexpectedValue);
        }

        /// <summary>
        /// Confirms that the string parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value contains the unexpected value.</exception>
        public static Parameter<string> DoesNotContain(string value, string unexpectedValue, string parameterName, string exceptionMessage)
        {
            return new Parameter<string>(value, parameterName).DoesNotContain(unexpectedValue, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the string parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The string parameter.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <returns>
        /// The input string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value contains the unexpected value.</exception>
        public static Parameter<string> DoesNotContain(this Parameter<string> parameter, string unexpectedValue)
        {
            return DoesNotContain(parameter, unexpectedValue, null);
        }

        /// <summary>
        /// Confirms that the string parameter value does not contain the unexpected value, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The string parameter.</param>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value contains the unexpected value.</exception>
        public static Parameter<string> DoesNotContain(this Parameter<string> parameter, string unexpectedValue, string exceptionMessage)
        {
            parameter = parameter.IsNotNull(exceptionMessage);

            if (!parameter.Value.Contains(unexpectedValue))
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "String contains an unexpected value. Value:<{0}>.",
                    unexpectedValue);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
