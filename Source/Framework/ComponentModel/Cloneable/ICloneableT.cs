namespace Framework
{
    using System;

    /// <summary>
    /// Produces deep clones of objects.
    /// </summary>
    /// <typeparam name="T">The type of the object to clone.</typeparam>
    public interface ICloneable<out T> //: ICloneable
    {
        /// <summary>
        /// Clones the clone-able object of type <typeparamref name="T"/>.
        /// </summary>
        /// <returns>
        /// The cloned object of type <typeparamref name="T"/>.
        /// </returns>
        T Clone();
    }
}
