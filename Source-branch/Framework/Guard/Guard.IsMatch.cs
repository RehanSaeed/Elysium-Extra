namespace Framework
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(string value, string pattern, string parameterName)
        {
            return new Parameter<string>(value, parameterName).IsMatch(pattern);
        }

        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="regexOptions">The regex options.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(string value, string pattern, RegexOptions regexOptions, string parameterName)
        {
            return new Parameter<string>(value, parameterName).IsMatch(pattern, regexOptions);
        }

        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(string value, string pattern, string parameterName, string exceptionMessage)
        {
            return new Parameter<string>(value, parameterName).IsMatch(pattern, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="regexOptions">The regex options.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(string value, string pattern, RegexOptions regexOptions, string parameterName, string exceptionMessage)
        {
            return new Parameter<string>(value, parameterName).IsMatch(pattern, regexOptions, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="pattern">The pattern.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(this Parameter<string> parameter, string pattern)
        {
            return IsMatch(parameter, pattern, null);
        }

        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="regexOptions">The regex options.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(this Parameter<string> parameter, string pattern, RegexOptions regexOptions)
        {
            return IsMatch(parameter, pattern, regexOptions, null);
        }

        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(this Parameter<string> parameter, string pattern, string exceptionMessage)
        {
            return IsMatch(parameter, pattern, RegexOptions.None, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the string parameter value matches the specified regular expression pattern, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="regexOptions">The regex options.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter value does not match the regular expression pattern.</exception>
        public static Parameter<string> IsMatch(this Parameter<string> parameter, string pattern, RegexOptions regexOptions, string exceptionMessage)
        {
            parameter = parameter.IsNotNull(exceptionMessage);

            if (Regex.IsMatch(parameter.Value, pattern, regexOptions))
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "String does not match the expected pattern. Pattern:<{0}> Actual:<{1}>.",
                    pattern,
                    parameter.Value);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
