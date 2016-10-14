namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Extended version of the text box.
    /// </summary>
    public class TextBoxExtended : TextBox
    {
        #region Dependency Properties

        public static readonly DependencyProperty AutoMoveFocusProperty = DependencyProperty.Register(
            "AutoMoveFocus",
            typeof(bool),
            typeof(TextBoxExtended),
            new UIPropertyMetadata(true));

        public static readonly DependencyProperty FocusOnVisibleProperty = DependencyProperty.Register(
            "FocusOnVisible",
            typeof(bool),
            typeof(TextBoxExtended),
            new PropertyMetadata(false));

        public static readonly DependencyProperty SelectAllOnGotFocusProperty = DependencyProperty.Register(
            "SelectAllOnGotFocus",
            typeof(bool),
            typeof(TextBoxExtended),
            new PropertyMetadata(false));

        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark",
            typeof(object),
            typeof(TextBoxExtended),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register(
            "WatermarkTemplate",
            typeof(DataTemplate),
            typeof(TextBoxExtended),
            new UIPropertyMetadata(null));

        #endregion

        public static readonly RoutedEvent QueryMoveFocusEvent = EventManager.RegisterRoutedEvent(
            "QueryMoveFocus",
            RoutingStrategy.Bubble,
            typeof(QueryMoveFocusEventHandler),
            typeof(TextBoxExtended));

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="TextBoxExtended"/> class.
        /// </summary>
        static TextBoxExtended()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextBoxExtended), new FrameworkPropertyMetadata(typeof(TextBoxExtended)));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TextBoxExtended"/> class.
        /// </summary>
        public TextBoxExtended()
        {
            this.IsVisibleChanged += this.OnIsVisibleChanged;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether auto move focus.
        /// </summary>
        public bool AutoMoveFocus
        {
            get { return (bool)this.GetValue(AutoMoveFocusProperty); }
            set { this.SetValue(AutoMoveFocusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to focus when this instance becomes visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if focus on visible; otherwise, <c>false</c>.
        /// </value>
        public bool FocusOnVisible
        {
            get { return (bool)this.GetValue(FocusOnVisibleProperty); }
            set { this.SetValue(FocusOnVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether select all on got focus.
        /// </summary>
        public bool SelectAllOnGotFocus
        {
            get { return (bool)this.GetValue(SelectAllOnGotFocusProperty); }
            set { this.SetValue(SelectAllOnGotFocusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the watermark.
        /// </summary>
        public object Watermark
        {
            get { return (object)this.GetValue(WatermarkProperty); }
            set { this.SetValue(WatermarkProperty, value); }
        }

        /// <summary>
        /// Gets or sets the watermark template.
        /// </summary>
        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)this.GetValue(WatermarkTemplateProperty); }
            set { this.SetValue(WatermarkTemplateProperty, value); }
        }

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// The on got focus.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (this.SelectAllOnGotFocus)
            {
                this.SelectAll();
            }
        }

        /// <summary>
        /// The on preview key down.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!this.AutoMoveFocus)
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            if ((e.Key == Key.Left)
              && ((Keyboard.Modifiers == ModifierKeys.None)
                || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                e.Handled = this.MoveFocusLeft();
            }

            if ((e.Key == Key.Right)
              && ((Keyboard.Modifiers == ModifierKeys.None)
                || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                e.Handled = this.MoveFocusRight();
            }

            if ((e.Key == Key.Return)
              && ((Keyboard.Modifiers == ModifierKeys.None)
                || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                this.MoveFocusRight();
            }

            if (((e.Key == Key.Up) || (e.Key == Key.PageUp))
              && ((Keyboard.Modifiers == ModifierKeys.None)
                || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                e.Handled = this.MoveFocusUp();
            }

            if (((e.Key == Key.Down) || (e.Key == Key.PageDown))
             && ((Keyboard.Modifiers == ModifierKeys.None)
               || (Keyboard.Modifiers == ModifierKeys.Control)))
            {
                e.Handled = this.MoveFocusDown();
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// The on preview mouse left button down.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!this.IsKeyboardFocused && this.SelectAllOnGotFocus)
            {
                e.Handled = true;
                this.Focus();
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        /// <summary>
        /// The on text changed.
        /// </summary>
        /// <param name="e"> The e. </param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (!this.AutoMoveFocus)
            {
                return;
            }

            if ((this.Text.Length != 0) &&
                (this.Text.Length == this.MaxLength) &&
                (this.CaretIndex == this.MaxLength) &&
                this.CanMoveFocus(FocusNavigationDirection.Right, true))
            {
                FocusNavigationDirection direction = (this.FlowDirection == FlowDirection.LeftToRight)
                  ? FocusNavigationDirection.Right
                  : FocusNavigationDirection.Left;

                this.MoveFocus(new TraversalRequest(direction));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The can move focus.
        /// </summary>
        /// <param name="direction"> The direction. </param>
        /// <param name="reachedMax"> The reached max. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool CanMoveFocus(FocusNavigationDirection direction, bool reachedMax)
        {
            QueryMoveFocusEventArgs e = new QueryMoveFocusEventArgs(direction, reachedMax);
            this.RaiseEvent(e);
            return e.CanMoveFocus;
        }

        /// <summary>
        /// The move focus left.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool MoveFocusLeft()
        {
            if (this.FlowDirection == FlowDirection.LeftToRight)
            {
                // Occurs only if the cursor is at the beginning of the text
                if ((this.CaretIndex == 0) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusBack.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusBack.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Left, false))
                    {
                        this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                        return true;
                    }
                }
            }
            else
            {
                // Occurs only if the cursor is at the end of the text
                if ((this.CaretIndex == this.Text.Length) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusBack.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusBack.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Left, false))
                    {
                        this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Left));
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// The move focus right.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool MoveFocusRight()
        {
            if (this.FlowDirection == FlowDirection.LeftToRight)
            {
                // Occurs only if the cursor is at the beginning of the text
                if ((this.CaretIndex == this.Text.Length) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusForward.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusForward.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Right, false))
                    {
                        this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                        return true;
                    }
                }
            }
            else
            {
                // Occurs only if the cursor is at the end of the text
                if ((this.CaretIndex == 0) && (this.SelectionLength == 0))
                {
                    if (ComponentCommands.MoveFocusForward.CanExecute(null, this))
                    {
                        ComponentCommands.MoveFocusForward.Execute(null, this);
                        return true;
                    }
                    else if (this.CanMoveFocus(FocusNavigationDirection.Right, false))
                    {
                        this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Right));
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// The move focus up.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool MoveFocusUp()
        {
            int lineNumber = this.GetLineIndexFromCharacterIndex(this.SelectionStart);

            // Occurs only if the cursor is on the first line
            if (lineNumber == 0)
            {
                if (ComponentCommands.MoveFocusUp.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusUp.Execute(null, this);
                    return true;
                }
                else if (this.CanMoveFocus(FocusNavigationDirection.Up, false))
                {
                    this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The move focus down.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool MoveFocusDown()
        {
            int lineNumber = this.GetLineIndexFromCharacterIndex(this.SelectionStart);

            // Occurs only if the cursor is on the first line
            if (lineNumber == (this.LineCount - 1))
            {
                if (ComponentCommands.MoveFocusDown.CanExecute(null, this))
                {
                    ComponentCommands.MoveFocusDown.Execute(null, this);
                    return true;
                }
                else if (this.CanMoveFocus(FocusNavigationDirection.Down, false))
                {
                    this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Called when the is visible property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible && this.FocusOnVisible)
            {
                this.Focus();
            }
        }

        #endregion
    }
}
