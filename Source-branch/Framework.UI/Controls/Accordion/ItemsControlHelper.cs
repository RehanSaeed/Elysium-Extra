namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// The ItemContainerGenerator provides useful utilities for ItemsControls.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    internal sealed partial class ItemsControlHelper
    {
        #region Fields

        /// <summary>
        /// A Panel that is used as the ItemsHost of the ItemsControl.  This
        /// property will only be valid when the ItemsControl is live in the
        /// tree and has generated containers for some of its items.
        /// </summary>
        private Panel itemsHost;

        /// <summary>
        /// A ScrollViewer that is used to scroll the items in the ItemsHost.
        /// </summary>
        private ScrollViewer scrollHost;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ItemsControlHelper"/> class.
        /// </summary>
        /// <param name="control">
        /// The ItemsControl being tracked by the ItemContainerGenerator.
        /// </param>
        internal ItemsControlHelper(ItemsControl control)
        {
            Debug.Assert(control != null, "control cannot be null!");
            ItemsControl = control;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets a Panel that is used as the ItemsHost of the ItemsControl.
        /// This property will only be valid when the ItemsControl is live in
        /// the tree and has generated containers for some of its items.
        /// </summary>
        internal Panel ItemsHost
        {
            get
            {
                // Lookup the ItemsHost if we haven't already cached it.
                if (this.itemsHost == null && ItemsControl != null && ItemsControl.ItemContainerGenerator != null)
                {
                    // Get any live container
                    DependencyObject container = ItemsControl.ItemContainerGenerator.ContainerFromIndex(0);
                    if (container != null)
                    {
                        // Get the parent of the container
                        this.itemsHost = VisualTreeHelper.GetParent(container) as Panel;
                    }
                }

                return this.itemsHost;
            }
        }

        /// <summary>
        /// Gets a ScrollViewer that is used to scroll the items in the
        /// ItemsHost.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
        internal ScrollViewer ScrollHost
        {
            get
            {
                if (this.scrollHost == null)
                {
                    Panel itemsHost = this.ItemsHost;
                    if (itemsHost != null)
                    {
                        for (DependencyObject obj = itemsHost; obj != this.ItemsControl && obj != null; obj = VisualTreeHelper.GetParent(obj))
                        {
                            ScrollViewer viewer = obj as ScrollViewer;
                            if (viewer != null)
                            {
                                this.scrollHost = viewer;
                                break;
                            }
                        }
                    }
                }

                return this.scrollHost;
            }
        } 

        #endregion

        /// <summary>
        /// Gets or sets the ItemsControl being tracked by the
        /// ItemContainerGenerator.
        /// </summary>
        private ItemsControl ItemsControl { get; set; }

        #region Internal Static Methods

        /// <summary>
        /// Prepares the specified container to display the specified item.
        /// </summary>
        /// <param name="element">
        /// Container element used to display the specified item.
        /// </param>
        /// <param name="parentItemContainerStyle">
        /// The ItemContainerStyle for the parent ItemsControl.
        /// </param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Code is linked into multiple projects.")]
        internal static void PrepareContainerForItemOverride(DependencyObject element, Style parentItemContainerStyle)
        {
            // Apply the ItemContainerStyle to the item
            Control control = element as Control;
            if (parentItemContainerStyle != null && control != null && control.Style == null)
            {
                control.SetValue(Control.StyleProperty, parentItemContainerStyle);
            }

            // Note: WPF also does preparation for ContentPresenter,
            // ContentControl, HeaderedContentControl, and ItemsControl.  Since
            // we don't have any other ItemsControls using this
            // ItemContainerGenerator, we've removed that code for now.  It
            // should be added back later when necessary.
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Apply a control template to the ItemsControl.
        /// </summary>
        internal void OnApplyTemplate()
        {
            // Clear the cached ItemsHost, ScrollHost
            this.itemsHost = null;
            this.scrollHost = null;
        }

        /// <summary>
        /// Update the style of any generated items when the ItemContainerStyle
        /// has been changed.
        /// </summary>
        /// <param name="itemContainerStyle">The ItemContainerStyle.</param>
        /// <remarks>
        /// Silverlight does not support setting a Style multiple times, so we
        /// only attempt to set styles on elements whose style hasn't already
        /// been set.
        /// </remarks>
        internal void UpdateItemContainerStyle(Style itemContainerStyle)
        {
            if (itemContainerStyle == null)
            {
                return;
            }

            Panel itemsHost = this.ItemsHost;
            if (itemsHost == null || itemsHost.Children == null)
            {
                return;
            }

            foreach (UIElement element in itemsHost.Children)
            {
                FrameworkElement obj = element as FrameworkElement;
                if (obj.Style == null)
                {
                    obj.Style = itemContainerStyle;
                }
            }
        }

        /// <summary>
        /// Scroll the desired element into the ScrollHost's viewport.
        /// </summary>
        /// <param name="element">Element to scroll into view.</param>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "File is linked across multiple projects and this method is used in some but not others.")]
        internal void ScrollIntoView(FrameworkElement element)
        {
            // Get the ScrollHost
            ScrollViewer scrollHost = this.ScrollHost;
            if (scrollHost == null)
            {
                return;
            }

            // Get the position of the element relative to the ScrollHost
            GeneralTransform transform = null;
            try
            {
                transform = element.TransformToVisual(scrollHost);
            }
            catch (ArgumentException)
            {
                // Ignore failures when not in the visual tree
                return;
            }

            Rect itemRect = new Rect(
                transform.Transform(new Point()),
                transform.Transform(new Point(element.ActualWidth, element.ActualHeight)));

            // Scroll vertically
            double verticalOffset = scrollHost.VerticalOffset;
            double verticalDelta = 0;
            double hostBottom = scrollHost.ViewportHeight;
            double itemBottom = itemRect.Bottom;
            if (hostBottom < itemBottom)
            {
                verticalDelta = itemBottom - hostBottom;
                verticalOffset += verticalDelta;
            }

            double itemTop = itemRect.Top;
            if (itemTop - verticalDelta < 0)
            {
                verticalOffset -= verticalDelta - itemTop;
            }

            scrollHost.ScrollToVerticalOffset(verticalOffset);

            // Scroll horizontally
            double horizontalOffset = scrollHost.HorizontalOffset;
            double horizontalDelta = 0;
            double hostRight = scrollHost.ViewportWidth;
            double itemRight = itemRect.Right;
            if (hostRight < itemRight)
            {
                horizontalDelta = itemRight - hostRight;
                horizontalOffset += horizontalDelta;
            }

            double itemLeft = itemRect.Left;
            if (itemLeft - horizontalDelta < 0)
            {
                horizontalOffset -= horizontalDelta - itemLeft;
            }

            scrollHost.ScrollToHorizontalOffset(horizontalOffset);
        }

        #endregion
    }
}