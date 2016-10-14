namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;

    /// <summary>
    /// The combo box column for the <see cref="DataGrid."/>
    /// </summary>
    public class DataGridComboBoxColumn : System.Windows.Controls.DataGridComboBoxColumn
    {
        #region Dependency Properties

        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(
            "ToolTipTemplate",
            typeof(DataTemplate),
            typeof(DataGridComboBoxColumn),
            new PropertyMetadata(null));

        #endregion

        private BindingBase itemsSourceBinding;
        private BindingBase tooltip;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridComboBoxColumn"/> class.
        /// </summary>
        public DataGridComboBoxColumn()
        {
            this.EditingElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridComboBoxColumnEditingElementStyle");
            this.ElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridComboBoxColumnElementStyle");
        } 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the items source binding.
        /// </summary>
        /// <value>
        /// The items source binding.
        /// </value>
        public BindingBase ItemsSourceBinding
        {
            get
            {
                return this.itemsSourceBinding;
            }

            set
            {
                if (this.itemsSourceBinding != value)
                {
                    BindingBase oldBinding = this.itemsSourceBinding;
                    this.itemsSourceBinding = value;
                    this.CoerceValue(DataGridColumn.IsReadOnlyProperty);
                    this.CoerceValue(DataGridColumn.SortMemberPathProperty);
                    this.OnItemsSourceBindingChanged(oldBinding, this.itemsSourceBinding);
                }
            }
        }

        /// <summary>
        /// Gets or sets the tool tip binding.
        /// </summary>
        /// <value>
        /// The tool tip binding.
        /// </value>
        public BindingBase ToolTip
        {
            get
            {
                return this.tooltip;
            }

            set
            {
                this.tooltip = value;
                this.NotifyPropertyChanged("ToolTip");
            }
        }

        /// <summary>
        /// Gets or sets the tool tip data template.
        /// </summary>
        /// <value>
        /// The tool tip data template.
        /// </value>
        public DataTemplate ToolTipTemplate
        {
            get { return (DataTemplate)this.GetValue(ToolTipTemplateProperty); }
            set { this.SetValue(ToolTipTemplateProperty, value); }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Applies the binding.
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <param name="target">The target.</param>
        /// <param name="property">The property.</param>
        internal void ApplyBinding(BindingBase binding, DependencyObject target, DependencyProperty property)
        {
            if (binding != null)
            {
                BindingOperations.SetBinding(target, property, binding);
            }
            else
            {
                BindingOperations.ClearBinding(target, property);
            }
        }

        /// <summary>
        /// Applies the style.
        /// </summary>
        /// <param name="isEditing">if set to <c>true</c> it is editing.</param>
        /// <param name="defaultToElementStyle">if set to <c>true</c> default to the element style.</param>
        /// <param name="element">The element.</param>
        internal void ApplyStyle(bool isEditing, bool defaultToElementStyle, FrameworkElement element)
        {
            Style style = this.PickStyle(isEditing, defaultToElementStyle);
            if (style != null)
            {
                element.Style = style;
            }
        } 

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.
        /// </returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            ComboBox comboBox = (ComboBox)base.GenerateEditingElement(cell, dataItem);
            this.ApplyStyle(true, false, comboBox);
            this.ApplyBinding(this.ItemsSourceBinding, comboBox, Selector.ItemsSourceProperty);
            DataGridColumnToolTipHelper.SetToolTip(comboBox, this.ToolTip, this.ToolTipTemplate, dataItem);
            return comboBox;
        }

        /// <summary>
        /// Gets a read-only combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new, read-only combo box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedItemBinding" />, <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.SelectedValueBinding" />, and <see cref="P:System.Windows.Controls.DataGridComboBoxColumn.TextBinding" /> values.
        /// </returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            ComboBox comboBox = (ComboBox)base.GenerateEditingElement(cell, dataItem);
            this.ApplyStyle(false, false, comboBox);
            this.ApplyBinding(this.ItemsSourceBinding, comboBox, Selector.ItemsSourceProperty);
            DataGridColumnToolTipHelper.SetToolTip(comboBox, this.ToolTip, this.ToolTipTemplate, dataItem);
            return comboBox;
        }

        /// <summary>
        /// Called when the items source binding is changed.
        /// </summary>
        /// <param name="oldBinding">The old binding.</param>
        /// <param name="newBinding">The new binding.</param>
        protected void OnItemsSourceBindingChanged(BindingBase oldBinding, BindingBase newBinding)
        {
            this.NotifyPropertyChanged("ItemsSourceBinding");
        } 

        /// <summary>
        /// Refreshes the contents of a cell in the column in response to a binding change.
        /// </summary>
        /// <param name="element">The cell to update.</param>
        /// <param name="propertyName">The name of the column property that has changed.</param>
        protected override void RefreshCellContent(FrameworkElement element, string propertyName)
        {
            DataGridCell cell = element as DataGridCell;
            if (cell != null)
            {
                bool isEditing = cell.IsEditing;
                if (((string.Compare(propertyName, "ElementStyle", StringComparison.Ordinal) == 0) && !isEditing) || ((string.Compare(propertyName, "EditingElementStyle", StringComparison.Ordinal) == 0) && isEditing))
                {
                }
                else
                {
                    ComboBox content = cell.Content as ComboBox;
                    if (propertyName == "ItemsSourceBinding")
                    {
                        this.ApplyBinding(this.ItemsSourceBinding, content, Selector.ItemsSourceProperty);
                    }
                }
            }

            base.RefreshCellContent(element, propertyName);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Picks the style based on the specified settings.
        /// </summary>
        /// <param name="isEditing">if set to <c>true</c> it is editing.</param>
        /// <param name="defaultToElementStyle">if set to <c>true</c> default to element style.</param>
        /// <returns>The style with the specified settings.</returns>
        private Style PickStyle(bool isEditing, bool defaultToElementStyle)
        {
            Style elementStyle = isEditing ? this.EditingElementStyle : this.ElementStyle;

            if ((isEditing && defaultToElementStyle) && (elementStyle == null))
            {
                elementStyle = this.ElementStyle;
            }

            return elementStyle;
        } 

        #endregion
    }
}
