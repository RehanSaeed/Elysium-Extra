using System.Windows;
namespace Framework.UI.Controls
{
    /// <summary>
    /// The text box column for the <see cref="DataGrid."/>
    /// </summary>
    public sealed class DataGridMultiLineTextColumn : System.Windows.Controls.DataGridTextColumn
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridMultiLineTextColumn"/> class.
        /// </summary>
        public DataGridMultiLineTextColumn()
        {
            this.EditingElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridMultiLineTextColumnEditingElementStyle");
            this.ElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridMultiLineTextColumnElementStyle");
        }
    }
}
