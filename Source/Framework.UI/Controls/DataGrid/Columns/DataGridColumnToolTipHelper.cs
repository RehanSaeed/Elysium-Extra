namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Helps to set tooltips for data grid columns.
    /// </summary>
    internal static class DataGridColumnToolTipHelper
    {
        /// <summary>
        /// Sets the given tool tip on the specified <see cref="FrameworkElement" />.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <param name="tooltip">The tooltip binding.</param>
        /// <param name="toolTipTemplate">The tool tip template.</param>
        /// <param name="dataItem">The data context.</param>
        /// <param name="textPath">The text path.</param>
        public static void SetToolTip(
            FrameworkElement frameworkElement, 
            BindingBase tooltip, 
            DataTemplate toolTipTemplate, 
            object dataItem,
            string textPath = "Text")
        {
            if (tooltip == null)
            {
                if (textPath != null)
                {
                    Style style = new Style(typeof(ToolTip));

                    style.Setters.Add(new Setter(ToolTip.ContentProperty, new Binding(textPath)));
                    DataTrigger nullTrigger = new DataTrigger()
                    {
                        Binding = new Binding(textPath),
                        Value = null,
                    };
                    nullTrigger.Setters.Add(new Setter(ToolTip.VisibilityProperty, Visibility.Collapsed));
                    style.Triggers.Add(nullTrigger);

                    DataTrigger emptyTrigger = new DataTrigger()
                    {
                        Binding = new Binding(textPath),
                        Value = string.Empty,
                    };
                    emptyTrigger.Setters.Add(new Setter(ToolTip.VisibilityProperty, Visibility.Collapsed));
                    style.Triggers.Add(emptyTrigger);

                    ToolTip toolTipElement = new ToolTip()
                    {
                        DataContext = frameworkElement,
                        Style = style
                    };

                    frameworkElement.ToolTip = toolTipElement;
                }
            }
            else
            {
                ContentControl contentControl = new ContentControl()
                {
                    DataContext = dataItem,
                    ContentTemplate = toolTipTemplate
                };
                BindingOperations.SetBinding(
                    contentControl, 
                    ContentControl.ContentProperty, 
                    tooltip);
                frameworkElement.ToolTip = contentControl;
            }
        }
    }
}
