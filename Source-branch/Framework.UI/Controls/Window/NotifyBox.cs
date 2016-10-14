namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    /// <summary>
    /// Shows Outlook style notification windows above the Windows tray icons.
    /// </summary>
    public static class NotifyBox
    {
        #region Fields

        /// <summary>
        /// The margin between notification windows.
        /// </summary>
        private const double Margin = 5;

        /// <summary>
        /// The default display duration for a notification window.
        /// </summary>
        private static readonly TimeSpan DefaultDisplayDuration = TimeSpan.FromSeconds(4);

        private static List<WindowInfo> windows;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="NotifyBox"/> class.
        /// </summary>
        static NotifyBox()
        {
            windows = new List<WindowInfo>();
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Shows the specified notification for four seconds.
        /// </summary>
        /// <param name="icon">The notification icon.</param>
        /// <param name="title">The notification title.</param>
        /// <param name="content">The notification content.</param>
        public static void Show(ImageSource icon, string title, object content)
        {
            Show(icon, title, content, DefaultDisplayDuration, false);
        }

        /// <summary>
        /// Shows the specified notification for user defined number of seconds.
        /// </summary>
        /// <param name="icon">The notification icon.</param>
        /// <param name="title">The notification title.</param>
        /// <param name="content">The notification content.</param>
        /// <param name="displayDuration">The notification display duration.</param>
        public static void Show(ImageSource icon, string title, object content, TimeSpan displayDuration)
        {
            Show(icon, title, content, displayDuration, false);
        }

        /// <summary>
        /// Shows the specified notification for user defined number of seconds.
        /// </summary>
        /// <param name="icon">The notification icon.</param>
        /// <param name="title">The notification title.</param>
        /// <param name="content">The notification content.</param>
        /// <param name="isDoubleHeight">Option to double the notification height</param>
        public static void Show(ImageSource icon, string title, object content, bool isDoubleHeight)
        {
            Show(icon, title, content, DefaultDisplayDuration, isDoubleHeight);
        }

        /// <summary>
        /// Shows the specified notification.
        /// </summary>
        /// <param name="icon">The notification icon.</param>
        /// <param name="title">The notification title.</param>
        /// <param name="content">The notification content.</param>
        /// <param name="displayDuration">The notification display duration.</param>
        /// <param name="isDoubleHeight">Option to double the notification height</param>
        public static void Show(ImageSource icon, string title, object content, TimeSpan displayDuration, bool isDoubleHeight)
        {
            object contentWrapper = content;
            if (content is string)
            {
                // If the content is just a string, we need to put it in a TextBlock so we can wrap the text.
                contentWrapper = new System.Windows.Controls.TextBlock()
                {
                    Text = (string)content,
                    TextWrapping = TextWrapping.Wrap
                };
            }

            Window window = new Window()
            {
                Content = contentWrapper,
                Icon = icon,
                Title = title
            };
            window.Style = (Style)window.FindResource("NotifyBoxWindowStyle");
            if (isDoubleHeight)
            {
                window.Height *= 2;
            }

            Show(window, displayDuration);
        }

        /// <summary>
        /// Shows the specified window as a notification.
        /// </summary>
        /// <param name="window">The window.</param>
        public static void Show(Window window)
        {
            Show(window, DefaultDisplayDuration);
        }

        /// <summary>
        /// Shows the specified window as a notification.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="displayDuration">The display duration.</param>
        public static void Show(Window window, TimeSpan displayDuration)
        {
            BehaviorCollection behaviors = Interaction.GetBehaviors(window);
            behaviors.Add(new FadeBehavior());
            behaviors.Add(new SlideBehavior());

            window.Top = SystemParameters.WorkArea.Height - window.Height - window.Margin.Top -
                windows.Sum(x => x.Window.Height - x.Window.Margin.Top + Margin);
            window.Left = SystemParameters.WorkArea.Width - window.Width - window.Margin.Right - Margin;

            WindowInfo windowInfo = new WindowInfo()
            {
                DisplayDuration = displayDuration,
                Window = window
            };
            windows.Add(windowInfo);
            window.Show();

            Observable
                .Timer(displayDuration)
                .ObserveOnDispatcher()
                .Subscribe(x => OnTimerElapsed(windowInfo));
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the timer has elapsed. Removes any stale notifications.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private static void OnTimerElapsed(WindowInfo windowInfo)
        {
            DateTime now = DateTime.Now;

            if (windowInfo.Window.IsMouseOver)
            {
                Observable
                    .Timer(windowInfo.DisplayDuration)
                    .ObserveOnDispatcher()
                    .Subscribe(x => OnTimerElapsed(windowInfo));
            }
            else
            {
                BehaviorCollection behaviors = Interaction.GetBehaviors(windowInfo.Window);
                FadeBehavior fadeBehavior = behaviors.OfType<FadeBehavior>().First();
                SlideBehavior slideBehavior = behaviors.OfType<SlideBehavior>().First();

                fadeBehavior.FadeOut();
                slideBehavior.SlideOut();

                EventHandler eventHandler = null;
                eventHandler = (sender2, e2) =>
                {
                    fadeBehavior.FadeOutCompleted -= eventHandler;
                    windows.Remove(windowInfo);
                    windowInfo.Window.Close();
                };
                fadeBehavior.FadeOutCompleted += eventHandler;
            }
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Window metadata.
        /// </summary>
        private sealed class WindowInfo
        {
            /// <summary>
            /// Gets or sets the display duration.
            /// </summary>
            /// <value>
            /// The display duration.
            /// </value>
            public TimeSpan DisplayDuration { get; set; }

            /// <summary>
            /// Gets or sets the window.
            /// </summary>
            /// <value>
            /// The window.
            /// </value>
            public Window Window { get; set; }
        }

        #endregion
    }
}
