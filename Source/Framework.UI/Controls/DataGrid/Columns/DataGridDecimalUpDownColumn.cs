namespace Framework.UI.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The decimal column for the <see cref="DataGrid."/>
    /// </summary>
    public class DataGridDecimalColumn : DataGridNumberColumn<DataGridDecimalColumn, DecimalUpDown, decimal>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DataGridDecimalColumn"/> class.
        /// </summary>
        static DataGridDecimalColumn()
        {
            DataGridDecimalColumn.UpdateMetadata(1M, decimal.MinValue, decimal.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridDecimalColumn"/> class.
        /// </summary>
        public DataGridDecimalColumn()
        {
            this.EditingElementStyle = (Style)Application.Current.FindResource("DataGridDecimalUpDownColumnEditingElementStyle");
            this.ElementStyle = (Style)Application.Current.FindResource("DataGridDecimalUpDownColumnElementStyle");
        } 

        #endregion
    }
}
