namespace Framework.UI.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Represents the header for an accordion item.
    /// </summary>
    /// <remarks>By creating a separate control, there is more flexibility in the re-template possibilities.</remarks>
    /// <QualityBand>Preview</QualityBand>
    [TemplateVisualState(Name = VisualStates.StateNormal, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateMouseOver, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StatePressed, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateDisabled, GroupName = VisualStates.GroupCommon)]
    [TemplateVisualState(Name = VisualStates.StateFocused, GroupName = VisualStates.GroupFocus)]
    [TemplateVisualState(Name = VisualStates.StateUnfocused, GroupName = VisualStates.GroupFocus)]
    [TemplateVisualState(Name = VisualStates.StateExpanded, GroupName = VisualStates.GroupExpansion)]
    [TemplateVisualState(Name = VisualStates.StateCollapsed, GroupName = VisualStates.GroupExpansion)]
    [TemplateVisualState(Name = VisualStates.StateExpandDown, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandUp, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandLeft, GroupName = VisualStates.GroupExpandDirection)]
    [TemplateVisualState(Name = VisualStates.StateExpandRight, GroupName = VisualStates.GroupExpandDirection)]
    public class AccordionButton : ToggleButton
    {
        #region Constructors

        /// <summary>
        /// Initialises static members of the <see cref="AccordionButton"/> class.
        /// </summary>
        static AccordionButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AccordionButton), new FrameworkPropertyMetadata(typeof(AccordionButton)));
        } 

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets a reference to the parent AccordionItem 
        /// of an AccordionButton.
        /// </summary>
        /// <value>The parent accordion item.</value>
        internal AccordionItem ParentAccordionItem { get; set; }

        #endregion

        #region Internal Methods
        
        /// <summary>
        /// Updates the state of the visual.
        /// </summary>
        /// <param name="useTransitions">If set to <c>true</c> use transitions.</param>
        /// <remarks>The header will follow the parent <see cref="AccordionItem"/> states.</remarks>
        internal virtual void UpdateVisualState(bool useTransitions)
        {
            // the visualstate of the header is completely dependent on the parent state.
            if (this.ParentAccordionItem == null)
            {
                return;
            }

            if (this.ParentAccordionItem.IsSelected)
            {
                VisualStates.GoToState(this, useTransitions, VisualStates.StateExpanded);
            }
            else
            {
                VisualStates.GoToState(this, useTransitions, VisualStates.StateCollapsed);
            }

            switch (this.ParentAccordionItem.ExpandDirection)
            {
                // no animations on an expanddirection change.
                case ExpandDirection.Down:
                    VisualStates.GoToState(this, false, VisualStates.StateExpandDown);
                    break;

                case ExpandDirection.Up:
                    VisualStates.GoToState(this, false, VisualStates.StateExpandUp);
                    break;

                case ExpandDirection.Left:
                    VisualStates.GoToState(this, false, VisualStates.StateExpandLeft);
                    break;

                default:
                    VisualStates.GoToState(this, false, VisualStates.StateExpandRight);
                    break;
            }
        } 

        #endregion
    }
}
