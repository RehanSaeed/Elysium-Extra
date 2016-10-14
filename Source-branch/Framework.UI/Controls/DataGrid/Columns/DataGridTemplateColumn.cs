namespace Framework.UI.Controls
{
    using System.Windows;

    /// <summary>
    /// The template column for the <see cref="DataGrid."/>
    /// </summary>
    public class DataGridTemplateColumn : System.Windows.Controls.DataGridTemplateColumn
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridTemplateColumn"/> class.
        /// </summary>
        public DataGridTemplateColumn()
        {
            this.CellStyle = (System.Windows.Style)Application.Current.FindResource("DataGridTemplateColumnCellStyle");
        } 

        #endregion
    }
}
