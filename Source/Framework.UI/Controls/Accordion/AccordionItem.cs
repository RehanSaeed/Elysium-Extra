namespace Framework.UI.Controls
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using Framework.UI.Automation.Peers;

    /// <summary>
    /// Represents a control that displays a header and has a collapsible 
    /// content window.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [TemplateVisualState(Name = VisualStates.StateNormal, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateMouseOver, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StatePressed, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateDisabled, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateFocused, GroupName = VisualStates.GroupFocus)]
    [TemplateVisualState(Name = VisualStates.StateUnfocused, GroupName = VisualStates.GroupFocus)]
    [TemplateVisualState(Name = VisualStates.StateExpanded, GroupName = VisualStates.GroupExpansion)]
    [TemplateVisualState(Name = VisualStates.StateCollapsed, GroupName = VisualStates.GroupExpansion)]
    [TemplateVisualState(Name = VisualStates.StateLocked, GroupName = VisualStates.GroupLocked)]
    [TemplateVisualState(Name = VisualStates.StateUnlocked, GroupName = VisualStates.GroupLocked)]
    [TemplateVisualState(Name = VisualStates.StateExpandDown, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandUp, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandLeft, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandRight, GroupName = VisualStates.GroupExpandDirection)]
    [TemplatePart(Name = ElementExpandSiteName, Type = typeof(ExpandableContentControl))]
    [TemplatePart(Name = ElementExpanderButtonName, Type = typeof(AccordionButton))]
    [StyleTypedProperty(Property = "AccordionButtonStyle", StyleTargetType = typeof(AccordionButton))]
    [StyleTypedProperty(Property = "ExpandableContentControlStyle", StyleTargetType = typeof(ExpandableContentControl))]
    public class AccordionItem : HeaderedContentControl, IUpdateVisualState
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the AccordionButtonStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty AccordionButtonStyleProperty = DependencyProperty.Register(
            "AccordionButtonStyle",
            typeof(Style),
            typeof(AccordionItem),
            new PropertyMetadata(OnAccordionButtonStylePropertyChanged));

        /// <summary>
        /// Identifies the ContentTargetSize dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTargetSizeProperty = DependencyProperty.Register(
            "ContentTargetSize",
            typeof(Size),
            typeof(AccordionItem),
            new PropertyMetadata(new Size(double.NaN, double.NaN), OnContentTargetSizePropertyChanged));

        /// <summary>
        /// Identifies the ExpandableContentControlStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpandableContentControlStyleProperty =
            DependencyProperty.Register(
            "ExpandableContentControlStyle",
            typeof(Style),
            typeof(AccordionItem),
            new PropertyMetadata(OnExpandableContentControlStylePropertyChanged));

        /// <summary>
        /// Identifies the ExpandDirection dependency property. 
        /// </summary>
        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(
            "ExpandDirection",
            typeof(ExpandDirection),
            typeof(AccordionItem),
            new PropertyMetadata(ExpandDirection.Down, OnExpandDirectionPropertyChanged));

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected",
            typeof(bool),
            typeof(AccordionItem),
            new PropertyMetadata(OnIsSelectedPropertyChanged));

        #endregion

        #region Routed Events

        /// <summary>
        /// Occurs when the accordionItem is selected.
        /// </summary>
        public static RoutedEvent SelectedEvent = EventManager.RegisterRoutedEvent(
            "Selected",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(AccordionItem));

        /// <summary>
        /// Occurs when the accordionItem is unselected.
        /// </summary>
        public static RoutedEvent UnselectedEvent = EventManager.RegisterRoutedEvent(
            "Unselected",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(AccordionItem));

        #endregion

        #region Fields

        /// <summary>
        /// The name of the ExpanderButton template part.
        /// </summary>
        private const string ElementExpanderButtonName = "ExpanderButton";

        /// <summary>
        /// The name of the ExpandSite template part.
        /// </summary>
        private const string ElementExpandSiteName = "ExpandSite";

        /// <summary>
        /// Determines whether it is allowed to set the ContentTargetSize
        /// property.
        /// </summary>
        private bool allowedToWriteContentTargetSize;

        /// <summary>
        /// Determines whether the ExpandDirection property may be written.
        /// </summary>
        private bool allowedToWriteExpandDirection;

        /// <summary>
        /// The ExpanderButton template part is a ToggleButton contained within a template that's 
        /// used to select and unselect this AccordionItem.
        /// </summary>
        private AccordionButton expanderButton;

        /// <summary>
        /// BackingField for the ExpandSite property.
        /// </summary>
        private ExpandableContentControl expandSite;

        /// <summary>
        /// Gets or sets the helper that provides all of the standard
        /// interaction functionality.
        /// </summary>
        private InteractionHelper interaction;

        /// <summary>
        /// Indicates that the control is currently executing an action.
        /// </summary>
        private bool isBusyWithAction;

        /// <summary>
        /// BackingField for IsLocked.
        /// </summary>
        private bool isLocked;

        /// <summary>
        /// Nested level for IsSelectedCoercion.
        /// </summary>
        private int isSelectedNestedLevel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="AccordionItem"/> class.
        /// </summary>
        static AccordionItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionItem), new FrameworkPropertyMetadata(typeof(AccordionItem)));
        }

        /// <summary>
        /// Initialises a new instance of the AccordionItem class.
        /// </summary>
        public AccordionItem()
        {
            // initialize to no action.
            this.ScheduledAction = AccordionAction.None;

            this.interaction = new InteractionHelper(this);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when the accordionItem is selected.
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add { this.AddHandler(SelectedEvent, value); }
            remove { this.RemoveHandler(SelectedEvent, value); }
        }

        /// <summary>
        /// Occurs when the accordionItem is unselected.
        /// </summary>
        public event RoutedEventHandler Unselected
        {
            add { this.AddHandler(UnselectedEvent, value); }
            remove { this.RemoveHandler(UnselectedEvent, value); }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Style used by AccordionButton.
        /// </summary>
        public Style AccordionButtonStyle
        {
            get { return this.GetValue(AccordionButtonStyleProperty) as Style; }
            set { this.SetValue(AccordionButtonStyleProperty, value); }
        }

        /// <summary>
        /// Gets the Size that the content will animate to.
        /// </summary>
        public Size ContentTargetSize
        {
            get
            {
                return (Size)this.GetValue(ContentTargetSizeProperty);
            }

            internal set
            {
                this.allowedToWriteContentTargetSize = true;
                this.SetValue(ContentTargetSizeProperty, value);
                this.allowedToWriteContentTargetSize = false;
            }
        }

        /// <summary>
        /// Gets or sets the Style used by ExpandableContentControl.
        /// </summary>
        public Style ExpandableContentControlStyle
        {
            get { return this.GetValue(ExpandableContentControlStyleProperty) as Style; }
            set { this.SetValue(ExpandableContentControlStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the direction in which the AccordionItem content window opens.
        /// </summary>
        public ExpandDirection ExpandDirection
        {
            get
            {
                return (ExpandDirection)this.GetValue(ExpandDirectionProperty);
            }

            protected internal set
            {
                this.allowedToWriteExpandDirection = true;
                this.SetValue(ExpandDirectionProperty, value);
                this.allowedToWriteExpandDirection = false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the AccordionItem cannot be 
        /// selected by the user.
        /// </summary>
        /// <value><c>True</c> if this instance is locked; otherwise, <c>false</c>.</value>
        /// <remarks>The IsSelected property may not be changed when the 
        /// AccordionItem is locked. Locking occurs when the item is the first 
        /// in the list, the SelectionMode of Accordion requires at least one selected
        /// AccordionItem and the AccordionItem is currently selected.</remarks>
        public bool IsLocked
        {
            get
            {
                return this.isLocked;
            }

            internal set
            {
                if (this.isLocked != value)
                {
                    this.isLocked = value;

                    this.UpdateVisualState(true);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the AccordionItem is 
        /// selected and its content window is visible.
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets a reference to the parent Accordion of an
        /// AccordionItem.
        /// </summary>
        internal Accordion ParentAccordion { get; set; }

        /// <summary>
        /// Gets the relevant size of the current content.
        /// </summary>
        internal Size RelevantContentSize
        {
            get { return this.ExpandSite == null ? new Size(0, 0) : this.ExpandSite.RelevantContentSize; }
        }

        /// <summary>
        /// Gets the scheduled action.
        /// </summary>
        /// <value>The scheduled action.</value>
        internal AccordionAction ScheduledAction { get; private set; }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets or sets the ExpanderButton template part.
        /// </summary>
        private AccordionButton ExpanderButton
        {
            get
            {
                return this.expanderButton;
            }

            set
            {
                // Detach from old ExpanderButton
                if (this.expanderButton != null)
                {
                    this.expanderButton.Click -= this.OnExpanderButtonClicked;
                    this.expanderButton.ParentAccordionItem = null;
                    this.expanderButton.SizeChanged -= this.OnHeaderSizeChanged;
                }

                this.expanderButton = value;

                if (this.expanderButton != null)
                {
                    this.expanderButton.IsChecked = this.IsSelected;
                    this.expanderButton.Click += this.OnExpanderButtonClicked;
                    this.expanderButton.ParentAccordionItem = this;
                    this.expanderButton.SizeChanged += this.OnHeaderSizeChanged;
                }
            }
        }

        /// <summary>
        /// Gets or sets the expand site template part.
        /// </summary>
        private ExpandableContentControl ExpandSite
        {
            get
            {
                return this.expandSite;
            }

            set
            {
                if (this.expandSite != null)
                {
                    this.expandSite.ContentSizeChanged -= this.OnExpandSiteContentSizeChanged;
                }

                this.expandSite = value;

                if (this.expandSite != null)
                {
                    this.expandSite.ContentSizeChanged += this.OnExpandSiteContentSizeChanged;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the AccordionItem fills height.
        /// </summary>
        private bool ShouldFillHeight
        {
            get
            {
                return (this.ExpandDirection == ExpandDirection.Down || this.ExpandDirection == ExpandDirection.Up) &&
                       (!double.IsNaN(this.ContentTargetSize.Height) || this.VerticalAlignment == VerticalAlignment.Stretch);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the AccordionItem fills width.
        /// </summary>
        private bool ShouldFillWidth
        {
            get
            {
                return (this.ExpandDirection == ExpandDirection.Left || this.ExpandDirection == ExpandDirection.Right) &&
                       (!double.IsNaN(this.ContentTargetSize.Width) || this.HorizontalAlignment == HorizontalAlignment.Stretch);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the visual tree for the AccordionItem control when a new 
        /// template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.ExpanderButton = this.GetTemplateChild(ElementExpanderButtonName) as AccordionButton;
            this.ExpandSite = this.GetTemplateChild(ElementExpandSiteName) as ExpandableContentControl;

            if (VisualTreeHelper.GetChildrenCount(this) > 0)
            {
                FrameworkElement root = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
            }

            this.interaction.OnApplyTemplateBase();

            this.UpdateVisualState(false);

            // the UpdateVisualState will not set the expand or collapse state.
            if (this.IsSelected)
            {
                this.Schedule(AccordionAction.Expand);
            }
            else
            {
                this.Schedule(AccordionAction.Collapse);
            }
        }

        /// <summary>
        /// Update the visual state of the control.
        /// </summary>
        /// <param name="useTransitions">
        /// A value indicating whether to automatically generate transitions to
        /// the new state, or instantly transition to the new state.
        /// </param>
        void IUpdateVisualState.UpdateVisualState(bool useTransitions)
        {
            this.UpdateVisualState(useTransitions);
        }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Prepares the specified container to display the specified item.
        /// </summary>
        /// <param name="element">
        /// Container element used to display the specified item.
        /// </param>
        /// <param name="item">Specified item to display.</param>
        /// <param name="parent">The parent ItemsControl.</param>
        /// <param name="parentItemContainerStyle">
        /// The ItemContainerStyle for the parent ItemsControl.
        /// </param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parent", Justification = "Following ItemsControl signature.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "parentItemContainerStyle", Justification = "Following ItemsControl signature.")]
        internal static void PreparePrepareHeaderedContentControlContainerForItemOverride(HeaderedContentControl element, object item, ItemsControl parent, Style parentItemContainerStyle)
        {
            if (element != item)
            {
                // We do not have proper access to Visual.
                // Nor do we keep track of the HeaderIsItem property.
                if (!(item is UIElement) && HasDefaultValue(element, AccordionItem.HeaderProperty))
                {
                    element.Header = item;
                }
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Starts an action, such as resize, collapse or expand.
        /// </summary>
        internal virtual void StartAction()
        {
            if (this.ScheduledAction == AccordionAction.None)
            {
                throw new InvalidOperationException(AccordionResources.AccordionItem_StartAction_InvalidCall);
            }

            Action layoutAction;

            switch (this.ScheduledAction)
            {
                case AccordionAction.Collapse:
                    layoutAction = () =>
                    {
                        VisualStateManager.GoToState(this, VisualStates.StateExpanded, false);

                        // We only want to notify the parent that this action is finished when we are done with
                        // the state transition. In SL this is done through OnStoryboardFinished, but the same code
                        // in WPF would result in a corrupted state because OnStoryboardFinished gets called more frequently than expected.
                        // In the WPF case, we make the scheduled action synchornous to ensure the transition is properly done.
                        if (VisualStateManager.GoToState(this, VisualStates.StateCollapsed, true))
                        {
                            ParentAccordion.OnActionFinish(this);
                        }
                    };
                    break;
                case AccordionAction.Expand:
                    layoutAction = () =>
                    {
                        VisualStateManager.GoToState(this, VisualStates.StateCollapsed, false);

                        if (VisualStateManager.GoToState(this, VisualStates.StateExpanded, true))
                        {
                            ParentAccordion.OnActionFinish(this);
                        }
                    };
                    break;
                case AccordionAction.Resize:
                    layoutAction = () =>
                    {
                        // trigger ExpandedState to run again, by quickly moving to collapsed. 
                        // the effect is not noticeable because no layout pass is done.
                        VisualStateManager.GoToState(this, VisualStates.StateExpanded, false);
                        VisualStateManager.GoToState(this, VisualStates.StateCollapsed, false);

                        if (VisualStateManager.GoToState(this, VisualStates.StateExpanded, true))
                        {
                            ParentAccordion.OnActionFinish(this);
                        }
                    };
                    break;
                default:
                    {
                        string message = string.Format(
                            CultureInfo.InvariantCulture,
                            AccordionResources.AccordionItem_StartAction_InvalidAction,
                            this.ScheduledAction);

                        throw new NotSupportedException(message);
                    }
            }

            this.ScheduledAction = AccordionAction.None;
            this.isBusyWithAction = true;
            layoutAction();
        }

        /// <summary>
        /// Update the current visual state of the button.
        /// </summary>
        /// <param name="useTransitions">
        /// True to use transitions when updating the visual state, false to
        /// snap directly to the new visual state.
        /// </param>
        internal virtual void UpdateVisualState(bool useTransitions)
        {
            if (this.IsLocked)
            {
                VisualStates.GoToState(this, useTransitions, VisualStates.StateLocked);
            }
            else
            {
                VisualStates.GoToState(this, useTransitions, VisualStates.StateUnlocked);
            }

            switch (this.ExpandDirection)
            {
                case ExpandDirection.Down:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandDown);
                    break;

                case ExpandDirection.Up:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandUp);
                    break;

                case ExpandDirection.Left:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandLeft);
                    break;

                default:
                    VisualStates.GoToState(this, useTransitions, VisualStates.StateExpandRight);
                    break;
            }

            // let the header know a change has possibly occured.
            if (this.ExpanderButton != null)
            {
                this.ExpanderButton.UpdateVisualState(useTransitions);
            }

            // Handle the Common and Focused states
            this.interaction.UpdateVisualStateBase(useTransitions);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Called when AccordionButtonStyle is changed.
        /// </summary>
        /// <param name="oldStyle">The old style.</param>
        /// <param name="newStyle">The new style.</param>
        protected virtual void OnAccordionButtonStyleChanged(Style oldStyle, Style newStyle)
        {
        }

        /// <summary>
        /// Returns a AccordionItemAutomationPeer for use by the Silverlight
        /// automation infrastructure.
        /// </summary>
        /// <returns>A AccordionItemAutomationPeer object for the AccordionItem.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new AccordionItemWrapperAutomationPeer(this);
        }

        /// <summary>
        /// Called when ExpandableContentControlStyle is changed.
        /// </summary>
        /// <param name="oldStyle">The old style.</param>
        /// <param name="newStyle">The new style.</param>
        protected virtual void OnExpandableContentControlStyleChanged(Style oldStyle, Style newStyle)
        {
        }

        /// <summary>
        /// Provides handling for the GotFocus event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (this.interaction.AllowGotFocus(e))
            {
                this.interaction.OnGotFocusBase();
                base.OnGotFocus(e);
            }
        }

        /// <summary>
        /// Provides handling for the LostFocus event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (this.interaction.AllowLostFocus(e))
            {
                this.interaction.OnLostFocusBase();
                base.OnLostFocus(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseEnter event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (this.interaction.AllowMouseEnter(e))
            {
                this.interaction.OnMouseEnterBase();
                base.OnMouseEnter(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseLeave event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.interaction.AllowMouseLeave(e))
            {
                this.interaction.OnMouseLeaveBase();
                base.OnMouseLeave(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseLeftButtonDown event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (this.interaction.AllowMouseLeftButtonDown(e))
            {
                this.interaction.OnMouseLeftButtonDownBase();
                base.OnMouseLeftButtonDown(e);
            }
        }

        /// <summary>
        /// Called before the MouseLeftButtonUp event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (this.interaction.AllowMouseLeftButtonUp(e))
            {
                this.interaction.OnMouseLeftButtonUpBase();
                base.OnMouseLeftButtonUp(e);
            }
        }

        /// <summary>
        /// Provides handling for the KeyDown event.
        /// </summary>
        /// <param name="e">Key event args.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Handled || !this.IsEnabled || this.IsLocked)
            {
                return;
            }

            bool isSelected = this.IsSelected;
            switch (this.ExpandDirection)
            {
                case ExpandDirection.Down:
                    if ((isSelected && e.Key == Key.Up) || (!isSelected && e.Key == Key.Down))
                    {
                        this.IsSelected = !isSelected;
                    }

                    break;
                case ExpandDirection.Up:
                    if ((isSelected && e.Key == Key.Down) || (!isSelected && e.Key == Key.Up))
                    {
                        this.IsSelected = !isSelected;
                    }

                    break;
                case ExpandDirection.Left:
                    if ((isSelected && e.Key == Key.Right) || (!isSelected && e.Key == Key.Left))
                    {
                        this.IsSelected = !isSelected;
                    }

                    break;
                case ExpandDirection.Right:
                    if ((isSelected && e.Key == Key.Left) || (!isSelected && e.Key == Key.Right))
                    {
                        this.IsSelected = !isSelected;
                    }

                    break;
            }
        }

        /// <summary>
        /// Raises the Selected event when the IsSelected property changes 
        /// from false to true.
        /// </summary>
        protected virtual void OnSelected()
        {
            this.ToggleSelected(new RoutedEventArgs(SelectedEvent));
        }

        /// <summary>
        /// Raises the Unselected event when the IsSelected property changes 
        /// from true to false.
        /// </summary>
        protected virtual void OnUnselected()
        {
            this.ToggleSelected(new RoutedEventArgs(UnselectedEvent));
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Check whether a control has the default value for a property.
        /// </summary>
        /// <param name="control">The control to check.</param>
        /// <param name="property">The property to check.</param>
        /// <returns>
        /// True if the property has the default value; false otherwise.
        /// </returns>
        private static bool HasDefaultValue(Control control, DependencyProperty property)
        {
            Debug.Assert(control != null, "control should not be null!");
            Debug.Assert(property != null, "property should not be null!");
            return control.ReadLocalValue(property) == DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// AccordionButtonStyleProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">AccordionItem that changed its AccordionButtonStyle.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnAccordionButtonStylePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem source = (AccordionItem)dependencyObject;
            source.OnAccordionButtonStyleChanged(e.OldValue as Style, e.NewValue as Style);
        }

        /// <summary>
        /// ContentTargetSizeProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">AccordionItem that changed its ContentTargetSize.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnContentTargetSizePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem source = (AccordionItem)dependencyObject;
            Size targetSize = (Size)e.NewValue;

            if (!source.allowedToWriteContentTargetSize)
            {
                // revert to old value
                source.ContentTargetSize = (Size)e.OldValue;

                throw new InvalidOperationException(AccordionResources.AccordionItem_InvalidWriteToContentTargetSize);
            }

            // Pass the value to the expandSite
            // This is done explicitly so an animation action can be scheduled
            // deterministicly.
            ExpandableContentControl expandSite = source.ExpandSite;
            if (expandSite != null && !expandSite.TargetSize.Equals(targetSize))
            {
                expandSite.TargetSize = targetSize;
                if (source.IsSelected)
                {
                    if (source.ParentAccordion != null && source.ParentAccordion.IsResizing)
                    {
                        // if the accordion is resizing, this item should snap immediately
                        expandSite.RecalculatePercentage(1);
                    }
                    else
                    {
                        // otherwise schedule the resize
                        source.Schedule(AccordionAction.Resize);
                    }
                }
            }
        }

        /// <summary>
        /// ExpandableContentControlStyleProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">AccordionItem that changed its ExpandableContentControlStyle.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnExpandableContentControlStylePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem source = (AccordionItem)dependencyObject;
            source.OnExpandableContentControlStyleChanged(e.OldValue as Style, e.NewValue as Style);
        }

        /// <summary>
        /// ExpandDirectionProperty <see cref="PropertyChangedCallback"/> call back static 
        /// function.
        /// This function validates the new value before calling virtual function 
        /// OnExpandDirectionChanged.
        /// </summary>
        /// <param name="dependencyObject">Expander object whose ExpandDirection property is 
        /// changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs which contains 
        /// the old and new values.</param>
        private static void OnExpandDirectionPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem ctrl = (AccordionItem)dependencyObject;
            ExpandDirection oldValue = (ExpandDirection)e.OldValue;
            ExpandDirection newValue = (ExpandDirection)e.NewValue;

            if (!ctrl.allowedToWriteExpandDirection)
            {
                // revert to old value
                ctrl.ExpandDirection = oldValue;

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    AccordionResources.AccordionItem_InvalidWriteToExpandDirection,
                    newValue);
                throw new InvalidOperationException(message);
            }

            // invalid value. This check is not of great importance anymore since 
            // the previous check should catch all invalid sets.
            if (newValue != ExpandDirection.Down &&
                newValue != ExpandDirection.Left &&
                newValue != ExpandDirection.Right &&
                newValue != ExpandDirection.Up)
            {
                // revert to old value
                ctrl.ExpandDirection = oldValue;

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    AccordionResources.Expander_OnExpandDirectionPropertyChanged_InvalidValue,
                    newValue);
                throw new ArgumentException(message, "e");
            }

            if (ctrl.ExpandSite != null)
            {
                // Jump to correct percentage after a direction change
                ctrl.ExpandSite.RecalculatePercentage(ctrl.IsSelected ? 1 : 0);
            }

            ctrl.UpdateVisualState(true);
        }

        /// <summary>
        /// SelectedProperty <see cref="PropertyChangedCallback"/> static function.
        /// </summary>
        /// <param name="dependencyObject">Expander object whose Expanded property is changed.</param>
        /// <param name="e">DependencyPropertyChangedEventArgs which contains the 
        /// old and new values.</param>
        private static void OnIsSelectedPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            AccordionItem ctrl = (AccordionItem)dependencyObject;
            bool isSelected = (bool)e.NewValue;

            // Not allowed to change the IsSelected state when locked.
            if (ctrl.IsLocked && ctrl.isSelectedNestedLevel == 0)
            {
                ctrl.isSelectedNestedLevel++;
                ctrl.SetValue(IsSelectedProperty, e.OldValue);
                ctrl.isSelectedNestedLevel--;

                throw new InvalidOperationException(AccordionResources.AccordionItem_OnIsSelectedPropertyChanged_InvalidChange);
            }

            if (ctrl.isSelectedNestedLevel == 0)
            {
                Accordion parent = ctrl.ParentAccordion;
                if (parent != null)
                {
                    if (isSelected)
                    {
                        parent.OnAccordionItemSelected(ctrl);
                    }
                    else
                    {
                        parent.OnAccordionItemUnselected(ctrl);
                    }
                }

                if (isSelected)
                {
                    ctrl.OnSelected();
                }
                else
                {
                    ctrl.OnUnselected();
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Handle ExpanderButton's click event.
        /// </summary>
        /// <param name="sender">The ExpanderButton in template.</param>
        /// <param name="e">Routed event argument.</param>
        private void OnExpanderButtonClicked(object sender, RoutedEventArgs e)
        {
            // If the item is locked, the item is not permitted to change its selection state
            if (!this.IsLocked)
            {
                this.IsSelected = !this.IsSelected;
            }
        }

        /// <summary>
        /// Called when the content changes size.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnExpandSiteContentSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // only needed if we are not currently already working on a transition
            // if a sizechange occurs during a transition, it is not possible
            // to distinquish between one triggered by the expand/collapse 
            // storyboard, or one by the content itself.
            if (this.IsSelected && this.isBusyWithAction == false)
            {
                // only undertake this in a situation where the resized content
                // can be shown, ie. in a non-fixed scenario.
                if ((!this.ShouldFillWidth && e.PreviousSize.Width != e.NewSize.Width) ||
                    (!this.ShouldFillHeight && e.PreviousSize.Height != e.NewSize.Height))
                {
                    // since size has changed, a fresh approach should be taken
                    this.ExpandSite.MeasureContent(this.ExpandSite.CalculateDesiredContentSize());
                    this.ExpandSite.RecalculatePercentage(this.ExpandSite.TargetSize);

                    // schedule a resize to move to this new size
                    this.Schedule(AccordionAction.Resize);
                }
            }
        }

        /// <summary>
        /// Called when the size of the control changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnHeaderSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // allow the parent to reschedule a layout pass.
            if (this.ParentAccordion != null)
            {
                this.ParentAccordion.OnHeaderSizeChange(this);
            }
        }

        /// <summary>
        /// Schedules the specified action.
        /// </summary>
        /// <param name="action">The action to be performed.</param>
        private void Schedule(AccordionAction action)
        {
            this.ScheduledAction = action;

            if (this.ParentAccordion == null)
            {
                // no parentaccordion to notify, so just execute.
                this.StartAction();
            }
            else
            {
                bool directExecute = this.ParentAccordion.ScheduleAction(this, action);
                if (directExecute)
                {
                    this.StartAction();
                }
            }
        }

        /// <summary>
        /// Handle changes to the IsSelected property.
        /// </summary>
        /// <param name="args">Event arguments.</param>
        private void ToggleSelected(RoutedEventArgs args)
        {
            ToggleButton expander = this.ExpanderButton;
            if (expander != null)
            {
                expander.IsChecked = this.IsSelected;
            }

            if (this.IsSelected)
            {
                this.Schedule(AccordionAction.Expand);
            }
            else
            {
                this.Schedule(AccordionAction.Collapse);
            }

            this.UpdateVisualState(true);

            // WPF has a slightly different mechanism for raising routed events
            this.RaiseEvent(args);
        }

        #endregion Layout
    }
}
