namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The progress bar column for the <see cref="DataGrid." />
    /// </summary>
    public class DataGridProgressBarColumn : System.Windows.Controls.DataGridBoundColumn
    {
        #region Dependency Properties

        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register(
            "ToolTipTemplate",
            typeof(DataTemplate),
            typeof(DataGridProgressBarColumn),
            new PropertyMetadata(null));

        #endregion

        #region Fields

        private static Style defaultEditingElementStyle;
        private static Style defaultElementStyle;

        private BindingBase largeChangeBinding;
        private BindingBase maximumBinding;
        private BindingBase minimumBinding;
        private BindingBase smallChangeBinding;
        private BindingBase stateChangeBinding;
        private BindingBase tooltipBinding;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="DataGridProgressBarColumn"/> class.
        /// </summary>
        static DataGridProgressBarColumn()
        {
            DataGridProgressBarColumn.ElementStyleProperty.OverrideMetadata(typeof(DataGridProgressBarColumn), new FrameworkPropertyMetadata(DefaultElementStyle));
            DataGridProgressBarColumn.EditingElementStyleProperty.OverrideMetadata(typeof(DataGridProgressBarColumn), new FrameworkPropertyMetadata(DefaultEditingElementStyle));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="DataGridProgressBarColumn"/> class.
        /// </summary>
        public DataGridProgressBarColumn()
        {
            this.EditingElementStyle = (Style)Application.Current.FindResource("DataGridProgressBarColumnEditingElementStyle");
            this.ElementStyle = (Style)Application.Current.FindResource("DataGridProgressBarColumnElementStyle");
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
                    Style style = new Style(typeof(Elysium.Controls.ProgressBar));
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
                    Style style = new Style(typeof(Slider));
                    style.Seal();
                    defaultElementStyle = style;
                }

                return defaultElementStyle;
            }
        }

        #endregion

        #region Public Properties

        public BindingBase LargeChange
        {
            get
            {
                return this.largeChangeBinding;
            }

            set
            {
                this.largeChangeBinding = value;
                this.NotifyPropertyChanged("LargeChange");
            }
        }

        public BindingBase Maximum
        {
            get
            {
                return this.maximumBinding;
            }

            set
            {
                this.maximumBinding = value;
                this.NotifyPropertyChanged("Maximum");
            }
        }

        public BindingBase Minimum
        {
            get
            {
                return this.minimumBinding;
            }

            set
            {
                this.minimumBinding = value;
                this.NotifyPropertyChanged("Minimum");
            }
        }

        public BindingBase SmallChange
        {
            get
            {
                return this.smallChangeBinding;
            }

            set
            {
                this.smallChangeBinding = value;
                this.NotifyPropertyChanged("SmallChange");
            }
        }

        public BindingBase State
        {
            get
            {
                return this.stateChangeBinding;
            }

            set
            {
                this.stateChangeBinding = value;
                this.NotifyPropertyChanged("State");
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
                return this.tooltipBinding;
            }

            set
            {
                this.tooltipBinding = value;
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

        internal void ApplyBinding(DependencyObject target, DependencyProperty property)
        {
            this.ApplyBinding(this.Binding, target, property);
        }

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
        /// When overridden in a derived class, gets an editing element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.
        /// </summary>
        /// <param name="cell">The cell that will contain the generated element.</param>
        /// <param name="dataItem">The data item that is represented by the row that contains the intended cell.</param>
        /// <returns>
        /// A new editing element that is bound to the <see cref="P:System.Windows.Controls.DataGridBoundColumn.Binding" /> property value of the column.
        /// </returns>
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            Slider slider = new Slider()
            {
                DataContext = dataItem
            };
            this.ApplyStyle(true, false, slider);
            this.ApplyBinding(this.LargeChange, slider, Slider.LargeChangeProperty);
            this.ApplyBinding(this.Minimum, slider, Slider.MinimumProperty);
            this.ApplyBinding(this.Maximum, slider, Slider.MaximumProperty);
            this.ApplyBinding(this.SmallChange, slider, Slider.SmallChangeProperty);
            this.ApplyBinding(slider, Slider.ValueProperty);
            DataGridColumnToolTipHelper.SetToolTip(slider, this.ToolTip, this.ToolTipTemplate, dataItem);
            return slider;
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
            Elysium.Controls.ProgressBar progressBar = new Elysium.Controls.ProgressBar()
            {
                DataContext = dataItem
            };
            this.ApplyStyle(false, false, progressBar);
            this.ApplyBinding(this.LargeChange, progressBar, Elysium.Controls.ProgressBar.LargeChangeProperty);
            this.ApplyBinding(this.Minimum, progressBar, Elysium.Controls.ProgressBar.MinimumProperty);
            this.ApplyBinding(this.Maximum, progressBar, Elysium.Controls.ProgressBar.MaximumProperty);
            this.ApplyBinding(this.SmallChange, progressBar, Elysium.Controls.ProgressBar.SmallChangeProperty);
            this.ApplyBinding(this.State, progressBar, Elysium.Controls.ProgressBar.StateProperty);
            this.ApplyBinding(progressBar, Elysium.Controls.ProgressBar.ValueProperty);
            DataGridColumnToolTipHelper.SetToolTip(cell, this.ToolTip, this.ToolTipTemplate, dataItem);
            return progressBar;
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
            Slider datePicker = editingElement as Slider;

            if (datePicker == null)
            {
                return null;
            }

            datePicker.Focus();
            return datePicker.Value;
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
