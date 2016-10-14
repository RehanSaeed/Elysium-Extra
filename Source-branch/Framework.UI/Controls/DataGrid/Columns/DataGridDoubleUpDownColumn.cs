namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The double column for the <see cref="DataGrid."/>
    /// </summary>
    public class DataGridDoubleColumn : DataGridNumberColumn<DataGridDoubleColumn, DoubleUpDown, double>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DataGridDoubleColumn"/> class.
        /// </summary>
        static DataGridDoubleColumn()
        {
            DataGridDoubleColumn.UpdateMetadata(1D, double.MinValue, double.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridDoubleColumn"/> class.
        /// </summary>
        public DataGridDoubleColumn()
        {
            this.EditingElementStyle = (Style)Application.Current.FindResource("DataGridDoubleUpDownColumnEditingElementStyle");
            this.ElementStyle = (Style)Application.Current.FindResource("DataGridDoubleUpDownColumnElementStyle");
        } 

        #endregion
    }
}
