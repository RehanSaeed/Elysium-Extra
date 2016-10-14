namespace Framework.UI.Controls
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;

    /// <summary>
    /// <see cref="DataGrid"/> attached properties.
    /// </summary>
    public static class DataGridAttached
    {
        #region Dependency Properties

        public static readonly DependencyProperty CanUserHideColumnsProperty = DependencyProperty.RegisterAttached(
            "CanUserHideColumns", 
            typeof(bool), 
            typeof(DataGridAttached), 
            new PropertyMetadata(true));

        public static readonly DependencyProperty ColumnSettingsProperty = DependencyProperty.RegisterAttached(
            "ColumnSettings",
            typeof(string),
            typeof(DataGridAttached),
            new PropertyMetadata(null, OnColumnSettingsPropertyChanged));

        public static readonly DependencyProperty IsColumnSettingsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsColumnSettingsEnabled",
            typeof(bool),
            typeof(DataGridAttached),
            new PropertyMetadata(false, OnIsColumnSettingsEnabledPropertyChanged));

        public static readonly DependencyProperty IsDeselectionEnabledProperty = DependencyProperty.RegisterAttached(
            "IsDeselectionEnabled",
            typeof(bool),
            typeof(DataGridAttached),
            new PropertyMetadata(false, OnIsDeselectionEnabledChanged));

        private static readonly DependencyPropertyKey IsUpdatingColumnSettingsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsUpdatingColumnSettings",
            typeof(bool),
            typeof(DataGridAttached),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsUpdatingColumnSettingsProperty = IsUpdatingColumnSettingsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty GroupSummaryProperty = DependencyProperty.RegisterAttached(
            "GroupSummary",
            typeof(DataGridGroupSummaryCollection),
            typeof(DataGridAttached),
            new PropertyMetadata(null, OnGroupSummaryPropertyChanged));

        private static readonly DependencyPropertyKey GroupSummaryInternalPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "GroupSummaryInternal",
            typeof(DataGridGroupSummaryCollection),
            typeof(DataGridAttached),
            new PropertyMetadata(null));

        public static readonly DependencyProperty GroupSummaryInternalProperty = GroupSummaryInternalPropertyKey.DependencyProperty;

        public static readonly DependencyProperty IsSingleClickEditProperty = DependencyProperty.RegisterAttached(
            "IsSingleClickEdit",
            typeof(bool),
            typeof(DataGridAttached),
            new PropertyMetadata(true, OnIsSingleClickEditPropertyChanged));

        #endregion

        #region Public Static Methods

        public static string GetColumnSettings(DataGrid dataGrid)
        {
            return (string)dataGrid.GetValue(ColumnSettingsProperty);
        }

        public static void SetColumnSettings(DataGrid dataGrid, string value)
        {
            dataGrid.SetValue(ColumnSettingsProperty, value);
        }

        public static bool GetIsColumnSettingsEnabled(DataGrid dataGrid)
        {
            return (bool)dataGrid.GetValue(IsColumnSettingsEnabledProperty);
        }

        public static void SetIsColumnSettingsEnabled(DataGrid dataGrid, bool value)
        {
            dataGrid.SetValue(IsColumnSettingsEnabledProperty, value);
        }

        public static bool GetIsUpdatingColumnSettings(DataGrid dataGrid)
        {
            return (bool)dataGrid.GetValue(IsUpdatingColumnSettingsProperty);
        }

        private static void SetIsUpdatingColumnSettings(DataGrid dataGrid, bool value)
        {
            dataGrid.SetValue(IsUpdatingColumnSettingsPropertyKey, value);
        }

        /// <summary>
        /// Gets whether the user can hide columns.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <returns><c>true</c> if the user can hide columns, otherwise <c>false</c>.</returns>
        public static bool GetCanUserHideColumns(DataGrid dataGrid)
        {
            return (bool)dataGrid.GetValue(CanUserHideColumnsProperty);
        }

        /// <summary>
        /// Sets whether the user can hide columns.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="value">if set to <c>true</c> the user can hide columns.</param>
        public static void SetCanUserHideColumns(DataGrid dataGrid, bool value)
        {
            dataGrid.SetValue(CanUserHideColumnsProperty, value);
        }

        /// <summary>
        /// Gets the group summary items.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <returns>The group summary items</returns>
        public static DataGridGroupSummaryCollection GetGroupSummary(DataGrid dataGrid)
        {
            return (DataGridGroupSummaryCollection)dataGrid.GetValue(GroupSummaryProperty);
        }

        /// <summary>
        /// Sets the group summary items.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="value">The value.</param>
        public static void SetGroupSummary(DataGrid dataGrid, DataGridGroupSummaryCollection value)
        {
            dataGrid.SetValue(GroupSummaryProperty, value);
        }

        /// <summary>
        /// Gets the deselection enabled property. If enabled, and the white space on the grid is clicked, all rows are deselected.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <returns><c>true</c> if deselecting all rows when white space is clicked, otherwise <c>false</c>.</returns>
        public static bool GetIsDeselectionEnabled(DataGrid dataGrid)
        {
            return (bool)dataGrid.GetValue(IsDeselectionEnabledProperty);
        }

        /// <summary>
        /// Sets the deselection enabled property. If enabled, and the white space on the grid is clicked, all rows are deselected.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="value">if set to <c>true</c> deselect all rows when white space is clicked.</param>
        public static void SetIsDeselectionEnabled(DataGrid dataGrid, bool value)
        {
            dataGrid.SetValue(IsDeselectionEnabledProperty, value);
        }

        /// <summary>
        /// Gets the edit cells with a single click mode. 
        /// By default a cell has to be double clicked to enter editing mode, this property changes this to be a single click.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <returns><c>true</c> if single click mode, otherwise <c>false</c>.</returns>
        public static bool GetIsSingleClickEdit(DataGrid dataGrid)
        {
            return (bool)dataGrid.GetValue(IsSingleClickEditProperty);
        }

        /// <summary>
        /// Sets the edit cells with a single click mode. 
        /// By default a cell has to be double clicked to enter editing mode, this property changes this to be a single click.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="value">if set to <c>true</c> edit cells with a single click.</param>
        public static void SetIsSingleClickEdit(DataGrid dataGrid, bool value)
        {
            dataGrid.SetValue(IsSingleClickEditProperty, value);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Gets the internal group summary items, representing all columns.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <returns>The internal group summary items.</returns>
        private static DataGridGroupSummaryCollection GetGroupSummaryInternal(DataGrid dataGrid)
        {
            return (DataGridGroupSummaryCollection)dataGrid.GetValue(GroupSummaryInternalProperty);
        }

        /// <summary>
        /// Sets the internal group summary representing all columns.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        /// <param name="value">The value.</param>
        private static void SetGroupSummaryInternal(DataGrid dataGrid, DataGridGroupSummaryCollection value)
        {
            dataGrid.SetValue(GroupSummaryInternalPropertyKey, value);
        }

        /// <summary>
        /// Called when the group summary property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnGroupSummaryPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)dependencyObject;
            DataGridGroupSummaryCollection groupSummary = GetGroupSummary(dataGrid);

            DataGridGroupSummaryCollection groupSummaryInternal = new DataGridGroupSummaryCollection();

            foreach (DataGridColumn dataGridColumn in dataGrid.Columns)
            {
                DataGridGroupSummary item = groupSummary.FirstOrDefault(x => x.Column == dataGridColumn);
                if (item == null)
                {
                    groupSummaryInternal.Add(
                        new DataGridGroupSummary()
                        {
                            Column = dataGridColumn,
                            Template = new DataTemplate()
                        });
                }
                else
                {
                    groupSummaryInternal.Add(
                        new DataGridGroupSummary()
                        {
                            Column = dataGridColumn,
                            Template = item.Template
                        });
                }
            }

            SetGroupSummaryInternal(dataGrid, groupSummaryInternal);
        }

        /// <summary>
        /// Called when the deselection enabled property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsDeselectionEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)dependencyObject;
            if (GetIsDeselectionEnabled(dataGrid))
            {
                dataGrid.MouseLeftButtonDown += OnDataGridMouseLeftButtonDown;
            }
            else
            {
                dataGrid.MouseLeftButtonDown -= OnDataGridMouseLeftButtonDown;
            }
        }

        /// <summary>
        /// Called when the data grid mouse left button is down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if ((dataGrid != null) && (dataGrid.SelectedItems != null) && (dataGrid.SelectedItems.Count > 0))
            {
                foreach (object selectedItem in dataGrid.SelectedItems.Cast<object>().ToList())
                {
                    DataGridRow dataGridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromItem(selectedItem);
                    if ((dataGridRow != null) && !dataGridRow.IsMouseOver)
                    {
                        dataGridRow.IsSelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// Called when the edit cells with a single click mode property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsSingleClickEditPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)dependencyObject;
            if (GetIsSingleClickEdit(dataGrid))
            {
                dataGrid.AddHandler(DataGridCell.GotFocusEvent, new RoutedEventHandler(OnDataGridCellGotFocus));
            }
            else
            {
                dataGrid.RemoveHandler(DataGridCell.GotFocusEvent, new RoutedEventHandler(OnDataGridCellGotFocus));
            }
        }

        /// <summary>
        /// Called when a data grid cell got focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridCellGotFocus(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                DataGrid dataGrid = (DataGrid)sender;
                dataGrid.BeginEdit(e);

                Control control = VisualTreeHelpers.FindChild<Control>(e.OriginalSource as DataGridCell);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }

        /// <summary>
        /// Called when the column settings enabled property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsColumnSettingsEnabledPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)dependencyObject;

            if (GetIsColumnSettingsEnabled(dataGrid))
            {
                dataGrid.ColumnReordered += OnDataGridColumnReordered;
                dataGrid.Loaded += OnDataGridLoaded;
                dataGrid.MouseLeftButtonUp += OnDataGridMouseLeftButtonUp;
                dataGrid.Columns.CollectionChanged += OnDataGridColumnsCollectionChanged;
            }
            else
            {
                dataGrid.ColumnReordered -= OnDataGridColumnReordered;
                dataGrid.Loaded -= OnDataGridLoaded;
                dataGrid.MouseLeftButtonUp -= OnDataGridMouseLeftButtonUp;
                dataGrid.Columns.CollectionChanged -= OnDataGridColumnsCollectionChanged;
            }
        }

        /// <summary>
        /// Called when the column settings property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnColumnSettingsPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)dependencyObject;
            UpdateColumnsFromColumnSettings(dataGrid);
        }

        /// <summary>
        /// Called when data grid columns are reordered
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridColumnEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridColumnReordered(object sender, DataGridColumnEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            UpdateColumnSettingsFromColumns(dataGrid);
        }

        /// <summary>
        /// Called when the data grid is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridLoaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            UpdateColumnSettingsFromColumns(dataGrid);
        }

        /// <summary>
        /// Called when the data grid mouse left button up is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            UpdateColumnSettingsFromColumns(dataGrid);
        }

        /// <summary>
        /// Called when the data grid columns collection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ICollection<DataGridColumn> dataGridColumns = (ICollection<DataGridColumn>)sender;
            if (dataGridColumns.Count > 0)
            {
                DataGridColumn column = dataGridColumns.First();
                DataGrid dataGrid = (DataGrid)column.GetLogicalParent();
                if (dataGrid != null)
                {
                    UpdateColumnSettingsFromColumns(dataGrid);
                }
            }
        }

        /// <summary>
        /// Updates the column settings from columns.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        private static void UpdateColumnSettingsFromColumns(DataGrid dataGrid)
        {
            if (!GetIsUpdatingColumnSettings(dataGrid))
            {
                SetIsUpdatingColumnSettings(dataGrid, true);

                ArrayList columnInfos = new ArrayList(dataGrid.Columns.Select(x => new DataGridColumnInfo(x)).ToArray());
                string columnSettings = XamlWriter.Save(columnInfos);
                SetColumnSettings(dataGrid, columnSettings);

                SetIsUpdatingColumnSettings(dataGrid, false);
            }
        }

        /// <summary>
        /// Updates the columns from column settings.
        /// </summary>
        /// <param name="dataGrid">The data grid.</param>
        private static void UpdateColumnsFromColumnSettings(DataGrid dataGrid)
        {
            if (!GetIsUpdatingColumnSettings(dataGrid))
            {
                SetIsUpdatingColumnSettings(dataGrid, true);

                string columnSettings = GetColumnSettings(dataGrid);
                ArrayList columnInfos = (ArrayList)XamlReader.Parse(columnSettings);
                foreach (DataGridColumnInfo columnInfo in columnInfos)
                {
                    DataGridColumn column = dataGrid.Columns.FirstOrDefault(
                        x => string.Equals(columnInfo.Name, AutomationProperties.GetName(x), System.StringComparison.Ordinal));
                    columnInfo.Apply(column, dataGrid.Columns.Count);
                }

                SetIsUpdatingColumnSettings(dataGrid, false);
            }
        }

        #endregion
    }
}
