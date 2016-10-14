namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Windows;
    using System.Windows.Data;
    using Framework.ComponentModel;
    using Framework.UI.Input;

    /// <summary>
    /// The wizard item model.
    /// </summary>
    public sealed class WizardItemModel : NotifyPropertyChanges, IEquatable<WizardItemModel>
    {
        #region Fields

        private DelegateCommand<object> backCommand;
        private WizardItemModelCollection children;
        private string description;
        private DelegateCommand<object> forwardCommand;
        private object icon;
        private DataTemplate iconTemplate;
        private string id;
        private bool isBackwardEnabled;
        private bool isEntering;
        private bool isForwardEnabled;
        private bool isForwardVisible;
        private bool isLeaving;
        private bool isSelected;
        private bool isVisited;
        private string parentId;
        private WizardItemModel parent;
        private DelegateCommand<object> selectCommand;
        private string shortTitle;
        private string title;
        private Visibility visibility;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="WizardItemModel"/> class.
        /// </summary>
        public WizardItemModel()
        {
            this.children = new WizardItemModelCollection();
            this.children.WhenCollectionChanged.Subscribe(this.OnChildrenChanged);
            this.children.WhenItemChanged
                .Where(x => string.Equals(x.PropertyName, "Visibility", StringComparison.Ordinal))
                .Subscribe(this.OnChildChanged);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the back command.
        /// </summary>
        public DelegateCommand<object> BackCommand
        {
            get { return this.backCommand; }
            set { this.SetProperty(ref this.backCommand, value); }
        }

        /// <summary>
        /// Gets the children.
        /// </summary>
        public WizardItemModelCollection Children
        {
            get { return this.children; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.SetProperty(ref this.description, value); }
        }

        /// <summary>
        /// Gets or sets the forward command.
        /// </summary>
        public DelegateCommand<object> ForwardCommand
        {
            get { return this.forwardCommand; }
            set { this.SetProperty(ref this.forwardCommand, value); }
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public object Icon
        {
            get { return this.icon; }
            set { this.SetProperty(ref this.icon, value); }
        }

        /// <summary>
        /// Gets or sets the icon template.
        /// </summary>
        public DataTemplate IconTemplate
        {
            get { return this.iconTemplate; }
            set { this.SetProperty(ref this.iconTemplate, value); }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id
        {
            get { return this.id; }
            set { this.SetProperty(ref this.id, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is backward enabled.
        /// </summary>
        public bool IsBackwardEnabled
        {
            get { return this.isBackwardEnabled; }
            set { this.SetProperty(ref this.isBackwardEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is entering.
        /// </summary>
        public bool IsEntering
        {
            get { return this.isEntering; }
            set { this.SetProperty(ref this.isEntering, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is forward enabled.
        /// </summary>
        public bool IsForwardEnabled
        {
            get { return this.isForwardEnabled; }
            set { this.SetProperty(ref this.isForwardEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is forward visible.
        /// </summary>
        public bool IsForwardVisible
        {
            get { return this.isForwardVisible; }
            set { this.SetProperty(ref this.isForwardVisible, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is leaving.
        /// </summary>
        public bool IsLeaving
        {
            get { return this.isLeaving; }
            set { this.SetProperty(ref this.isLeaving, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.SetProperty(ref this.isSelected, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is visited.
        /// </summary>
        public bool IsVisited
        {
            get { return this.isVisited; }
            set { this.SetProperty(ref this.isVisited, value); }
        }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public string ParentId
        {
            get { return this.parentId; }
            set { this.SetProperty(ref this.parentId, value); }
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public WizardItemModel Parent
        {
            get { return this.parent; }
            set { this.SetProperty(ref this.parent, value); }
        }

        /// <summary>
        /// Gets or sets the select command.
        /// </summary>
        public DelegateCommand<object> SelectCommand
        {
            get { return this.selectCommand; }
            set { this.SetProperty(ref this.selectCommand, value); }
        }

        /// <summary>
        /// Gets or sets the short title.
        /// </summary>
        public string ShortTitle
        {
            get { return this.shortTitle; }
            set { this.SetProperty(ref this.shortTitle, value); }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.SetProperty(ref this.title, value); }
        }

        /// <summary>
        /// Gets or sets the visibility.
        /// </summary>
        public Visibility Visibility
        {
            get { return this.visibility; }
            set { this.SetProperty(ref this.visibility, value); }
        }

        /// <summary>
        /// Gets the visible children view.
        /// </summary>
        /// <value>The visible children view.</value>
        public IEnumerable<WizardItemModel> VisibleChildrenView
        {
            get { return this.Children.Where(x => x.Visibility == Visibility.Visible); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public bool Equals(WizardItemModel other)
        {
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.Id, other.Id, StringComparison.Ordinal);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj"> The object. </param>
        /// <returns> The <see cref="bool"/>. </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as WizardItemModel);
        }

        /// <summary>
        /// The get hash code.
        /// </summary>
        /// <returns> The <see cref="int"/>. </returns>
        public override int GetHashCode()
        {
            if (this.Id == null)
            {
                return base.GetHashCode();
            }

            return this.Id.GetHashCode();
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns> The <see cref="string"/>. </returns>
        public override string ToString()
        {
            if (this.Title == null)
            {
                return null;
            }

            return this.Title.ToString();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when the children collection is changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private void OnChildrenChanged(NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("VisibleChildrenView");
        }

        /// <summary>
        /// Called when a child is changed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        private void OnChildChanged(ItemChangedEventArgs<WizardItemModel> e)
        {
            this.OnPropertyChanged("VisibleChildrenView");
        }

        #endregion
    }
}
