namespace Framework.UI.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// The wizard item.
    /// </summary>
    public class WizardItem : ContentControl, IEquatable<WizardItem>
    {
        #region Dependency Properties

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description",
            typeof(string),
            typeof(WizardItem),
            new PropertyMetadata(null, OnDescriptionChanged));

        public static readonly DependencyProperty EnteringCommandProperty = DependencyProperty.Register(
            "EnteringCommand",
            typeof(ICommand),
            typeof(WizardItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EnteringCommandParameterProperty = DependencyProperty.Register(
            "EnteringCommandParameter",
            typeof(object),
            typeof(WizardItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EnteringFirstTimeCommandProperty = DependencyProperty.Register(
            "EnteringFirstTimeCommand",
            typeof(ICommand),
            typeof(WizardItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty EnteringFirstTimeCommandParameterProperty = DependencyProperty.Register(
            "EnteringFirstTimeCommandParameter",
            typeof(object),
            typeof(WizardItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            "Icon",
            typeof(object),
            typeof(WizardItem),
            new PropertyMetadata(null, OnIconChanged));

        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register(
            "IconTemplate", 
            typeof(DataTemplate), 
            typeof(WizardItem),
            new PropertyMetadata(null, OnIconTemplateChanged));

        public static readonly DependencyProperty IdProperty = DependencyProperty.Register(
            "Id",
            typeof(string),
            typeof(WizardItem),
            new PropertyMetadata(null, OnIdChanged));

        public static readonly DependencyProperty IsBackwardEnabledProperty = DependencyProperty.Register(
            "IsBackwardEnabled",
            typeof(bool),
            typeof(WizardItem),
            new PropertyMetadata(true, OnIsBackwardEnabledChanged));

        private static readonly DependencyPropertyKey IsEnteringPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsEntering",
            typeof(bool),
            typeof(WizardItem),
            new PropertyMetadata(false, OnIsEnteringChanged));

        public static readonly DependencyProperty IsEnteringProperty = IsEnteringPropertyKey.DependencyProperty;

        public static readonly DependencyProperty IsForwardEnabledProperty = DependencyProperty.Register(
            "IsForwardEnabled",
            typeof(bool),
            typeof(WizardItem),
            new PropertyMetadata(true, OnIsForwardEnabledChanged));

        public static readonly DependencyProperty IsForwardVisibleProperty = DependencyProperty.Register(
            "IsForwardVisible", 
            typeof(bool), 
            typeof(WizardItem),
            new PropertyMetadata(false, OnIsForwardVisibleChanged));

        private static readonly DependencyPropertyKey IsLeavingPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsLeaving",
            typeof(bool),
            typeof(WizardItem),
            new PropertyMetadata(false, OnIsLeavingChanged));

        public static readonly DependencyProperty IsLeavingProperty = IsLeavingPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsSelected",
            typeof(bool),
            typeof(WizardItem),
            new PropertyMetadata(false, OnIsSelectedChanged));

        public static readonly DependencyProperty IsSelectedProperty = IsSelectedPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsVisitedPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsVisited",
            typeof(bool),
            typeof(WizardItem),
            new PropertyMetadata(false, OnIsVisitedChanged));

        public static readonly DependencyProperty IsVisitedProperty = IsVisitedPropertyKey.DependencyProperty;

        public static readonly DependencyProperty LeavingCommandProperty = DependencyProperty.Register(
            "LeavingCommand",
            typeof(ICommand),
            typeof(WizardItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty LeavingCommandParameterProperty = DependencyProperty.Register(
            "LeavingCommandParameter",
            typeof(object),
            typeof(WizardItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty LeavingBranchCommandProperty = DependencyProperty.Register(
            "LeavingBranchCommand",
            typeof(ICommand),
            typeof(WizardItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty LeavingBranchCommandParameterProperty = DependencyProperty.Register(
            "LeavingBranchCommandParameter",
            typeof(object),
            typeof(WizardItem),
            new PropertyMetadata(null));

        private static readonly DependencyPropertyKey ModelPropertyKey = DependencyProperty.RegisterReadOnly(
            "Model",
            typeof(WizardItemModel),
            typeof(WizardItem),
            new PropertyMetadata(null, OnModelChanged));

        public static readonly DependencyProperty ModelProperty = ModelPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ParentIdProperty = DependencyProperty.Register(
            "ParentId",
            typeof(string),
            typeof(WizardItem),
            new PropertyMetadata(null, OnParentIdChanged));

        public static readonly DependencyProperty ShortTitleProperty = DependencyProperty.Register(
            "ShortTitle",
            typeof(string),
            typeof(WizardItem),
            new PropertyMetadata(null, OnShortTitleChanged));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(WizardItem),
            new PropertyMetadata(null, OnTitleChanged));

        #endregion

        #region Routed Events

        public static readonly RoutedEvent EnteringEvent = EventManager.RegisterRoutedEvent(
            "Entering",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(WizardItem));

        public static readonly RoutedEvent LeavingEvent = EventManager.RegisterRoutedEvent(
            "Leaving",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(WizardItem));

        public static readonly RoutedEvent ParentChangedEvent = EventManager.RegisterRoutedEvent(
            "ParentChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(WizardItem));

        public static readonly RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
            "Selected",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(WizardItem));

        public static readonly RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent(
            "Unselected",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(WizardItem));

        #endregion

        private bool isUpdating;
        private IDisposable modelChangedSubscription;

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="WizardItem"/> class.
        /// </summary>
        static WizardItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WizardItem), new FrameworkPropertyMetadata(typeof(WizardItem)));
            VisibilityProperty.OverrideMetadata(
                typeof(WizardItem),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnVisibilityChanged)));
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The entering.
        /// </summary>
        public event RoutedEventHandler Entering
        {
            add { this.AddHandler(EnteringEvent, value); }
            remove { this.RemoveHandler(EnteringEvent, value); }
        }

        /// <summary>
        /// The leaving.
        /// </summary>
        public event RoutedEventHandler Leaving
        {
            add { this.AddHandler(LeavingEvent, value); }
            remove { this.RemoveHandler(LeavingEvent, value); }
        }

        /// <summary>
        /// The parent changed.
        /// </summary>
        public event RoutedEventHandler ParentChanged
        {
            add { this.AddHandler(ParentChangedEvent, value); }
            remove { this.RemoveHandler(ParentChangedEvent, value); }
        }

        /// <summary>
        /// The selected.
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add { this.AddHandler(SelectedEvent, value); }
            remove { this.RemoveHandler(SelectedEvent, value); }
        }

        /// <summary>
        /// The unselected.
        /// </summary>
        public event RoutedEventHandler Unselected
        {
            add { this.AddHandler(UnselectedEvent, value); }
            remove { this.RemoveHandler(UnselectedEvent, value); }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get { return (string)this.GetValue(DescriptionProperty); }
            set { this.SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the entering command.
        /// </summary>
        public ICommand EnteringCommand
        {
            get { return (ICommand)this.GetValue(EnteringCommandProperty); }
            set { this.SetValue(EnteringCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the entering command parameter.
        /// </summary>
        public object EnteringCommandParameter
        {
            get { return (object)this.GetValue(EnteringCommandParameterProperty); }
            set { this.SetValue(EnteringCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the entering first time command.
        /// </summary>
        public ICommand EnteringFirstTimeCommand
        {
            get { return (ICommand)this.GetValue(EnteringFirstTimeCommandProperty); }
            set { this.SetValue(EnteringFirstTimeCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the entering command parameter.
        /// </summary>
        public object EnteringFirstTimeCommandParameter
        {
            get { return (object)this.GetValue(EnteringFirstTimeCommandParameterProperty); }
            set { this.SetValue(EnteringFirstTimeCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public object Icon
        {
            get { return (object)this.GetValue(IconProperty); }
            set { this.SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Gets or sets the icon data template.
        /// </summary>
        /// <value>
        /// The icon data template.
        /// </value>
        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)this.GetValue(IconTemplateProperty); }
            set { this.SetValue(IconTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id
        {
            get { return (string)this.GetValue(IdProperty); }
            set { this.SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is backward enabled.
        /// </summary>
        public bool IsBackwardEnabled
        {
            get { return (bool)this.GetValue(IsBackwardEnabledProperty); }
            set { this.SetValue(IsBackwardEnabledProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is entering.
        /// </summary>
        public bool IsEntering
        {
            get { return (bool)this.GetValue(IsEnteringProperty); }
            internal set { this.SetValue(IsEnteringPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is forward enabled.
        /// </summary>
        public bool IsForwardEnabled
        {
            get { return (bool)this.GetValue(IsForwardEnabledProperty); }
            set { this.SetValue(IsForwardEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is forward visible.
        /// </summary>
        public bool IsForwardVisible
        {
            get { return (bool)this.GetValue(IsForwardVisibleProperty); }
            set { this.SetValue(IsForwardVisibleProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is leaving.
        /// </summary>
        public bool IsLeaving
        {
            get { return (bool)this.GetValue(IsLeavingProperty); }
            internal set { this.SetValue(IsLeavingPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            internal set { this.SetValue(IsSelectedPropertyKey, value); }
        }

        /// <summary>
        /// Gets a value indicating whether is visited.
        /// </summary>
        public bool IsVisited
        {
            get { return (bool)this.GetValue(IsVisitedProperty); }
            internal set { this.SetValue(IsVisitedPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the leaving command.
        /// </summary>
        public ICommand LeavingCommand
        {
            get { return (ICommand)this.GetValue(LeavingCommandProperty); }
            set { this.SetValue(LeavingCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the leaving command parameter.
        /// </summary>
        public object LeavingCommandParameter
        {
            get { return (object)this.GetValue(LeavingCommandParameterProperty); }
            set { this.SetValue(LeavingCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the leaving branch command. This instance is being left and the <see cref="WizardItem"/> being entered is not a child of this instance.
        /// </summary>
        public ICommand LeavingBranchCommand
        {
            get { return (ICommand)this.GetValue(LeavingBranchCommandProperty); }
            set { this.SetValue(LeavingBranchCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the leaving branch command parameter.
        /// </summary>
        public object LeavingBranchCommandParameter
        {
            get { return (object)this.GetValue(LeavingBranchCommandParameterProperty); }
            set { this.SetValue(LeavingBranchCommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        public WizardItemModel Model
        {
            get { return (WizardItemModel)this.GetValue(ModelProperty); }
            internal set { this.SetValue(ModelPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public string ParentId
        {
            get { return (string)this.GetValue(ParentIdProperty); }
            set { this.SetValue(ParentIdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the short title.
        /// </summary>
        public string ShortTitle
        {
            get { return (string)this.GetValue(ShortTitleProperty); }
            set { this.SetValue(ShortTitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool Equals(WizardItem other)
        {
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.Id, other.Id, StringComparison.Ordinal);
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns> The <see cref="string"/>. </returns>
        public override string ToString()
        {
            return this.Title;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The on description changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnDescriptionChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on icon changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIconChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on icon template changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIconTemplateChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on id changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIdChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on is backward enabled changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIsBackwardEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();

            if (wizardItem.Model != null)
            {
                wizardItem.Model.BackCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The on is entering changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIsEnteringChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            if (wizardItem.IsEntering)
            {
                wizardItem.RaiseEvent(new RoutedEventArgs(EnteringEvent));
            }

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on is forward enabled changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIsForwardEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();

            if (wizardItem.Model != null)
            {
                wizardItem.Model.ForwardCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The on is forward visible changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIsForwardVisibleChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on is leaving changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event Arguments. </param>
        private static void OnIsLeavingChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            if (wizardItem.IsLeaving)
            {
                wizardItem.RaiseEvent(new RoutedEventArgs(LeavingEvent));
            }

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on is selected changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIsSelectedChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            if (wizardItem.IsSelected)
            {
                wizardItem.RaiseEvent(new RoutedEventArgs(SelectedEvent));
            }
            else
            {
                wizardItem.RaiseEvent(new RoutedEventArgs(UnselectedEvent));
            }

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on is visited changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnIsVisitedChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on model changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnModelChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnModelChanged();

            if (wizardItem.Model != null)
            {
                wizardItem.Model.BackCommand.RaiseCanExecuteChanged();
                wizardItem.Model.ForwardCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// The on parent id changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnParentIdChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();

            wizardItem.RaiseEvent(new RoutedEventArgs(ParentChangedEvent));
        }

        /// <summary>
        /// The on short title changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnShortTitleChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on title changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnTitleChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }

        /// <summary>
        /// The on visibility changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The event arguments. </param>
        private static void OnVisibilityChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            WizardItem wizardItem = (WizardItem)dependencyObject;

            wizardItem.OnPropertyChanged();
        }
            
        #endregion

        #region Private Methods

        /// <summary>
        /// The on model changed.
        /// </summary>
        private void OnModelChanged()
        {
            if (this.modelChangedSubscription != null)
            {
                this.modelChangedSubscription.Dispose();
                this.modelChangedSubscription = null;
            }

            if (this.Model != null)
            {
                this.UpdateToModel(this.Model);

                this.modelChangedSubscription = this.Model.WhenPropertyChanged.Subscribe(
                    this.OnModelPropertyChanged);
            }
        }

        /// <summary>
        /// The on model property changed.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void OnModelPropertyChanged(string propertyName)
        {
            this.UpdateFromModel(this.Model);
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        private void OnPropertyChanged()
        {
            this.UpdateToModel(this.Model);
        }

        /// <summary>
        /// The update from model.
        /// </summary>
        /// <param name="model"> The model. </param>
        private void UpdateFromModel(WizardItemModel model)
        {
            if (model != null && !this.isUpdating)
            {
                this.isUpdating = true;

                // this.Description = model.Description;
                // this.Icon = model.Icon;
                // this.IconTemplate = model.IconTemplate;
                // this.Id = model.Id;
                // this.IsBackwardEnabled = model.IsBackwardEnabled;
                this.IsEntering = model.IsEntering;

                // this.IsForwardEnabled = model.IsForwardEnabled;
                this.IsLeaving = model.IsLeaving;
                this.IsSelected = model.IsSelected;
                this.IsVisited = model.IsVisited;

                // this.ParentId = model.ParentId;
                // this.ShortTitle = model.ShortTitle;
                // this.Title = model.Title;
                this.isUpdating = false;
            }
        }

        /// <summary>
        /// The update to model.
        /// </summary>
        /// <param name="model"> The model. </param>
        private void UpdateToModel(WizardItemModel model)
        {
            if (model != null && !this.isUpdating)
            {
                this.isUpdating = true;

                model.Description = this.Description;
                model.Icon = this.Icon;
                model.IconTemplate = this.IconTemplate;
                model.Id = this.Id;
                model.IsBackwardEnabled = this.IsBackwardEnabled;
                model.IsEntering = this.IsEntering;
                model.IsForwardEnabled = this.IsForwardEnabled;
                model.IsLeaving = this.IsLeaving;
                model.IsSelected = this.IsSelected;
                model.IsVisited = this.IsVisited;
                model.ParentId = this.ParentId;
                model.ShortTitle = this.ShortTitle;
                model.Title = this.Title;
                model.Visibility = this.Visibility;

                this.isUpdating = false;
            }
        }

        #endregion
    }
}
