namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;

    /// <summary>
    /// The elastic list box.
    /// </summary>
    public class ElasticListBox : ListBox
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsResizeSuspendedProperty = DependencyProperty.Register(
            "IsResizeSuspended",
            typeof(bool),
            typeof(ElasticListBox),
            new PropertyMetadata(false));

        public static readonly DependencyProperty ResizeDurationProperty = DependencyProperty.Register(
            "ResizeDuration",
            typeof(TimeSpan),
            typeof(ElasticListBox),
            new PropertyMetadata(TimeSpan.FromMilliseconds(700)));

        public static readonly DependencyProperty TotalProperty = DependencyProperty.Register(
            "Total",
            typeof(double),
            typeof(ElasticListBox),
            new PropertyMetadata(0D));

        #endregion

        #region Fields

        private readonly Subject<object> resizeSubject = new Subject<object>();
        private ItemsPresenter itemsPresenter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ElasticListBox"/> class.
        /// </summary>
        public ElasticListBox()
        {
            this.Loaded += this.OnLoaded;
            this.SizeChanged += this.OnSizeChanged;
            this.resizeSubject
                .Buffer(TimeSpan.FromMilliseconds(300))
                .Where(x => x.Count > 0)
                .ObserveOnDispatcher()
                .Subscribe(x => this.ResizeItemsInternal());
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether is resize suspended.
        /// </summary>
        public bool IsResizeSuspended
        {
            get { return (bool)this.GetValue(IsResizeSuspendedProperty); }
            set { this.SetValue(IsResizeSuspendedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the resize duration.
        /// </summary>
        public TimeSpan ResizeDuration
        {
            get { return (TimeSpan)this.GetValue(ResizeDurationProperty); }
            set { this.SetValue(ResizeDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        public double Total
        {
            get { return (double)this.GetValue(TotalProperty); }
            set { this.SetValue(TotalProperty, value); }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the items presenter.
        /// </summary>
        private ItemsPresenter ItemsPresenter
        {
            get
            {
                if (this.itemsPresenter == null)
                {
                    this.itemsPresenter = (ItemsPresenter)this.GetTemplateChild("ItemsPresenter");
                }

                return this.itemsPresenter;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The resize items.
        /// </summary>
        public void ResizeItems()
        {
            if (this.IsLoaded && !this.IsResizeSuspended)
            {
                this.resizeSubject.OnNext(null);
            }
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
            return new ElasticListBoxItem()
            {
                ParentListBox = this
            };
        }

        /// <summary>
        /// The on loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ResizeItems();
        }

        /// <summary>
        /// The on selection changed.
        /// </summary>
        /// <param name="e"> The Event Arguments. </param>
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            var items = e.AddedItems
                .Cast<object>()
                .Select(x => this.ItemContainerGenerator.ContainerFromItem(x))
                .Where(x => x != null)
                .Cast<ElasticListBoxItem>();

            this.IsResizeSuspended = true;

            // Highlight selected items.
            foreach (var item in items)
            {
                item.IsHighlighted = true;
            }

            this.IsResizeSuspended = false;
            this.ResizeItems();
        }

        /// <summary>
        /// Called when the size is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.ResizeItems();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the height of the specified list box item.
        /// </summary>
        /// <param name="listBoxItem">The list box item.</param>
        /// <returns>The height.</returns>
        private double GetHeight(ElasticListBoxItem listBoxItem)
        {
            double height;

            if (listBoxItem.IsHighlighted)
            {
                height = 0;
            }
            else
            {
                height = listBoxItem.MinHeight;
            }

            return height;
        }

        private void ResizeItemsInternal()
        {
            IEnumerable<ElasticListBoxItem> listBoxItems = this.Items
                .Cast<object>()
                .Select(x => this.ItemContainerGenerator.ContainerFromItem(x))
                .Where(x => x != null)
                .Cast<ElasticListBoxItem>();

            double totalHeight = this.ActualHeight - this.Padding.Top - this.Padding.Bottom;
            bool resetAll = listBoxItems.Any(x => x.ActualHeight == 0);
            double totalCalculatedHeight = 0;

            foreach (ElasticListBoxItem listBoxItem in listBoxItems)
            {
                double height = 0;

                listBoxItem.MinHeight = 0;

                if (listBoxItem.IsHighlighted)
                {
                    double nonHighlightedHeight = listBoxItems.Where(x => !x.IsHighlighted).Sum(x => x.MinDisabledHeight);
                    double highlightedHeight = listBoxItems.Where(x => x.IsHighlighted).Sum(x => x.MinEnabledHeight);

                    if (double.IsInfinity(listBoxItem.MaxHeight))
                    {
                        height = ((totalHeight - nonHighlightedHeight - highlightedHeight) * listBoxItem.Percentage) + listBoxItem.MinEnabledHeight;
                        if (height <= listBoxItem.MinEnabledHeight)
                        {
                            listBoxItem.MinHeight = listBoxItem.MinEnabledHeight;
                            totalCalculatedHeight += listBoxItem.MinEnabledHeight;
                        }
                        else
                        {
                            totalCalculatedHeight += height;
                        }
                    }
                    else
                    {
                        height = ((listBoxItem.MaxHeight - nonHighlightedHeight - highlightedHeight) * listBoxItem.Percentage) + listBoxItem.MinEnabledHeight;
                        if (height <= listBoxItem.MinEnabledHeight)
                        {
                            listBoxItem.MinHeight = listBoxItem.MinEnabledHeight;
                            totalCalculatedHeight += listBoxItem.MinEnabledHeight;
                        }
                        else
                        {
                            totalCalculatedHeight += height;
                        }
                    }
                }
                else
                {
                    height = listBoxItem.MinDisabledHeight;
                    listBoxItem.MinHeight = listBoxItem.MinDisabledHeight;

                    totalCalculatedHeight += height;
                }

                if ((listBoxItem.ActualHeight != height) && (height > 0))
                {
                    if (resetAll)
                    {
                        listBoxItem.Height = height;
                    }
                    else
                    {
                        Storyboard storyboard = new Storyboard();

                        DoubleAnimation doubleAnimation = new DoubleAnimation(
                            listBoxItem.ActualHeight,
                            height,
                            this.ResizeDuration)
                        {
                            EasingFunction = new QuarticEase()
                        };
                        Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(ElasticListBox.HeightProperty));
                        storyboard.Children.Add(doubleAnimation);
                        storyboard.Completed += (sender, e) =>
                        {
                            // Scroll to the first highlighted item if the user has not got his mouse over the list box.
                            ListBoxItem firstListBoxItem = listBoxItems.FirstOrDefault(x => x.IsHighlighted);
                            if ((firstListBoxItem != null) && !this.IsMouseOver)
                            {
                                firstListBoxItem.BringIntoView();
                            }
                        };
                        storyboard.Begin(listBoxItem);
                    }
                }
            }

            if (totalCalculatedHeight > this.ActualHeight)
            {
                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Auto);
            }
            else
            {
                ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            }
        }

        #endregion
    }
}
