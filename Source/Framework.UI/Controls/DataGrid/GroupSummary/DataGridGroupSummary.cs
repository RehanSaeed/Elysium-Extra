namespace Framework.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// A group summary item for a grouped column.
    /// </summary>
    public sealed class DataGridGroupSummary : Freezable
    {
        #region Dependency Properties

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(
            "Column",
            typeof(DataGridColumn),
            typeof(DataGridGroupSummary),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TemplateProperty = DependencyProperty.Register(
            "Template",
            typeof(DataTemplate),
            typeof(DataGridGroupSummary),
            new PropertyMetadata(null)); 

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the column.
        /// </summary>
        /// <value>
        /// The column.
        /// </value>
        public DataGridColumn Column
        {
            get { return (DataGridColumn)this.GetValue(ColumnProperty); }
            set { this.SetValue(ColumnProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public DataTemplate Template
        {
            get { return (DataTemplate)this.GetValue(TemplateProperty); }
            set { this.SetValue(TemplateProperty, value); }
        } 

        #endregion

        #region Protected Methods
        
        /// <summary>
        /// When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable" /> derived class.
        /// </summary>
        /// <returns>
        /// The new instance.
        /// </returns>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        } 

        #endregion
    }
}
