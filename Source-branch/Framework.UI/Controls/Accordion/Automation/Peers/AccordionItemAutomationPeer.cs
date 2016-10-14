namespace Framework.UI.Automation.Peers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Automation;
    using System.Windows.Automation.Peers;
    using System.Windows.Automation.Provider;
    using System.Windows.Controls;
    using Framework.UI.Controls;

    /// <summary>
    /// Exposes AccordionItem types to UI Automation.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.Collapse()", Justification = "Required for subset compat with WPF")]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.Expand()", Justification = "Required for subset compat with WPF")]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.IExpandCollapseProvider.ExpandCollapseState", Justification = "Required for subset compat with WPF")]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.AddToSelection()", Justification = "Required for subset compat with WPF")]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.IsSelected", Justification = "Required for subset compat with WPF")]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.RemoveFromSelection()", Justification = "Required for subset compat with WPF")]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.Select()", Justification = "Required for subset compat with WPF")]
    [SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "System.Windows.Automation.Peers.AccordionItemAutomationPeer.#System.Windows.Automation.Provider.ISelectionItemProvider.SelectionContainer", Justification = "Required for subset compat with WPF")]
    public sealed class AccordionItemAutomationPeer : ItemAutomationPeer, IExpandCollapseProvider, ISelectionItemProvider
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="AccordionItemAutomationPeer"/> class.
        /// </summary>
        /// <param name="item">The item associated with this AutomationPeer</param>
        /// <param name="itemsControlAutomationPeer">The Accordion that is associated with this item.</param>
        public AccordionItemAutomationPeer(object item, ItemsControlAutomationPeer itemsControlAutomationPeer)
            : base(item, itemsControlAutomationPeer)
        {
        } 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the state (expanded or collapsed) of the Accordion.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        ExpandCollapseState IExpandCollapseProvider.ExpandCollapseState
        {
            get
            {
                return this.OwnerAccordionItem.IsSelected ? ExpandCollapseState.Expanded : ExpandCollapseState.Collapsed;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Accordion is selected.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        bool ISelectionItemProvider.IsSelected
        {
            get { return this.OwnerAccordionItem.IsSelected; }
        }

        /// <summary>
        /// Gets the UI Automation provider that implements ISelectionProvider
        /// and acts as the container for the calling object.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        IRawElementProviderSimple ISelectionItemProvider.SelectionContainer
        {
            get
            {
                Accordion parent = this.OwnerAccordionItem.ParentAccordion;
                if (parent != null)
                {
                    AutomationPeer peer = UIElementAutomationPeer.FromElement(parent);

                    if (peer != null)
                    {
                        return this.ProviderFromPeer(peer);
                    }
                }

                return null;
            }
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Gets the AccordionItem that owns this AccordionItemAutomationPeer.
        /// </summary>
        private AccordionItem OwnerAccordionItem
        {
            get { return this.Item as AccordionItem; }
        } 

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the control pattern for the AccordionItem that is associated
        /// with this AccordionItemAutomationPeer.
        /// </summary>
        /// <param name="patternInterface">The desired PatternInterface.</param>
        /// <returns>The desired AutomationPeer or null.</returns>
        public override object GetPattern(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.ExpandCollapse ||
                patternInterface == PatternInterface.SelectionItem)
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Adds the AccordionItem to the collection of selected items.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.AddToSelection()
        {
            AccordionItem owner = this.OwnerAccordionItem;
            Accordion parent = owner.ParentAccordion;
            if (parent == null)
            {
                throw new InvalidOperationException(AccordionResources.Automation_OperationCannotBePerformed);
            }

            parent.SelectedItems.Add(owner);
        }

        /// <summary>
        /// Collapses the AccordionItem.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void IExpandCollapseProvider.Collapse()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            AccordionItem owner = this.OwnerAccordionItem;
            if (owner.IsLocked)
            {
                throw new InvalidOperationException(AccordionResources.Automation_OperationCannotBePerformed);
            }

            owner.IsSelected = false;
        }

        /// <summary>
        /// Expands the AccordionItem.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void IExpandCollapseProvider.Expand()
        {
            if (!this.IsEnabled())
            {
                throw new ElementNotEnabledException();
            }

            AccordionItem owner = this.OwnerAccordionItem;
            if (owner.IsLocked)
            {
                throw new InvalidOperationException(AccordionResources.Automation_OperationCannotBePerformed);
            }

            owner.IsSelected = true;
        }

        /// <summary>
        /// Removes the current Accordion from the collection of selected
        /// items.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.RemoveFromSelection()
        {
            AccordionItem owner = this.OwnerAccordionItem;
            Accordion parent = owner.ParentAccordion;
            if (parent == null)
            {
                throw new InvalidOperationException(AccordionResources.Automation_OperationCannotBePerformed);
            }

            parent.SelectedItems.Remove(owner);
        }

        /// <summary>
        /// Clears selection from currently selected items and then proceeds to
        /// select the current Accordion.
        /// </summary>
        /// <remarks>
        /// This API supports the .NET Framework infrastructure and is not 
        /// intended to be used directly from your code.
        /// </remarks>
        void ISelectionItemProvider.Select()
        {
            this.OwnerAccordionItem.IsSelected = true;
        }

        #endregion

        #region Protected Methods
        
        /// <summary>
        /// Gets the control type for the AccordionItem that is associated
        /// with this AccordionItemAutomationPeer.  This method is called by
        /// GetAutomationControlType.
        /// </summary>
        /// <returns>Custom AutomationControlType.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.ListItem;
        }

        /// <summary>
        /// Gets the name of the AccordionItem that is associated with this
        /// AccordionItemAutomationPeer.  This method is called by GetClassName.
        /// </summary>
        /// <returns>The name AccordionItem.</returns>
        protected override string GetClassNameCore()
        {
            return "AccordionItem";
        } 

        #endregion
    }
}
