namespace Framework.UI.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// The up down base.
    /// </summary>
    /// <typeparam name="T">The Type </typeparam>
    [TemplatePart(Name = TextBoxPART, Type = typeof(TextBox))]
    [TemplatePart(Name = SpinnerPART, Type = typeof(Spinner))]
    public abstract class UpDownBase<T> : InputBase, IValidateInput
    {
        #region Fields

        public static readonly DependencyProperty AllowSpinProperty = DependencyProperty.Register(
            "AllowSpin", 
            typeof(bool), 
            typeof(UpDownBase<T>), 
            new UIPropertyMetadata(false));

        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register(
            "DefaultValue", 
            typeof(T), 
            typeof(UpDownBase<T>), 
            new UIPropertyMetadata(default(T), OnDefaultValueChanged));

        public static readonly DependencyProperty MouseWheelActiveOnFocusProperty = DependencyProperty.Register(
            "MouseWheelActiveOnFocus", 
            typeof(bool), 
            typeof(UpDownBase<T>), 
            new UIPropertyMetadata(true));

        public static readonly DependencyProperty ShowButtonSpinnerProperty = DependencyProperty.Register(
            "ShowButtonSpinner", 
            typeof(bool), 
            typeof(UpDownBase<T>), 
            new UIPropertyMetadata(false));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", 
            typeof(T), 
            typeof(UpDownBase<T>), 
            new FrameworkPropertyMetadata(
                default(T), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                OnValueChanged, 
                OnCoerceValue, 
                false, 
                UpdateSourceTrigger.PropertyChanged));

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", 
            RoutingStrategy.Bubble, 
            typeof(RoutedPropertyChangedEventHandler<object>), 
            typeof(UpDownBase<T>));

        /// <summary>
        /// Name constant for Text template part.
        /// </summary>
        internal const string TextBoxPART = "PART_TextBox";

        /// <summary>
        /// Name constant for Spinner template part.
        /// </summary>
        internal const string SpinnerPART = "PART_Spinner";

        /// <summary>
        /// Flags if the Text and Value properties are in the process of being sync'd
        /// </summary>
        private bool isSyncingTextAndValueProperties;
        private bool isTextChangedFromUI;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="UpDownBase{T}"/> class.
        /// </summary>
        internal UpDownBase()
        {
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The input validation error.
        /// </summary>
        public event InputValidationErrorEventHandler InputValidationError;

        /// <summary>
        /// The value changed.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<object> ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
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
        /// Gets or sets the default value.
        /// </summary>
        public T DefaultValue
        {
            get { return (T)this.GetValue(DefaultValueProperty); }
            set { this.SetValue(DefaultValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether mouse wheel active on focus.
        /// </summary>
        public bool MouseWheelActiveOnFocus
        {
            get { return (bool)this.GetValue(MouseWheelActiveOnFocusProperty); }
            set { this.SetValue(MouseWheelActiveOnFocusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show button spinner.
        /// </summary>
        public bool ShowButtonSpinner
        {
            get { return (bool)this.GetValue(ShowButtonSpinnerProperty); }
            set { this.SetValue(ShowButtonSpinnerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value
        {
            get { return (T)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the spinner.
        /// </summary>
        protected Spinner Spinner
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the text box.
        /// </summary>
        protected TextBox TextBox
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The commit input.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CommitInput()
        {
            return this.SyncTextAndValueProperties(true, this.Text);
        }

        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.TextBox = this.GetTemplateChild(TextBoxPART) as TextBox;
            if (TextBox != null)
            {
                this.TextBox.Text = this.Text;
                this.TextBox.LostFocus += new RoutedEventHandler(this.OnTextBoxLostFocus);
                this.TextBox.TextChanged += new TextChangedEventHandler(this.OnTextBoxTextChanged);
            }

            if (this.Spinner != null)
            {
                this.Spinner.Spin -= this.OnSpinnerSpin;
            }

            this.Spinner = this.GetTemplateChild(SpinnerPART) as Spinner;

            if (this.Spinner != null)
            {
                this.Spinner.Spin += this.OnSpinnerSpin;
            }

            this.SetValidSpinDirection();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on access key.
        /// </summary>
        /// <param name="e"> The event arguments. </param>
        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            if (TextBox != null)
            {
                TextBox.Focus();
            }

            base.OnAccessKey(e);
        }

        /// <summary>
        /// The on coerce value.
        /// </summary>
        /// <param name="newValue"> The new value. </param>
        /// <returns> The <see cref="object"/>. </returns>
        protected virtual object OnCoerceValue(object newValue)
        {
            return newValue;
        }

        /// <summary>
        /// The on initialized.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // When both Value and Text are initialized, Value has priority.
            bool updateValueFromText = this.Value == null;
            this.SyncTextAndValueProperties(updateValueFromText, this.Text);
        }

        /// <summary>
        /// The on got focus.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (TextBox != null)
            {
                TextBox.Focus();
            }
        }

        /// <summary>
        /// The on preview key down.
        /// </summary>
        /// <param name="e"> The event Arguments. </param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    {
                        if (this.AllowSpin && !this.IsReadOnly)
                        {
                            this.DoIncrement();
                            e.Handled = true;
                        }

                        break;
                    }

                case Key.Down:
                    {
                        if (this.AllowSpin && !this.IsReadOnly)
                        {
                            this.DoDecrement();
                            e.Handled = true;
                        }
                        
                        break;
                    }
            }
        }

        /// <summary>
        /// The on key down.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    {
                        // Commit Text on "Enter" to raise Error event.
                        bool commitSuccess = this.CommitInput();

                        // Only handle if an exception is detected (Commit fails).
                        e.Handled = !commitSuccess;
                        break;
                    }
            }
        }

        /// <summary>
        /// The on mouse wheel.
        /// </summary>
        /// <param name="e"> The event Arguments. </param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            if (!e.Handled && this.AllowSpin && !this.IsReadOnly && (this.TextBox.IsFocused && this.MouseWheelActiveOnFocus))
            {
                if (e.Delta < 0)
                {
                    this.DoDecrement();
                }
                else if (0 < e.Delta)
                {
                    this.DoIncrement();
                }

                e.Handled = true;
            }
        }

        /// <summary>
        /// The on text changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected override void OnTextChanged(string oldValue, string newValue)
        {
            if (this.IsInitialized)
            {
                this.SyncTextAndValueProperties(true, this.Text);
            }
        }

        /// <summary>
        /// The on culture info changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected override void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
        {
            if (this.IsInitialized)
            {
                this.SyncTextAndValueProperties(false, null);
            }
        }

        /// <summary>
        /// The on read only changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected override void OnReadOnlyChanged(bool oldValue, bool newValue)
        {
            this.SetValidSpinDirection();
        }

        /// <summary>
        /// The on spin.
        /// </summary>
        /// <param name="e"> The e. </param>
        /// <exception cref="ArgumentNullException">Any Exception </exception>
        protected virtual void OnSpin(SpinEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            if (e.Direction == SpinDirection.Increase)
            {
                this.DoIncrement();
            }
            else
            {
                this.DoDecrement();
            }
        }

        /// <summary>
        /// The on value changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        protected virtual void OnValueChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized)
            {
                this.SyncTextAndValueProperties(false, null);
            }

            this.SetValidSpinDirection();

            RoutedPropertyChangedEventArgs<object> args = new RoutedPropertyChangedEventArgs<object>(oldValue, newValue);
            args.RoutedEvent = ValueChangedEvent;
            this.RaiseEvent(args);
        }

        /// <summary>
        /// Converts the formatted text to a value.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <returns> The <see cref="T"/>. </returns>
        protected abstract T ConvertTextToValue(string text);

        /// <summary>
        /// Converts the value to formatted text.
        /// </summary>
        /// <returns>The Returned String</returns>
        protected abstract string ConvertValueToText();

        /// <summary>
        /// Called by OnSpin when the spin direction is SpinDirection.Increase.
        /// </summary>
        protected abstract void OnIncrement();

        /// <summary>
        /// Called by OnSpin when the spin direction is SpinDirection.Decrease.
        /// </summary>
        protected abstract void OnDecrement();

        /// <summary>
        /// Sets the valid spin directions.
        /// </summary>
        protected abstract void SetValidSpinDirection();

        /// <summary>
        /// The sync text and value properties.
        /// </summary>
        /// <param name="updateValueFromText"> The update value from text. </param>
        /// <param name="text"> The text. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        protected bool SyncTextAndValueProperties(bool updateValueFromText, string text)
        {
            if (this.isSyncingTextAndValueProperties)
            {
                return true;
            }

            this.isSyncingTextAndValueProperties = true;
            bool parsedTextIsValid = true;
            try
            {
                if (updateValueFromText)
                {
                    if (string.IsNullOrEmpty(text))
                    {
                        // An empty input sets the value to the default value.
                        if (!object.Equals(this.Value, this.DefaultValue))
                        {
                            this.Value = this.DefaultValue;
                        }
                    }
                    else
                    {
                        try
                        {
                            this.Value = this.ConvertTextToValue(text);
                        }
                        catch (Exception e)
                        {
                            parsedTextIsValid = false;

                            // From the UI, just allow any input.
                            if (!this.isTextChangedFromUI)
                            {
                                // This call may throw an exception. 
                                // See RaiseInputValidationError() implementation.
                                this.RaiseInputValidationError(e);
                            }
                        }
                    }
                }

                // Do not touch the ongoing text input from user.
                if (!this.isTextChangedFromUI)
                {
                    // Don't replace the empty Text with the non-empty representation of DefaultValue.
                    bool shouldKeepEmpty = string.IsNullOrEmpty(this.Text) && object.Equals(this.Value, this.DefaultValue);
                    if (!shouldKeepEmpty)
                    {
                        this.Text = this.ConvertValueToText();
                    }

                    // Sync Text and textBox
                    if (this.TextBox != null)
                    {
                        this.TextBox.Text = this.Text;
                    }
                }

                if (this.isTextChangedFromUI && !parsedTextIsValid)
                {
                    // Text input was made from the user and the text
                    // represents an invalid value. Disable the spinner
                    // in this case.
                    if (this.Spinner != null)
                    {
                        this.Spinner.ValidSpinDirection = ValidSpinDirections.None;
                    }
                }
                else
                {
                    this.SetValidSpinDirection();
                }
            }
            finally
            {
                this.isSyncingTextAndValueProperties = false;
            }

            return parsedTextIsValid;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The on coerce value.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="basevalue"> The base value. </param>
        /// <returns> The <see cref="object"/>. </returns>
        private static object OnCoerceValue(DependencyObject o, object basevalue)
        {
            return ((UpDownBase<T>)o).OnCoerceValue(basevalue);
        }

        /// <summary>
        /// The on default value changed.
        /// </summary>
        /// <param name="source"> The source. </param>
        /// <param name="args"> The arguments. </param>
        private static void OnDefaultValueChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            ((UpDownBase<T>)source).OnDefaultValueChanged((T)args.OldValue, (T)args.NewValue);
        }

        /// <summary>
        /// The on value changed.
        /// </summary>
        /// <param name="o"> The o. </param>
        /// <param name="e"> The e. </param>
        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            UpDownBase<T> updownBase = o as UpDownBase<T>;
            if (updownBase != null)
            {
                updownBase.OnValueChanged((T)e.OldValue, (T)e.NewValue);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Performs an increment if conditions allow it.
        /// </summary>
        private void DoDecrement()
        {
            if (this.Spinner == null || (this.Spinner.ValidSpinDirection & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease)
            {
                this.OnDecrement();
            }
        }

        /// <summary>
        /// Performs a decrement if conditions allow it.
        /// </summary>
        private void DoIncrement()
        {
            if (this.Spinner == null || (this.Spinner.ValidSpinDirection & ValidSpinDirections.Increase) == ValidSpinDirections.Increase)
            {
                this.OnIncrement();
            }
        }

        /// <summary>
        /// The on default value changed.
        /// </summary>
        /// <param name="oldValue"> The old value. </param>
        /// <param name="newValue"> The new value. </param>
        private void OnDefaultValueChanged(T oldValue, T newValue)
        {
            if (this.IsInitialized && string.IsNullOrEmpty(this.Text))
            {
                this.SyncTextAndValueProperties(true, this.Text);
            }
        }

        /// <summary>
        /// The on spinner spin.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnSpinnerSpin(object sender, SpinEventArgs e)
        {
            if (this.AllowSpin && !this.IsReadOnly)
            {
                this.OnSpin(e);
            }
        }

        /// <summary>
        /// The on text box text changed.
        /// </summary>
        ///  <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.isTextChangedFromUI = true;
                this.Text = ((TextBox)sender).Text;
            }
            finally
            {
                this.isTextChangedFromUI = false;
            }
        }

        /// <summary>
        /// The on text box lost focus.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            this.CommitInput();
        }

        /// <summary>
        /// The raise input validation error.
        /// </summary>
        /// <param name="e"> The e. </param>
        /// <exception cref="Exception">Any Exception </exception>
        private void RaiseInputValidationError(Exception e)
        {
            if (this.InputValidationError != null)
            {
                InputValidationErrorEventArgs args = new InputValidationErrorEventArgs(e);
                this.InputValidationError(this, args);
                if (args.ThrowException)
                {
                    throw args.Exception;
                }
            }
        }

        #endregion
    }
}
