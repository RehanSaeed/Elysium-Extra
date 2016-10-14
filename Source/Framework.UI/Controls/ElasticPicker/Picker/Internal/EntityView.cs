namespace Framework.UI.Controls
{
    /// <summary>
    /// The Entity View
    /// </summary>
    public sealed class EntityView
    {
        private EntityGroupCollection groups;

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityView"/> class.
        /// </summary>
        public EntityView()
        {
            this.groups = new EntityGroupCollection();
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        public EntityGroupCollection Groups
        {
            get { return this.groups; }
        }
    }
}
