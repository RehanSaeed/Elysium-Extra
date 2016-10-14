namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Markup;

    /// <summary>
    /// Represents a spinner control that includes two Buttons.
    /// </summary>
    [TemplatePart(Name = IncreaseButtonPART, Type = typeof(ButtonBase))]
    [TemplatePart(Name = DecreaseButtonPART, Type = typeof(ButtonBase))]
    [ContentProperty("Content")]
    public class ButtonSpinner : Spinner
    {
        #region Dependency Properties

        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register(
            "AllowSpin",
            typeof(bool),
            typeof(ButtonSpinner),
            new UIPropertyMetadata(true, AllowSpinPropertyChanged));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object),
            typeof(ButtonSpinner),
            new PropertyMetadata(null, OnContentPropertyChanged));

        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register(
            "ShowButtonSpinner",
            typeof(bool),
            typeof(ButtonSpinner),
            new UIPropertyMetadata(true)); 

        #endregion

        #region Fields

        private const string IncreaseButtonPART = "PART_IncreaseButton";
        private const string DecreaseButtonPART = "PART_DecreaseButton";

        private ButtonBase decreaseButton;
        private ButtonBase increaseButton; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="ButtonSpinner"/> class.
        /// </summary>
        static ButtonSpinner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonSpinner), new FrameworkPropertyMetadata(typeof(ButtonSpinner)));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether allow spin.
        /// </summary>
        public bool AllowSpin
        {
            get { return (bool)this.GetValue(AllowSpinProperty); }
            set { this.SetValue(AllowSpinProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public object Content
        {
            get { return this.GetValue(ContentProperty) as object; }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show button spinner.
        /// </summary>
        public bool ShowButtonSpinner
        {
            get { return (bool)this.GetValue(ShowButtonSpinnerProperty); }
            set { this.SetValue(ShowButtonSpinnerProperty, value); }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the DecreaseButton template part.
        /// </summary>
        private ButtonBase DecreaseButton
        {
            get
            {
                return this.decreaseButton;
            }

            set
            {
                if (this.decreaseButton != null)
                {
                    this.decreaseButton.Click -= this.OnButtonClick;
                }

                this.decreaseButton = value;

                if (this.decreaseButton != null)
                {
                    this.decreaseButton.Click += this.OnButtonClick;
                }
            }
        }
        
        /// <summary>
        /// Gets or sets the IncreaseButton template part.
        /// </summary>
        private ButtonBase IncreaseButton
        {
            get
            {
                return this.increaseButton;
            }

            set
            {
                if (this.increaseButton != null)
                {
                    this.increaseButton.Click -= this.OnButtonClick;
                }

                this.increaseButton = value;

                if (this.increaseButton != null)
                {
                    this.increaseButton.Click += this.OnButtonClick;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.IncreaseButton = this.GetTemplateChild(IncreaseButtonPART) as ButtonBase;
            this.DecreaseButton = this.GetTemplateChild(DecreaseButtonPART) as ButtonBase;

            this.SetButtonUsage();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the Content property value changed.
        /// </summary>
        /// <param name="oldValue">The old value of the Content property.</param>
        /// <param name="newValue">The new value of the Content property.</param>
        protected virtual void OnContentChanged(object oldValue, object newValue)
        {
        }

        /// <summary>
        /// The on allow spin changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnAllowSpinChanged(bool oldValue, bool newValue)
        {
            this.SetButtonUsage();
        }

        /// <summary>
        /// Cancel LeftMouseButtonUp events originating from a button that has
        /// been changed to disabled.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            Point mousePosition;
            if (this.IncreaseButton != null && this.IncreaseButton.IsEnabled == false)
            {
                mousePosition = e.GetPosition(this.IncreaseButton);
                if (mousePosition.X > 0 && mousePosition.X < this.IncreaseButton.ActualWidth &&
                    mousePosition.Y > 0 && mousePosition.Y < this.IncreaseButton.ActualHeight)
                {
                    e.Handled = true;
                }
            }

            if (this.DecreaseButton != null && this.DecreaseButton.IsEnabled == false)
            {
                mousePosition = e.GetPosition(this.DecreaseButton);
                if (mousePosition.X > 0 && mousePosition.X < this.DecreaseButton.ActualWidth &&
                    mousePosition.Y > 0 && mousePosition.Y < this.DecreaseButton.ActualHeight)
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Called when valid spin direction changed.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected override void OnValidSpinDirectionChanged(ValidSpinDirections oldValue, ValidSpinDirections newValue)
        {
            this.SetButtonUsage();
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The allow spin property changed.
        /// </summary>
        /// <param name="d"> The d. </param>
        /// <param name="e"> The e. </param>
        private static void AllowSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonSpinner source = d as ButtonSpinner;
            source.OnAllowSpinChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// ContentProperty property changed handler.
        /// </summary>
        /// <param name="d">ButtonSpinner that changed its Content.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ButtonSpinner source = d as ButtonSpinner;
            source.OnContentChanged(e.OldValue, e.NewValue);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handle click event of IncreaseButton and DecreaseButton template parts,
        /// translating Click to appropriate Spin event.
        /// </summary>
        /// <param name="sender">Event sender, should be either IncreaseButton or DecreaseButton template part.</param>
        /// <param name="e">Event args.</param>
        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (this.AllowSpin)
            {
                SpinDirection direction = sender == this.IncreaseButton ? SpinDirection.Increase : SpinDirection.Decrease;
                this.OnSpin(new SpinEventArgs(direction));
            }
        }

        /// <summary>
        /// Disables or enables the buttons based on the valid spin direction.
        /// </summary>
        private void SetButtonUsage()
        {
            // button spinner adds buttons that spin, so disable accordingly.
            if (this.IncreaseButton != null)
            {
                this.IncreaseButton.IsEnabled = this.AllowSpin && ((this.ValidSpinDirection & ValidSpinDirections.Increase) == ValidSpinDirections.Increase);
            }

            if (this.DecreaseButton != null)
            {
                this.DecreaseButton.IsEnabled = this.AllowSpin && ((this.ValidSpinDirection & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease);
            }
        }

        #endregion
    }
}
