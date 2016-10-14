namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Markup;

    /// <summary>
    /// The elastic view.
    /// </summary>
    [ContentProperty("Groups")]
    public class ElasticView : Freezable
    {
        #region Dependency Properties

        private static readonly DependencyPropertyKey GroupsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Groups",
            typeof(ElasticGroupCollection),
            typeof(ElasticView),
            new PropertyMetadata(null));

        public static readonly DependencyProperty GroupsProperty = GroupsPropertyKey.DependencyProperty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ElasticView"/> class.
        /// </summary>
        public ElasticView()
        {
            this.Groups = new ElasticGroupCollection();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the groups.
        /// </summary>
        public ElasticGroupCollection Groups
        {
            get { return (ElasticGroupCollection)GetValue(GroupsProperty); }
            private set { this.SetValue(GroupsPropertyKey, value); }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The create instance core.
        /// </summary>
        /// <returns>
        /// The <see cref="Freezable"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">The Exception</exception>
        protected override Freezable CreateInstanceCore()
        {
            throw new System.NotImplementedException();
        } 

        #endregion
    }
}
