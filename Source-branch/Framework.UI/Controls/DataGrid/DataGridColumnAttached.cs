namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// <see cref="DataGridColumn"/> attached properties
    /// </summary>
    public static class DataGridColumnAttached
    {
        #region Dependency Properties

        public static readonly DependencyProperty CanUserHideColumnProperty = DependencyProperty.RegisterAttached(
            "CanUserHideColumn", 
            typeof(bool), 
            typeof(DataGridColumnAttached), 
            new PropertyMetadata(true)); 

	    #endregion

        #region Public Static Methods

        public static bool GetCanUserHideColumn(DataGridColumn dataGridColumn)
        {
            return (bool)dataGridColumn.GetValue(CanUserHideColumnProperty);
        }

        public static void SetCanUserHideColumn(DataGridColumn dataGridColumn, bool value)
        {
            dataGridColumn.SetValue(CanUserHideColumnProperty, value);
        } 

	    #endregion
    }
}
