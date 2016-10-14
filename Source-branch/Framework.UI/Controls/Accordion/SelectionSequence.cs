namespace Framework.UI.Controls
{
    /// <summary>
    /// Determines the order in which visual states are set.
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public enum SelectionSequence
    {
        /// <summary>
        /// Collapses are set before expansions.
        /// </summary>
        CollapseBeforeExpand,

        /// <summary>
        /// No delays, all states are set immediately.
        /// </summary>
        Simultaneous
    }
}