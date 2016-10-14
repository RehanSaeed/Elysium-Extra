namespace Framework.UI
{
    using System.Collections.ObjectModel;
    using System.Windows;

    /// <summary>
    /// Enables visibility of when an object is being dragged.
    /// </summary>
    public static class DragViewer
    {
        #region Dependency Properties

        public static readonly DependencyProperty FormatProperty = DependencyProperty.RegisterAttached(
            "Format",
            typeof(string),
            typeof(DragViewer),
            new UIPropertyMetadata(null, OnFormatChanged));

        private static readonly DependencyPropertyKey IsDraggingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsDragging",
            typeof(bool),
            typeof(DragViewer),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

        #endregion

        public static readonly RoutedEvent DragFinishedEvent = EventManager.RegisterRoutedEvent(
            "DragFinished",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(DragViewer));

        public static readonly RoutedEvent DragStartedEvent = EventManager.RegisterRoutedEvent(
            "DragStarted",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(DragViewer));

        #region Fields
        
        /// <summary>
        /// A collection of formats currently being dragged.
        /// </summary>
        private static ObservableCollection<string> draggingFormats = new ObservableCollection<string>(); 

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Adds the drag finished event handler.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="eventHandler">The event handler.</param>
        public static void AddDragFinishedHandler(DependencyObject dependencyObject, RoutedEventHandler eventHandler)
        {
            UIElement uiElement = dependencyObject as UIElement;
            if (uiElement != null)
            {
                uiElement.AddHandler(DragViewer.DragFinishedEvent, eventHandler);
            }
        }

        /// <summary>
        /// Adds the drag started event handler.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="eventHandler">The event handler.</param>
        public static void AddDragStartedHandler(DependencyObject dependencyObject, RoutedEventHandler eventHandler)
        {
            UIElement uiElement = dependencyObject as UIElement;
            if (uiElement != null)
            {
                uiElement.AddHandler(DragViewer.DragStartedEvent, eventHandler);
            }
        }

        /// <summary>
        /// Gets the drag and drop format. A unique ID for the data being dragged.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <returns>The drag and drop format.</returns>
        public static string GetFormat(DependencyObject obj)
        {
            return (string)obj.GetValue(FormatProperty);
        }

        /// <summary>
        /// Gets a value determining if the content is being dragged.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <returns><c>true</c> if the content is being dragged, otherwise <c>false</c>.</returns>
        public static bool GetIsDragging(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsDraggingProperty);
        }

        /// <summary>
        /// Removes the drag finished event handler.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="eventHandler">The event handler.</param>
        public static void RemoveDragFinishedHandler(DependencyObject dependencyObject, RoutedEventHandler eventHandler)
        {
            UIElement uiElement = dependencyObject as UIElement;
            if (uiElement != null)
            {
                uiElement.RemoveHandler(DragViewer.DragFinishedEvent, eventHandler);
            }
        }

        /// <summary>
        /// Removes the drag started event handler.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="eventHandler">The event handler.</param>
        public static void RemoveDragStartedHandler(DependencyObject dependencyObject, RoutedEventHandler eventHandler)
        {
            UIElement uiElement = dependencyObject as UIElement;
            if (uiElement != null)
            {
                uiElement.RemoveHandler(DragViewer.DragStartedEvent, eventHandler);
            }
        }

        /// <summary>
        /// Sets the drag and drop format. A unique ID for the data being dragged.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <param name="value">The drag and drop format.</param>
        public static void SetFormat(DependencyObject obj, string value)
        {
            obj.SetValue(FormatProperty, value);
        }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Sets the value determining if the specified value is being dragged.
        /// </summary>
        /// <param name="format">The drag format.</param>
        /// <param name="isDragging">if set to <c>true</c> the specified format is being dragged.</param>
        internal static void SetIsDragging(string format, bool isDragging)
        {
            if (isDragging)
            {
                if (!draggingFormats.Contains(format))
                {
                    draggingFormats.Add(format);
                }
            }
            else
            {
                if (draggingFormats.Contains(format))
                {
                    draggingFormats.Remove(format);
                }
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the format is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnFormatChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = dependencyObject as UIElement;

            if (uiElement != null)
            {
                draggingFormats.CollectionChanged +=
                    (sender, e2) =>
                    {
                        string format = GetFormat(uiElement);
                        SetIsDragging(uiElement, draggingFormats.Contains(format));
                    };
            }
        }

        /// <summary>
        /// Sets the value determining if the content is being dragged.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">if set to <c>true</c> the content is being dragged.</param>
        private static void SetIsDragging(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsDraggingPropertyKey, value);
        }

        #endregion
    }
}
