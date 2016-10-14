namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// The button text box.
    /// </summary>
    public class ButtonTextBox : TextBoxExtended
    {
        #region Dependency Properties

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(ButtonTextBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter",
            typeof(object),
            typeof(ButtonTextBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object),
            typeof(ButtonTextBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(ButtonTextBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            "ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ButtonTextBox),
            new PropertyMetadata(null)); 

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="ButtonTextBox"/> class.
        /// </summary>
        static ButtonTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonTextBox), new FrameworkPropertyMetadata(typeof(ButtonTextBox)));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ButtonTextBox"/> class.
        /// </summary>
        public ButtonTextBox()
        {
            this.KeyUp += this.OnKeyUp;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        public object CommandParameter
        {
            get { return (object)this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public object Content
        {
            get { return (object)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)this.GetValue(ContentTemplateProperty); }
            set { this.SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content template selector.
        /// </summary>
        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty); }
            set { this.SetValue(ContentTemplateSelectorProperty, value); }
        } 

        #endregion

        #region Private Methods

        /// <summary>
        /// The on key up.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
                {
                    this.Command.Execute(this.CommandParameter);
                }
            }
        }

        #endregion
    }
}
