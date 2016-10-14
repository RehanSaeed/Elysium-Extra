namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Interactivity;
    using System.Windows.Media.Animation;
    using Framework.UI.Input;

    /// <summary>
    /// The wizard.
    /// </summary>
    public class Wizard : Selector
    {
        #region Attached Properties

        public static readonly DependencyProperty AnimationProperty = DependencyProperty.RegisterAttached(
            "Animation",
            typeof(object),
            typeof(Wizard),
            new PropertyMetadata(WizardAnimation.FadeAndSlide, OnAnimationChanged));

        public static readonly DependencyProperty CollectionAnimationProperty = DependencyProperty.RegisterAttached(
            "CollectionAnimation",
            typeof(object),
            typeof(Wizard),
            new PropertyMetadata(WizardCollectionAnimation.Sequential, OnCollectionAnimationChanged));

        #endregion

        #region Dependency Properties

        private static readonly DependencyPropertyKey BackCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "BackCommand",
            typeof(DelegateCommand<object>),
            typeof(Wizard),
            new PropertyMetadata(null));

        public static readonly DependencyProperty BackCommandProperty = BackCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey BreadcrumbPropertyKey = DependencyProperty.RegisterReadOnly(
            "Breadcrumb",
            typeof(WizardItemModelCollection),
            typeof(Wizard),
            new PropertyMetadata(null));

        public static readonly DependencyProperty BreadcrumbProperty = BreadcrumbPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DisplayItemPropertyKey = DependencyProperty.RegisterReadOnly(
            "DisplayItem",
            typeof(WizardItem),
            typeof(Wizard),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DisplayItemProperty = DisplayItemPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey ForwardCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "ForwardCommand",
            typeof(DelegateCommand<object>),
            typeof(Wizard),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ForwardCommandProperty = ForwardCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsBackAllowedPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsBackAllowed",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsBackAllowedProperty = IsBackAllowedPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsForwardAllowedPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsForwardAllowed",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsForwardAllowedProperty = IsForwardAllowedPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNavigatingPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNavigating",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNavigatingProperty = IsNavigatingPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNavigatingBackwardPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNavigatingBackward",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNavigatingBackwardProperty = IsNavigatingBackwardPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNavigatingCancelledPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNavigatingCancelled",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNavigatingCancelledProperty = IsNavigatingCancelledPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNavigatingForwardPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNavigatingForward",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNavigatingForwardProperty = IsNavigatingForwardPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsNavigatingHorizontallyPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsNavigatingHorizontally",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsNavigatingHorizontallyProperty = IsNavigatingHorizontallyPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode",
            typeof(WizardMode),
            typeof(Wizard),
            new PropertyMetadata(WizardMode.Tree));

        private static readonly DependencyPropertyKey SelectCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "SelectCommand",
            typeof(DelegateCommand<object>),
            typeof(Wizard),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectCommandProperty = SelectCommandPropertyKey.DependencyProperty;

        public static readonly DependencyProperty TransitionDurationProperty = DependencyProperty.Register(
            "TransitionDuration",
            typeof(Duration),
            typeof(Wizard),
            new PropertyMetadata(new Duration(TimeSpan.FromMilliseconds(600))));

        #endregion

        #region Fields

        private Storyboard backgroundStoryboard;
        private Border currentItemBorder;
        private WizardItemModelCollection models = new WizardItemModelCollection();
        private Border previousItemBorder;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="Wizard"/> class.
        /// </summary>
        static Wizard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Wizard), new FrameworkPropertyMetadata(typeof(Wizard)));
            SelectedItemProperty.OverrideMetadata(
                typeof(Wizard),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSelectedItemChanged)));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Wizard"/> class.
        /// </summary>
        public Wizard()
        {
            this.Loaded += this.OnLoaded;
            ((INotifyCollectionChanged)this.Items).CollectionChanged += this.OnItemsCollectionChanged;

            this.Breadcrumb = new WizardItemModelCollection();
            this.BackCommand = new DelegateCommand<object>(obj => this.Back(), obj => this.CanBack());
            this.ForwardCommand = new DelegateCommand<object>(obj => this.Forward(), obj => this.CanForward());
            this.SelectCommand = new DelegateCommand<object>(this.Select, this.CanSelect);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the breadcrumb.
        /// </summary>
        public WizardItemModelCollection Breadcrumb
        {
            get { return (WizardItemModelCollection)this.GetValue(BreadcrumbProperty); }
            private set { this.SetValue(BreadcrumbPropertyKey, value); }
        }

        /// <summary>
        /// Gets the back command.
        /// </summary>
        public DelegateCommand<object> BackCommand
        {
            get { return (DelegateCommand<object>)this.GetValue(BackCommandProperty); }
            internal set { this.SetValue(BackCommandPropertyKey, value); }
        }

        /// <summary>
        /// Gets the display item.
        /// </summary>
        public WizardItem DisplayItem
        {
            get { return (WizardItem)this.GetValue(DisplayItemProperty); }
            internal set { this.SetValue(DisplayItemPropertyKey, value); }
        }

        /// <summary>
        /// Gets the forward command.
        /// </summary>
        public DelegateCommand<object> ForwardCommand
        {
            get { return (DelegateCommand<object>)this.GetValue(ForwardCommandProperty); }
            internal set { this.SetValue(ForwardCommandPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is back allowed.
        /// </summary>
        public bool IsBackAllowed
        {
            get { return (bool)this.GetValue(IsBackAllowedProperty); }
            private set { this.SetValue(IsBackAllowedPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is forward allowed.
        /// </summary>
        public bool IsForwardAllowed
        {
            get { return (bool)this.GetValue(IsForwardAllowedProperty); }
            private set { this.SetValue(IsForwardAllowedPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is navigating from one <see cref="WizardItem"/> to another.
        /// </summary>
        public bool IsNavigating
        {
            get { return (bool)this.GetValue(IsNavigatingProperty); }
            private set { this.SetValue(IsNavigatingPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is navigating backward.
        /// </summary>
        public bool IsNavigatingBackward
        {
            get { return (bool)this.GetValue(IsNavigatingBackwardProperty); }
            private set { this.SetValue(IsNavigatingBackwardPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether navigation was cancelled.
        /// </summary>
        public bool IsNavigatingCancelled
        {
            get { return (bool)this.GetValue(IsNavigatingCancelledProperty); }
            private set { this.SetValue(IsNavigatingCancelledPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is navigating forward.
        /// </summary>
        public bool IsNavigatingForward
        {
            get { return (bool)this.GetValue(IsNavigatingForwardProperty); }
            private set { this.SetValue(IsNavigatingForwardPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is navigating horizontally.
        /// </summary>
        public bool IsNavigatingHorizontally
        {
            get { return (bool)this.GetValue(IsNavigatingHorizontallyProperty); }
            private set { this.SetValue(IsNavigatingHorizontallyPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        public WizardMode Mode
        {
            get { return (WizardMode)this.GetValue(ModeProperty); }
            set { this.SetValue(ModeProperty, value); }
        }

        /// <summary>
        /// Gets the select command.
        /// </summary>
        public DelegateCommand<object> SelectCommand
        {
            get { return (DelegateCommand<object>)this.GetValue(SelectCommandProperty); }
            internal set { this.SetValue(SelectCommandPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the transition duration.
        /// </summary>
        public Duration TransitionDuration
        {
            get { return (Duration)this.GetValue(TransitionDurationProperty); }
            set { this.SetValue(TransitionDurationProperty, value); }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the selected model.
        /// </summary>
        private WizardItemModel SelectedModel
        {
            get
            {
                WizardItemModel model = null;

                WizardItem wizardItem = this.SelectedItem as WizardItem;
                if (wizardItem != null)
                {
                    model = wizardItem.Model;
                }

                return model;
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// The get animation.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public static object GetAnimation(DependencyObject dependencyObject)
        {
            return (object)dependencyObject.GetValue(AnimationProperty);
        }

        /// <summary>
        /// The set animation.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="value"> The value. </param>
        public static void SetAnimation(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(AnimationProperty, value);
        }

        /// <summary>
        /// The get collection animation.
        /// </summary>
        /// <param name="itemsControl"> The items control. </param>
        /// <returns> The <see cref="object"/>. </returns>
        public static object GetCollectionAnimation(DependencyObject itemsControl)
        {
            return (object)itemsControl.GetValue(CollectionAnimationProperty);
        }

        /// <summary>
        /// The set collection animation.
        /// </summary>
        /// <param name="itemsControl"> The items control. </param>
        /// <param name="value"> The value. </param>
        public static void SetCollectionAnimation(DependencyObject itemsControl, object value)
        {
            itemsControl.SetValue(CollectionAnimationProperty, value);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.previousItemBorder = (Border)this.GetTemplateChild("PART_PreviousItemBorder");
            this.currentItemBorder = (Border)this.GetTemplateChild("PART_CurrentItemBorder");

            this.CrossfadeBackground(null, this.DisplayItem);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The get container for item override.
        /// </summary>
        /// <returns> The <see cref="DependencyObject"/>. </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new WizardItem();
        }

        /// <summary>
        /// The is item its own container override.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return false;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.UpdateModels();
            this.SelectRoot();
        }

        /// <summary>
        /// The on loaded.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// The on navigation started.
        /// </summary>
        /// <param name="oldItem"> The old item. </param>
        /// <param name="newItem"> The new item. </param>
        protected virtual void OnNavigationStarted(WizardItem oldItem, WizardItem newItem)
        {
            this.CrossfadeBackground(oldItem, newItem);

            if (oldItem != null)
            {
                oldItem.IsLeaving = true;
                if (oldItem.LeavingCommand != null)
                {
                    oldItem.LeavingCommand.Execute(oldItem.LeavingCommandParameter);
                }

                if ((oldItem.LeavingBranchCommand != null) && 
                    oldItem.Model.Children.All(x => !string.Equals(x.Id, newItem.Id, StringComparison.Ordinal)) &&
                    oldItem.Model.Children.SelectMany(x => x.Children).All(x => !string.Equals(x.Id, newItem.Id, StringComparison.Ordinal)))
                {
                    oldItem.LeavingBranchCommand.Execute(oldItem.LeavingBranchCommandParameter);
                }
            }
        }

        /// <summary>
        /// The on navigating.
        /// </summary>
        /// <param name="oldItem"> The old item. </param>
        /// <param name="newItem"> The new item. </param>
        protected virtual void OnNavigating(WizardItem oldItem, WizardItem newItem)
        {
            if (oldItem != null)
            {
                oldItem.IsLeaving = false;
                oldItem.IsSelected = false;
            }

            if (newItem != null)
            {
                newItem.IsEntering = true;

                if (!newItem.IsVisited && (newItem.EnteringFirstTimeCommand != null))
                {
                    newItem.EnteringFirstTimeCommand.Execute(newItem.EnteringFirstTimeCommandParameter);
                }

                if (newItem.EnteringCommand != null)
                {
                    newItem.EnteringCommand.Execute(newItem.EnteringCommandParameter);
                }

                newItem.IsSelected = true;

                this.DisplayItem = newItem;

                this.UpdateBreadcrumb();

                this.IsBackAllowed = this.CanBackAllowed();
                this.IsForwardAllowed = this.CanForwardAllowed();

                this.BackCommand.RaiseCanExecuteChanged();
                this.ForwardCommand.RaiseCanExecuteChanged();
                this.SelectCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The on navigation ended.
        /// </summary>
        /// <param name="oldItem"> The old item. </param>
        /// <param name="newItem"> The new item. </param>
        protected virtual void OnNavigationEnded(WizardItem oldItem, WizardItem newItem)
        {
            if (newItem != null)
            {
                newItem.IsEntering = false;
            }

            this.IsNavigatingBackward = false;
            this.IsNavigatingForward = false;
            this.IsNavigatingHorizontally = false;
        }

        /// <summary>
        /// Called when the selected item is changed.
        /// </summary>
        /// <param name="oldItem">The old wizard item.</param>
        /// <param name="newItem">The new wizard item.</param>
        /// <returns>A task representing the operation.</returns>
        protected virtual async Task OnSelectedItemChanged(WizardItem oldItem, WizardItem newItem)
        {
            this.IsNavigating = true;

            this.OnNavigationStarted(oldItem, newItem);

            // Skip the exiting transition if there was no old item.
            if (oldItem != null)
            {
                await Task.Delay(this.TransitionDuration.TimeSpan);
            }

            this.OnNavigating(oldItem, newItem);

            await Task.Delay(this.TransitionDuration.TimeSpan);

            this.OnNavigationEnded(oldItem, newItem);

            this.IsNavigating = false;
        }

        /// <summary>
        /// The on wizard item parent changed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event Arguments. </param>
        protected virtual void OnWizardItemParentChanged(object sender, RoutedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)sender;
            WizardItemCollection items = new WizardItemCollection(this.Items.OfType<WizardItem>());

            WizardItemModel model = wizardItem.Model;
            WizardItemModel oldParent = wizardItem.Model.Parent;

            // Disconnect from old parent.
            if (oldParent != null)
            {
                oldParent.Children.Remove(model);
                model.Parent = null;
            }

            // Connect to new parent.
            if ((wizardItem.ParentId != null) && items.Contains(wizardItem.ParentId))
            {
                WizardItemModel newParent = items[wizardItem.ParentId].Model;

                newParent.Children.Add(model);
                model.Parent = newParent;
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Determines whether this instance can navigate the specified new item from the old item.
        /// </summary>
        /// <param name="oldItem">The old item.</param>
        /// <param name="newItem">The new item.</param>
        /// <returns><c>true</c> if the wizard can navigate to the new item. Otherwise <c>false</c>.</returns>
        private static bool CanNavigate(WizardItem oldItem, WizardItem newItem)
        {
            bool canNavigate = true;

            var children = oldItem.Model.Children.SelectMany(x => x.Children);
            var d = children.All(x => !string.Equals(x.Id, newItem.Id));

            if ((oldItem != null) &&
                (oldItem.LeavingCommand != null) &&
                !oldItem.LeavingCommand.CanExecute(oldItem.LeavingCommandParameter))
            {
                canNavigate = false;
            }
            else if ((oldItem != null) &&
                (oldItem.LeavingBranchCommand != null) &&
                oldItem.Model.Children.All(x => !string.Equals(x.Id, newItem.Id, StringComparison.Ordinal)) &&
                oldItem.Model.Children.SelectMany(x => x.Children).All(x => !string.Equals(x.Id, newItem.Id, StringComparison.Ordinal)) &&
                !oldItem.LeavingBranchCommand.CanExecute(oldItem.LeavingBranchCommandParameter))
            {
                canNavigate = false;
            }
            else if ((newItem != null) &&
                !newItem.IsVisited &&
                (newItem.EnteringFirstTimeCommand != null) &&
                !newItem.EnteringFirstTimeCommand.CanExecute(newItem.EnteringFirstTimeCommandParameter))
            {
                canNavigate = false;
            }
            else if ((newItem != null) &&
                (newItem.EnteringCommand != null) &&
                !newItem.EnteringCommand.CanExecute(newItem.EnteringCommandParameter))
            {
                canNavigate = false;
            }

            return canNavigate;
        }

        /// <summary>
        /// The initial animation.
        /// </summary>
        /// <param name="frameworkElement"> The framework element. </param>
        private static void InitAnimation(FrameworkElement frameworkElement)
        {
            Wizard wizard = frameworkElement.FindVisualParent<Wizard>();
            WizardItem wizardItem = frameworkElement.FindVisualParent<WizardItem>();

            if ((wizard != null) && (wizardItem != null))
            {
                WizardAnimation animation = (WizardAnimation)Enum.Parse(
                    typeof(WizardAnimation),
                    GetAnimation(frameworkElement).ToString());

                BehaviorCollection behaviors = Interaction.GetBehaviors(frameworkElement);

                FadeBehavior fadeBehavior = null;
                if ((animation == WizardAnimation.Fade) || (animation == WizardAnimation.FadeAndSlide))
                {
                    fadeBehavior = behaviors.OfType<FadeBehavior>().FirstOrDefault();
                    if (fadeBehavior == null)
                    {
                        fadeBehavior = new FadeBehavior()
                        {
                            Duration = wizard.TransitionDuration.TimeSpan,
                        };
                        behaviors.Add(fadeBehavior);
                    }
                }

                SlideBehavior slideBehavior = null;
                if ((animation == WizardAnimation.Slide) || (animation == WizardAnimation.FadeAndSlide))
                {
                    slideBehavior = behaviors.OfType<SlideBehavior>().FirstOrDefault();
                    if (slideBehavior == null)
                    {
                        slideBehavior = new SlideBehavior()
                        {
                            Duration = wizard.TransitionDuration.TimeSpan,
                        };
                        behaviors.Add(slideBehavior);
                    }
                }

                wizardItem.Entering +=
                    (sender, e2) =>
                    {
                        if (fadeBehavior != null)
                        {
                            fadeBehavior.FadeIn();
                        }

                        if (slideBehavior != null)
                        {
                            slideBehavior.SlideIn();
                        }
                    };
                wizardItem.Leaving +=
                    (sender, e2) =>
                    {
                        if (fadeBehavior != null)
                        {
                            fadeBehavior.FadeOut();
                        }

                        if (slideBehavior != null)
                        {
                            slideBehavior.SlideOut();
                        }
                    };
            }
        }

        /// <summary>
        /// The initial collection animation.
        /// </summary>
        /// <param name="itemsControl"> The items control. </param>
        private static void InitCollectionAnimation(ItemsControl itemsControl)
        {
            Wizard wizard = itemsControl.FindVisualParent<Wizard>();
            WizardItem wizardItem = itemsControl.FindVisualParent<WizardItem>();

            if ((wizard != null) && (wizardItem != null))
            {
                WizardCollectionAnimation animation = (WizardCollectionAnimation)Enum.Parse(
                    typeof(WizardCollectionAnimation),
                    GetCollectionAnimation(itemsControl).ToString());

                BehaviorCollection behaviors = Interaction.GetBehaviors(itemsControl);
                AnimatingItemsControlBehavior behavior = behaviors.OfType<AnimatingItemsControlBehavior>().FirstOrDefault();

                if (behavior == null)
                {
                    behavior = new AnimatingItemsControlBehavior()
                    {
                        Duration = wizard.TransitionDuration.TimeSpan,
                        IsRandom = animation == WizardCollectionAnimation.Random
                    };
                    behaviors.Add(behavior);
                }

                wizardItem.Entering +=
                    (sender, e2) =>
                    {
                        behavior.AnimateIn();
                    };
                wizardItem.Leaving +=
                    (sender, e2) =>
                    {
                        behavior.AnimateOut();
                    };
            }
        }

        /// <summary>
        /// The on animation changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event Arguments. </param>
        private static void OnAnimationChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)dependencyObject;
            if (frameworkElement.IsLoaded)
            {
                InitAnimation(frameworkElement);
            }
            else
            {
                frameworkElement.Loaded += OnFrameworkElementLoaded;
            }
        }

        /// <summary>
        /// The on collection animation changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event Arguments. </param>
        private static void OnCollectionAnimationChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)dependencyObject;
            if (itemsControl.IsLoaded)
            {
                InitCollectionAnimation(itemsControl);
            }
            else
            {
                itemsControl.Loaded += OnItemsControlLoaded;
            }
        }

        /// <summary>
        /// The on framework element loaded.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event Arguments. </param>
        private static void OnFrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)sender;
            frameworkElement.Loaded -= OnFrameworkElementLoaded;
            InitAnimation(frameworkElement);
        }

        /// <summary>
        /// The on items control loaded.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event Arguments. </param>
        private static void OnItemsControlLoaded(object sender, RoutedEventArgs e)
        {
            ItemsControl itemsControl = (ItemsControl)sender;
            itemsControl.Loaded -= OnItemsControlLoaded;
            InitCollectionAnimation(itemsControl);
        }

        /// <summary>
        /// The on selected item changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event Arguments. </param>
        private static async void OnSelectedItemChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            Wizard wizard = (Wizard)dependencyObject;
            WizardItem oldItem = e.OldValue as WizardItem;
            WizardItem newItem = e.NewValue as WizardItem;

            await wizard.OnSelectedItemChanged(oldItem, newItem);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The get item.
        /// </summary>
        /// <param name="model"> The model. </param>
        /// <returns> The <see cref="WizardItem"/>. </returns>
        private WizardItem GetItem(WizardItemModel model)
        {
            return this.Items.OfType<WizardItem>().FirstOrDefault(x => string.Equals(x.Id, model.Id, StringComparison.Ordinal));
        }

        /// <summary>
        /// The can back allowed.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool CanBackAllowed()
        {
            bool canback = false;

            WizardItemModel selectedModel = this.SelectedModel;
            if ((selectedModel != null) && (selectedModel.Parent != null))
            {
                canback = true;
            }

            return canback;
        }

        /// <summary>
        /// The can back.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool CanBack()
        {
            bool canback = false;

            WizardItemModel selectedModel = this.SelectedModel;
            if ((selectedModel != null) && (selectedModel.Parent != null) && selectedModel.IsBackwardEnabled)
            {
                canback = true;
            }

            return canback;
        }

        /// <summary>
        /// The back.
        /// </summary>
        private void Back()
        {
            this.IsNavigatingBackward = true;
            WizardItem newItem = this.GetItem(this.SelectedModel.Parent);
            this.SetSelectedItemIfCanNavigate(newItem);
        }

        /// <summary>
        /// The can forward allowed.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool CanForwardAllowed()
        {
            bool canForward = false;

            WizardItemModel selectedModel = this.SelectedModel;
            if ((selectedModel != null) && (selectedModel.Children.Count > 0))
            {
                canForward = true;
            }

            return canForward;
        }

        /// <summary>
        /// The can forward.
        /// </summary>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool CanForward()
        {
            bool canForward = false;

            WizardItemModel selectedModel = this.SelectedModel;
            if ((selectedModel != null) && (selectedModel.Children.Count > 0) && selectedModel.IsForwardEnabled)
            {
                canForward = true;
            }

            return canForward;
        }

        /// <summary>
        /// The forward.
        /// </summary>
        private void Forward()
        {
            this.IsNavigatingForward = true;
            WizardItem newItem = this.GetItem(this.SelectedModel.Children.First());
            this.SetSelectedItemIfCanNavigate(newItem);
        }

        /// <summary>
        /// The can select.
        /// </summary>
        /// <param name="parameter"> The parameter. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        private bool CanSelect(object parameter)
        {
            WizardItemModel model = this.GetModel(parameter);
            return (model != null) && (this.SelectedItem != this.GetItem(model));
        }

        /// <summary>
        /// The on items collection changed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event Arguments. </param>
        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (WizardItem wizardItem in this.Items)
            {
                wizardItem.ParentChanged -= this.OnWizardItemParentChanged;
                wizardItem.ParentChanged += this.OnWizardItemParentChanged;
            }

            if (e.OldItems != null)
            {
                foreach (WizardItem wizardItem in e.OldItems)
                {
                    wizardItem.Model = null;
                }
            }

            if (this.IsLoaded)
            {
                this.UpdateModels();
            }
        }

        /// <summary>
        /// The select.
        /// </summary>
        /// <param name="parameter"> The parameter. </param>
        private void Select(object parameter)
        {
            WizardItemModel model = this.GetModel(parameter);

            if (model != null)
            {
                if (this.DisplayItem != null)
                {
                    WizardItemModel displayModel = this.DisplayItem.Model;
                    if (displayModel != null)
                    {
                        if (this.Breadcrumb.Contains(model))
                        {
                            // Going back through the breadcrumb.
                            this.IsNavigatingBackward = true;
                        }
                        else if (this.Breadcrumb
                            .Traverse(x => x.Children)
                            .Where(x => x != this.Breadcrumb.Last() && !this.Breadcrumb.Last().Children.Traverse(y => y.Children).Contains(x))
                            .Any(x => x.Equals(model)))
                        {
                            // Going sideways through the breadcrumb.
                            this.IsNavigatingHorizontally = true;
                        }
                        else
                        {
                            // Go forward with the breadcrumb.
                            this.IsNavigatingForward = true;
                        }
                    }
                }

                WizardItem newItem = this.GetItem(model);
                this.SetSelectedItemIfCanNavigate(newItem);
            }
        }

        /// <summary>
        /// Sets the selected item if the current instance can navigate to it.
        /// </summary>
        /// <param name="newItem">The new item.</param>
        private void SetSelectedItemIfCanNavigate(WizardItem newItem)
        {
            if (CanNavigate((WizardItem)this.SelectedItem, newItem))
            {
                this.SelectedItem = newItem;
            }
        }

        /// <summary>
        /// The cross fade background.
        /// </summary>
        /// <param name="previousItem"> The previous item. </param>
        /// <param name="currentItem"> The current item. </param>
        private void CrossfadeBackground(WizardItem previousItem, WizardItem currentItem)
        {
            if ((this.previousItemBorder != null) &&
                (previousItem != currentItem))
            {
                if (this.backgroundStoryboard != null)
                {
                    this.backgroundStoryboard.Stop();
                }

                if (previousItem != null)
                {
                    this.previousItemBorder.Background = previousItem.Background;
                    this.previousItemBorder.BorderBrush = previousItem.BorderBrush;
                    this.previousItemBorder.BorderThickness = previousItem.BorderThickness;
                }
                else
                {
                    this.previousItemBorder.Background = null;
                    this.previousItemBorder.BorderBrush = null;
                    this.previousItemBorder.BorderThickness = new Thickness();
                }

                if (currentItem != null)
                {
                    this.currentItemBorder.Background = currentItem.Background;
                    this.currentItemBorder.BorderBrush = currentItem.BorderBrush;
                    this.currentItemBorder.BorderThickness = currentItem.BorderThickness;
                }
                else
                {
                    this.currentItemBorder.Background = null;
                    this.currentItemBorder.BorderBrush = null;
                    this.currentItemBorder.BorderThickness = new Thickness();
                }

                if (this.backgroundStoryboard == null)
                {
                    this.backgroundStoryboard = new Storyboard()
                    {
                        Duration = this.TransitionDuration + this.TransitionDuration
                    };

                    DoubleAnimation previousDoubleAnimation = new DoubleAnimation()
                    {
                        From = 1,
                        To = 0
                    };
                    Storyboard.SetTarget(previousDoubleAnimation, this.previousItemBorder);
                    Storyboard.SetTargetProperty(previousDoubleAnimation, new PropertyPath("Opacity"));
                    this.backgroundStoryboard.Children.Add(previousDoubleAnimation);

                    DoubleAnimation currentDoubleAnimation = new DoubleAnimation()
                    {
                        From = 0,
                        To = 1
                    };
                    Storyboard.SetTarget(currentDoubleAnimation, this.currentItemBorder);
                    Storyboard.SetTargetProperty(currentDoubleAnimation, new PropertyPath("Opacity"));
                    this.backgroundStoryboard.Children.Add(currentDoubleAnimation);
                }

                this.backgroundStoryboard.Begin();
            }
        }

        /// <summary>
        /// Gets a <see cref="WizardItemModel"/> from the specified object which can be a <see cref="WizardItemModel"/>, <see cref="WizardItem"/> or wizard item ID.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The <see cref="WizardItemModel"/>.</returns>
        private WizardItemModel GetModel(object parameter)
        {
            WizardItemModel model = parameter as WizardItemModel;

            if (model == null)
            {
                WizardItem wizardItem = parameter as WizardItem;
                if (wizardItem != null)
                {
                    model = wizardItem.Model;
                }
            }

            if (model == null)
            {
                string wizardItemId = parameter as string;
                if (wizardItemId != null)
                {
                    model = this.models.FirstOrDefault(x => string.Equals(x.Id, wizardItemId, StringComparison.Ordinal));
                }
            }

            return model;
        }

        /// <summary>
        /// The select root.
        /// </summary>
        private void SelectRoot()
        {
            if (this.SelectedItem == null)
            {
                this.SelectedItem = this.Items.OfType<WizardItem>().FirstOrDefault(x => x.ParentId == null);
            }
        }

        /// <summary>
        /// The update breadcrumb.
        /// </summary>
        private void UpdateBreadcrumb()
        {
            WizardItemModel selectedModel = this.SelectedModel;
            if (selectedModel != null)
            {
                if (this.Breadcrumb.Contains(selectedModel))
                {
                    // Going back through the breadcrumb.
                    for (WizardItemModel lastModel = this.Breadcrumb.Last();
                        !lastModel.Equals(selectedModel);
                        lastModel = this.Breadcrumb.Last())
                    {
                        lastModel.IsVisited = false;
                        this.Breadcrumb.Remove(lastModel);
                    }
                }
                else if (this.Breadcrumb
                    .Traverse(x => x.Children)
                    .Where(x => x != this.Breadcrumb.Last() && !this.Breadcrumb.Last().Children.Traverse(y => y.Children).Contains(x))
                    .Any(x => x.Equals(selectedModel)))
                {
                    // Going sideways through the breadcrumb.
                    List<WizardItemModel> newBreadcrumb = new List<WizardItemModel>();
                    for (WizardItemModel parent = selectedModel.Parent; parent != null; parent = parent.Parent)
                    {
                        newBreadcrumb.Add(parent);
                    }

                    newBreadcrumb.Reverse();
                    newBreadcrumb.Add(selectedModel);

                    for (int i = 0; i < newBreadcrumb.Count; ++i)
                    {
                        WizardItemModel newModel = newBreadcrumb[i];

                        if ((i >= this.Breadcrumb.Count) || !this.Breadcrumb[i].Equals(newModel))
                        {
                            while (i < this.Breadcrumb.Count)
                            {
                                WizardItemModel oldLastModel = this.Breadcrumb.Last();
                                oldLastModel.IsVisited = false;
                                this.Breadcrumb.Remove(oldLastModel);
                            }

                            newModel.IsVisited = true;
                            this.Breadcrumb.Add(newModel);
                        }
                    }
                }
                else
                {
                    // Go forward with the breadcrumb.
                    if (!this.Breadcrumb.Contains(selectedModel))
                    {
                        selectedModel.IsVisited = true;

                        // If skipping a few pages to go ahead, add the in-between bread crumb items.
                        List<WizardItemModel> inBetweenModels = new List<WizardItemModel>();
                        for (WizardItemModel parent = selectedModel.Parent; parent != null; parent = parent.Parent)
                        {
                            if (!this.Breadcrumb.Contains(parent))
                            {
                                parent.IsVisited = true;
                                inBetweenModels.Add(parent);
                            }
                            else
                            {
                                break;
                            }
                        }

                        inBetweenModels.Reverse();
                        this.Breadcrumb.AddRange(inBetweenModels);

                        this.Breadcrumb.Add(selectedModel);
                    }
                }

                for (int i = 0; i < this.Breadcrumb.Count; ++i)
                {
                    WizardItemModel model = this.Breadcrumb[i];

                    // Select last breadcrumb.
                    model.IsSelected = i == (this.Breadcrumb.Count - 1);
                }
            }
        }

        /// <summary>
        /// The update models.
        /// </summary>
        private void UpdateModels()
        {
            WizardItemCollection items = new WizardItemCollection(this.Items.OfType<WizardItem>());

            // Remove any models that no longer exist.
            foreach (WizardItemModel model in this.models.ToList())
            {
                if (!items.Contains(model.Id))
                {
                    if (model.Parent != null)
                    {
                        model.Parent.Children.Remove(model);
                        model.Parent = null;
                    }

                    this.models.Remove(model);
                }
            }

            // Set up new models.
            foreach (WizardItem item in items)
            {
                if (item.Model == null)
                {
                    WizardItemModel model = new WizardItemModel()
                    {
                        BackCommand = this.BackCommand,
                        ForwardCommand = this.ForwardCommand,
                        SelectCommand = this.SelectCommand
                    };
                    item.Model = model;
                    this.models.Add(model);
                }
            }

            // Update the model tree structure.
            foreach (WizardItemModel model in this.models)
            {
                if (model.ParentId == null)
                {
                    model.Parent = null;
                }
                else
                {
                    WizardItemModel parent = this.models.First(x => string.Equals(x.Id, model.ParentId));

                    if (!parent.Children.Contains(model))
                    {
                        parent.Children.Add(model);
                    }

                    model.Parent = parent;
                }
            }
        }

        #endregion
    }
}
