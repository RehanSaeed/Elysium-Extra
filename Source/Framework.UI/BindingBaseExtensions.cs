namespace Framework.UI
{
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// <see cref="BindingBase"/> extension methods.
    /// </summary>
    public static class BindingBaseExtensions
    {
        /// <summary>
        /// Resolves the specified binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <param name="dataContext">The data context.</param>
        /// <returns>The result of the resolved binding.</returns>
        public static object Resolve(this BindingBase binding, object dataContext)
        {
            ContentControl contentControl = new ContentControl();
            contentControl.DataContext = dataContext;
            contentControl.SetBinding(ContentControl.ContentProperty, binding);

            if (!string.IsNullOrEmpty(binding.StringFormat))
            {
                contentControl.Content = string.Format(binding.StringFormat, contentControl.Content);
            }

            return contentControl.Content;
        }

        /// <summary>
        /// Updates the specified binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <param name="dataContext">The data context.</param>
        /// <param name="value">The value.</param>
        public static void Update(this BindingBase binding, object dataContext, object value)
        {
            Binding b = binding as Binding;
            if (b != null && b.Mode != BindingMode.TwoWay)
            {
                b.Mode = BindingMode.TwoWay;
            }

            ContentControl contentControl = new ContentControl();
            contentControl.DataContext = dataContext;
            contentControl.SetBinding(ContentControl.ContentProperty, b);
            contentControl.Content = value;
        }
    }
}
