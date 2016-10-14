namespace Framework.UI.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Framework.UI.Input;

    /// <summary>
    /// The elastic picker.
    /// </summary>
    public class ElasticPicker : ItemsControl, INotifyPropertyChanged
    {
        #region Dependency Properties

        private static readonly DependencyPropertyKey InternalViewPropertyKey = DependencyProperty.RegisterReadOnly(
            "InternalView",
            typeof(EntityView),
            typeof(ElasticPicker),
            new PropertyMetadata(null));

        public static readonly DependencyProperty InternalViewProperty = InternalViewPropertyKey.DependencyProperty;

        public static readonly DependencyProperty IsResetButtonVisibleProperty = DependencyProperty.Register(
            "IsResetButtonVisible", 
            typeof(bool), 
            typeof(ElasticPicker),
            new PropertyMetadata(false, OnIsResetButtonVisibleChanged));

        public static readonly DependencyProperty ResetAllCommandProperty = DependencyProperty.Register(
            "ResetAllCommand",
            typeof(DelegateCommand),
            typeof(ElasticPicker),
            new PropertyMetadata(null));

        private static readonly DependencyPropertyKey SelectedItemsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SelectedItems",
            typeof(IList),
            typeof(ElasticPicker),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemsProperty = SelectedItemsPropertyKey.DependencyProperty;

        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(
            "View",
            typeof(ElasticView),
            typeof(ElasticPicker),
            new PropertyMetadata(null, OnViewChanged));

        #endregion

        #region Routed Events

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(ElasticPicker));

        #endregion

        private BindingBase isSelectedBinding;
        private FlipControl flipControl;

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ElasticPicker"/> class.
        /// </summary>
        public ElasticPicker()
        {
            this.InternalView = new EntityView();
            this.ResetAllCommand = new DelegateCommand(this.ResetAll, this.CanResetAll);
            this.SelectedItems = new ObservableCollection<object>();
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
        public event RoutedEventHandler SelectionChanged
        {
            add { this.AddHandler(SelectionChangedEvent, value); }
            remove { this.RemoveHandler(SelectionChangedEvent, value); }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the internal view.
        /// </summary>
        public EntityView InternalView
        {
            get { return (EntityView)this.GetValue(InternalViewProperty); }
            private set { this.SetValue(InternalViewPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the reset button is visible.
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
        /// Gets or sets the is selected binding.
        /// </summary>
        public BindingBase IsSelectedBinding
        {
            get
            {
                return this.isSelectedBinding;
            }

            set
            {
                this.isSelectedBinding = value;
                this.OnPropertyChanged("IsSelectedBinding");
            }
        }

        /// <summary>
        /// Gets or sets the reset command.
        /// </summary>
        public DelegateCommand ResetAllCommand
        {
            get { return (DelegateCommand)this.GetValue(ResetAllCommandProperty); }
            set { this.SetValue(ResetAllCommandProperty, value); }
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
        /// Gets or sets the view.
        /// </summary>
        public ElasticView View
        {
            get { return (ElasticView)this.GetValue(ViewProperty); }
            set { this.SetValue(ViewProperty, value); }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The on apply template.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.flipControl = (FlipControl)this.GetTemplateChild("PART_FlipControl");
            this.flipControl.ItemsSource = new List<int>()
            {
                0
            };

            this.Update();
        }

        /// <summary>
        /// The on items changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            this.ResolveGroups();
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler eventHandler = this.PropertyChanged;

            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when the reset button visibility is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsResetButtonVisibleChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs e)
        {
            ElasticPicker elasticPicker = (ElasticPicker)dependencyObject;
            elasticPicker.ResetAllCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Called when the view is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnViewChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ElasticPicker elasticPicker = (ElasticPicker)dependencyObject;

            if (elasticPicker.IsLoaded)
            {
                elasticPicker.ResolveGroups();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determines whether this instance can deselect all entities.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance can reset all; otherwise, <c>false</c>.
        /// </returns>
        private bool CanResetAll()
        {
            return this.InternalView.Groups.Any(x => x.SelectedItems.Count > 0);
        }

        /// <summary>
        /// Deselects all entities.
        /// </summary>
        private void ResetAll()
        {
            foreach (Entity entity in this.InternalView.Groups.SelectMany(x => x.Entities))
            {
                entity.IsSelected = false;
            }
        }

        /// <summary>
        /// The resolve groups.
        /// </summary>
        private void ResolveGroups()
        {
            this.InternalView.Groups.Clear();

            foreach (ElasticGroup elasticGroup in this.View.Groups)
            {
                EntityGroup entityGroup = new EntityGroup()
                {
                    Header = elasticGroup.Header,
                    HeaderTemplate = elasticGroup.HeaderTemplate,
                    IsCountVisible = elasticGroup.IsCountVisible,
                    IsFilterEnabled = elasticGroup.IsFilterEnabled,
                    IsResetButtonVisible = elasticGroup.IsResetButtonVisible,
                    IsSelectAllButtonVisible = elasticGroup.IsSelectAllButtonVisible,
                    IsVisible = elasticGroup.IsVisible,
                    MaxHeight = elasticGroup.MaxHeight,
                    MinDisabledHeight = elasticGroup.MinDisabledHeight,
                    MinEnabledHeight = elasticGroup.MinEnabledHeight,
                    NumberFormat = elasticGroup.NumberFormat,
                    SelectionMode = elasticGroup.SelectionMode,
                    SortDirection = elasticGroup.SortDirection,
                    SortMode = elasticGroup.SortMode,
                    Width = elasticGroup.Width
                };
                elasticGroup.EntityGroup = entityGroup;
                entityGroup.SelectionChanged += this.OnSelectionChanged;

                foreach (object item in this.Items)
                {
                    object content = elasticGroup.DisplayMemberBinding.Resolve(item);

                    // Handle null group values.
                    if (content == null)
                    {
                        content = elasticGroup.NullContent;
                    }

                    Entity entity;
                    if (entityGroup.Entities.Contains(content))
                    {
                        entity = entityGroup.Entities[content];
                    }
                    else
                    {
                        entity = new Entity(entityGroup)
                        {
                            Content = content,
                            ContentTemplate = elasticGroup.ContentTemplate,
                            IsEnabled = true,
                            IsSelected = false,
                            Total = 0,
                            Value = 0
                        };
                        entityGroup.Entities.Add(entity);
                    }

                    entity.Items.Add(item);
                    ++entity.Value;
                    ++entity.Total;
                }

                this.InternalView.Groups.Add(entityGroup);
            }

            this.Update();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        private void Update()
        {
            System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();

            List<object> selectedItems;
            if (this.InternalView.Groups.Any(x => x.SelectedItems.Count > 0))
            {
                // Get the groups where something is selected.
                List<EntityGroup> groups = this.InternalView.Groups
                    .Where(y => y.Entities.Any(x => x.IsSelected))
                    .ToList();

                // Get the items where they exist in all selected groups.
                selectedItems = this.InternalView.Groups
                    .SelectMany(x => x.SelectedItems)
                    .Distinct()
                    .Where(x => groups.All(y => y.SelectedItems.Contains(x)))
                    .ToList();

                this.UpdateEnabledItems(selectedItems);
            }
            else
            {
                // Select all if nothing selected.
                selectedItems = this.InternalView.Groups
                    .SelectMany(x => x.Entities)
                    .SelectMany(x => x.Items)
                    .Distinct()
                    .ToList();
                this.UpdateEnabledItems(new List<object>());
            }

            this.UpdateSelectedItems(selectedItems);
        }

        /// <summary>
        /// The update enabled items.
        /// </summary>
        /// <param name="selectedItems">
        /// The selected items.
        /// </param>
        private void UpdateEnabledItems(IList<object> selectedItems)
        {
            IEnumerable<Entity> entities = this.InternalView.Groups.SelectMany(x => x.Entities);

            if (selectedItems.Count == 0)
            {
                foreach (Entity entity in entities)
                {
                    entity.IsEnabled = true;

                    if (entity.IsSelected)
                    {
                        entity.Value = 0;
                    }
                    else
                    {
                        entity.Value = entity.Items.Count;
                    }
                }
            }
            else
            {
                foreach (Entity entity in entities)
                {
                    if (entity.IsSelected)
                    {
                        entity.IsEnabled = true;
                        entity.Value = entity.Items.Count(x => selectedItems.Contains(x));
                    }
                    else
                    {
                        if (entity.Group.SelectionMode == SelectionMode.Single)
                        {
                            bool isEnabled = false;
                            int count = 0;
                            foreach (object item in entity.Items)
                            {
                                if (selectedItems.Contains(item))
                                {
                                    isEnabled = true;
                                    ++count;
                                }
                            }

                            entity.IsEnabled = isEnabled;
                            entity.Value = count;
                        }
                        else
                        {
                            List<Entity> otherSelectedEntities = this.InternalView.Groups
                                .SelectMany(x => x.Entities)
                                .Where(x => x.IsSelected && (x.Group != entity.Group))
                                .ToList();

                            if (otherSelectedEntities.Count == 0)
                            {
                                // Select all if nothing selected.
                                entity.Value = entity.Items.Count;
                            }
                            else
                            {
                                List<object> otherSelectedItems = otherSelectedEntities
                                    .GroupBy(x => x.Group)
                                    .SelectMany(x => x.Key.SelectedItems)
                                    .Distinct()
                                    .ToList();
                                entity.Value = entity.Items.Count(x => otherSelectedItems.Contains(x));
                                entity.IsEnabled = entity.Value != 0;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The update selected items.
        /// </summary>
        /// <param name="selectedItems">
        /// The selected items.
        /// </param>
        private void UpdateSelectedItems(IEnumerable<object> selectedItems)
        {
            // Update the selected items.
            this.SelectedItems.Clear();
            foreach (object item in selectedItems)
            {
                this.SelectedItems.Add(item);
            }

            // Update IsSelectedBinding
            if (this.IsSelectedBinding != null)
            {
                foreach (object item in this.Items)
                {
                    this.IsSelectedBinding.Update(item, this.SelectedItems.Contains(item));
                }
            }

            this.OnSelectionChanged();
        }

        /// <summary>
        /// Called when the selection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnSelectionChanged(object sender, EventArgs e)
        {
            this.Update();
        }

        /// <summary>
        /// The on selection changed.
        /// </summary>
        private void OnSelectionChanged()
        {
            // Raise selection changed.
            RoutedEventArgs e = new RoutedEventArgs(ElasticPicker.SelectionChangedEvent);
            this.RaiseEvent(e);

            this.ResetAllCommand.RaiseCanExecuteChanged();

            if (this.flipControl != null)
            {
                this.flipControl.ItemsSource = Enumerable.Range(0, this.SelectedItems.Count + 1);
            }
        }

        #endregion
    }
}
