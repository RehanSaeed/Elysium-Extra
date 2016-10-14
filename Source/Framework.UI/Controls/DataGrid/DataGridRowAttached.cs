namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// <see cref="DataGridRow"/> attached properties.
    /// </summary>
    public static class DataGridRowAttached
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsDeselectionEnabledProperty = DependencyProperty.RegisterAttached(
            "IsDeselectionEnabled",
            typeof(bool),
            typeof(DataGridRowAttached),
            new PropertyMetadata(false, OnIsDeselectionEnabledChanged));

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached(
            "IsReadOnly",
            typeof(bool),
            typeof(DataGridRowAttached),
            new PropertyMetadata(false, OnIsReadOnlyChanged));

        public static readonly DependencyProperty MoveAboveCommandProperty = DependencyProperty.RegisterAttached(
            "MoveAboveCommand",
            typeof(ICommand),
            typeof(DataGridRowAttached),
            new PropertyMetadata(null));

        public static readonly DependencyProperty MoveBelowCommandProperty = DependencyProperty.RegisterAttached(
            "MoveBelowCommand",
            typeof(ICommand),
            typeof(DataGridRowAttached),
            new PropertyMetadata(null));

        public static readonly DependencyProperty MoveDragContentTemplateProperty = DependencyProperty.RegisterAttached(
            "MoveDragContentTemplate",
            typeof(DataTemplate),
            typeof(DataGridRowAttached),
            new PropertyMetadata(null));

        public static readonly DependencyProperty MoveDragFormatProperty = DependencyProperty.RegisterAttached(
            "MoveDragFormat",
            typeof(string),
            typeof(DataGridRowAttached),
            new PropertyMetadata(null));

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the deselection enabled property. If enabled, and the row is clicked while selected, the row is deselected.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <returns><c>true</c> if deselecting row when selected and clicked, otherwise <c>false</c>.</returns>
        public static bool GetIsDeselectionEnabled(DataGridRow dataGridRow)
        {
            return (bool)dataGridRow.GetValue(IsDeselectionEnabledProperty);
        }

        /// <summary>
        /// Sets the deselection enabled property. If enabled, and the row is clicked while selected, the row is deselected.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="value">if set to <c>true</c> deselects the row when selected and clicked.</param>
        public static void SetIsDeselectionEnabled(DataGridRow dataGridRow, bool value)
        {
            dataGridRow.SetValue(IsDeselectionEnabledProperty, value);
        }

        /// <summary>
        /// Gets the is read only flag for the row.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <returns><c>true</c> if the row is read only, otherwise <c>false</c>.</returns>
        public static bool GetIsReadOnly(DataGridRow dataGridRow)
        {
            return (bool)dataGridRow.GetValue(IsReadOnlyProperty);
        }

        /// <summary>
        /// Sets the is read only flag for the row.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="value">if set to <c>true</c> the row is read only.</param>
        public static void SetIsReadOnly(DataGridRow dataGridRow, bool value)
        {
            dataGridRow.SetValue(IsReadOnlyProperty, value);
        }

        /// <summary>
        /// Gets the command used to move another row above this one using drag and drop.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <returns>The command to move a row above this instance.</returns>
        public static ICommand GetMoveAboveCommand(DataGridRow dataGridRow)
        {
            return (ICommand)dataGridRow.GetValue(MoveAboveCommandProperty);
        }

        /// <summary>
        /// Sets the command used to move another row above this one using drag and drop.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="command">The command to move a row above this instance.</param>
        public static void SetMoveAboveCommand(DataGridRow dataGridRow, ICommand command)
        {
            dataGridRow.SetValue(MoveAboveCommandProperty, command);
        }

        /// <summary>
        /// Gets the command used to move another row below this one using drag and drop.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <returns>The command to move a row below this instance.</returns>
        public static ICommand GetMoveBelowCommand(DataGridRow dataGridRow)
        {
            return (ICommand)dataGridRow.GetValue(MoveBelowCommandProperty);
        }

        /// <summary>
        /// Sets the command used to move another row below this one using drag and drop.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="command">The command to move a row below this instance.</param>
        public static void SetMoveBelowCommand(DataGridRow dataGridRow, ICommand command)
        {
            dataGridRow.SetValue(MoveBelowCommandProperty, command);
        }

        /// <summary>
        /// Gets the content template when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <returns>A data template used when this instance is being dragged above or below another row.</returns>
        public static DataTemplate GetMoveDragContentTemplate(DataGridRow dataGridRow)
        {
            return (DataTemplate)dataGridRow.GetValue(MoveDragContentTemplateProperty);
        }

        /// <summary>
        /// Sets the content template when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="value">A data template used when this instance is being dragged above or below another row.</param>
        public static void SetMoveDragContentTemplate(DataGridRow dataGridRow, DataTemplate value)
        {
            dataGridRow.SetValue(MoveDragContentTemplateProperty, value);
        }

        /// <summary>
        /// Gets the drag format when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <returns>The drag format used when this instance is being dragged above or below another row.</returns>
        public static string GetMoveDragFormat(DataGridRow dataGridRow)
        {
            return (string)dataGridRow.GetValue(MoveDragFormatProperty);
        }

        /// <summary>
        /// Sets the drag format when this instance is being dragged above or below another row.
        /// </summary>
        /// <param name="dataGridRow">The data grid row.</param>
        /// <param name="format">The drag format used when this instance is being dragged above or below another row.</param>
        public static void SetMoveDragFormat(DataGridRow dataGridRow, string format)
        {
            dataGridRow.SetValue(MoveDragFormatProperty, format);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the deselection enabled property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsDeselectionEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            DataGridRow dataGridRow = (DataGridRow)dependencyObject;
            if (GetIsDeselectionEnabled(dataGridRow))
            {
                dataGridRow.MouseLeftButtonDown += OnDataGridMouseLeftButtonDown;
            }
            else
            {
                dataGridRow.MouseLeftButtonDown -= OnDataGridMouseLeftButtonDown;
            }
        }

        /// <summary>
        /// Called when the data grid mouse left button is down.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow dataGridRow = sender as DataGridRow;
            if (dataGridRow.IsSelected)
            {
                dataGridRow.IsSelected = false;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Called when the is read only flag is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsReadOnlyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            DataGridRow dataGridRow = (DataGridRow)dependencyObject;
            DataGrid dataGrid = dataGridRow.FindVisualParent<DataGrid>();

            if (GetIsReadOnly(dataGridRow))
            {
                dataGrid.BeginningEdit += OnDataGridBeginningEdit;
            }
            else
            {
                dataGrid.BeginningEdit -= OnDataGridBeginningEdit;
            }
        }

        /// <summary>
        /// Called when the data grid begins editing.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataGridBeginningEditEventArgs"/> instance containing the event data.</param>
        private static void OnDataGridBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            if (GetIsReadOnly(e.Row))
            {
                e.Cancel = true;
            }
        }

        #endregion
    }
}
