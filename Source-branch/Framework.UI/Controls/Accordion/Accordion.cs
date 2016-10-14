namespace Framework.UI.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using Framework.UI.Automation.Peers;

    /// <summary>
    /// Represents a collection of collapsed and expanded AccordionItem controls.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(AccordionItem))]
    [StyleTypedProperty(Property = AccordionButtonStyleName, StyleTargetType = typeof(AccordionButton))]
    [TemplateVisualState(Name = VisualStates.StateNormal, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateMouseOver, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StatePressed, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateDisabled, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateFocused, GroupName = VisualStates.GroupFocus)]
    [TemplateVisualState(Name = VisualStates.StateUnfocused, GroupName = VisualStates.GroupFocus)]
    public class Accordion : ItemsControl, IUpdateVisualState
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the AccordionButtonStyle dependency property.
        /// </summary>
        public static readonly DependencyProperty AccordionButtonStyleProperty = DependencyProperty.Register(
            AccordionButtonStyleName,
            typeof(Style),
            typeof(Accordion),
            new PropertyMetadata(null, OnAccordionButtonStylePropertyChanged));

        /// <summary>
        /// Identifies the ContentTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(Accordion),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the ExpandDirection dependency property.
        /// </summary>
        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(
            "ExpandDirection",
            typeof(ExpandDirection),
            typeof(Accordion),
            new PropertyMetadata(ExpandDirection.Down, OnExpandDirectionPropertyChanged));

        /// <summary>
        /// Identifies the SelectedIndices dependency property.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Indices", Justification = "Framework uses indices.")]
        public static readonly DependencyProperty SelectedIndicesProperty = DependencyProperty.Register(
            "SelectedIndices",
            typeof(IList<int>),
            typeof(Accordion),
            new PropertyMetadata(null, OnSelectedIndicesChanged));

        /// <summary>
        /// Identifies the SelectedIndex dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(Accordion),
            new PropertyMetadata(-1, OnSelectedIndexPropertyChanged));

        /// <summary>
        /// Identifies the SelectedItem dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(Accordion),
            new PropertyMetadata(null, OnSelectedItemPropertyChanged));

        /// <summary>
        /// Identifies the SelectedItems dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
            "SelectedItems",
            typeof(IList),
            typeof(Accordion),
            new PropertyMetadata(OnSelectedItemsChanged));

        /// <summary>
        /// Identifies the SelectionMode dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode",
            typeof(AccordionSelectionMode),
            typeof(Accordion),
            new PropertyMetadata(AccordionSelectionMode.One, OnSelectionModePropertyChanged));

        /// <summary>
        /// Identifies the SelectionSequence dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionSequenceProperty = DependencyProperty.Register(
            "SelectionSequence",
            typeof(SelectionSequence),
            typeof(Accordion),
            new PropertyMetadata(SelectionSequence.Simultaneous, OnSelectionSequencePropertyChanged));

        #endregion

        #region Routed Events
        
        /// <summary>
        /// Occurs when the SelectedItem or SelectedItems property value changes.
        /// </summary>
        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanged",
            RoutingStrategy.Bubble,
            typeof(SelectionChangedEventHandler),
            typeof(Accordion)); 

        #endregion

        #region Fields

        /// <summary>
        /// The name used to indicate AccordionButtonStyle property.
        /// </summary>
        private const string AccordionButtonStyleName = "AccordionButtonStyle";

        /// <summary>
        /// The items that are currently waiting to perform an action.
        /// </summary>
        /// <remarks>An action can be expanding, resizing or collapsing.</remarks>
        private readonly List<AccordionItem> scheduledActions;

        /// <summary>
        /// Determines whether the SelectedItemsProperty may be written.
        /// </summary>
        private bool isAllowedToWriteSelectedItems;

        /// <summary>
        /// Determines whether the SelectedIndicesProperty may be written.
        /// </summary>
        private bool isAllowedToWriteSelectedIndices;

        /// <summary>
        /// Indicates that changes to the SelectedIndices collection should
        /// be ignored.
        /// </summary>
        private bool isIgnoringSelectedIndicesChanges;

        /// <summary>
        /// Indicates that changes to the SelectedItems collection should
        /// be ignored.
        /// </summary>
        private bool isIgnoringSelectedItemsChanges;

        /// <summary>
        /// Determines whether we are currently in the SelectedItems Collection
        /// Changed handling.
        /// </summary>
        private bool isInSelectedItemsCollectionChanged;

        /// <summary>
        /// Determines whether we are currently in the SelectedIndices Collection
        /// Changed handling.
        /// </summary>
        private bool isInSelectedIndicesCollectionChanged;

        /// <summary>
        /// The item that is currently visually performing an action.
        /// </summary>
        /// <remarks>An action can be expanding, resizing or collapsing.</remarks>
        private AccordionItem currentActioningItem;

        /// <summary>
        /// Coercion level.
        /// </summary>
        private int selectedIndexNestedLevel;

        /// <summary>
        /// Nested level for SelectedItemCoercion.
        /// </summary>
        private int selectedItemNestedLevel;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="Accordion"/> class.
        /// </summary>
        static Accordion()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Accordion), new FrameworkPropertyMetadata(typeof(Accordion)));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Accordion"/> class.
        /// </summary>
        public Accordion()
        {
            ItemsControlHelper = new ItemsControlHelper(this);

            ObservableCollection<object> items = new ObservableCollection<object>();
            ObservableCollection<int> indices = new ObservableCollection<int>();

            this.SelectedItems = items;
            this.SelectedIndices = indices;

            items.CollectionChanged += this.OnSelectedItemsCollectionChanged;
            indices.CollectionChanged += this.OnSelectedIndicesCollectionChanged;

            this.scheduledActions = new List<AccordionItem>();
            this.SizeChanged += this.OnAccordionSizeChanged;
            this.Interaction = new InteractionHelper(this);
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when the SelectedItems collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler SelectedItemsChanged;

        /// <summary>
        /// Occurs when the SelectedItem or SelectedItems property value changes.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged
        {
            add { this.AddHandler(SelectionChangedEvent, value); }
            remove { this.RemoveHandler(SelectionChangedEvent, value); }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the Style that is applied to AccordionButton elements
        /// in the AccordionItems.
        /// </summary>
        public Style AccordionButtonStyle
        {
            get { return this.GetValue(AccordionButtonStyleProperty) as Style; }
            set { this.SetValue(AccordionButtonStyleProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the DataTemplate used to display the content 
        /// of each generated AccordionItem. 
        /// </summary>
        /// <remarks>Either ContentTemplate or ItemTemplate is used. 
        /// Setting both will result in an exception.</remarks>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)this.GetValue(ContentTemplateProperty); }
            set { this.SetValue(ContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ExpandDirection property of each 
        /// AccordionItem in the Accordion control and the direction in which
        /// the Accordion does layout.
        /// </summary>
        /// <remarks>Setting the ExpandDirection will set the expand direction 
        /// on the accordionItems.</remarks>
        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)this.GetValue(ExpandDirectionProperty); }
            set { this.SetValue(ExpandDirectionProperty, value); }
        }

        /// <summary>
        /// Gets the indices of the currently selected AccordionItems.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Indices", Justification = "Framework uses indices.")]
        public IList<int> SelectedIndices
        {
            get 
            { 
                return this.GetValue(SelectedIndicesProperty) as IList<int>; 
            }

            private set
            {
                this.isAllowedToWriteSelectedIndices = true;
                this.SetValue(SelectedIndicesProperty, value);
                this.isAllowedToWriteSelectedIndices = false;
            }
        }

        /// <summary>
        /// Gets or sets the index of the currently selected AccordionItem.
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <remarks>
        /// The default value is null.
        /// When multiple items are allowed (IsMaximumOneSelected false), 
        /// return the first of the selectedItems.
        /// </remarks>
        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }
        
        /// <summary>
        /// Gets the selected items.
        /// </summary>
        /// <remarks>Does not allow setting.</remarks>
        public IList SelectedItems
        {
            get
            { 
                return this.GetValue(SelectedItemsProperty) as IList; 
            }

            private set
            {
                this.isAllowedToWriteSelectedItems = true;
                this.SetValue(SelectedItemsProperty, value);
                this.isAllowedToWriteSelectedItems = false;
            }
        }

        /// <summary>
        /// Gets or sets the AccordionSelectionMode used to determine the minimum 
        /// and maximum selected AccordionItems allowed in the Accordion.
        /// </summary>
        public AccordionSelectionMode SelectionMode
        {
            get { return (AccordionSelectionMode)this.GetValue(SelectionModeProperty); }
            set { this.SetValue(SelectionModeProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the SelectionSequence used to determine 
        /// the order of AccordionItem selection.
        /// </summary>
        public SelectionSequence SelectionSequence
        {
            get { return (SelectionSequence)this.GetValue(SelectionSequenceProperty); }
            set { this.SetValue(SelectionSequenceProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the ItemsControlHelper that is associated with this control.
        /// </summary>
        internal ItemsControlHelper ItemsControlHelper { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is currently resizing.
        /// </summary>
        /// <value>True if this instance is resizing; otherwise, false.</value>
        internal bool IsResizing { get; private set; } 

        #endregion

        #region Private Properties
        
        /// <summary>
        /// Gets or sets the helper that provides all of the standard
        /// interaction functionality.
        /// </summary>
        private InteractionHelper Interaction { get; set; }

        /// <summary>
        /// Gets a value indicating whether at most one item is selected at all times.
        /// </summary>
        private bool IsMaximumOneSelected
        {
            get { return (this.SelectionMode == AccordionSelectionMode.One) || (this.SelectionMode == AccordionSelectionMode.ZeroOrOne); }
        }

        /// <summary>
        /// Gets a value indicating whether at least one item is selected at 
        /// all times.
        /// </summary>
        private bool IsMinimumOneSelected
        {
            get { return (this.SelectionMode == AccordionSelectionMode.One) || (this.SelectionMode == AccordionSelectionMode.OneOrMore); }
        }

        /// <summary>
        /// Gets a value indicating whether the accordion fills height.
        /// </summary>
        private bool IsShouldFillHeight
        {
            get
            {
                return (this.ExpandDirection == ExpandDirection.Down || this.ExpandDirection == ExpandDirection.Up) &&
                       (!double.IsNaN(this.Height) || this.VerticalAlignment == VerticalAlignment.Stretch);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the accordion fills width.
        /// </summary>
        private bool IsShouldFillWidth
        {
            get
            {
                return (this.ExpandDirection == ExpandDirection.Left || this.ExpandDirection == ExpandDirection.Right) &&
                       (!double.IsNaN(this.Width) || this.HorizontalAlignment == HorizontalAlignment.Stretch);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the visual tree for the Accordion control when a 
        /// new template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.ItemsControlHelper.OnApplyTemplate();
            base.OnApplyTemplate();
            this.Interaction.OnApplyTemplateBase();
        }
        
        /// <summary>
        /// Selects all the AccordionItems in the Accordion control.
        /// </summary>
        /// <remarks>If the Accordion SelectionMode is OneOrMore or ZeroOrMore all 
        /// AccordionItems would be selected. If the Accordion SelectionMode is 
        /// One or ZeroOrOne all items would be selected and unselected. Only 
        /// the last AccordionItem would remain selected. </remarks>
        public void SelectAll()
        {
            this.UpdateAccordionItemsSelection(true);
        }

        /// <summary>
        /// Unselects all the AccordionItems in the Accordion control.
        /// </summary>
        /// <remarks>If the Accordion SelectionMode is Zero or ZeroOrMore all 
        /// AccordionItems would be Unselected. If SelectionMode is One or  
        /// OneOrMode  than all items would be Unselected and selected. Only the 
        /// first AccordionItem would still be selected.</remarks>
        public void UnselectAll()
        {
            this.UpdateAccordionItemsSelection(false);
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

        #region Internal Methods

        /// <summary>
        /// Called when an AccordionItem is unselected.
        /// </summary>
        /// <param name="accordionItem">The accordion item that was unselected.</param>
        internal void OnAccordionItemUnselected(AccordionItem accordionItem)
        {
            this.UnselectItem(this.ItemContainerGenerator.IndexFromContainer(accordionItem), this.ItemContainerGenerator.ItemFromContainer(accordionItem));
        }

        /// <summary>
        /// Called when an AccordionItem selected.
        /// </summary>
        /// <param name="accordionItem">The accordion item that was selected.</param>
        internal void OnAccordionItemSelected(AccordionItem accordionItem)
        {
            this.SelectItem(this.ItemContainerGenerator.IndexFromContainer(accordionItem));
        }

        /// <summary>
        /// Signals the finish of an action by an item.
        /// </summary>
        /// <param name="item">The AccordionItem that finishes an action.</param>
        /// <remarks>An AccordionItem should always signal a finish, for this call
        /// will start the next scheduled action.</remarks>
        internal virtual void OnActionFinish(AccordionItem item)
        {
            if (SelectionSequence == SelectionSequence.CollapseBeforeExpand)
            {
                lock (this)
                {
                    if (!this.currentActioningItem.Equals(item))
                    {
                        throw new InvalidOperationException(AccordionResources.Accordion_OnActionFinish_InvalidFinish);
                    }

                    this.currentActioningItem = null;

                    this.StartNextAction();
                }
            }
        }

        /// <summary>
        /// Called when size of a Header on the item changes.
        /// </summary>
        /// <param name="item">The item whose Header changed.</param>
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "item", Justification = "Passing the AccordionItem leads to a better API if we wish to change modifier to protected in the future.")]
        internal void OnHeaderSizeChange(AccordionItem item)
        {
            this.LayoutChildren();
        }

        /// <summary>
        /// Allows an AccordionItem to signal the need for a visual action 
        /// (resize, collapse, expand).
        /// </summary>
        /// <param name="item">The AccordionItem that signals for a schedule.</param>
        /// <param name="action">The action it is scheduling for.</param>
        /// <returns>True if the item is allowed to proceed without scheduling, 
        /// false if the item needs to wait for a signal to execute the action.</returns>
        internal virtual bool ScheduleAction(AccordionItem item, AccordionAction action)
        {
            if (SelectionSequence == SelectionSequence.CollapseBeforeExpand)
            {
                lock (this)
                {
                    if (!this.scheduledActions.Contains(item))
                    {
                        this.scheduledActions.Add(item);
                    }
                }

                if (this.currentActioningItem == null)
                {
                    this.Dispatcher.BeginInvoke(new Action(this.StartNextAction));
                }

                return false;
            }
            else
            {
                return true;
            }
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
            // Handle the Common and Focused states
            this.Interaction.UpdateVisualStateBase(useTransitions);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Undoes the effects of the <see cref="M:System.Windows.Controls.ItemsControl.PrepareContainerForItemOverride(System.Windows.DependencyObject,System.Object)"/> 
        /// method.
        /// </summary>
        /// <param name="dependencyObject">The container element.</param>
        /// <param name="item">The item that should be cleared.</param>
        protected override void ClearContainerForItemOverride(DependencyObject dependencyObject, object item)
        {
            AccordionItem accordionItem = dependencyObject as AccordionItem;
            if (accordionItem != null)
            {
                accordionItem.IsLocked = false;
                accordionItem.IsSelected = false;

                // release the parent child relationship.
                accordionItem.ParentAccordion = null;
            }

            base.ClearContainerForItemOverride(dependencyObject, item);
        }

        /// <summary>
        /// Creates or identifies the element that is used to display the given 
        /// item.
        /// </summary>
        /// <returns>
        /// The element that is used to display the given item.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new AccordionItem();
        }

        /// <summary>
        /// Determines if the specified item is (or is eligible to be) its own 
        /// container.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>
        /// True if the item is (or is eligible to be) its own container; 
        /// otherwise, false.
        /// </returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is AccordionItem;
        }

        /// <summary>
        /// Returns a AccordionAutomationPeer for use by the Silverlight
        /// automation infrastructure.
        /// </summary>
        /// <returns>A AccordionAutomationPeer object for the Accordion.</returns>
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new AccordionAutomationPeer(this);
        }

        /// <summary>
        /// Invoked when the <see cref="P:System.Windows.Controls.ItemsControl.Items"/> 
        /// property changes.
        /// </summary>
        /// <param name="e">Information about the change.</param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    try
                    {
                        this.isIgnoringSelectedIndicesChanges = true;
                        for (int i = 0; i < this.SelectedIndices.Count; i++)
                        {
                            if (this.SelectedIndices[i] >= e.NewStartingIndex)
                            {
                                // add a value of one
                                this.SelectedIndices[i] = this.SelectedIndices[i] + 1;
                            }
                        }
                    }
                    finally
                    {
                        this.isIgnoringSelectedIndicesChanges = false;
                    }

                    if (this.SelectedIndex >= e.NewStartingIndex && this.SelectedIndex > -1)
                    {
                        this.SelectedIndex++;
                    }

                    // now add the item, will also add indice at correct position.
                    if (this.SelectedItem == null && this.IsMinimumOneSelected)
                    {
                        if (!this.SelectedItems.OfType<object>().Contains(e.NewItems[0]))
                        {
                            this.SelectedItems.Add(e.NewItems[0]);
                        }

                        this.SelectedItem = e.NewItems[0];
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        this.isIgnoringSelectedIndicesChanges = true;
                        this.isIgnoringSelectedItemsChanges = true;
                        try
                        {
                            // Items has been cleared.
                            // so clear selecteditems as well
                            this.SelectedItems.Clear();
                            this.SelectedIndices.Clear();
                            this.SelectedItem = null;
                            this.SelectedIndex = -1;
                        }
                        finally
                        {
                            this.isIgnoringSelectedIndicesChanges = false;
                            this.isIgnoringSelectedItemsChanges = false;
                        }

                        // we receive this action when an itemssource is set
                        this.InitializeNewItemsSource();
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        int index = e.OldStartingIndex;
                        object item = e.OldItems[0];

                        try
                        {
                            this.isIgnoringSelectedIndicesChanges = true;

                            if (this.SelectedIndices.Contains(index))
                            {
                                this.SelectedIndices.Remove(index);
                            }

                            for (int i = 0; i < this.SelectedIndices.Count; i++)
                            {
                                if (this.SelectedIndices[i] > index)
                                {
                                    // lower the value by one
                                    this.SelectedIndices[i] = this.SelectedIndices[i] - 1;
                                }
                            }
                        }
                        finally
                        {
                            this.isIgnoringSelectedIndicesChanges = false;
                        }

                        try
                        {
                            this.isIgnoringSelectedItemsChanges = true;

                            if (this.SelectedItems.Contains(item))
                            {
                                // check that there are no indices pointing to similar
                                // items that are still in the collection
                                if (this.SelectedIndices.Count(i => i < this.Items.Count && this.Items[i].Equals(item)) == 0)
                                {
                                    this.SelectedItems.Remove(item);
                                }
                            }
                        }
                        finally
                        {
                            this.isIgnoringSelectedItemsChanges = false;
                        }

                        if (this.SelectedIndex == index)
                        {
                            // that item is no longer in the Items collection
                            // so the index is incorrect as well
                            this.SelectedIndex = -1;
                        }

                        if (this.SelectedIndex > e.OldStartingIndex && this.SelectedIndex > -1)
                        {
                            this.SelectedIndex -= 1;
                        }
                    }

                    break;
            }

            this.SetPanelOrientation();
        }
        
        /// <summary>
        /// Raises the SelectedItemChanged event when the SelectedItem 
        /// property value changes.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> 
        /// instance containing the event data.</param>
        protected virtual void OnSelectedItemChanged(SelectionChangedEventArgs e)
        {
            this.RaiseEvent(e);
        }
        
        /// <summary>
        /// Provides handling for the GotFocus event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (this.Interaction.AllowGotFocus(e))
            {
                this.Interaction.OnGotFocusBase();
                base.OnGotFocus(e);
            }
        }

        /// <summary>
        /// Provides handling for the LostFocus event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (this.Interaction.AllowLostFocus(e))
            {
                this.Interaction.OnLostFocusBase();
                base.OnLostFocus(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseEnter event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            if (this.Interaction.AllowMouseEnter(e))
            {
                this.Interaction.OnMouseEnterBase();
                base.OnMouseEnter(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseLeave event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            if (this.Interaction.AllowMouseLeave(e))
            {
                this.Interaction.OnMouseLeaveBase();
                base.OnMouseLeave(e);
            }
        }

        /// <summary>
        /// Provides handling for the MouseLeftButtonDown event.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (this.Interaction.AllowMouseLeftButtonDown(e))
            {
                this.Interaction.OnMouseLeftButtonDownBase();
                base.OnMouseLeftButtonDown(e);
            }
        }

        /// <summary>
        /// Called before the MouseLeftButtonUp event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (this.Interaction.AllowMouseLeftButtonUp(e))
            {
                this.Interaction.OnMouseLeftButtonUpBase();
                base.OnMouseLeftButtonUp(e);
            }
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="dependencyObject">The element used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject dependencyObject, object item)
        {
            AccordionItem accordionItem = dependencyObject as AccordionItem;

            if (accordionItem != null)
            {
                DataTemplate specifiedContentTemplate = accordionItem.ContentTemplate;

                base.PrepareContainerForItemOverride(dependencyObject, item);
                ItemsControlHelper.PrepareContainerForItemOverride(accordionItem, this.ItemContainerStyle);
                AccordionItem.PreparePrepareHeaderedContentControlContainerForItemOverride(accordionItem, item, this, this.ItemContainerStyle);

                // after base.prepare, item template has replaced contenttemplate
                DataTemplate displayMemberTemplate = accordionItem.ContentTemplate;

                // put original contenttemplate back that was overwritten
                // It takes precendence over a generated itemtemplate.
                // this might mean setting back a null, which is correct given the bindings
                // that follow.
                accordionItem.ContentTemplate = specifiedContentTemplate;

                // potentially set contenttemplate if accordionItem did not specify one explicitly
                if (accordionItem.ContentTemplate == null)
                {
                    accordionItem.SetBinding(
                        ContentControl.ContentTemplateProperty,
                        new Binding("ContentTemplate")
                        {
                            Source = this,
                            Mode = BindingMode.OneWay
                        });
                }

                // potentially set headertemplate if accordionItem did not specify one explicitly
                if (accordionItem.HeaderTemplate == null)
                {
                    accordionItem.SetBinding(
                        HeaderedContentControl.HeaderTemplateProperty,
                        new Binding("ItemTemplate")
                        {
                            Source = this,
                            Mode = BindingMode.OneWay
                        });
                }

                // potentially bind AccordionButtonStyle.
                if (accordionItem.AccordionButtonStyle == null)
                {
                    accordionItem.SetBinding(
                        AccordionItem.AccordionButtonStyleProperty,
                        new Binding(AccordionButtonStyleName)
                        {
                            Source = this,
                            Mode = BindingMode.OneWay
                        });
                }

                // possibly set a displaymemberPath on header or content.
                if (displayMemberTemplate != null && !string.IsNullOrEmpty(this.DisplayMemberPath))
                {
                    if (accordionItem.ContentTemplate == null)
                    {
                        accordionItem.ContentTemplate = displayMemberTemplate;
                    }

                    if (accordionItem.HeaderTemplate == null)
                    {
                        accordionItem.HeaderTemplate = displayMemberTemplate;
                    }
                }

                // give accordionItem a reference back to the parent Accordion.
                accordionItem.ParentAccordion = this;

                // SelectedItem is expected to be set while adding items.
                // Check: does this item belong in the selectedindices
                int index = ItemContainerGenerator.IndexFromContainer(accordionItem);
                if (!accordionItem.IsSelected && this.SelectedIndices.Contains(index))
                {
                    accordionItem.IsSelected = true;
                }

                // could also be adding an item with the IsSelected set to true.
                if (accordionItem.IsSelected)
                {
                    this.SelectedItem = item;
                }

                // item might have been preselected when added to the item collection. 
                // at that point the parent had not been registered yet, so no notification was done.
                if (accordionItem.IsSelected)
                {
                    if (!this.SelectedItems.OfType<object>().Contains(item))
                    {
                        this.SelectedItems.Add(item);
                    }

                    if (!this.SelectedIndices.Contains(index))
                    {
                        this.SelectedIndices.Add(index);
                    }
                }

                accordionItem.ExpandDirection = this.ExpandDirection;
            }
            else
            {
                base.PrepareContainerForItemOverride(dependencyObject, item);
                ItemsControlHelper.PrepareContainerForItemOverride(dependencyObject, this.ItemContainerStyle);
            }

            // The panel will register itself when it has had a child to add.
            this.SetPanelOrientation();

            // change has occured, re-evaluate the locked status on items
            this.SetLockedProperties();

            // At this moment this item has not been added to the panel yet, so we schedule a layoutpass
            this.Dispatcher.BeginInvoke(new Action(this.LayoutChildren));
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// AccordionButtonStyleProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed its AccordionButtonStyle.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnAccordionButtonStylePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// ExpandDirectionProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed its ExpandDirection.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnExpandDirectionPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Accordion source = (Accordion)dependencyObject;
            ExpandDirection expandDirection = (ExpandDirection)e.NewValue;

            if (expandDirection != ExpandDirection.Down &&
                expandDirection != ExpandDirection.Up &&
                expandDirection != ExpandDirection.Left &&
                expandDirection != ExpandDirection.Right)
            {
                // revert to old value
                source.SetValue(ExpandDirectionProperty, e.OldValue);

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    AccordionResources.Accordion_OnExpandDirectionPropertyChanged_InvalidValue,
                    expandDirection);

                throw new ArgumentOutOfRangeException("e", message);
            }

            // force this change to all AccordionItems
            for (int i = 0; i < source.Items.Count; i++)
            {
                AccordionItem accordionItem = source.ItemContainerGenerator.ContainerFromIndex(i) as AccordionItem;

                if (accordionItem != null)
                {
                    accordionItem.ExpandDirection = expandDirection;
                }
            }

            // set panel to align to the change
            source.SetPanelOrientation();

            // schedule a layout pass after this panel has had time to rearrange.
            source.Dispatcher.BeginInvoke(new Action(source.LayoutChildren));
        }
        
        /// <summary>
        /// Property changed handler of SelectedIndices.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed the collection.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnSelectedIndicesChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Accordion accordion = (Accordion)dependencyObject;

            if (!accordion.isAllowedToWriteSelectedIndices)
            {
                // revert to old value
                accordion.SelectedIndices = e.OldValue as IList<int>;

                throw new InvalidOperationException(AccordionResources.Accordion_OnSelectedIndicesChanged_InvalidWrite);
            }
        }

        /// <summary>
        /// SelectedIndexProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed its SelectedIndex.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnSelectedIndexPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Accordion source = (Accordion)dependencyObject;
            int oldValue = (int)e.OldValue;
            int newValue = (int)e.NewValue;

            // SelectedIndex will be changed when modifying the SelectionCollections.
            // Those should not trigger changes in the SelectedIndex.
            if (source.isIgnoringSelectedIndicesChanges)
            {
                return;
            }

            if (!source.IsValidIndexForSelection(newValue))
            {
                // oldvalue might not be valid anymore (because of items removed from collection)
                if (source.IsValidIndexForSelection(oldValue))
                {
                    // oldvalue is valid, repress events
                    source.selectedIndexNestedLevel++;
                    source.SetValue(SelectedIndexProperty, oldValue);
                    source.selectedIndexNestedLevel--;
                }
                else
                {
                    // select new
                    source.SetValue(SelectedIndexProperty, source.ProposeSelectedIndexCandidate(newValue));
                }
            }
            else if (source.selectedIndexNestedLevel == 0)
            {
                // synchronize with SelectedItem

                // In .NET 3.5, ElementAtOrDefault will throw an exception when newValue is out of bounds.
                // Avoid the exception by explicitly checking the value is within the bounds
                source.SelectedItem = (newValue >= 0 && newValue < source.Items.Count) ? source.Items[newValue] : null;

                // SelectedIndex is responsible for kicking off the real work.
                source.ChangeSelectedIndex(oldValue, newValue);
            }
        }

        /// <summary>
        /// SelectedItemProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed its SelectedItem.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnSelectedItemPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Accordion source = (Accordion)dependencyObject;

            object oldValue = e.OldValue;
            object newValue = e.NewValue;
            object[] newValues = newValue == null ? new object[0] : new[] { newValue };
            object[] oldValues = oldValue == null ? new object[0] : new[] { oldValue };

            if (oldValue != null && oldValue.Equals(newValue))
            {
                // when value types are used as items, there is a possibility of getting a change notification.
                source.OnSelectedItemChanged(new SelectionChangedEventArgs(SelectionChangedEvent, oldValues, newValues));
                return;
            }

            if (!source.IsValidItemForSelection(newValue))
            {
                // reset to oldvalue
                source.selectedItemNestedLevel++;
                source.SetValue(SelectedItemProperty, oldValue);
                source.selectedItemNestedLevel--;
            }
            else if (source.selectedItemNestedLevel == 0)
            {
                if (newValue == null)
                {
                    source.SelectedIndex = -1;
                }
                else
                {
                    // be cautious about choosing a new index.
                    int currentIndex = source.SelectedIndex;

                    // use current SelectedIndex if possible
                    if (currentIndex < 0 || currentIndex > source.Items.Count || !newValue.Equals(source.Items[currentIndex]))
                    {
                        // use an index out of SelectedIndices if possible
                        // or fallback to finding the index in the ItemsCollection
                        IEnumerable<int> validIndices = source.SelectedIndices.Where(i => i >= 0 && i < source.Items.Count && newValue.Equals(source.Items[i]));
                        currentIndex = validIndices.Count() > 0 ? validIndices.First() : source.Items.IndexOf(newValue);
                    }

                    source.SelectedIndex = currentIndex;
                }

                source.OnSelectedItemChanged(new SelectionChangedEventArgs(SelectionChangedEvent, oldValues, newValues));
            }
        }
        
        /// <summary>
        /// Property changed handler of SelectedItems.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed the collection.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnSelectedItemsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Accordion accordion = (Accordion)dependencyObject;

            if (!accordion.isAllowedToWriteSelectedItems)
            {
                // revert to old value
                accordion.SelectedItems = e.OldValue as IList;

                throw new InvalidOperationException(AccordionResources.Accordion_OnSelectedItemsChanged_InvalidWrite);
            }
        }

        /// <summary>
        /// SelectionModeProperty property changed handler.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed its SelectionMode.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnSelectionModePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Accordion source = (Accordion)dependencyObject;

            AccordionSelectionMode newValue = (AccordionSelectionMode)e.NewValue;

            if (newValue != AccordionSelectionMode.One &&
                newValue != AccordionSelectionMode.OneOrMore &&
                newValue != AccordionSelectionMode.ZeroOrMore &&
                newValue != AccordionSelectionMode.ZeroOrOne)
            {
                // revert to old value
                source.SetValue(SelectionModeProperty, e.OldValue);

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    AccordionResources.Accordion_OnSelectionModePropertyChanged_InvalidValue,
                    newValue);

                throw new ArgumentOutOfRangeException("e", message);
            }

            // unlock all items
            // a selectionmode change is expected to change the locks.
            for (int i = 0; i < source.Items.Count; i++)
            {
                AccordionItem item = source.ItemContainerGenerator.ContainerFromIndex(i) as AccordionItem;
                if (item != null)
                {
                    item.IsLocked = false;
                }
            }

            // single selection coercion
            if (source.IsMinimumOneSelected)
            {
                // a minimum of one item should be selected
                if (source.GetValue(SelectedItemProperty) == null && source.Items.Count > 0)
                {
                    // select first accordionitem
                    source.SetValue(SelectedItemProperty, source.Items[0]);
                }
            }

            // multi selection coeercion
            if (source.IsMaximumOneSelected)
            {
                // allow at most one item.
                if (source.SelectedIndices.Count > 1)
                {
                    // make copy of collection, since it will be modified
                    List<int> indices = source.SelectedIndices.ToList();
                    foreach (int index in indices)
                    {
                        // unselect all items except the currently selected item.
                        if (index != source.SelectedIndex)
                        {
                            source.UnselectItem(index, null);
                        }
                    }
                }
            }

            // re-evaluate the locking status of the items in this new configuration
            source.SetLockedProperties();
        }

        /// <summary>
        /// Called when SelectionSequenceProperty changed.
        /// </summary>
        /// <param name="dependencyObject">Accordion that changed its SelectionSequence property.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private static void OnSelectionSequencePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            SelectionSequence newValue = (SelectionSequence)e.NewValue;

            if (newValue != SelectionSequence.CollapseBeforeExpand &&
                newValue != SelectionSequence.Simultaneous)
            {
                // revert to old value
                dependencyObject.SetValue(Accordion.SelectionSequenceProperty, e.OldValue);

                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    AccordionResources.Accordion_OnSelectionSequencepropertyChanged_InvalidValue,
                    newValue);
                throw new ArgumentOutOfRangeException("e", message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Changes the selected item, by unselecting and selecting where 
        /// necessary.
        /// </summary>
        /// <param name="oldIndex">The old index.</param>
        /// <param name="newIndex">The new index.</param>
        private void ChangeSelectedIndex(int oldIndex, int newIndex)
        {
            AccordionItem oldAccordionItem = oldIndex >= 0 && oldIndex < Items.Count ? ItemContainerGenerator.ContainerFromIndex(oldIndex) as AccordionItem : null;
            AccordionItem newAccordionItem = newIndex >= 0 && newIndex < Items.Count ? ItemContainerGenerator.ContainerFromIndex(newIndex) as AccordionItem : null;

            // unselect the previous item, if we need to
            // we should be able to be called when the oldvalue equals the newvalue
            if (oldIndex != newIndex)
            {
                // we only need to explicitly deselect the oldvalue if there is a maximum
                // of one selected. However, if user explicitly set SelectedItem
                // to null, we should still deselect the old value.
                if (this.IsMaximumOneSelected || newIndex == -1)
                {
                    if (oldAccordionItem != null)
                    {
                        // unselection can be triggered by selection of another item.
                        oldAccordionItem.IsLocked = false;
                        oldAccordionItem.IsSelected = false;
                    }
                    else if (oldIndex > -1)
                    {
                        // there was no wrapper yet, fallback to regular unselecting
                        this.UnselectItem(oldIndex, null);
                    }

                    // raise event for UIAutomation.
                    if (newAccordionItem != null &&
                        AutomationPeer.ListenerExists(
                                AutomationEvents.SelectionItemPatternOnElementSelected))
                    {
                        AutomationPeer peer =
                                FrameworkElementAutomationPeer.CreatePeerForElement(newAccordionItem);
                        if (peer != null)
                        {
                            peer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementSelected);
                        }
                    }
                }
            }

            // make the selection through the container if possible
            if (newAccordionItem != null)
            {
                newAccordionItem.IsSelected = true;
            }
            else if (newIndex != -1)
            {
                // there was no wrapper yet, fallback to regular selecting
                this.SelectItem(newIndex);
            }

            this.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Initializes the SelectedItem property when a new ItemsSource is set.
        /// </summary>
        private void InitializeNewItemsSource()
        {
            // todo: remove the SelectedItem == null check (should not be necessary)

            // possibly an ItemsSource has been set
            if (this.IsMinimumOneSelected && this.SelectedItem == null && this.Items.Count > 0)
            {
                if (!this.SelectedItems.OfType<object>().Contains(this.Items[0]))
                {
                    this.SelectedItems.Add(this.Items[0]);
                }

                this.SelectedItem = this.Items[0];
            }
        }

        /// <summary>
        /// Determines whether the new value can be selected.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        /// <c>True</c> if this item can be selected; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidIndexForSelection(int newValue)
        {
            // setting to null is supported in some cases
            if (newValue == -1)
            {
                // we can always return something since null is not a valid item.
                // if accordion allows no selection, null is accepted
                // if there are currently no items, null is accepted
                return (this.IsMinimumOneSelected == false) || (this.Items.Count == 0);
            }

            // index should be contained inside the items collection.
            return newValue >= 0 && newValue < Items.Count;
        }

        /// <summary>
        /// Determines whether the new value can be selected.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        /// <returns>
        /// <c>True</c> if this item can be selected; otherwise, <c>false</c>.
        /// </returns>
        private bool IsValidItemForSelection(object newValue)
        {
            // setting to null is supported in some cases
            if (newValue == null)
            {
                // we can always return something since null is not a valid item.
                // if accordion allows no selection, null is accepted
                // if there are currently no items, null is accepted
                return (this.IsMinimumOneSelected == false) || (this.Items.Count == 0);
            }

            // item should be contained inside the items collection.
            return this.Items.OfType<object>().Contains(newValue);
        }

        /// <summary>
        /// Determines and sets the height of the accordion items.
        /// </summary>
        private void LayoutChildren()
        {
            ScrollViewer root = ItemsControlHelper.ScrollHost;
            Size targetSize = new Size(double.NaN, double.NaN);
            if (root != null && ItemsControlHelper.ItemsHost != null)
            {
                if (this.IsShouldFillWidth)
                {
                    // selected items should fill the remaining width of the container.
                    targetSize.Width = Math.Max(0, root.ViewportWidth - ItemsControlHelper.ItemsHost.ActualWidth);

                    // calculate space currently occupied by items. This space will be redistributed.
                    foreach (object item in this.Items)
                    {
                        AccordionItem accordionItem = this.ItemContainerGenerator.ContainerFromItem(item) as AccordionItem;
                        if (accordionItem != null)
                        {
                            targetSize.Width += accordionItem.RelevantContentSize.Width;
                        }
                    }

                    // offset for the real difference in viewportheight and actualheight. This happens when accordion
                    // was made smaller.
                    double smaller = root.ViewportWidth - ItemsControlHelper.ItemsHost.ActualWidth;

                    if (smaller < 0)
                    {
                        targetSize.Width = Math.Max(0, targetSize.Width + smaller);
                    }

                    // calculated the targetsize for all selected items. Because of rounding issues, the
                    // actual space taken sometimes exceeds the appropriate amount by a fraction. 
                    if (targetSize.Width > 1)
                    {
                        targetSize.Width -= 1;
                    }

                    // possibly we are bigger than we would want, the items
                    // are overflowing. Always try to fit in current viewport.
                    if (root.ExtentWidth > root.ViewportWidth)
                    {
                        targetSize.Width = Math.Max(0, targetSize.Width - (root.ExtentWidth - root.ViewportWidth));
                    }

                    // calculate targetsize per selected item. This is redistribution.
                    targetSize.Width = this.SelectedItems.Count > 0 ? targetSize.Width / this.SelectedItems.Count : targetSize.Width;
                }
                else if (this.IsShouldFillHeight)
                {
                    // selected items should fill the remaining width of the container.
                    targetSize.Height = Math.Max(0, root.ViewportHeight - ItemsControlHelper.ItemsHost.ActualHeight);

                    // calculate space currently occupied by items. This space will be redistributed.
                    foreach (object item in this.Items)
                    {
                        AccordionItem accordionItem = ItemContainerGenerator.ContainerFromItem(item) as AccordionItem;
                        if (accordionItem != null)
                        {
                            targetSize.Height += accordionItem.RelevantContentSize.Height;
                        }
                    }

                    // offset for the real difference in viewportheight and actualheight. This happens when accordion
                    // was made smaller.
                    double smaller = root.ViewportHeight - ItemsControlHelper.ItemsHost.ActualHeight;

                    if (smaller < 0)
                    {
                        targetSize.Height = Math.Max(0, targetSize.Height + smaller);
                    }

                    // calculated the targetsize for all selected items. Because of rounding issues, the
                    // actual space taken sometimes exceeds the appropriate amount by a fraction. 
                    if (targetSize.Height > 1)
                    {
                        targetSize.Height -= 1;
                    }

                    // calculate targetsize per selected item. This is redistribution.
                    targetSize.Height = this.SelectedItems.Count > 0 ? targetSize.Height / this.SelectedItems.Count : targetSize.Height;
                }

                // set that targetsize
                foreach (object item in this.Items)
                {
                    AccordionItem accordionItem = ItemContainerGenerator.ContainerFromItem(item) as AccordionItem;
                    if (accordionItem != null)
                    {
                        // the calculated target size is calculated for the selected items.
                        if (accordionItem.IsSelected)
                        {
                            accordionItem.ContentTargetSize = targetSize;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when the size of the Accordion changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private async void OnAccordionSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Hack to get the Accordion to resize correctly when the window is resized or maximised.
            // Apparently this is due to a timing bug in this control which was never fixed.
            await Task.Run(() => Task.Delay(10));
            this.IsResizing = true;
            this.LayoutChildren();
            this.IsResizing = false;
        }

        /// <summary>
        /// Called when selected indices collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> 
        /// instance containing the event data.</param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Method is best kept coherent.")]
        private void OnSelectedIndicesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.isIgnoringSelectedIndicesChanges)
            {
                return;
            }

            this.isInSelectedIndicesCollectionChanged = true;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (this.IsMaximumOneSelected)
                        {
                            // selectedindex always trails the actual state
                            if (((this.SelectedItem != null) && 
                                (e.NewItems.Count != 1)) || 
                                ((int)e.NewItems[0] < this.Items.Count && !this.Items[(int)e.NewItems[0]].Equals(this.SelectedItem)))
                            {
                                // will always lead to manipulation of the collection
                                throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                            }
                        }

                        foreach (int index in e.NewItems)
                        {
                            if (index < this.Items.Count)
                            {
                                this.SelectedIndex = index;

                                // raise event for UIAutomation, which uses SelectedIndices to query SelectedItems.
                                if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementAddedToSelection))
                                {
                                    AutomationPeer peer = FrameworkElementAutomationPeer.CreatePeerForElement(this);
                                    if (peer != null)
                                    {
                                        peer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementAddedToSelection);
                                    }
                                }
                            }
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (this.IsMinimumOneSelected && e.OldItems.Contains(this.SelectedIndex))
                        {
                            if (this.SelectedIndex < this.Items.Count && this.Items[this.SelectedIndex].Equals(this.SelectedItem) && this.SelectedIndices.Count == 0)
                            {
                                // will always lead to manipulation of the collection
                                throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                            }
                        }

                        foreach (int index in e.OldItems)
                        {
                            if (index < this.Items.Count)
                            {
                                this.UnselectItem(index, null);

                                // raise event for UIAutomation, which uses SelectedIndices to query SelectedItems.
                                if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
                                {
                                    AutomationPeer peer = FrameworkElementAutomationPeer.CreatePeerForElement(this);
                                    if (peer != null)
                                    {
                                        peer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
                                    }
                                }
                            }
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        // unselect all items.
                        // we use the selectedindices collection to pinpoint the
                        // items we need to unselect
                        if (this.IsMinimumOneSelected && this.Items.Count > 0)
                        {
                            // will always lead to manipulation of the collection
                            throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                        }

                        // unselect all items.
                        // we use the selectedItems collection to pinpoint the
                        // items we need to unselect
                        for (int i = this.SelectedItems.Count - 1; i >= 0; i--)
                        {
                            object item = this.SelectedItems[i];
                            this.UnselectItem(i, item);

                            // raise event for UIAutomation, which uses SelectedIndices to query SelectedItems.
                            if (AutomationPeer.ListenerExists(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection))
                            {
                                AutomationPeer peer = FrameworkElementAutomationPeer.CreatePeerForElement(this);
                                if (peer != null)
                                {
                                    peer.RaiseAutomationEvent(AutomationEvents.SelectionItemPatternOnElementRemovedFromSelection);
                                }
                            }
                        }
                    }

                    break;
                default:
                    {
                        string message = string.Format(
                            CultureInfo.InvariantCulture,
                            AccordionResources.Accordion_UnsupportedCollectionAction,
                            e.Action);

                        throw new NotSupportedException(message);
                    }
            }

            // change has occured, re-evaluate the locked status on items
            this.SetLockedProperties();

            // do a layout pass.
            this.LayoutChildren();

            this.isInSelectedIndicesCollectionChanged = false;
        }

        /// <summary>
        /// Called when selected items collection changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> 
        /// instance containing the event data.</param>
        private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.isIgnoringSelectedItemsChanges)
            {
                return;
            }

            this.isInSelectedItemsCollectionChanged = true;

            Action<object> unselectItem = item =>
            {
                // since we removed this selecteditem, all selectedindices need to be removed as well
                List<int> valid = SelectedIndices.Where(i => i < Items.Count && item.Equals(Items[i])).ToList();
                if (valid.Count > 0)
                {
                    foreach (int index in valid)
                    {
                        UnselectItem(index, item);
                    }
                }
                else
                {
                    UnselectItem(Items.IndexOf(item), item);
                }
            };

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (this.IsMaximumOneSelected && (this.SelectedItem != null && !e.NewItems.Contains(this.SelectedItem)))
                        {
                            // will always lead to manipulation of the collection
                            throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                        }

                        foreach (object item in e.NewItems)
                        {
                            object tempItem = item;
                            this.SelectedItem = tempItem;
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        if (this.IsMinimumOneSelected && e.OldItems.Contains(this.SelectedItem))
                        {
                            // will always lead to manipulation of the collection
                            throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                        }

                        foreach (object item in e.OldItems)
                        {
                            object tempItem = item;
                            unselectItem(tempItem);
                        }
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    {
                        // unselect all items.
                        // we use the selectedindices collection to pinpoint the
                        // items we need to unselect
                        if (this.IsMinimumOneSelected && this.Items.Count > 0)
                        {
                            // will always lead to manipulation of the collection
                            throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                        }

                        for (int i = this.SelectedIndices.Count - 1; i >= 0; i--)
                        {
                            int selectedIndex = this.SelectedIndices[i];

                            if (selectedIndex < this.Items.Count)
                            {
                                object tempItem = this.Items[selectedIndex];
                                unselectItem(tempItem);
                            }
                        }
                    }

                    break;
                default:
                    {
                        string message = string.Format(
                                CultureInfo.InvariantCulture,
                                AccordionResources.Accordion_UnsupportedCollectionAction,
                                e.Action);

                        throw new NotSupportedException(message);
                    }
            }

            // let the outside world know
            this.RaiseOnSelectedItemsCollectionChanged(e);

            this.isInSelectedItemsCollectionChanged = false;
        }
        
        /// <summary>
        /// Gets an item that is suitable for selection.
        /// </summary>
        /// <param name="nonCandidateIndex">Index that should not be considered if 
        /// possible.</param>
        /// <returns>An item that should be selected. This could be nonCandidateIndex, 
        /// if no other possibility was found.</returns>
        private int ProposeSelectedIndexCandidate(int nonCandidateIndex)
        {
            // other non candidates are items that are exactly like this item.
            object item = (nonCandidateIndex >= 0 && nonCandidateIndex < Items.Count) ? Items[nonCandidateIndex] : null;

            // see if we can find a suitable item in the selecteditems collection
            IEnumerable<int> validIndices = this.SelectedIndices.Where(i => i != nonCandidateIndex && (item == null || !item.Equals(Items[i])));

            if (validIndices.Count() > 0)
            {
                return validIndices.First();
            }

            if (this.IsMinimumOneSelected && this.Items.Count > 0)
            {
                return 0;
            }

            return -1;
        }

        /// <summary>
        /// Raise the SelectedItemsCollectionChanged event.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Controls.SelectionChangedEventArgs"/> 
        /// instance containing the event data.</param>
        /// <remarks>This event is raised after the changes to the collection 
        /// have been processed.</remarks>
        private void RaiseOnSelectedItemsCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler handler = this.SelectedItemsChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        } 

        /// <summary>
        /// Selects the item.
        /// </summary>
        /// <param name="index">The index of the item to select.</param>
        private void SelectItem(int index)
        {
            // try through accordionitem
            AccordionItem container = index >= 0 && index < Items.Count ? ItemContainerGenerator.ContainerFromIndex(index) as AccordionItem : null;
            if (container != null && !container.IsSelected)
            {
                container.IsSelected = true;
                return;
            }

            this.SelectedIndex = index;

            object item = this.Items[index];
            if (item != null)
            {
                // update selecteditems collection
                if (!this.SelectedItems.OfType<object>().Contains(item))
                {
                    if (this.isInSelectedItemsCollectionChanged)
                    {
                        throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                    }

                    this.SelectedItems.Add(item);
                }

                if (!this.SelectedIndices.Contains(index))
                {
                    if (this.isInSelectedIndicesCollectionChanged)
                    {
                        throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                    }

                    this.SelectedIndices.Add(index);
                }
            }
        }
        
        /// <summary>
        /// Sets the locked properties on all the items.
        /// </summary>
        private void SetLockedProperties()
        {
            // an item that can not be unselected is locked.
            // This happens in 'One' or 'OneOrMore' selection mode, when the first item is selected.
            for (int i = 0; i < Items.Count; i++)
            {
                AccordionItem item = ItemContainerGenerator.ContainerFromIndex(i) as AccordionItem;

                if (item != null)
                {
                    item.IsLocked = item.IsSelected && this.IsMinimumOneSelected && (this.SelectedIndices.Count == 1);
                }
            }
        }
        
        /// <summary>
        /// Sets the orientation of the panel.
        /// </summary>
        private void SetPanelOrientation()
        {
            StackPanel panel = ItemsControlHelper.ItemsHost as StackPanel;
            if (panel != null)
            {
                switch (ExpandDirection)
                {
                    case ExpandDirection.Down:
                    case ExpandDirection.Up:
                        panel.HorizontalAlignment = HorizontalAlignment.Stretch;
                        panel.VerticalAlignment = ExpandDirection == ExpandDirection.Down ? VerticalAlignment.Top : VerticalAlignment.Bottom;
                        panel.Orientation = Orientation.Vertical;
                        break;
                    case ExpandDirection.Left:
                    case ExpandDirection.Right:
                        panel.VerticalAlignment = VerticalAlignment.Stretch;
                        panel.HorizontalAlignment = ExpandDirection == ExpandDirection.Left ? HorizontalAlignment.Right : HorizontalAlignment.Left;
                        panel.Orientation = Orientation.Horizontal;
                        break;
                }
            }
        }

        /// <summary>
        /// Starts the next action in the list, in a particular order.
        /// </summary>
        /// <remarks>An AccordionItem is should always signal that it is 
        /// finished with an action.</remarks>
        private void StartNextAction()
        {
            if (this.currentActioningItem != null)
            {
                return;
            }

            // First do collapses, then resizes and finally expands.
            AccordionItem next = this.scheduledActions.FirstOrDefault(item => item.ScheduledAction == AccordionAction.Collapse);
            if (next == null)
            {
                next = this.scheduledActions.FirstOrDefault(item => item.ScheduledAction == AccordionAction.Resize);
            }

            if (next == null)
            {
                next = this.scheduledActions.FirstOrDefault(item => item.ScheduledAction == AccordionAction.Expand);
            }

            if (next != null)
            {
                this.currentActioningItem = next;
                this.scheduledActions.Remove(next);
                next.StartAction();
            }
        }

        /// <summary>
        /// Unselects the item.
        /// </summary>
        /// <param name="index">The index of the item that will be unselected.</param>
        /// <param name="item">The item that will be unselected. Can be null.</param>
        private void UnselectItem(int index, object item)
        {
            if (index < 0 || index > Items.Count)
            {
                // invalid
                return;
            }

            // try through accordionitem
            AccordionItem container = index >= 0 && index < Items.Count ? ItemContainerGenerator.ContainerFromIndex(index) as AccordionItem : null;
            if (container != null && container.IsSelected)
            {
                container.IsLocked = false;
                container.IsSelected = false;
                return;
            }

            item = item ?? this.Items[index];

            int newSelectedIndex = -1;

            // shortcuts to new item selection.
            if (this.SelectedIndex > -1 && this.SelectedIndex == index)
            {
                // this item is no longer the selected item. 
                // in order to keep the amount of raised events down, will select a new selected item here.
                newSelectedIndex = this.ProposeSelectedIndexCandidate(index);

                // no cancelling possible, undo the action.
                // current template makes sure accordionheader does not allow this unselect
                // that behavior is not enforced which means that it could come
                // from a SelectedItems/Indices manipulation
                this.SelectedIndex = newSelectedIndex;
            }

            // update selecteditems collection
            if (this.SelectedItems.OfType<object>().Contains(item) && index != newSelectedIndex && !item.Equals(this.SelectedItem))
            {
                // if there are indices still pointing to a similar item, do not remove
                if (this.SelectedIndices.Count(i => i != index && i < Items.Count && Items[i].Equals(item)) == 0)
                {
                    if (this.isInSelectedItemsCollectionChanged)
                    {
                        throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                    }

                    this.SelectedItems.Remove(item);
                }
            }

            // indexes should always be changed, even if we are reselecting a similar item
            if (this.SelectedIndices.Contains(index))
            {
                if (this.isInSelectedIndicesCollectionChanged)
                {
                    throw new InvalidOperationException(AccordionResources.Accordion_InvalidManipulationOfSelectionCollections);
                }

                this.SelectedIndices.Remove(index);
            }
        }

        /// <summary>
        /// Updates all accordionItems to be selected or unselected.
        /// </summary>
        /// <param name="selectedValue">True to select all items, false to unselect.</param>
        /// <remarks>Will not attempt to change a locked accordionItem.</remarks>
        private void UpdateAccordionItemsSelection(bool selectedValue)
        {
            foreach (object item in this.Items)
            {
                AccordionItem accordionItem = ItemContainerGenerator.ContainerFromItem(item) as AccordionItem;

                if (accordionItem != null && !accordionItem.IsLocked)
                {
                    accordionItem.IsSelected = selectedValue;
                }
            }
        }

        #endregion
    }
}
