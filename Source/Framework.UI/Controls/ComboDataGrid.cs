namespace Framework.UI.Controls
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// A combo box which displays a data grid in the popup.
    /// </summary>
    public sealed class ComboDataGrid : ComboBox
    {
        private readonly ObservableCollection<DataGridColumn> columns;
        private DataGrid dataGrid;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ComboDataGrid"/> class.
        /// </summary>
        public ComboDataGrid()
        {
            this.columns = new ObservableCollection<DataGridColumn>();
            this.columns.CollectionChanged += this.OnColumnsCollectionChanged;
        } 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the data grid columns.
        /// </summary>
        /// <value>
        /// The data grid columns.
        /// </value>
        public ObservableCollection<DataGridColumn> Columns
        {
            get { return this.columns; }
        } 

        #endregion

        #region Public Methods

        /// <summary>
        /// Called when <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" /> is called.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.dataGrid = (DataGrid)this.GetTemplateChild("PART_DataGrid");
            this.dataGrid.MouseLeftButtonUp += this.OnDataGridMouseLeftButtonUp;
            this.UpdateDataGridColumns();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when the columns collection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.dataGrid != null)
            {
                this.UpdateDataGridColumns();
            }
        }

        /// <summary>
        /// Called when the left mouse button up event on the data grid is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void OnDataGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.IsDropDownOpen = false;
        } 

        /// <summary>
        /// Updates the data grid columns.
        /// </summary>
        private void UpdateDataGridColumns()
        {
            this.dataGrid.Columns.Clear();
            foreach (DataGridColumn column in this.Columns)
            {
                column.IsReadOnly = true;
                this.dataGrid.Columns.Add(column);
            }
        } 

        #endregion
    }
}
