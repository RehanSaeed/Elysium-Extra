namespace Framework.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Reactive;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Interop;
    using System.Windows.Media;

    public abstract class WindowBase : Elysium.Controls.Window
    {
        #region Dependency Properties

        public static readonly DependencyProperty BackgroundContentProperty = DependencyProperty.Register(
            "BackgroundContent",
            typeof(object),
            typeof(WindowBase));

        public static readonly DependencyProperty BackgroundContentTemplateProperty = DependencyProperty.Register(
            "BackgroundContentTemplate",
            typeof(DataTemplate),
            typeof(WindowBase));

        public static readonly DependencyProperty BackgroundContentTemplateSelectorProperty = DependencyProperty.Register(
            "BackgroundContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WindowBase));

        public static readonly DependencyProperty ForegroundContentProperty = DependencyProperty.Register(
            "ForegroundContent",
            typeof(object),
            typeof(WindowBase));

        public static readonly DependencyProperty ForegroundContentTemplateProperty = DependencyProperty.Register(
            "ForegroundContentTemplate",
            typeof(DataTemplate),
            typeof(WindowBase));

        public static readonly DependencyProperty ForegroundContentTemplateSelectorProperty = DependencyProperty.Register(
            "ForegroundContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WindowBase));

        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(
            "TitleBarBackground",
            typeof(Brush),
            typeof(WindowBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarBorderBrushProperty = DependencyProperty.Register(
            "TitleBarBorderBrush",
            typeof(Brush),
            typeof(WindowBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarBorderThicknessProperty = DependencyProperty.Register(
            "TitleBarBorderThickness",
            typeof(Thickness),
            typeof(WindowBase),
            new PropertyMetadata(new Thickness()));

        public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register(
            "TitleBarContent",
            typeof(object),
            typeof(WindowBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarContentTemplateProperty = DependencyProperty.Register(
            "TitleBarContentTemplate",
            typeof(DataTemplate),
            typeof(WindowBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarContentTemplateSelectorProperty = DependencyProperty.Register(
            "TitleBarContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WindowBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register(
            "TitleBarForeground",
            typeof(Brush),
            typeof(WindowBase),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleBarVisibilityProperty = DependencyProperty.Register(
            "TitleBarVisibility",
            typeof(Visibility),
            typeof(WindowBase),
            new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty WindowPlacementProperty = DependencyProperty.Register(
            "WindowPlacement",
            typeof(string),
            typeof(Window),
            new PropertyMetadata(null));

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="WindowBase"/> class.
        /// </summary>
        public WindowBase()
        {
            DispatcherScheduler scheduler = new DispatcherScheduler(this.Dispatcher);
            var locationChanges = Observable.FromEventPattern<EventHandler, EventArgs>(
                h => LocationChanged += h,
                h => LocationChanged -= h);
            var sizeChanges = Observable.FromEventPattern<SizeChangedEventHandler, SizeChangedEventArgs>(
                h => SizeChanged += h,
                h => SizeChanged -= h);
            var merged = Observable.Merge(
                sizeChanges.Select(_ => Unit.Default),
                locationChanges.Select(_ => Unit.Default));
            merged
                .Throttle(TimeSpan.FromSeconds(5), scheduler)
                .Subscribe(_ => this.UpdateWindowPlacement());

            DependencyPropertyDescriptor
                .FromProperty(ApplicationBarProperty, typeof(Elysium.Controls.Window))
                .AddValueChanged(this, OnApplicationBarChanged);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the content of the background.
        /// </summary>
        /// <value>
        /// The content of the background.
        /// </value>
        public object BackgroundContent
        {
            get { return this.GetValue(BackgroundContentProperty); }
            set { this.SetValue(BackgroundContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background content template.
        /// </summary>
        /// <value>
        /// The background content template.
        /// </value>
        public DataTemplate BackgroundContentTemplate
        {
            get { return (DataTemplate)this.GetValue(BackgroundContentTemplateProperty); }
            set { this.SetValue(BackgroundContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background content template selector.
        /// </summary>
        /// <value>
        /// The background content template selector.
        /// </value>
        public DataTemplateSelector BackgroundContentTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(BackgroundContentTemplateSelectorProperty); }
            set { this.SetValue(BackgroundContentTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the foreground.
        /// </summary>
        /// <value>
        /// The content of the foreground.
        /// </value>
        public object ForegroundContent
        {
            get { return this.GetValue(ForegroundContentProperty); }
            set { this.SetValue(ForegroundContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the foreground content template.
        /// </summary>
        /// <value>
        /// The foreground content template.
        /// </value>
        public DataTemplate ForegroundContentTemplate
        {
            get { return (DataTemplate)this.GetValue(ForegroundContentTemplateProperty); }
            set { this.SetValue(ForegroundContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the foreground content template selector.
        /// </summary>
        /// <value>
        /// The foreground content template selector.
        /// </value>
        public DataTemplateSelector ForegroundContentTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(ForegroundContentTemplateSelectorProperty); }
            set { this.SetValue(ForegroundContentTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title bar background.
        /// </summary>
        /// <value>
        /// The title bar background.
        /// </value>
        public Brush TitleBarBackground
        {
            get { return (Brush)this.GetValue(TitleBarBackgroundProperty); }
            set { this.SetValue(TitleBarBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title bar border brush.
        /// </summary>
        /// <value>
        /// The title bar border brush.
        /// </value>
        public Brush TitleBarBorderBrush
        {
            get { return (Brush)this.GetValue(TitleBarBorderBrushProperty); }
            set { this.SetValue(TitleBarBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title bar border thickness.
        /// </summary>
        /// <value>
        /// The title bar border thickness.
        /// </value>
        public Thickness TitleBarBorderThickness
        {
            get { return (Thickness)this.GetValue(TitleBarBorderThicknessProperty); }
            set { this.SetValue(TitleBarBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the title bar.
        /// </summary>
        /// <value>
        /// The content of the title bar.
        /// </value>
        public object TitleBarContent
        {
            get { return (object)this.GetValue(TitleBarContentProperty); }
            set { this.SetValue(TitleBarContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the title bar.
        /// </summary>
        /// <value>
        /// The content of the title bar.
        /// </value>
        public DataTemplate TitleBarContentTemplate
        {
            get { return (DataTemplate)this.GetValue(TitleBarContentTemplateProperty); }
            set { this.SetValue(TitleBarContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title content template selector.
        /// </summary>
        /// <value>
        /// The title content template selector.
        /// </value>
        public DataTemplateSelector TitleBarContentTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(TitleBarContentTemplateSelectorProperty); }
            set { this.SetValue(TitleBarContentTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title bar foreground.
        /// </summary>
        /// <value>
        /// The title bar foreground.
        /// </value>
        public Brush TitleBarForeground
        {
            get { return (Brush)this.GetValue(TitleBarForegroundProperty); }
            set { this.SetValue(TitleBarForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title bar visibility.
        /// </summary>
        /// <value>
        /// The title bar visibility.
        /// </value>
        public Visibility TitleBarVisibility
        {
            get { return (Visibility)this.GetValue(TitleBarVisibilityProperty); }
            set { this.SetValue(TitleBarVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the window placement XML. Can be used to save and restore the windoe placement.
        /// </summary>
        /// <value>
        /// The window placement.
        /// </value>
        public string WindowPlacement
        {
            get { return (string)this.GetValue(WindowPlacementProperty); }
            set { this.SetValue(WindowPlacementProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetApplicationBarInternal(this);
        }

        #region Protected Methods

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Window.SourceInitialized" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override void OnSourceInitialized(System.EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (this.WindowPlacement != null)
            {
                Framework.UI.Controls.Window.WindowPlacementHelper.SetPlacement(new WindowInteropHelper(this).Handle, this.WindowPlacement);
            }
        }

        protected override void Dispose(bool disposing)
        {
            DependencyPropertyDescriptor
                .FromProperty(ApplicationBarProperty, typeof(Elysium.Controls.Window))
                .RemoveValueChanged(this, OnApplicationBarChanged);
            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        private static void OnApplicationBarChanged(object sender, EventArgs e)
        {
            WindowBase window = sender as WindowBase;
            if (window != null)
            {
                window.SetApplicationBarInternal(window);
            }
        }

        private void SetApplicationBarInternal(WindowBase window)
        {
            Decorator decorator = (Decorator)window.GetTemplateChild("ApplicationBarHost");
            if (decorator != null)
            {
                Elysium.Controls.ApplicationBar applicationBar = GetApplicationBar(window);
                decorator.Child = applicationBar;
            }
        }

        /// <summary>
        /// Updates the window placement.
        /// </summary>
        private void UpdateWindowPlacement()
        {
            this.WindowPlacement = Framework.UI.Controls.Window.WindowPlacementHelper.GetPlacement(new WindowInteropHelper(this).Handle);
        }

        #endregion
    }
}
