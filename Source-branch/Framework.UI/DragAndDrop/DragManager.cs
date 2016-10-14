namespace Framework.UI
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Enables and initiates a drag action on an object.
    /// </summary>
    public sealed class DragManager
    {
        #region Attached Properties

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.RegisterAttached(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(DragManager),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty DataProperty = DependencyProperty.RegisterAttached(
            "Data",
            typeof(object),
            typeof(DragManager),
            new UIPropertyMetadata(null));

        public static readonly DependencyProperty EffectsProperty = DependencyProperty.RegisterAttached(
            "Effects",
            typeof(DragDropEffects),
            typeof(DragManager),
            new UIPropertyMetadata(DragDropEffects.Copy));

        public static readonly DependencyProperty FormatProperty = DependencyProperty.RegisterAttached(
            "Format",
            typeof(string),
            typeof(DragManager),
            new UIPropertyMetadata(null, OnFormatChanged));

        public static readonly DependencyProperty IsContentVisibleProperty = DependencyProperty.RegisterAttached(
            "IsContentVisible",
            typeof(bool),
            typeof(DragManager),
            new UIPropertyMetadata(true, OnIsContentVisibleChanged));

        private static readonly DependencyPropertyKey IsDraggingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsDragging",
            typeof(bool),
            typeof(DragManager),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsDraggingProperty = IsDraggingPropertyKey.DependencyProperty;

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled", 
            typeof(bool), 
            typeof(DragManager), 
            new PropertyMetadata(true));

        #endregion

        #region Fields
        
        private static Point dragStartPoint;
        private static Popup popup;
        private static bool hasMouseUp; 

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the drag and drop content data template.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <returns>The drag and drop content template.</returns>
        public static DataTemplate GetContentTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(ContentTemplateProperty);
        }

        /// <summary>
        /// Gets the drag and drop data.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <returns>The drag and drop data.</returns>
        public static object GetData(DependencyObject obj)
        {
            return (object)obj.GetValue(DataProperty);
        }

        /// <summary>
        /// Gets the drag and drop effects. This has an effect on mouse cursor look and feel.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <returns>The drag and drop effects.</returns>
        public static DragDropEffects GetEffects(DependencyObject obj)
        {
            return (DragDropEffects)obj.GetValue(EffectsProperty);
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
        /// Gets a value determining whether drag and drop is visible.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <returns><c>true</c> if visible, otherwise <c>false</c>.</returns>
        public static bool GetIsContentVisible(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsContentVisibleProperty);
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
        /// Gets a value determining whether drag and drop is enabled.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <returns><c>true</c> if enabled, otherwise <c>false</c>.</returns>
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        /// <summary>
        /// Sets a value determining whether drag and drop is enabled.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <param name="value">if set to <c>true</c> drag and drop is enabled.</param>
        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        /// <summary>
        /// Sets the content data template for the data being drag and dropped.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <param name="value">The content data template.</param>
        public static void SetContentTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(ContentTemplateProperty, value);
        }

        /// <summary>
        /// Sets the data being drag and dropped.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <param name="value">The drag and drop data.</param>
        public static void SetData(DependencyObject obj, object value)
        {
            obj.SetValue(DataProperty, value);
        }

        /// <summary>
        /// Sets the drag and drop effects. This has an effect on mouse cursor style.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <param name="value">The drag and drop effects.</param>
        public static void SetEffects(DependencyObject obj, DragDropEffects value)
        {
            obj.SetValue(EffectsProperty, value);
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

        /// <summary>
        /// Sets a value determining whether drag and drop is visible.
        /// </summary>
        /// <param name="obj">The drag and drop object.</param>
        /// <param name="value">if set to <c>true</c> drag and drop is visible.</param>
        public static void SetIsContentVisible(DependencyObject obj, bool value)
        {
            obj.SetValue(IsContentVisibleProperty, value);
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
                uiElement.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
                uiElement.PreviewMouseMove += OnPreviewMouseMove;

                UpdateIsContentVisible(uiElement);
            }
        }

        /// <summary>
        /// Called when the is content visible is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsContentVisibleChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            UIElement uiElement = dependencyObject as UIElement;

            UpdateIsContentVisible(uiElement);
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

        /// <summary>
        /// Updates the drag and drop operation based on the is content visible property.
        /// </summary>
        /// <param name="uiElement">The UI element.</param>
        private static void UpdateIsContentVisible(UIElement uiElement)
        {
            if (uiElement != null)
            {
                if (GetIsContentVisible(uiElement))
                {
                    uiElement.PreviewGiveFeedback += OnPreviewGiveFeedback;
                    uiElement.PreviewQueryContinueDrag += OnPreviewQueryContinueDrag;
                }
                else
                {
                    uiElement.PreviewGiveFeedback -= OnPreviewGiveFeedback;
                    uiElement.PreviewQueryContinueDrag -= OnPreviewQueryContinueDrag;
                }
            }
        }

        /// <summary>
        /// Called when the preview mouse left button down is pressed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private static void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement dragSource = (FrameworkElement)sender;

            if (GetIsEnabled(dragSource))
            {
                hasMouseUp = false;
                dragStartPoint = e.GetPosition(null);
            }
        }

        /// <summary>
        /// Called when the preview mouse move is called.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private static void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement dragSource = (FrameworkElement)sender;

            if (GetIsEnabled(dragSource))
            {
                Point mousePoint = e.GetPosition(null);
                Vector difference = dragStartPoint - mousePoint;

                if (e.LeftButton == MouseButtonState.Released)
                {
                    hasMouseUp = true;
                }
                else if (!hasMouseUp &&
                    ((Math.Abs(difference.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(difference.Y) > SystemParameters.MinimumVerticalDragDistance)))
                {
                    object data = GetData(dragSource);
                    DragDropEffects dragDropEffects = GetEffects(dragSource);
                    string format = GetFormat(dragSource);

                    if ((data != null) && !string.IsNullOrWhiteSpace(format))
                    {
                        DataObject dragObject = new DataObject(format, data);

                        SetIsDragging(dragSource, true);
                        DragViewer.SetIsDragging(format, true);
                        DragDrop.DoDragDrop(dragSource, dragObject, dragDropEffects);
                        
                        SetIsDragging(dragSource, false);
                        if (popup != null)
                        {
                            popup.IsOpen = false;
                            popup = null;
                        }

                        DragViewer.SetIsDragging(format, false);
                    }
                }
            }
        }

        /// <summary>
        /// Called when the preview give feedback is called.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GiveFeedbackEventArgs"/> instance containing the event data.</param>
        private static void OnPreviewGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            FrameworkElement dragSource = (FrameworkElement)sender;

            if (popup == null)
            {
                DataTemplate dataTemplate = GetContentTemplate(dragSource);

                FrameworkElement popupContent;
                if (dataTemplate == null)
                {
                    popupContent = CloneFrameworkElement(dragSource);
                }
                else
                {
                    popupContent = new ContentPresenter()
                    {
                        Content = GetData(dragSource),
                        ContentTemplate = dataTemplate,
                    };
                }

                popup = new Popup()
                {
                    AllowsTransparency = true,
                    Child = popupContent,
                    DataContext = dragSource.DataContext,
                    IsHitTestVisible = false,
                    IsOpen = true
                };
            }

            System.Drawing.Point cursorPosition = System.Windows.Forms.Cursor.Position;
            Point mousePoint = new Point(cursorPosition.X, cursorPosition.Y);

            popup.HorizontalOffset = mousePoint.X + 20;
            popup.VerticalOffset = mousePoint.Y + 5;
        }

        /// <summary>
        /// Called when the preview query continue drag is called.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="QueryContinueDragEventArgs"/> instance containing the event data.</param>
        private static void OnPreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if ((e.EscapePressed ||
                ((e.KeyStates & DragDropKeyStates.LeftMouseButton) != DragDropKeyStates.LeftMouseButton)) && 
                (popup != null))
            {
                popup.IsOpen = false;
                popup = null;
            }
        }

        /// <summary>
        /// Clones the framework element by creating a reflection of it using a <see cref="VisualBrush"/>.
        /// </summary>
        /// <param name="frameworkElement">The framework element.</param>
        /// <returns>The cloned reflection.</returns>
        private static FrameworkElement CloneFrameworkElement(FrameworkElement frameworkElement)
        {
            Rectangle rectangle = new Rectangle()
            {
                Height = frameworkElement.ActualHeight,
                Fill = new VisualBrush(frameworkElement),
                Opacity = 0.7,
                Width = frameworkElement.ActualWidth
            };

            return rectangle;
        }

        #endregion
    }
}
