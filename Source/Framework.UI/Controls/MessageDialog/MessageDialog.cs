namespace Framework.UI.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Framework.UI.Input;

    public sealed class MessageDialog : ItemsControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty CancelCommandIndexProperty = DependencyProperty.Register(
            "CancelCommandIndex",
            typeof(int),
            typeof(MessageDialog),
            new PropertyMetadata(0));

        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(object),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            "ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DefaultCommandIndexProperty = DependencyProperty.Register(
            "DefaultCommandIndex",
            typeof(int),
            typeof(MessageDialog),
            new PropertyMetadata(0));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(object),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate",
            typeof(DataTemplate),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register(
            "HeaderTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(object),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
            "IconTemplate",
            typeof(DataTemplate),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register(
            "IconTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(MessageDialog),
            new PropertyMetadata(null));

        #endregion

        private ListBox listBox;

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="MessageDialog"/> class.
        /// </summary>
        static MessageDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageDialog), new FrameworkPropertyMetadata(typeof(MessageDialog)));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MessageDialog"/> class.
        /// </summary>
        public MessageDialog()
        {
            this.Loaded += this.OnLoaded;
        }

        #endregion

        #region Public Properties

        public int CancelCommandIndex
        {
            get { return (int)this.GetValue(CancelCommandIndexProperty); }
            set { this.SetValue(CancelCommandIndexProperty, value); }
        }

        public object Content
        {
            get { return (object)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)this.GetValue(ContentTemplateProperty); }
            set { this.SetValue(ContentTemplateProperty, value); }
        }

        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty); }
            set { this.SetValue(ContentTemplateSelectorProperty, value); }
        }

        public int DefaultCommandIndex
        {
            get { return (int)this.GetValue(DefaultCommandIndexProperty); }
            set { this.SetValue(DefaultCommandIndexProperty, value); }
        }

        public object Header
        {
            get { return (object)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)this.GetValue(HeaderTemplateProperty); }
            set { this.SetValue(HeaderTemplateProperty, value); }
        }

        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(HeaderTemplateSelectorProperty); }
            set { this.SetValue(HeaderTemplateSelectorProperty, value); }
        }

        public object Icon
        {
            get { return (object)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)this.GetValue(IconTemplateProperty); }
            set { this.SetValue(IconTemplateProperty, value); }
        }

        public DataTemplateSelector IconTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(IconTemplateSelectorProperty); }
            set { this.SetValue(IconTemplateSelectorProperty, value); }
        }

        #endregion

        #region Public Static Methods

        public static Task<MessageBoxResult> ShowAsync(
            string header,
            string content,
            IEnumerable<MessageDialogButton> buttons,
            MessageDialogType type = MessageDialogType.Light,
            System.Windows.Window owner = null,
            int defaultCommandIndex = 0,
            int cancelCommandIndex = -1)
        {
            TaskCompletionSource<MessageBoxResult> taskCompletionSource = new TaskCompletionSource<MessageBoxResult>();

            if (owner == null)
            {
                owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            }

            MessageDialog messageDialog = new MessageDialog()
            {
                Header = header,
                Content = content
            };

            OverlayWindow window = new OverlayWindow()
            {
                Content = messageDialog,
                Owner = owner
            };

            switch (type)
            {
                case MessageDialogType.Accent:
                    window.Style = (Style)window.FindResource("AccentOverlayWindowStyle");
                    break;
                case MessageDialogType.Dark:
                    window.Style = (Style)window.FindResource("DarkOverlayWindowStyle");
                    break;
            }

            foreach (MessageDialogButton button in buttons)
            {
                messageDialog.Items.Add(button);
            }

            if (cancelCommandIndex == -1)
            {
                messageDialog.CancelCommandIndex = messageDialog.Items.Count - 1;
            }

            window.Show();

            messageDialog
                .FindVisualChildren<Button>()
                .ToList()
                .ForEach(
                    x => x.Click += (sender, e) =>
                    {
                        if (!taskCompletionSource.Task.IsCompleted)
                        {
                            window.Close();
                            taskCompletionSource.SetResult(MessageBoxResult.None);
                        }
                    });

            return taskCompletionSource.Task;
        }

        public static Task<MessageBoxResult> ShowAsync(
            string header,
            string content,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageDialogType type = MessageDialogType.Light,
            System.Windows.Window owner = null)
        {
            TaskCompletionSource<MessageBoxResult> taskCompletionSource = new TaskCompletionSource<MessageBoxResult>();

            if (owner == null)
            {
                owner = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            }

            MessageDialog messageDialog = new MessageDialog()
            {
                Content = content
            };

            OverlayWindow window = new OverlayWindow()
            {
                Content = messageDialog,
                Owner = owner
            };

            switch (type)
            {
                case MessageDialogType.Accent:
                    window.Style = (Style)window.FindResource("AccentOverlayWindowStyle");
                    break;
                case MessageDialogType.Dark:
                    window.Style = (Style)window.FindResource("DarkOverlayWindowStyle");
                    break;
            }

            messageDialog.Header = new TextBlock()
            {
                Style = (Style)window.FindResource("HeaderTextStyle"),
                Text = header
            };

            if ((button == MessageBoxButton.OK) ||
                (button == MessageBoxButton.OKCancel))
            {
                messageDialog.Items.Add(
                    new MessageDialogButton()
                    {
                        Command = new DelegateCommand(
                            () =>
                            {
                                if (!taskCompletionSource.Task.IsCompleted)
                                {
                                    window.Close();
                                    taskCompletionSource.SetResult(MessageBoxResult.OK);
                                }
                            }),
                        Content = "Ok"
                    });
            }

            if ((button == MessageBoxButton.YesNo) ||
                (button == MessageBoxButton.YesNoCancel))
            {
                messageDialog.Items.Add(
                    new MessageDialogButton()
                    {
                        Command = new DelegateCommand(
                            () =>
                            {
                                if (!taskCompletionSource.Task.IsCompleted)
                                {
                                    window.Close();
                                    taskCompletionSource.SetResult(MessageBoxResult.Yes);
                                }
                            }),
                        Content = "Yes"
                    });
                messageDialog.Items.Add(
                    new MessageDialogButton()
                    {
                        Command = new DelegateCommand(
                            () =>
                            {
                                if (!taskCompletionSource.Task.IsCompleted)
                                {
                                    window.Close();
                                    taskCompletionSource.SetResult(MessageBoxResult.No);
                                }
                            }),
                        Content = "No"
                    });
            }

            if ((button == MessageBoxButton.OKCancel) ||
                (button == MessageBoxButton.YesNoCancel))
            {
                messageDialog.Items.Add(
                    new MessageDialogButton()
                    {
                        Command = new DelegateCommand(
                            () =>
                            {
                                if (!taskCompletionSource.Task.IsCompleted)
                                {
                                    window.Close();
                                    taskCompletionSource.SetResult(MessageBoxResult.Cancel);
                                }
                            }),
                        Content = "Cancel"
                    });
            }

            messageDialog.CancelCommandIndex = messageDialog.Items.Count - 1;
            window.Show();

            return taskCompletionSource.Task;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The get container for item override.
        /// </summary>
        /// <returns>
        /// The <see cref="DependencyObject"/>.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MessageDialogButton();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.listBox = (ListBox)this.GetTemplateChild("PART_Buttons");
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            ListBoxItem firstListBoxItem = this.listBox.FindVisualChildren<ListBoxItem>().FirstOrDefault();
            if (firstListBoxItem != null)
            {
                firstListBoxItem.FindVisualChild<Button>().Focus();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyUp" /> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.KeyEventArgs" /> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (this.IsHitTestVisible)
            {
                if ((e.Key == Key.Enter) &&
                    (this.DefaultCommandIndex >= 0) &&
                    (this.DefaultCommandIndex < this.Items.Count))
                {
                    MessageDialogButton messageDialogCommand = this.Items
                        .OfType<MessageDialogButton>()
                        .ElementAtOrDefault(this.DefaultCommandIndex);
                    if ((messageDialogCommand != null) &&
                        messageDialogCommand.Command.CanExecute(messageDialogCommand.CommandParameter))
                    {
                        messageDialogCommand.Command.Execute(messageDialogCommand.CommandParameter);
                    }
                }

                if ((e.Key == Key.Escape) &&
                    (this.CancelCommandIndex >= 0) &&
                    (this.CancelCommandIndex < this.Items.Count))
                {
                    MessageDialogButton messageDialogCommand = this.Items
                        .OfType<MessageDialogButton>()
                        .ElementAtOrDefault(this.CancelCommandIndex);
                    if ((messageDialogCommand != null) &&
                        messageDialogCommand.Command.CanExecute(messageDialogCommand.CommandParameter))
                    {
                        messageDialogCommand.Command.Execute(messageDialogCommand.CommandParameter);
                    }
                }
            }
        }

        #endregion
    }
}
