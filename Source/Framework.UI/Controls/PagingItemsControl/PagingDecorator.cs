namespace Framework.UI.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;

    /// <summary>
    /// The paging decorator.
    /// </summary>
    public class PagingDecorator : Decorator
    {
        #region Dependency Properties

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items",
            typeof(ObservableCollection<NumberedPage>),
            typeof(PagingDecorator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register(
            "ItemsCount",
            typeof(int),
            typeof(PagingDecorator),
            new PropertyMetadata(-1, OnItemsCountChanged));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(PagingDecorator),
            new PropertyMetadata(0, OnSelectedIndexChanged));

        public static readonly DependencyProperty TransitionDurationProperty = DependencyProperty.Register(
            "TransitionDuration",
            typeof(int),
            typeof(PagingDecorator),
            new PropertyMetadata(600));

        public static readonly DependencyProperty TransitionKeySplineProperty = DependencyProperty.Register(
            "TransitionKeySpline",
            typeof(KeySpline),
            typeof(PagingDecorator),
            new PropertyMetadata(new KeySpline(0, 0.5, 0.5, 1)));

        public static readonly DependencyProperty TranslateTransformProperty = DependencyProperty.Register(
            "TranslateTransform",
            typeof(TranslateTransform),
            typeof(PagingDecorator),
            new PropertyMetadata(null));

        #endregion

        #region Fields

        private Size availableSize;
        private DispatcherTimer resizeTimer;

        #endregion

        /// <summary>
        /// The translate transform changed.
        /// </summary>
        public event EventHandler TranslateTransformChanged;

        #region Public Properties

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public ObservableCollection<NumberedPage> Items
        {
            get { return (ObservableCollection<NumberedPage>)this.GetValue(ItemsProperty); }
            set { this.SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the items count.
        /// </summary>
        public int ItemsCount
        {
            get { return (int)this.GetValue(ItemsCountProperty); }
            set { this.SetValue(ItemsCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected index.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the transition duration.
        /// </summary>
        public int TransitionDuration
        {
            get { return (int)this.GetValue(TransitionDurationProperty); }
            set { this.SetValue(TransitionDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the transition key spline.
        /// </summary>
        public KeySpline TransitionKeySpline
        {
            get { return (KeySpline)this.GetValue(TransitionKeySplineProperty); }
            set { this.SetValue(TransitionKeySplineProperty, value); }
        }

        /// <summary>
        /// Gets or sets the translate transform.
        /// </summary>
        public TranslateTransform TranslateTransform
        {
            get { return (TranslateTransform)this.GetValue(TranslateTransformProperty); }
            set { this.SetValue(TranslateTransformProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The go to page method.
        /// </summary>
        /// <param name="selectedIndex"> The selected index. </param>
        public void GotoPage(int selectedIndex)
        {
            selectedIndex = Math.Min(selectedIndex, this.ItemsCount - 1);
            selectedIndex = Math.Max(selectedIndex, 0);
            this.SelectedIndex = selectedIndex;
            double offset = this.availableSize.Width * selectedIndex * -1;

            TranslateTransform translateTransform = this.Child.RenderTransform as TranslateTransform;
            if (translateTransform == null)
            {
                translateTransform = new TranslateTransform();
                this.TranslateTransform = translateTransform;

                EventHandler eventHandler = this.TranslateTransformChanged;
                if (eventHandler != null)
                {
                    eventHandler(this, EventArgs.Empty);
                }

                this.Child.RenderTransform = translateTransform;
            }

            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames();
            animation.KeyFrames.Add(
                new SplineDoubleKeyFrame()
                {
                    KeySpline = this.TransitionKeySpline,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(this.TransitionDuration)),
                    Value = offset
                });

            translateTransform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The arrange override.
        /// </summary>
        /// <param name="finalSize"> The final size. </param>
        /// <returns> The <see cref="Size"/>. </returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // calculate the number of pages.
            this.ItemsCount = (int)Math.Ceiling(this.Child.DesiredSize.Width / this.availableSize.Width);

            this.Child.Arrange(new Rect(0, 0, this.Child.DesiredSize.Width, this.Child.DesiredSize.Height));

            if (this.resizeTimer == null)
            {
                this.resizeTimer = new DispatcherTimer();
                this.resizeTimer.Interval = TimeSpan.FromMilliseconds(250);
                this.resizeTimer.Tick += this.OnResizeTimerTick;
            }

            this.resizeTimer.Stop();
            this.resizeTimer.Start();

            return this.availableSize;
        }

        /// <summary>
        /// The measure override.
        /// </summary>
        /// <param name="availableSize"> The available size. </param>
        /// <returns> The <see cref="Size"/>. </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            this.availableSize = availableSize;

            this.Child.Measure(new Size(double.PositiveInfinity, this.availableSize.Height));

            return this.Child.DesiredSize;
        }

        /// <summary>
        /// The on selected index changed.
        /// </summary>
        /// <param name="e"> The event arguments. </param>
        protected virtual void OnSelectedIndexChanged(DependencyPropertyChangedEventArgs e)
        {
            this.GotoPage((int)e.NewValue);
        }

        /// <summary>
        /// The on items count changed.
        /// </summary>
        /// <param name="e"> The event arguments. </param>
        protected virtual void OnItemsCountChanged(DependencyPropertyChangedEventArgs e)
        {
            int count = (int)e.NewValue;

            // update the collection
            if (this.Items == null)
            {
                this.Items = new ObservableCollection<NumberedPage>();
            }

            while (this.Items.Count > count)
            {
                this.Items.RemoveAt(this.Items.Count - 1);
            }

            while (this.Items.Count < count)
            {
                this.Items.Add(new NumberedPage(count - (count - this.Items.Count) + 1));
            }

            this.SelectedIndex = Math.Min(this.SelectedIndex, count - 1);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The on selected index changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event Arguments. </param>
        private static void OnSelectedIndexChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ((PagingDecorator)dependencyObject).OnSelectedIndexChanged(e);
        }

        /// <summary>
        /// The on items count changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnItemsCountChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ((PagingDecorator)dependencyObject).OnItemsCountChanged(e);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The on resize timer tick.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnResizeTimerTick(object sender, EventArgs e)
        {
            this.resizeTimer.Stop();
            this.GotoPage(this.SelectedIndex);
        }

        #endregion
    }
}
