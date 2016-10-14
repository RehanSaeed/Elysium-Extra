namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The number column base class for the <see cref="DataGrid." />
    /// </summary>
    /// <typeparam name="T">The type of the columns data.</typeparam>
    /// <typeparam name="TControl">The type of the control.</typeparam>
    /// <typeparam name="TType">The <see cref="Type"/> of the type.</typeparam>
    public abstract class DataGridNumberColumn<T, TControl, TType> : DataGridBoundColumn
        where T : DataGridNumberColumn<T, TControl, TType>
        where TControl : CommonNumericUpDown<TType>, new()
        where TType : struct, IFormattable, IComparable<TType>
    {
        #region Dependency Properties

        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register(
            "AllowSpin",
            typeof(bool),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new PropertyMetadata(true, OnAllowSpinPropertyChanged));

        public static readonly DependencyProperty FormatStringProperty = DependencyProperty.Register(
            "FormatString",
            typeof(string),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new PropertyMetadata(null, OnFormatStringPropertyChanged));

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment",
            typeof(TType?),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new FrameworkPropertyMetadata(default(TType), OnIncrementPropertyChanged));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum",
            typeof(TType?),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new FrameworkPropertyMetadata(default(TType), OnMaximumPropertyChanged));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum",
            typeof(TType?),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new FrameworkPropertyMetadata(default(TType), OnMinimumPropertyChanged));

        public static readonly DependencyProperty SelectAllOnGotFocusProperty = DependencyProperty.Register(
            "SelectAllOnGotFocus",
            typeof(bool),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new PropertyMetadata(true, OnSelectAllOnGotFocusPropertyChanged));

        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register(
            "ShowButtonSpinner",
            typeof(bool),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new PropertyMetadata(false, OnShowButtonSpinnerPropertyChanged));

        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(
            "ToolTipTemplate",
            typeof(DataTemplate),
            typeof(DataGridNumberColumn<T, TControl, TType>),
            new PropertyMetadata(null)); 

        #endregion

        #region Fields

        private static Style defaultEditingElementStyle;
        private static Style defaultElementStyle;

        private BindingBase tooltip;

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
                    Style style = new Style(typeof(TControl))
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
        /// Gets or sets a value indicating whether to allow spinning of the value.
        /// </summary>
        /// <value>
        /// <c>true</c> if allow spinning; otherwise, <c>false</c>.
        /// </value>
        public bool AllowSpin
        {
            get { return (bool)this.GetValue(AllowSpinProperty); }
            set { this.SetValue(AllowSpinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the format string.
        /// </summary>
        /// <value>
        /// The format string.
        /// </value>
        public string FormatString
        {
            get { return (string)this.GetValue(FormatStringProperty); }
            set { this.SetValue(FormatStringProperty, value); }
        }

        /// <summary>
        /// Gets or sets the spinning increment.
        /// </summary>
        /// <value>
        /// The spinning increment.
        /// </value>
        public TType? Increment
        {
            get { return (TType?)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public TType? Maximum
        {
            get { return (TType?)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public TType? Minimum
        {
            get { return (TType?)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to select all on got focus.
        /// </summary>
        /// <value>
        /// <c>true</c> if select all on got focus; otherwise, <c>false</c>.
        /// </value>
        public bool SelectAllOnGotFocus
        {
            get { return (bool)this.GetValue(SelectAllOnGotFocusProperty); }
            set { this.SetValue(SelectAllOnGotFocusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the button spinner.
        /// </summary>
        /// <value>
        /// <c>true</c> if showing the button spinner; otherwise, <c>false</c>.
        /// </value>
        public bool ShowButtonSpinner
        {
            get { return (bool)this.GetValue(ShowButtonSpinnerProperty); }
            set { this.SetValue(ShowButtonSpinnerProperty, value); }
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
        /// <param name="stringFormat">The string format.</param>
        internal void ApplyBinding(BindingBase binding, DependencyObject target, DependencyProperty property, string stringFormat)
        {
            if (binding != null)
            {
                if (stringFormat == null)
                {
                    BindingOperations.SetBinding(target, property, binding);
                }
                else
                {
                    BindingBase clone = (BindingBase)XamlHelper.Clone(binding);
                    clone.StringFormat = stringFormat;
                    BindingOperations.SetBinding(target, property, clone);
                }
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

        #region Protected Static Methods

        /// <summary>
        /// Updates the metadata.
        /// </summary>
        /// <param name="increment">The default increment.</param>
        /// <param name="minValue">The default min value.</param>
        /// <param name="maxValue">The default max value.</param>
        protected static void UpdateMetadata(TType? increment, TType? minValue, TType? maxValue)
        {
            ElementStyleProperty.OverrideMetadata(typeof(T), new FrameworkPropertyMetadata(DefaultElementStyle));
            EditingElementStyleProperty.OverrideMetadata(typeof(T), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
            IncrementProperty.OverrideMetadata(typeof(T), new FrameworkPropertyMetadata(increment));
            MaximumProperty.OverrideMetadata(typeof(T), new FrameworkPropertyMetadata(maxValue));
            MinimumProperty.OverrideMetadata(typeof(T), new FrameworkPropertyMetadata(minValue));
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
            TControl control = new TControl()
            {
                AllowSpin = this.AllowSpin,
                FormatString = this.FormatString,
                Increment = this.Increment,
                Maximum = this.Maximum,
                Minimum = this.Minimum,
                SelectAllOnGotFocus = this.SelectAllOnGotFocus,
                ShowButtonSpinner = this.ShowButtonSpinner
            };
            this.ApplyStyle(true, false, control);
            this.ApplyBinding(this.Binding, control, CommonNumericUpDown<TType>.ValueProperty, null);
            DataGridColumnToolTipHelper.SetToolTip(control, this.ToolTip, this.ToolTipTemplate, dataItem);
            return control;
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
            this.ApplyBinding(this.Binding, textBlock, TextBlock.DataContextProperty, null);
            this.ApplyBinding(new Binding(), textBlock, TextBlock.TextProperty, this.FormatString);
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
            TControl control = editingElement as TControl;

            if (control == null)
            {
                return null;
            }

            control.Focus();
            return control.Value;
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
                    TControl control = cell.Content as TControl;

                    if (propertyName == "FormatString")
                    {
                        TextBlock textBlock = cell.Content as TextBlock;
                        if (textBlock != null)
                        {
                            this.ApplyBinding(new Binding(), textBlock, TextBlock.TextProperty, this.FormatString);
                        }

                        if (control != null)
                        {
                            control.FormatString = this.FormatString;
                        }
                    }

                    if (control != null)
                    {
                        control.AllowSpin = this.AllowSpin;
                        control.FormatString = this.FormatString;
                        control.Increment = this.Increment;
                        control.Maximum = this.Maximum;
                        control.Minimum = this.Minimum;
                        control.SelectAllOnGotFocus = this.SelectAllOnGotFocus;
                        control.ShowButtonSpinner = this.ShowButtonSpinner;
                    }
                }
            }

            base.RefreshCellContent(element, propertyName);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when allow spin property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAllowSpinPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((T)dependencyObject).NotifyPropertyChanged("AllowSpin");
        }

        /// <summary>
        /// Called when the format string property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFormatStringPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((T)dependencyObject).NotifyPropertyChanged("FormatString");
        }

        /// <summary>
        /// Called when the increment property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIncrementPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((T)dependencyObject).NotifyPropertyChanged("Increment");
        }

        /// <summary>
        /// Called when the maximum property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMaximumPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((T)dependencyObject).NotifyPropertyChanged("Maximum");
        }

        /// <summary>
        /// Called when the minimum property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnMinimumPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((T)dependencyObject).NotifyPropertyChanged("Minimum");
        }

        /// <summary>
        /// Called when the select all on got focus property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnSelectAllOnGotFocusPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((T)dependencyObject).NotifyPropertyChanged("SelectAllOnGotFocus");
        }

        /// <summary>
        /// Called when the show button spinner property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnShowButtonSpinnerPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            ((T)dependencyObject).NotifyPropertyChanged("ShowButtonSpinner");
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
