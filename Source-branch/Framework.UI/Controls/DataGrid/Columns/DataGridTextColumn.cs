namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The text box column for the <see cref="DataGrid."/>
    /// </summary>
    public sealed class DataGridTextColumn : System.Windows.Controls.DataGridTextColumn
    {
        #region Dependency Properties

        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(
            "ToolTipTemplate",
            typeof(DataTemplate),
            typeof(DataGridTextColumn),
            new PropertyMetadata(null)); 

        #endregion

        private BindingBase tooltip;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridTextColumn"/> class.
        /// </summary>
        public DataGridTextColumn()
        {
            this.EditingElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridTextColumnEditingElementStyle");
            this.ElementStyle = (System.Windows.Style)Application.Current.FindResource("DataGridTextColumnElementStyle");
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
            TextBox textBox = (TextBox)base.GenerateEditingElement(cell, dataItem);
            textBox.GotFocus += this.OnTextBoxGotFocus;
            DataGridColumnToolTipHelper.SetToolTip(textBox, this.ToolTip, this.ToolTipTemplate, dataItem);
            return textBox;
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
            TextBlock textBlock = (TextBlock)base.GenerateElement(cell, dataItem);
            DataGridColumnToolTipHelper.SetToolTip(textBlock, this.ToolTip, this.ToolTipTemplate, dataItem);
            return textBlock;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when the text box gets focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            textBox.SelectAll();
        }

        #endregion
    }
}
