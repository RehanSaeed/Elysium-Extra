namespace Framework
{
    using System;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the parameter value is not null, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the parameter is null.</exception>
        public static Parameter<T> IsNotNull<T>(T value, string parameterName)
        {
            return new Parameter<T>(value, parameterName).IsNotNull<T>();
        }

        /// <summary>
        /// Confirms that the parameter value is not null, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the parameter is null.</exception>
        public static Parameter<T> IsNotNull<T>(T value, string parameterName, string exceptionMessage)
        {
            return new Parameter<T>(value, parameterName).IsNotNull<T>(exceptionMessage);
        }

        /// <summary>
        /// Confirms that the parameter value is not null, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the parameter is null.</exception>
        public static Parameter<T> IsNotNull<T>(this Parameter<T> parameter)
        {
            return IsNotNull(parameter, null);
        }

        /// <summary>
        /// Confirms that the parameter value is not null, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown if the parameter is null.</exception>
        public static Parameter<T> IsNotNull<T>(this Parameter<T> parameter, string exceptionMessage)
        {
            if (parameter.Value != null)
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = "Value is null.";
            }

            throw new ArgumentNullException(parameter.ParameterName, exceptionMessage);
        }
    }
}
