namespace Framework
{
    using System;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the parameter value is an instance of the specified type, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter is not an instance of the specified type.</exception>
        public static Parameter<T> IsInstanceOfType<T>(T value, Type type, string parameterName)
        {
            return new Parameter<T>(value, parameterName).IsInstanceOfType<T>(type);
        }

        /// <summary>
        /// Confirms that the parameter value is an instance of the specified type, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter is not an instance of the specified type.</exception>
        public static Parameter<T> IsInstanceOfType<T>(T value, Type type, string parameterName, string exceptionMessage)
        {
            return new Parameter<T>(value, parameterName).IsInstanceOfType<T>(type, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the parameter value is an instance of the specified type, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter is not an instance of the specified type.</exception>
        public static Parameter<T> IsInstanceOfType<T>(this Parameter<T> parameter, Type type)
        {
            return IsInstanceOfType(parameter, type, null);
        }

        /// <summary>
        /// Confirms that the parameter value is an instance of the specified type, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="type">The type.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the parameter is not an instance of the specified type.</exception>
        public static Parameter<T> IsInstanceOfType<T>(this Parameter<T> parameter, Type type, string exceptionMessage)
        {
            parameter = parameter.IsNotNull(exceptionMessage);

            if (type.IsInstanceOfType(parameter.Value))
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format(
                    "Value is not an expected type. Expected:<{0}> Actual:<{1}>.",
                    type.FullName,
                    typeof(T).FullName);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
