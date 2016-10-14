namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The check box column for the <see cref="DataGrid."/>
    /// </summary>
    public sealed class DataGridCheckBoxColumn : System.Windows.Controls.DataGridCheckBoxColumn
    {
        #region Dependency Properties

        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(
            "ToolTipTemplate",
            typeof(DataTemplate),
            typeof(DataGridCheckBoxColumn),
            new PropertyMetadata(null));

        #endregion

        private BindingBase tooltip;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridCheckBoxColumn"/> class.
        /// </summary>
        public DataGridCheckBoxColumn()
        {
            this.EditingElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridCheckBoxColumnEditingElementStyle");
            this.ElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridCheckBoxColumnElementStyle");
        } 

        #endregion

        #region Public Properties

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

        #region Protected Methods

        /// <summary>
        /// Gets a <see cref="T:System.Windows.Controls.TextBox" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new text box control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            CheckBox checkBox = (CheckBox)base.GenerateEditingElement(cell, dataItem);
            DataGridColumnToolTipHelper.SetToolTip(checkBox, this.ToolTip, this.ToolTipTemplate, dataItem, null);
            return checkBox;
        }

        /// <summary>
        /// Gets a read-only <see cref="T:System.Windows.Controls.TextBlock" /> control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new, read-only text block control that is bound to the column's <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value.
        /// </returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            CheckBox checkBox = (CheckBox)base.GenerateElement(cell, dataItem);
            DataGridColumnToolTipHelper.SetToolTip(checkBox, this.ToolTip, this.ToolTipTemplate, dataItem, null);
            return checkBox;
        }

        #endregion
    }
}
