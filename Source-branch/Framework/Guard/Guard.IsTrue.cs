namespace Framework
{
    using System;

    /// <summary>
    /// Determines the validity of an argument and throws Argument exceptions if invalid.
    /// </summary>
    public static partial class Guard
    {
        /// <summary>
        /// Confirms that the specified condition is <c>true</c>, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="booleanFunction">The boolean condition function.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the specified condition is <c>false</c>.</exception>
        public static Parameter<T> IsTrue<T>(T value, Func<T, bool> booleanFunction, string parameterName)
        {
            return new Parameter<T>(value, parameterName).IsTrue(booleanFunction);
        }

        /// <summary>
        /// Confirms that the specified condition is <c>true</c>, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter value.</param>
        /// <param name="booleanFunction">The boolean condition function.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the specified condition is <c>false</c>.</exception>
        public static Parameter<T> IsTrue<T>(T value, Func<T, bool> booleanFunction, string parameterName, string exceptionMessage)
        {
            return new Parameter<T>(value, parameterName).IsTrue(booleanFunction, exceptionMessage);
        }

        /// <summary>
        /// Confirms that the specified condition is <c>true</c>, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="booleanFunction">The boolean condition function.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the specified condition is <c>false</c>.</exception>
        public static Parameter<T> IsTrue<T>(this Parameter<T> parameter, Func<T, bool> booleanFunction)
        {
            return IsTrue<T>(parameter, booleanFunction, null);
        }

        /// <summary>
        /// Confirms that the specified condition is <c>true</c>, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="parameter">The parameter.</param>
        /// <param name="booleanFunction">The boolean condition function.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the specified condition is <c>false</c>.</exception>
        public static Parameter<T> IsTrue<T>(this Parameter<T> parameter, Func<T, bool> booleanFunction, string exceptionMessage)
        {
            if (booleanFunction(parameter.Value))
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = string.Format("Value is invalid. Value:<{0}>.", parameter.Value);
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
