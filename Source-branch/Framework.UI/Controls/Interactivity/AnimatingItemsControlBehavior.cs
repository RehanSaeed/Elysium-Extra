namespace Framework.UI.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interactivity;

    /// <summary>
    /// The animating items control behaviour.
    /// </summary>
    public sealed class AnimatingItemsControlBehavior : Behavior<ItemsControl>
    {
        #region Dependency Properties

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register(
            "Duration",
            typeof(TimeSpan),
            typeof(AnimatingItemsControlBehavior),
            new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 300)));

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(
            "Interval",
            typeof(TimeSpan),
            typeof(AnimatingItemsControlBehavior),
            new PropertyMetadata(new TimeSpan(0, 0, 0, 0, 100)));

        public static readonly DependencyProperty IsAnimatingOnIsVisibleChangedProperty = DependencyProperty.Register(
            "IsAnimatingOnIsVisibleChanged",
            typeof(bool),
            typeof(AnimatingItemsControlBehavior),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsAnimatingOnLoadedProperty = DependencyProperty.Register(
            "IsAnimatingOnLoaded",
            typeof(bool),
            typeof(AnimatingItemsControlBehavior),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsRandomProperty = DependencyProperty.Register(
            "IsRandom",
            typeof(bool),
            typeof(AnimatingItemsControlBehavior),
            new PropertyMetadata(false));

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.Register(
            "Offset",
            typeof(double),
            typeof(AnimatingItemsControlBehavior),
            new PropertyMetadata(40.0D));

        #endregion

        private Random random;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="AnimatingItemsControlBehavior"/> class.
        /// </summary>
        public AnimatingItemsControlBehavior()
        {
            this.random = new Random();
        }

        #endregion

        #region Events

        /// <summary>
        /// The animate in completed.
        /// </summary>
        public event EventHandler AnimateInCompleted;

        /// <summary>
        /// The animate out completed.
        /// </summary>
        public event EventHandler AnimateOutCompleted;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get { return (TimeSpan)this.GetValue(DurationProperty); }
            set { this.SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        public TimeSpan Interval
        {
            get { return (TimeSpan)this.GetValue(IntervalProperty); }
            set { this.SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is animating on is visible changed.
        /// </summary>
        public bool IsAnimatingOnIsVisibleChanged
        {
            get { return (bool)this.GetValue(IsAnimatingOnIsVisibleChangedProperty); }
            set { this.SetValue(IsAnimatingOnIsVisibleChangedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is animating on loaded.
        /// </summary>
        public bool IsAnimatingOnLoaded
        {
            get { return (bool)this.GetValue(IsAnimatingOnLoadedProperty); }
            set { this.SetValue(IsAnimatingOnLoadedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is random.
        /// </summary>
        public bool IsRandom
        {
            get { return (bool)this.GetValue(IsRandomProperty); }
            set { this.SetValue(IsRandomProperty, value); }
        }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public double Offset
        {
            get { return (double)this.GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The animate in.
        /// </summary>
        public void AnimateIn()
        {
            if (this.AssociatedObject.Items.Count == 0)
            {
                this.OnAnimateInCompleted(this, EventArgs.Empty);
            }
            else
            {
                if (this.AssociatedObject.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    TimeSpan beginTime = this.Interval;

                    foreach (object item in this.AssociatedObject.Items)
                    {
                        FrameworkElement frameworkElement = (FrameworkElement)this.AssociatedObject.ItemContainerGenerator.ContainerFromItem(item);
                        if (frameworkElement != null)
                        {
                            bool isLast = this.AssociatedObject.Items.IndexOf(item) == this.AssociatedObject.Items.Count - 1;

                            if (this.IsRandom)
                            {
                                if (isLast)
                                {
                                    beginTime = new TimeSpan(0, 0, 0, 0, 7 * 35);
                                }
                                else
                                {
                                    beginTime = new TimeSpan(0, 0, 0, 0, this.random.Next(0, 7) * 35);
                                }
                            }
                            else
                            {
                                beginTime = beginTime + this.Interval;
                            }

                            FadeBehavior fadeBehavior = GetFadeBehavior(
                                frameworkElement,
                                beginTime,
                                this.Duration);
                            SlideBehavior slideBehavior = GetSlideBehavior(
                                frameworkElement,
                                beginTime,
                                this.Duration,
                                this.Offset);

                            if (isLast)
                            {
                                EventHandler eventHandler = null;
                                eventHandler = (sender, e) =>
                                    {
                                        slideBehavior.SlideInCompleted -= eventHandler;
                                        this.OnAnimateInCompleted(this, EventArgs.Empty);
                                    };
                                slideBehavior.SlideInCompleted += eventHandler;
                            }

                            fadeBehavior.FadeIn();
                            slideBehavior.SlideIn();
                        }
                    }
                }
                else
                {
                    EventHandler eventHandler = null;
                    eventHandler =
                        (sender, e) =>
                        {
                            this.AssociatedObject.ItemContainerGenerator.StatusChanged -= eventHandler;
                            this.AnimateIn();
                        };
                    this.AssociatedObject.ItemContainerGenerator.StatusChanged += eventHandler;
                }
            }
        }

        /// <summary>
        /// The animate out.
        /// </summary>
        public void AnimateOut()
        {
            if (this.AssociatedObject.Items.Count == 0)
            {
                this.OnAnimateOutCompleted(this, EventArgs.Empty);
            }
            else
            {
                if (this.AssociatedObject.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    TimeSpan beginTime = this.Interval;

                    FrameworkElement selectedItem = GetSelectedItem(this.AssociatedObject);

                    foreach (object item in this.AssociatedObject.Items.Cast<object>().Reverse())
                    {
                        FrameworkElement frameworkElement = (FrameworkElement)this.AssociatedObject.ItemContainerGenerator.ContainerFromItem(item);
                        if (frameworkElement != null)
                        {
                            bool isFirst = this.AssociatedObject.Items.IndexOf(item) == 0;

                            if (this.IsRandom)
                            {
                                if (isFirst)
                                {
                                    beginTime = new TimeSpan(0, 0, 0, 0, 7 * 35);
                                }
                                else
                                {
                                    beginTime = new TimeSpan(0, 0, 0, 0, this.random.Next(0, 7) * 35);
                                }
                            }
                            else
                            {
                                beginTime = beginTime + this.Interval;
                            }

                            FadeBehavior fadeBehavior = GetFadeBehavior(
                                frameworkElement,
                                beginTime,
                                this.Duration);
                            SlideBehavior slideBehavior = GetSlideBehavior(
                                frameworkElement,
                                beginTime,
                                this.Duration,
                                this.Offset);

                            if (isFirst)
                            {
                                if (item != selectedItem)
                                {
                                    EventHandler eventHandler = null;
                                    eventHandler = (sender, e) =>
                                    {
                                        slideBehavior.SlideOutCompleted -= eventHandler;
                                        this.OnAnimateOutCompleted(this, EventArgs.Empty);
                                    };
                                    slideBehavior.SlideOutCompleted += eventHandler;
                                }
                                else
                                {
                                    EventHandler eventHandler = null;
                                    eventHandler = (sender, e) =>
                                    {
                                        fadeBehavior.FadeOutCompleted -= eventHandler;
                                        this.OnAnimateOutCompleted(this, EventArgs.Empty);
                                    };
                                    fadeBehavior.FadeOutCompleted += eventHandler;
                                }
                            }

                            fadeBehavior.FadeOut();

                            // Do not animate out the selected item.
                            if (item != selectedItem)
                            {
                                slideBehavior.SlideOut();
                            }
                        }
                    }
                }
                else
                {
                    EventHandler eventHandler = null;
                    eventHandler =
                        (sender, e) =>
                        {
                            this.AssociatedObject.ItemContainerGenerator.StatusChanged -= eventHandler;
                            this.AnimateIn();
                        };
                    this.AssociatedObject.ItemContainerGenerator.StatusChanged += eventHandler;
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on attached.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.IsVisibleChanged += this.OnAssociatedObjectIsVisibleChanged;
            this.AssociatedObject.Loaded += this.OnAssociatedObjectLoaded;
        }

        /// <summary>
        /// The on detaching.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.IsVisibleChanged -= this.OnAssociatedObjectIsVisibleChanged;
            this.AssociatedObject.Loaded -= this.OnAssociatedObjectLoaded;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The get fade behaviour.
        /// </summary>
        /// <param name="frameworkElement"> The framework element. </param>
        /// <param name="beginTime"> The begin time. </param>
        /// <param name="duration"> The duration. </param>
        /// <returns> The <see cref="FadeBehavior"/>. </returns>
        private static FadeBehavior GetFadeBehavior(
            FrameworkElement frameworkElement,
            TimeSpan beginTime,
            TimeSpan duration)
        {
            BehaviorCollection behaviors = Interaction.GetBehaviors(frameworkElement);

            FadeBehavior fadeBehavior = behaviors
                .OfType<FadeBehavior>()
                .FirstOrDefault();

            if (fadeBehavior == null)
            {
                fadeBehavior =
                    new FadeBehavior()
                    {
                        IsAnimatingOnIsVisibleChanged = false,
                        IsAnimatingOnLoaded = false
                    };
                behaviors.Add(fadeBehavior);
            }

            fadeBehavior.BeginTime = beginTime;
            fadeBehavior.Duration = duration;

            return fadeBehavior;
        }

        /// <summary>
        /// The get selected item.
        /// </summary>
        /// <param name="itemsControl"> The items control. </param>
        /// <returns> The <see cref="FrameworkElement"/>. </returns>
        private static FrameworkElement GetSelectedItem(ItemsControl itemsControl)
        {
            Selector selector = itemsControl as Selector;
            FrameworkElement item = null;

            if (selector != null)
            {
                item = (FrameworkElement)itemsControl.ItemContainerGenerator.ContainerFromItem(
                    selector.SelectedItem);
            }

            return item;
        }

        /// <summary>
        /// The get slide behaviour.
        /// </summary>
        /// <param name="frameworkElement"> The framework element. </param>
        /// <param name="beginTime"> The begin time. </param>
        /// <param name="duration"> The duration. </param>
        /// <param name="offset"> The offset. </param>
        /// <returns> The <see cref="SlideBehavior"/>. </returns>
        private static SlideBehavior GetSlideBehavior(
            FrameworkElement frameworkElement,
            TimeSpan beginTime,
            TimeSpan duration,
            double offset)
        {
            BehaviorCollection behaviors = Interaction.GetBehaviors(frameworkElement);

            SlideBehavior slideBehavior = behaviors
                .OfType<SlideBehavior>()
                .FirstOrDefault();

            if (slideBehavior == null)
            {
                slideBehavior =
                    new SlideBehavior()
                    {
                        IsAnimatingOnIsVisibleChanged = false,
                        IsAnimatingOnLoaded = false
                    };
                behaviors.Add(slideBehavior);
            }

            slideBehavior.BeginTime = beginTime;
            slideBehavior.Duration = duration;
            slideBehavior.Offset = offset;

            return slideBehavior;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The on animate in completed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnAnimateInCompleted(object sender, EventArgs e)
        {
            EventHandler eventHandler = this.AnimateInCompleted;

            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        /// <summary>
        /// The on animate out completed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnAnimateOutCompleted(object sender, EventArgs e)
        {
            EventHandler eventHandler = this.AnimateOutCompleted;

            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        /// <summary>
        /// The on associated object is visible changed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnAssociatedObjectIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsAnimatingOnIsVisibleChanged)
            {
                if (this.AssociatedObject.Visibility == Visibility.Visible)
                {
                    this.AnimateIn();
                }

                // this.AnimateOut();
            }
        }

        /// <summary>
        /// The on associated object loaded.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The e. </param>
        private void OnAssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            if (this.IsAnimatingOnLoaded)
            {
                this.AnimateIn();
            }
        }

        #endregion
    }
}
