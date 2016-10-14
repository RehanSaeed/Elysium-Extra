namespace Framework
{
    using System;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the string parameter value is not null, empty or white space, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The string parameter value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter is empty or white space.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the string parameter is null.</exception>
        public static Parameter<string> IsNotNullOrWhiteSpace(string value, string parameterName)
        {
            return new Parameter<string>(value, parameterName).IsNotNullOrWhiteSpace();
        }

        /// <summary>
        /// Confirms that the string parameter value is not null, empty or white space, otherwise throws an exception.
        /// </summary>
        /// <param name="value">The string parameter value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter is empty or white space.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the string parameter is null.</exception>
        public static Parameter<string> IsNotNullOrWhiteSpace(string value, string parameterName, string exceptionMessage)
        {
            return new Parameter<string>(value, parameterName).IsNotNullOrWhiteSpace(exceptionMessage);
        }

        /// <summary>
        /// Confirms that the string parameter value is not null, empty or white space, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The string parameter.</param>
        /// <returns>
        /// The input string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter is empty or white space.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the string parameter is null.</exception>
        public static Parameter<string> IsNotNullOrWhiteSpace(this Parameter<string> parameter)
        {
            return IsNotNullOrWhiteSpace(parameter, null);
        }

        /// <summary>
        /// Confirms that the string parameter value is not null, empty or white space, otherwise throws an exception.
        /// </summary>
        /// <param name="parameter">The string parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input string parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the string parameter is empty or white space.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the string parameter is null.</exception>
        public static Parameter<string> IsNotNullOrWhiteSpace(this Parameter<string> parameter, string exceptionMessage)
        {
            parameter = parameter.IsNotNull(exceptionMessage);

            if (!string.IsNullOrWhiteSpace(parameter.Value))
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format("String is empty or whitespace. Value:<{0}>.", parameter.Value);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
