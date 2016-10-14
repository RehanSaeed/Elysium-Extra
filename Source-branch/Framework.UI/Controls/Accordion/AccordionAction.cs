namespace Framework.UI.Controls
{
    /// <summary>
    /// Determines the action the AccordionItem will perform.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    internal enum AccordionAction
    {
        /// <summary>
        /// No action will be performed.
        /// </summary>
        None,

        /// <summary>
        /// A collapse will be performed.
        /// </summary>
        Collapse,
        
        /// <summary>
        /// An expand will be performed.
        /// </summary>
        Expand,
        
        /// <summary>
        /// A resize will be performed.
        /// </summary>
        Resize
    }
}