namespace Framework
{
    /// <summary>
    /// Metadata for a parameter argument passed to a method.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    public sealed class Parameter<T>
    {
        private string parameterName;
        private T value;

        /// <summary>
        /// Initialises a new instance of the <see cref="Parameter{T}"/> class.
        /// </summary>
        public Parameter()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Parameter{T}"/> class.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        public Parameter(T value, string parameterName)
        {
            this.value = value;
            this.parameterName = parameterName;
        }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>
        /// The name of the parameter.
        /// </value>
        public string ParameterName
        {
            get { return this.parameterName; }
            set { this.parameterName = value; }
        }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        /// <value>
        /// The parameter value.
        /// </value>
        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
