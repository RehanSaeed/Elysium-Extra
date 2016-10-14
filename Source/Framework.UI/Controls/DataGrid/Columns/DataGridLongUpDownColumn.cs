namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The integer column for the <see cref="DataGrid."/>
    /// </summary>
    public class DataGridLongColumn : DataGridNumberColumn<DataGridLongColumn, LongUpDown, long>
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DataGridLongColumn"/> class.
        /// </summary>
        static DataGridLongColumn()
        {
            DataGridLongColumn.UpdateMetadata(1L, long.MinValue, long.MaxValue);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridLongColumn"/> class.
        /// </summary>
        public DataGridLongColumn()
        {
            this.EditingElementStyle = (Style)Application.Current.FindResource("DataGridLongUpDownColumnEditingElementStyle");
            this.ElementStyle = (Style)Application.Current.FindResource("DataGridLongUpDownColumnElementStyle");
        } 

        #endregion
    }
}
