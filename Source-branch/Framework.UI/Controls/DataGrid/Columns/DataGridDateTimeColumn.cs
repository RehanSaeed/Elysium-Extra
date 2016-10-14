namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The date and time picker column for the <see cref="DataGrid." />
    /// </summary>
    public class DataGridDateTimeColumn : System.Windows.Controls.DataGridBoundColumn
    {
        #region Dependency Properties

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register(
            "FormatString",
            typeof(string),
            typeof(DataGridDateTimeColumn),
            new PropertyMetadata("HH:mm:ss dd MMM yyyy"));

        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(
            "ToolTipTemplate",
            typeof(DataTemplate),
            typeof(DataGridDateTimeColumn),
            new PropertyMetadata(null));

        #endregion

        #region Fields

        private static Style defaultEditingElementStyle;
        private static Style defaultElementStyle;

        private BindingBase tooltip; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DataGridDateTimeColumn"/> class.
        /// </summary>
        static DataGridDateTimeColumn()
        {
            DataGridDateTimeColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridDateTimeColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            DataGridDateTimeColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridDateTimeColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridDateTimeColumn"/> class.
        /// </summary>
        public DataGridDateTimeColumn()
        {
            this.EditingElementStyle = (Style)Application.Current.FindResource("DataGridDateTimeColumnEditingElementStyle");
            this.ElementStyle = (Style)Application.Current.FindResource("DataGridDateTimeColumnElementStyle");
        } 

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the default editing element style.
        /// </summary>
        /// <value>
        /// The default editing element style.
        /// </value>
        public static Style DefaultEditingElementStyle
        {
            get
            {
                if (defaultEditingElementStyle == null)
                {
                    Style style = new Style(typeof(DatePicker))
                    {
                        Setters = { new Setter(Control.BorderThicknessProperty, new Thickness(0.0)), new Setter(Control.PaddingProperty, new Thickness(0.0)) }
                    };
                    style.Seal();
                    defaultEditingElementStyle = style;
                }

                return defaultEditingElementStyle;
            }
        }

        /// <summary>
        /// Gets the default element style.
        /// </summary>
        /// <value>
        /// The default element style.
        /// </value>
        public static Style DefaultElementStyle
        {
            get
            {
                if (defaultElementStyle == null)
                {
                    Style style = new Style(typeof(TextBlock))
                    {
                        Setters = { new Setter(FrameworkElement.MarginProperty, new Thickness(2.0, 0.0, 2.0, 0.0)) }
                    };
                    style.Seal();
                    defaultElementStyle = style;
                }

                return defaultElementStyle;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the date format string.
        /// </summary>
        /// <value>
        /// The date format string.
        /// </value>
        public string FormatString
        {
            get { return (string)this.GetValue(FormatStringProperty); }
            set { this.SetValue(FormatStringProperty, value); }
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
        /// <param name="target">The target.</param>
        /// <param name="property">The property.</param>
        internal void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            BindingBase binding = this.Binding;
            if (binding != null)
            {
                BindingBase clone = (BindingBase)XamlHelper.Clone(binding);
                clone.StringFormat = "{0:" + this.FormatString + "}";
                BindingOperations.SetBinding(target, property, clone);
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
        /// When overridden in a derived class, gets an editing element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item that is represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new editing element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.
        /// </returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            DatePicker datePicker = new DatePicker();
            this.ApplyStyle(true, false, datePicker);
            this.ApplyBinding(datePicker, DatePicker.SelectedDateProperty);
            DataGridColumnToolTipHelper.SetToolTip(datePicker, this.ToolTip, this.ToolTipTemplate, dataItem);
            return datePicker;
        }

        /// <summary>
        /// When overridden in a derived class, gets a read-only element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item that is represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new read-only element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.
        /// </returns>
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            TextBlock textBlock = new TextBlock();
            this.ApplyStyle(false, false, textBlock);
            this.ApplyBinding(textBlock, TextBlock.TextProperty);
            DataGridColumnToolTipHelper.SetToolTip(textBlock, this.ToolTip, this.ToolTipTemplate, dataItem);
            return textBlock;
        }

        /// <summary>
        /// When overridden in a derived class, sets cell content as needed for editing.
        /// </summary>
        /// <param name="editingElement">The element that the column displays for a cell in editing mode.</param>
        /// <param name="editingEventArgs">Information about the user gesture that is causing a cell to enter editing mode.</param>
        /// <returns>
        /// When returned by a derived class, the unedited cell value. This implementation returns null in all cases.
        /// </returns>
        protected override object PrepareCellForEdit(FrameworkElement editingElement, RoutedEventArgs editingEventArgs)
        {
            DatePicker datePicker = editingElement as DatePicker;

            if (datePicker == null)
            {
                return null;
            }

            datePicker.Focus();
            return datePicker.SelectedDate;
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
