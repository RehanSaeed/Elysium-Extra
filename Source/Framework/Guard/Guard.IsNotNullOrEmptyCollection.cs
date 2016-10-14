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
        /// Confirms that the collection parameter value is not null or empty, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection parameter.</typeparam>
        /// <param name="value">The collection parameter value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// The collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter is empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the collection parameter is null.</exception>
        public static Parameter<IEnumerable<T>> IsNotNullOrEmpty<T>(IEnumerable<T> value, string parameterName)
        {
            return new Parameter<IEnumerable<T>>(value, parameterName).IsNotNullOrEmpty();
        }

        /// <summary>
        /// Confirms that the collection parameter value is not null or empty, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection parameter.</typeparam>
        /// <param name="value">The collection parameter value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter is empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the collection parameter is null.</exception>
        public static Parameter<IEnumerable<T>> IsNotNullOrEmpty<T>(IEnumerable<T> value, string parameterName, string exceptionMessage)
        {
            return new Parameter<IEnumerable<T>>(value, parameterName).IsNotNullOrEmpty(exceptionMessage);
        }

        /// <summary>
        /// Confirms that the collection parameter value is not null or empty, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection parameter.</typeparam>
        /// <param name="parameter">The collection parameter.</param>
        /// <returns>
        /// The input collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter is empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the collection parameter is null.</exception>
        public static Parameter<IEnumerable<T>> IsNotNullOrEmpty<T>(this Parameter<IEnumerable<T>> parameter)
        {
            return IsNotNullOrEmpty(parameter, null);
        }

        /// <summary>
        /// Confirms that the collection parameter value is not null or empty, otherwise throws an exception.
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection parameter.</typeparam>
        /// <param name="parameter">The collection parameter.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <returns>
        /// The input collection parameter.
        /// </returns>
        /// <exception cref="System.ArgumentException">Thrown if the collection parameter is empty.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown if the collection parameter is null.</exception>
        public static Parameter<IEnumerable<T>> IsNotNullOrEmpty<T>(this Parameter<IEnumerable<T>> parameter, string exceptionMessage)
        {
            parameter = parameter.IsNotNull(exceptionMessage);

            if (parameter.Value.Count() > 0)
            {
                return parameter;
            }

            if (string.IsNullOrWhiteSpace(exceptionMessage))
            {
                exceptionMessage = "Collection is empty.";
            }

            throw new ArgumentException(exceptionMessage, parameter.ParameterName);
        }
    }
}
