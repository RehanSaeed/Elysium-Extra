namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The integer column for the <see cref="DataGrid."/>
    /// </summary>
    public class DataGridIntegerColumn : DataGridNumberColumn<DataGridIntegerColumn, IntegerUpDown, int>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DataGridIntegerColumn"/> class.
        /// </summary>
        static DataGridIntegerColumn()
        {
            DataGridIntegerColumn.UpdateMetadata(1, int.MinValue, int.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridIntegerColumn"/> class.
        /// </summary>
        public DataGridIntegerColumn()
        {
            this.EditingElementStyle = (Style)Application.Current.FindResource("DataGridIntegerUpDownColumnEditingElementStyle");
            this.ElementStyle = (Style)Application.Current.FindResource("DataGridIntegerUpDownColumnElementStyle");
        } 

        #endregion
    }
}
