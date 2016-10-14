namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Threading;

    /// <summary>
    /// A content control that allows an adorner for the content to be defined in XAML.
    /// </summary>
    public class AdornedControl : ContentControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty AdornedTemplatePartNameProperty = DependencyProperty.Register(
            "AdornedTemplatePartName", 
            typeof(string), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty AdornerContentProperty = DependencyProperty.Register(
            "AdornerContent", 
            typeof(FrameworkElement), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(OnAdornerContentPropertyChanged));

        public static readonly DependencyProperty AdornerOffsetXProperty = DependencyProperty.Register(
            "AdornerOffsetX", 
            typeof(double), 
            typeof(AdornedControl));

        public static readonly DependencyProperty AdornerOffsetYProperty = DependencyProperty.Register(
            "AdornerOffsetY", 
            typeof(double), 
            typeof(AdornedControl));

        public static readonly DependencyProperty CloseAdornerTimeOutProperty = DependencyProperty.Register(
            "CloseAdornerTimeOut", 
            typeof(double), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(2.0, OnCloseAdornerTimeOutPropertyChanged));

        public static readonly DependencyProperty HorizontalAdornerPlacementProperty = DependencyProperty.Register(
            "HorizontalAdornerPlacement", 
            typeof(AdornerPlacement), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        public static readonly DependencyProperty FadeInTimeProperty = DependencyProperty.Register(
            "FadeInTime", 
            typeof(double), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(0.25));

        public static readonly DependencyProperty FadeOutTimeProperty = DependencyProperty.Register(
            "FadeOutTime", 
            typeof(double), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(1.0));

        public static readonly DependencyProperty IsAdornerVisibleProperty = DependencyProperty.Register(
            "IsAdornerVisible", 
            typeof(bool), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(OnIsAdornerVisiblePropertyChanged));

        public static readonly DependencyProperty IsMouseOverShowEnabledProperty = DependencyProperty.Register(
            "IsMouseOverShowEnabled", 
            typeof(bool), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(false, OnIsMouseOverShowEnabledPropertyChanged));

        public static readonly DependencyProperty VerticalAdornerPlacementProperty = DependencyProperty.Register(
            "VerticalAdornerPlacement", 
            typeof(AdornerPlacement), 
            typeof(AdornedControl),
            new FrameworkPropertyMetadata(AdornerPlacement.Inside));

        #endregion

        #region Commands

        public static readonly RoutedCommand FadeInAdornerCommand = new RoutedCommand("FadeInAdorner", typeof(AdornedControl));
        public static readonly RoutedCommand FadeOutAdornerCommand = new RoutedCommand("FadeOutAdorner", typeof(AdornedControl));
        public static readonly RoutedCommand HideAdornerCommand = new RoutedCommand("HideAdorner", typeof(AdornedControl));
        public static readonly RoutedCommand ShowAdornerCommand = new RoutedCommand("ShowAdorner", typeof(AdornedControl));

        private static readonly CommandBinding ShowAdornerCommandBinding = new CommandBinding(ShowAdornerCommand, OnShowAdornerCommandExecuted);
        private static readonly CommandBinding FadeInAdornerCommandBinding = new CommandBinding(FadeInAdornerCommand, OnFadeInAdornerCommandExecuted);
        private static readonly CommandBinding HideAdornerCommandBinding = new CommandBinding(HideAdornerCommand, OnHideAdornerCommandExecuted);
        private static readonly CommandBinding FadeOutAdornerCommandBinding = new CommandBinding(FadeInAdornerCommand, OnFadeOutAdornerCommandExecuted);

        #endregion

        #region Fields

        /// <summary>
        /// The actual adorner create to contain our 'adorner UI content'.
        /// </summary>
        private FrameworkElementAdorner adorner = null;

        /// <summary>
        /// Caches the adorner layer.
        /// </summary>
        private AdornerLayer adornerLayer = null;

        /// <summary>
        /// Specifies the current show/hide state of the adorner.
        /// </summary>
        private AdornerShowState adornerShowState = AdornerShowState.Hidden;

        /// <summary>
        /// This timer is used to fade out and close the adorner.
        /// </summary>
        private DispatcherTimer closeAdornerTimer = new DispatcherTimer();

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="AdornedControl"/> class.
        /// </summary>
        static AdornedControl()
        {
            CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), ShowAdornerCommandBinding);
            CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), FadeOutAdornerCommandBinding);
            CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), HideAdornerCommandBinding);
            CommandManager.RegisterClassCommandBinding(typeof(AdornedControl), FadeInAdornerCommandBinding);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="AdornedControl"/> class.
        /// </summary>
        public AdornedControl()
        {
            this.Focusable = false; // By default don't want 'AdornedControl' to be focusable.

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.OnAdornedControlDataContextChanged);

            this.closeAdornerTimer.Tick += this.OnCloseAdornerTimerTick;
            this.closeAdornerTimer.Interval = TimeSpan.FromSeconds(this.CloseAdornerTimeOut);
        } 

        #endregion

        #region Private Enumerations

        /// <summary>
        /// Specifies the current show/hide state of the adorner.
        /// </summary>
        private enum AdornerShowState
        {
            /// <summary>
            /// The adorner is visible.
            /// </summary>
            Visible,

            /// <summary>
            /// The adorner is hidden.
            /// </summary>
            Hidden,

            /// <summary>
            /// The adorner is fading in.
            /// </summary>
            FadingIn,

            /// <summary>
            /// The adorner is fading out.
            /// </summary>
            FadingOut,
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the adorned template part. When set to non-null it specifies the part name of a UI element 
        /// in the visual tree of the AdornedControl content that is to be adorned. When this property is null it is the 
        /// AdornerControl content that is adorned.
        /// </summary>
        /// <value>
        /// The name of the adorned template part.
        /// </value>
        public string AdornedTemplatePartName
        {
            get { return (string)this.GetValue(AdornedTemplatePartNameProperty); }
            set { this.SetValue(AdornedTemplatePartNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the adorner.
        /// </summary>
        /// <value>
        /// The content of the adorner.
        /// </value>
        public FrameworkElement AdornerContent
        {
            get { return (FrameworkElement)this.GetValue(AdornerContentProperty); }
            set { this.SetValue(AdornerContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the adorner offset for the X-axis.
        /// </summary>
        /// <value>
        /// The adorner offset for the X-axis.
        /// </value>
        public double AdornerOffsetX
        {
            get { return (double)this.GetValue(AdornerOffsetXProperty); }
            set { this.SetValue(AdornerOffsetXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the adorner offset for the Y-axis.
        /// </summary>
        /// <value>
        /// The adorner offset for the Y-axis.
        /// </value>
        public double AdornerOffsetY
        {
            get { return (double)this.GetValue(AdornerOffsetYProperty); }
            set { this.SetValue(AdornerOffsetYProperty, value); }
        }

        /// <summary>
        /// Gets or sets the close adorner time out. Specifies the time (in seconds) after the mouse cursor moves away from the 
        /// adorned control (or the adorner) when the adorner begins to fade out.
        /// </summary>
        /// <value>
        /// The close adorner time out.
        /// </value>
        public double CloseAdornerTimeOut
        {
            get { return (double)this.GetValue(CloseAdornerTimeOutProperty); }
            set { this.SetValue(CloseAdornerTimeOutProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fade in time in seconds. 
        /// </summary>
        /// <value>
        /// The fade in time.
        /// </value>
        public double FadeInTime
        {
            get { return (double)this.GetValue(FadeInTimeProperty); }
            set { this.SetValue(FadeInTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the fade out time in seconds.
        /// </summary>
        /// <value>
        /// The fade out time.
        /// </value>
        public double FadeOutTime
        {
            get { return (double)this.GetValue(FadeOutTimeProperty); }
            set { this.SetValue(FadeOutTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the adorner is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the adorner is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsAdornerVisible
        {
            get { return (bool)this.GetValue(IsAdornerVisibleProperty); }
            set { this.SetValue(IsAdornerVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to fade in and out the adorner when the mouse is hovered over the adorned control.
        /// </summary>
        /// <value>
        /// <c>true</c> if fade in and out is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsMouseOverShowEnabled
        {
            get { return (bool)this.GetValue(IsMouseOverShowEnabledProperty); }
            set { this.SetValue(IsMouseOverShowEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal placement of the adorner relative to the adorned control.
        /// </summary>
        /// <value>
        /// The horizontal adorner placement.
        /// </value>
        public AdornerPlacement HorizontalAdornerPlacement
        {
            get { return (AdornerPlacement)this.GetValue(HorizontalAdornerPlacementProperty); }
            set { this.SetValue(HorizontalAdornerPlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical placement of the adorner relative to the adorned control.
        /// </summary>
        /// <value>
        /// The vertical adorner placement.
        /// </value>
        public AdornerPlacement VerticalAdornerPlacement
        {
            get { return (AdornerPlacement)this.GetValue(VerticalAdornerPlacementProperty); }
            set { this.SetValue(VerticalAdornerPlacementProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Fade the adorner in and make it visible.
        /// </summary>
        public void FadeInAdorner()
        {
            if (this.adornerShowState == AdornerShowState.Visible ||
                this.adornerShowState == AdornerShowState.FadingIn)
            {
                // Already visible or fading in.
                return;
            }

            this.ShowAdorner();

            if (this.adornerShowState != AdornerShowState.FadingOut)
            {
                this.adorner.Opacity = 0.0;
            }

            DoubleAnimation doubleAnimation = new DoubleAnimation(1.0, new Duration(TimeSpan.FromSeconds(this.FadeInTime)));
            doubleAnimation.Completed += this.OnFadeInAnimationCompleted;
            doubleAnimation.Freeze();

            this.adorner.BeginAnimation(FrameworkElement.OpacityProperty, doubleAnimation);

            this.adornerShowState = AdornerShowState.FadingIn;
        }

        /// <summary>
        /// Fade the adorner out and make it visible.
        /// </summary>
        public void FadeOutAdorner()
        {
            if (this.adornerShowState == AdornerShowState.FadingOut)
            {
                // Already fading out.
                return;
            }

            if (this.adornerShowState == AdornerShowState.Hidden)
            {
                // Adorner has already been hidden.
                return;
            }

            DoubleAnimation fadeOutAnimation = new DoubleAnimation(0.0, new Duration(TimeSpan.FromSeconds(this.FadeOutTime)));
            fadeOutAnimation.Completed += this.FadeOutAnimationCompleted;
            fadeOutAnimation.Freeze();

            this.adorner.BeginAnimation(FrameworkElement.OpacityProperty, fadeOutAnimation);

            this.adornerShowState = AdornerShowState.FadingOut;
        }

        /// <summary>
        /// Hide the adorner.
        /// </summary>
        public void HideAdorner()
        {
            this.IsAdornerVisible = false;
        }

        /// <summary>
        /// Show the adorner.
        /// </summary>
        public void ShowAdorner()
        {
            this.IsAdornerVisible = true;
        } 

        #endregion

        #region Public Methods

        /// <summary>
        /// Called to build the visual tree.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.ShowOrHideAdornerInternal();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when the mouse cursor enters the area of the adorned control.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            this.MouseEnterLogic();
        }

        /// <summary>
        /// Called when the mouse cursor leaves the area of the adorned control.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.Input.MouseEventArgs" /> that contains the event data.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            this.MouseLeaveLogic();
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Finds a child element in the visual tree that has the specified name.
        /// Returns null if no child with that name exists.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="childName">Name of the child.</param>
        /// <returns>The child element with the specified name.</returns>
        private static FrameworkElement FindNamedChild(FrameworkElement rootElement, string childName)
        {
            int numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (int i = 0; i < numChildren; ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(rootElement, i);
                FrameworkElement childElement = child as FrameworkElement;
                if (childElement != null && childElement.Name == childName)
                {
                    return childElement;
                }

                FrameworkElement foundElement = FindNamedChild(childElement, childName);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Event raised when the value of AdornerContent has changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAdornerContentPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AdornedControl c = (AdornedControl)dependencyObject;
            c.ShowOrHideAdornerInternal();

            FrameworkElement oldAdornerContent = (FrameworkElement)e.OldValue;
            if (oldAdornerContent != null)
            {
                oldAdornerContent.MouseEnter -= new MouseEventHandler(c.OnAdornerContentMouseEnter);
                oldAdornerContent.MouseLeave -= new MouseEventHandler(c.OnAdornerContentMouseLeave);
            }

            FrameworkElement newAdornerContent = (FrameworkElement)e.NewValue;
            if (newAdornerContent != null)
            {
                newAdornerContent.MouseEnter += new MouseEventHandler(c.OnAdornerContentMouseEnter);
                newAdornerContent.MouseLeave += new MouseEventHandler(c.OnAdornerContentMouseLeave);
            }
        }

        /// <summary>
        /// Event raised when the CloseAdornerTimeOut property has change.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnCloseAdornerTimeOutPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AdornedControl c = (AdornedControl)dependencyObject;
            c.closeAdornerTimer.Interval = TimeSpan.FromSeconds(c.CloseAdornerTimeOut);
        }

        /// <summary>
        /// Event raised when the FadeIn command is executed.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnFadeInAdornerCommandExecuted(object target, ExecutedRoutedEventArgs e)
        {
            AdornedControl c = (AdornedControl)target;
            c.FadeOutAdorner();
        }

        /// <summary>
        /// Event raised when the FadeOut command is executed.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnFadeOutAdornerCommandExecuted(object target, ExecutedRoutedEventArgs e)
        {
            AdornedControl c = (AdornedControl)target;
            c.FadeOutAdorner();
        }

        /// <summary>
        /// Event raised when the Hide command is executed.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnHideAdornerCommandExecuted(object target, ExecutedRoutedEventArgs e)
        {
            AdornedControl c = (AdornedControl)target;
            c.HideAdorner();
        }

        /// <summary>
        /// Event raised when the value of IsAdornerVisible has changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsAdornerVisiblePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AdornedControl c = (AdornedControl)dependencyObject;
            c.ShowOrHideAdornerInternal();
        }

        /// <summary>
        /// Event raised when the IsMouseOverShowEnabled property has changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsMouseOverShowEnabledPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AdornedControl c = (AdornedControl)dependencyObject;
            c.closeAdornerTimer.Stop();
            c.HideAdorner();
        }

        /// <summary>
        /// Event raised when the Show command is executed.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnShowAdornerCommandExecuted(object target, ExecutedRoutedEventArgs e)
        {
            AdornedControl c = (AdornedControl)target;
            c.ShowAdorner();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Internal method to hide the adorner.
        /// </summary>
        private void HideAdornerInternal()
        {
            if (this.adornerLayer == null || this.adorner == null)
            {
                // Not already adorned.
                return;
            }

            // Stop the timer that might be about to fade out the adorner.
            this.closeAdornerTimer.Stop();
            this.adornerLayer.Remove(this.adorner);
            this.adorner.DisconnectChild();

            this.adorner = null;
            this.adornerLayer = null;

            // Ensure that the state of the adorned control reflects that the the adorner is no longer.
            this.adornerShowState = AdornerShowState.Hidden;
        }

        /// <summary>
        /// Shared mouse enter code.
        /// </summary>
        private void MouseEnterLogic()
        {
            if (!this.IsMouseOverShowEnabled)
            {
                return;
            }

            this.closeAdornerTimer.Stop();

            this.FadeInAdorner();
        }

        /// <summary>
        /// Shared mouse leave code.
        /// </summary>
        private void MouseLeaveLogic()
        {
            if (!this.IsMouseOverShowEnabled)
            {
                return;
            }

            this.closeAdornerTimer.Start();
        }

        /// <summary>
        /// Event raised when the DataContext of the adorned control changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnAdornedControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateAdornerDataContext();
        }

        /// <summary>
        /// Event raised when the mouse cursor enters the area of the adorner.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnAdornerContentMouseEnter(object sender, MouseEventArgs e)
        {
            this.MouseEnterLogic();
        }

        /// <summary>
        /// Event raised when the mouse cursor leaves the area of the adorner.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnAdornerContentMouseLeave(object sender, MouseEventArgs e)
        {
            this.MouseLeaveLogic();
        }

        /// <summary>
        /// Called when the close adorner time-out has elapsed, the mouse has moved
        /// away from the adorned control and the adorner and it is time to close the adorner.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCloseAdornerTimerTick(object sender, EventArgs e)
        {
            this.closeAdornerTimer.Stop();

            this.FadeOutAdorner();
        }

        /// <summary>
        /// Event raised when the fade in animation has completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnFadeInAnimationCompleted(object sender, EventArgs e)
        {
            this.adornerShowState = AdornerShowState.Visible;
        }

        /// <summary>
        /// Event raised when the fade-out animation has completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FadeOutAnimationCompleted(object sender, EventArgs e)
        {
            if (this.adornerShowState == AdornerShowState.FadingOut)
            {
                // Still fading out, eg it wasn't aborted.
                this.HideAdorner();
            }
        }

        /// <summary>
        /// Internal method to show the adorner.
        /// </summary>
        private void ShowAdornerInternal()
        {
            if (this.adorner != null)
            {
                // Already adorned.
                return;
            }

            if (this.AdornerContent != null)
            {
                if (this.adornerLayer == null)
                {
                    this.adornerLayer = AdornerLayer.GetAdornerLayer(this);
                }

                if (this.adornerLayer != null)
                {
                    FrameworkElement adornedControl = this; // The control to be adorned defaults to 'this'.

                    if (!string.IsNullOrEmpty(this.AdornedTemplatePartName))
                    {
                        // If 'AdornedTemplatePartName' is set to a valid string then search the visual-tree
                        // for a UI element that has the specified part name.  If we find it then use it as the
                        // adorned control, otherwise throw an exception.
                        adornedControl = FindNamedChild(this, this.AdornedTemplatePartName);
                        if (adornedControl == null)
                        {
                            throw new ApplicationException("Failed to find a FrameworkElement in the visual-tree with the part name '" + this.AdornedTemplatePartName + "'.");
                        }
                    }

                    this.adorner = new FrameworkElementAdorner(
                        this.AdornerContent, 
                        adornedControl,
                        this.HorizontalAdornerPlacement, 
                        this.VerticalAdornerPlacement,
                        this.AdornerOffsetX, 
                        this.AdornerOffsetY);
                    this.adornerLayer.Add(this.adorner);

                    this.UpdateAdornerDataContext();
                }
            }

            this.adornerShowState = AdornerShowState.Visible;
        }

        /// <summary>
        /// Internal method to show or hide the adorner based on the value of IsAdornerVisible.
        /// </summary>
        private void ShowOrHideAdornerInternal()
        {
            if (this.IsAdornerVisible)
            {
                this.ShowAdornerInternal();
            }
            else
            {
                this.HideAdornerInternal();
            }
        }

        /// <summary>
        /// Update the DataContext of the adorner from the adorned control.
        /// </summary>
        private void UpdateAdornerDataContext()
        {
            if (this.AdornerContent != null)
            {
                this.AdornerContent.DataContext = this.DataContext;
            }
        }

        #endregion
    }
}
