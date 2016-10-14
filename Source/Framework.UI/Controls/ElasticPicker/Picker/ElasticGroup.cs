namespace Framework.UI.Controls
{
    using System;
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// The elastic group.
    /// </summary>
    public class ElasticGroup : Freezable, INotifyPropertyChanged
    {
        #region Dependency Properties

        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(
            "ContentTemplate",
            typeof(DataTemplate),
            typeof(ElasticGroup),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register(
            "ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(ElasticGroup),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header",
            typeof(object),
            typeof(ElasticGroup),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(
            "HeaderTemplate",
            typeof(DataTemplate),
            typeof(ElasticGroup),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsFilterEnabledProperty = DependencyProperty.Register(
            "IsFilterEnabled",
            typeof(bool),
            typeof(ElasticGroup),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsResetButtonVisibleProperty = DependencyProperty.Register(
            "IsResetButtonVisible",
            typeof(bool),
            typeof(ElasticGroup),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsCountVisibleProperty = DependencyProperty.Register(
            "IsCountVisible",
            typeof(bool),
            typeof(ElasticGroup),
            new PropertyMetadata(true));

        public static readonly DependencyProperty IsSelectAllButtonVisibleProperty = DependencyProperty.Register(
            "IsSelectAllButtonVisible",
            typeof(bool),
            typeof(ElasticGroup),
            new PropertyMetadata(false));

        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register(
            "IsVisible",
            typeof(bool),
            typeof(ElasticGroup),
            new PropertyMetadata(true));

        private static readonly DependencyPropertyKey ItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "Items",
            typeof(IList),
            typeof(ElasticGroup),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ItemsProperty = ItemsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty MaxHeightProperty = DependencyProperty.Register(
            "MaxHeight",
            typeof(double),
            typeof(ElasticGroup),
            new PropertyMetadata(double.PositiveInfinity));

        public static readonly DependencyProperty MinDisabledHeightProperty = DependencyProperty.Register(
            "MinDisabledHeight",
            typeof(double),
            typeof(ElasticGroup),
            new PropertyMetadata(10D));

        public static readonly DependencyProperty MinEnabledHeightProperty = DependencyProperty.Register(
            "MinEnabledHeight",
            typeof(double),
            typeof(ElasticGroup),
            new PropertyMetadata(24D));

        public static readonly DependencyProperty NullContentProperty = DependencyProperty.Register(
            "NullContent",
            typeof(object),
            typeof(ElasticGroup),
            new PropertyMetadata("None"));

        public static readonly DependencyProperty NumberFormatProperty = DependencyProperty.Register(
            "NumberFormat",
            typeof(ElasticGroupNumberFormat),
            typeof(ElasticGroup),
            new PropertyMetadata(ElasticGroupNumberFormat.Value));

        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SelectedItems",
            typeof(IList),
            typeof(ElasticGroup),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            "SelectionMode",
            typeof(SelectionMode),
            typeof(ElasticGroup),
            new PropertyMetadata(SelectionMode.Single));

        public static readonly DependencyProperty SortDirectionProperty = DependencyProperty.Register(
            "SortDirection",
            typeof(ListSortDirection),
            typeof(ElasticGroup),
            new PropertyMetadata(ListSortDirection.Descending));

        public static readonly DependencyProperty SortModeProperty = DependencyProperty.Register(
            "SortMode",
            typeof(ElasticGroupSortMode),
            typeof(ElasticGroup),
            new PropertyMetadata(ElasticGroupSortMode.Count));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width",
            typeof(GridLength),
            typeof(ElasticGroup),
            new PropertyMetadata(new GridLength(1, GridUnitType.Star)));

        #endregion

        private BindingBase displayMemberBinding;
        private EntityGroup entityGroup;
        private ObservableCollection<object> items;
        private bool suppressSelectionChanged;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ElasticGroup"/> class.
        /// </summary>
        public ElasticGroup()
        {
            this.items = new ObservableCollection<object>();
            this.Items = new ReadOnlyObservableCollection<object>(this.items);

            ObservableCollection<object> selectedItems = new ObservableCollection<object>();
            selectedItems.CollectionChanged += this.OnSelectedItemsCollectionChanged;
            this.SelectedItems = selectedItems;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The selection changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)this.GetValue(ContentTemplateProperty); }
            set { this.SetValue(ContentTemplateProperty, value); }
        }

        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)this.GetValue(ContentTemplateSelectorProperty); }
            set { this.SetValue(ContentTemplateSelectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the display member binding.
        /// </summary>
        public BindingBase DisplayMemberBinding
        {
            get
            {
                return this.displayMemberBinding;
            }

            set
            {
                this.displayMemberBinding = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public object Header
        {
            get { return (object)this.GetValue(HeaderProperty); }
            set { this.SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)this.GetValue(HeaderTemplateProperty); }
            set { this.SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the filter is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the user can filter; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilterEnabled
        {
            get { return (bool)this.GetValue(IsFilterEnabledProperty); }
            set { this.SetValue(IsFilterEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the reset button is visible. Only applicable in multi-select mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if the reset button is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsResetButtonVisible
        {
            get { return (bool)this.GetValue(IsResetButtonVisibleProperty); }
            set { this.SetValue(IsResetButtonVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the count is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the count is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsCountVisible
        {
            get { return (bool)this.GetValue(IsCountVisibleProperty); }
            set { this.SetValue(IsCountVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the select all button is visible. Only applicable in multi-select mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if the select all button is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelectAllButtonVisible
        {
            get { return (bool)this.GetValue(IsSelectAllButtonVisibleProperty); }
            set { this.SetValue(IsSelectAllButtonVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get { return (bool)this.GetValue(IsVisibleProperty); }
            set { this.SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IList Items
        {
            get { return (IList)this.GetValue(ItemsProperty); }
            private set { this.SetValue(ItemsPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the max height.
        /// </summary>
        public double MaxHeight
        {
            get { return (double)this.GetValue(MaxHeightProperty); }
            set { this.SetValue(MaxHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum height when an item is disabled.
        /// </summary>
        public double MinDisabledHeight
        {
            get { return (double)this.GetValue(MinDisabledHeightProperty); }
            set { this.SetValue(MinDisabledHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum height when an item is enabled.
        /// </summary>
        public double MinEnabledHeight
        {
            get { return (double)this.GetValue(MinEnabledHeightProperty); }
            set { this.SetValue(MinEnabledHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the content of the null values.
        /// </summary>
        /// <value>
        /// The content of the null values.
        /// </value>
        public object NullContent
        {
            get { return (object)this.GetValue(NullContentProperty); }
            set { this.SetValue(NullContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number format.
        /// </summary>
        /// <value>
        /// The number format.
        /// </value>
        public ElasticGroupNumberFormat NumberFormat
        {
            get { return (ElasticGroupNumberFormat)this.GetValue(NumberFormatProperty); }
            set { this.SetValue(NumberFormatProperty, value); }
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        public IList SelectedItems
        {
            get { return (IList)this.GetValue(SelectedItemsProperty); }
            private set { this.SetValue(SelectedItemsPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>
        /// The selection mode.
        /// </value>
        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)this.GetValue(SelectionModeProperty); }
            set { this.SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>
        /// The sort direction.
        /// </value>
        public ListSortDirection SortDirection
        {
            get { return (ListSortDirection)this.GetValue(SortDirectionProperty); }
            set { this.SetValue(SortDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the sort mode.
        /// </summary>
        /// <value>
        /// The sort mode.
        /// </value>
        public ElasticGroupSortMode SortMode
        {
            get { return (ElasticGroupSortMode)this.GetValue(SortModeProperty); }
            set { this.SetValue(SortModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public GridLength Width
        {
            get { return (GridLength)this.GetValue(WidthProperty); }
            set { this.SetValue(WidthProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the entity group.
        /// </summary>
        /// <value>
        /// The entity group.
        /// </value>
        internal EntityGroup EntityGroup
        {
            get
            {
                return this.entityGroup;
            }

            set
            {
                if (this.entityGroup != null)
                {
                    this.entityGroup.SelectionChanged -= this.OnEntityGroupSelectionChanged;
                    this.entityGroup.Entities.CollectionChanged -= this.OnEntitiesCollectionChanged;
                }

                this.entityGroup = value;

                if (this.entityGroup != null)
                {
                    this.entityGroup.SelectionChanged += this.OnEntityGroupSelectionChanged;
                    this.entityGroup.Entities.CollectionChanged += this.OnEntitiesCollectionChanged;

                    this.GetItemsFromEntities();
                    this.GetSelectedItemsFromEntities();
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The create instance core.
        /// </summary>
        /// <returns> The <see cref="Freezable"/>. </returns>
        /// <exception cref="NotImplementedException">The Exception</exception>
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName"> The property name. </param>
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;

            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets all of the items from the entities.
        /// </summary>
        private void GetItemsFromEntities()
        {
            this.items.Clear();
            foreach (object item in this.entityGroup.Entities.Select(x => x.Content))
            {
                this.items.Add(item);
            }
        }

        /// <summary>
        /// Gets the selected items from the entities.
        /// </summary>
        private void GetSelectedItemsFromEntities()
        {
            if (!this.suppressSelectionChanged)
            {
                this.suppressSelectionChanged = true;

                this.SelectedItems.Clear();
                foreach (object item in this.entityGroup.Entities
                    .Where(x => x.IsSelected)
                    .Select(x => x.Content))
                {
                    this.SelectedItems.Add(item);
                }

                this.OnSelectionChanged();

                this.suppressSelectionChanged = false;
            }
        }

        /// <summary>
        /// Sets the selected items in the entities.
        /// </summary>
        private void SetEntitiesSelectedItems()
        {
            if (!this.suppressSelectionChanged)
            {
                this.suppressSelectionChanged = true;

                foreach (Entity entity in this.entityGroup.Entities)
                {
                    entity.IsSelected = this.SelectedItems.Contains(entity.Content);
                }

                this.OnSelectionChanged();

                this.suppressSelectionChanged = false;
            }
        }

        /// <summary>
        /// Called when the entities collection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnEntitiesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.GetItemsFromEntities();
        }

        /// <summary>
        /// Called when the entity group selection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnEntityGroupSelectionChanged(object sender, EventArgs e)
        {
            this.GetSelectedItemsFromEntities();
        }

        /// <summary>
        /// Called when the selected items collection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnSelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.SetEntitiesSelectedItems();
        }

        /// <summary>
        /// Called when the selection is changed.
        /// </summary>
        private void OnSelectionChanged()
        {
            EventHandler eventHandler = this.SelectionChanged;
            if (eventHandler != null)
            {
                eventHandler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
