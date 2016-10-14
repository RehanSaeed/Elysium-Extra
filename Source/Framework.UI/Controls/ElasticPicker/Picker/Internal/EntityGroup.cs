namespace Framework.UI.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Framework.ComponentModel;
    using Framework.UI.Input;

    /// <summary>
    /// The entity group.
    /// </summary>
    public sealed class EntityGroup : NotifyPropertyChanges
    {
        #region Fields

        private readonly EntityCollection entities;

        private readonly DelegateCommand clearFilterCommand;
        private readonly DelegateCommand resetCommand;
        private readonly DelegateCommand selectAllCommand;

        private string filterText = string.Empty;
        private object header;
        private DataTemplate headerTemplate;
        private bool isCountVisible;
        private bool isFilterEnabled;
        private bool isResetButtonVisible;
        private bool isSelectAllButtonVisible;
        private bool isVisible = true;
        private double maxHeight;
        private double minDisabledHeight;
        private double minEnabledHeight;
        private ElasticGroupNumberFormat numberFormat;
        private ObservableCollection<object> selectedItems;
        private SelectionMode selectionMode;
        private ListSortDirection sortDirection;
        private ElasticGroupSortMode sortMode;
        private bool suppressSelectionChanged;
        private GridLength width;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityGroup"/> class.
        /// </summary>
        public EntityGroup()
        {
            this.entities = new EntityCollection();
            this.selectedItems = new ObservableCollection<object>();

            this.entities.WhenItemChanged.Subscribe(this.OnEntityChanged);
            this.entities.SelectionChanged += this.OnSelectionChanged;

            this.clearFilterCommand = new DelegateCommand(this.ClearFilter, this.CanClearFilter);
            this.resetCommand = new DelegateCommand(this.Reset, this.CanReset);
            this.selectAllCommand = new DelegateCommand(this.SelectAll, this.CanSelectAll);
        }

        #endregion

        /// <summary>
        /// Occurs when the selection is changed.
        /// </summary>
        public event EventHandler SelectionChanged;

        #region Public Properties

        /// <summary>
        /// Gets the entities.
        /// </summary>
        public EntityCollection Entities
        {
            get { return this.entities; }
        }

        /// <summary>
        /// Gets the entities view.
        /// </summary>
        public ICollectionView EntitiesView
        {
            get
            {
                ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Entities);

                collectionView.Filter = 
                    x =>
                    {
                        if (string.IsNullOrEmpty(this.FilterText))
                        {
                            return true;
                        }
                        
                        string content = ((Entity)x).Content + string.Empty;
                        return content.ToLower().Contains(this.FilterText.ToLower());
                    };
                collectionView.SortDescriptions.Clear();
                collectionView.SortDescriptions.Add(
                    new SortDescription("IsEnabled", ListSortDirection.Descending));
                if (this.SortMode == ElasticGroupSortMode.Content)
                {
                    collectionView.SortDescriptions.Add(
                        new SortDescription("Content", this.SortDirection));
                }
                else
                {
                    collectionView.SortDescriptions.Add(
                        new SortDescription("Total", this.SortDirection));
                }

                return collectionView;
            }
        }

        /// <summary>
        /// Gets or sets the filter text.
        /// </summary>
        /// <value>
        /// The filter text.
        /// </value>
        public string FilterText
        {
            get 
            { 
                return this.filterText; 
            }

            set 
            { 
                this.SetProperty(ref this.filterText, value);
                this.ClearFilterCommand.RaiseCanExecuteChanged();
                this.EntitiesView.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        public object Header
        {
            get { return this.header; }
            set { this.SetProperty(ref this.header, value); }
        }

        /// <summary>
        /// Gets or sets the header template.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return this.headerTemplate; }
            set { this.SetProperty(ref this.headerTemplate, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the count is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the count is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsCountVisible
        {
            get { return this.isCountVisible; }
            set { this.SetProperty(ref this.isCountVisible, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the filter is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the user can filter; otherwise, <c>false</c>.
        /// </value>
        public bool IsFilterEnabled
        {
            get { return this.isFilterEnabled; }
            set { this.SetProperty(ref this.isFilterEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the reset button is visible. Only applicable in multi-select mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if the reset button is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsResetButtonVisible
        {
            get { return this.isResetButtonVisible; }
            set { this.SetProperty(ref this.isResetButtonVisible, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the select all button is visible. Only applicable in multi-select mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if the select all button is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelectAllButtonVisible 
        {
            get { return this.isSelectAllButtonVisible; }
            set { this.SetProperty(ref this.isSelectAllButtonVisible, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.SetProperty(ref this.isVisible, value); }
        }

        /// <summary>
        /// Gets or sets the max height.
        /// </summary>
        public double MaxHeight
        {
            get { return this.maxHeight; }
            set { this.SetProperty(ref this.maxHeight, value); }
        }

        /// <summary>
        /// Gets or sets the minimum height when an item is disabled.
        /// </summary>
        public double MinDisabledHeight
        {
            get { return this.minDisabledHeight; }
            set { this.SetProperty(ref this.minDisabledHeight, value); }
        }

        /// <summary>
        /// Gets or sets the minimum height when an item is enabled.
        /// </summary>
        public double MinEnabledHeight
        {
            get { return this.minEnabledHeight; }
            set { this.SetProperty(ref this.minEnabledHeight, value); }
        }

        /// <summary>
        /// Gets or sets the number format.
        /// </summary>
        /// <value>
        /// The number format.
        /// </value>
        public ElasticGroupNumberFormat NumberFormat
        {
            get { return this.numberFormat; }
            set { this.SetProperty(ref this.numberFormat, value); }
        }

        /// <summary>
        /// Gets the selected items.
        /// </summary>
        public ObservableCollection<object> SelectedItems
        {
            get { return this.selectedItems; }
        }

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        /// <value>
        /// The selection mode.
        /// </value>
        public SelectionMode SelectionMode
        {
            get 
            { 
                return this.selectionMode; 
            }

            set 
            { 
                this.SetProperty(ref this.selectionMode, value);
                this.ResetCommand.RaiseCanExecuteChanged();
                this.SelectAllCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>
        /// The sort direction.
        /// </value>
        public ListSortDirection SortDirection
        {
            get { return this.sortDirection; }
            set { this.SetProperty(ref this.sortDirection, value); }
        }

        /// <summary>
        /// Gets or sets the sort mode.
        /// </summary>
        /// <value>
        /// The sort mode.
        /// </value>
        public ElasticGroupSortMode SortMode
        {
            get { return this.sortMode; }
            set { this.SetProperty(ref this.sortMode, value); }
        }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public GridLength Width
        {
            get { return this.width; }
            set { this.SetProperty(ref this.width, value); }
        }

        /// <summary>
        /// Gets the clear filter text command.
        /// </summary>
        /// <value>
        /// The clear filter text command.
        /// </value>
        public DelegateCommand ClearFilterCommand
        {
            get { return this.clearFilterCommand; }
        }

        /// <summary>
        /// Gets the reset command.
        /// </summary>
        /// <value>
        /// The reset command.
        /// </value>
        public DelegateCommand ResetCommand
        {
            get { return this.resetCommand; }
        }

        /// <summary>
        /// Gets the select all command.
        /// </summary>
        /// <value>
        /// The select all command.
        /// </value>
        public DelegateCommand SelectAllCommand
        {
            get { return this.selectAllCommand; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether this instance can clear the filter text.
        /// </summary>
        /// <returns><c>true</c> if the filter text can be cleared, otherwise <c>false</c>.</returns>
        private bool CanClearFilter()
        {
            return !string.IsNullOrEmpty(this.FilterText);
        }

        /// <summary>
        /// Clears the filter text.
        /// </summary>
        private void ClearFilter()
        {
            this.FilterText = string.Empty;
        }

        /// <summary>
        /// Determines whether this instance can deselect all selected entities.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can reset; otherwise, <c>false</c>.
        /// </returns>
        private bool CanReset()
        {
            return (this.SelectionMode != SelectionMode.Single) &&
                this.Entities.Any(x => x.IsSelected);
        }

        /// <summary>
        /// De-selects all entities.
        /// </summary>
        private void Reset()
        {
            this.suppressSelectionChanged = true;

            foreach (Entity entity in this.Entities)
            {
                entity.IsSelected = false;
            }

            this.suppressSelectionChanged = false;
            this.OnSelectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether this instance can select all its entities.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can select all; otherwise, <c>false</c>.
        /// </returns>
        private bool CanSelectAll()
        {
            return (this.SelectionMode != SelectionMode.Single) &&
                this.Entities.Any(x => !x.IsSelected);
        }

        /// <summary>
        /// Selects all entities.
        /// </summary>
        private void SelectAll()
        {
            this.suppressSelectionChanged = true;
            
            foreach (Entity entity in this.Entities)
            {
                entity.IsSelected = true;
            }

            this.suppressSelectionChanged = false;
            this.OnSelectionChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Called when an entity is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OnEntityChanged(ItemChangedEventArgs<Entity> e)
        {
            this.ResetCommand.RaiseCanExecuteChanged();
            this.SelectAllCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Called when the selection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (!this.suppressSelectionChanged)
            {
                this.UpdateSelectedItems();

                EventHandler eventHandler = this.SelectionChanged;

                if (eventHandler != null)
                {
                    eventHandler(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Updates the selected items.
        /// </summary>
        private void UpdateSelectedItems()
        {
            this.SelectedItems.Clear();
            foreach (object item in this.Entities.Where(x => x.IsSelected).SelectMany(x => x.Items))
            {
                this.SelectedItems.Add(item);
            }
        }

        #endregion
    }
}
