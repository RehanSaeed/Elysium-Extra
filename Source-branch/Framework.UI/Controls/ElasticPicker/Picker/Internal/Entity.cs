namespace Framework.UI.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using Framework.ComponentModel;

    /// <summary>
    /// The entity.
    /// </summary>
    public sealed class Entity : NotifyPropertyChanges
    {
        #region Fields

        private object content;
        private DataTemplate contentTemplate;
        private EntityGroup group;
        private bool isEnabled;
        private bool isSelected;
        private ObservableCollection<object> items;
        private double total;
        private double value;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="group">The group.</param>
        public Entity(EntityGroup group)
        {
            this.group = group;
            this.items = new ObservableCollection<object>();
        }

        #endregion

        /// <summary>
        /// The is selected changed.
        /// </summary>
        public event EventHandler IsSelectedChanged;

        #region Public Properties

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        public object Content
        {
            get { return this.content; }
            set { this.SetProperty(ref this.content, value); }
        }

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get { return this.contentTemplate; }
            set { this.SetProperty(ref this.contentTemplate, value); }
        }

        /// <summary>
        /// Gets the display value.
        /// </summary>
        /// <value>
        /// The display value.
        /// </value>
        public string DisplayValue
        {
            get
            {
                string displayValue = null;

                switch (this.Group.NumberFormat)
                {
                    case ElasticGroupNumberFormat.Total:
                        displayValue = this.Total.ToString();
                        break;
                    case ElasticGroupNumberFormat.Value:
                        displayValue = this.Value.ToString();
                        break;
                    case ElasticGroupNumberFormat.ValueAndTotal:
                        displayValue = string.Format("{0}/{1}", this.Value, this.Total);
                        break;
                }

                return displayValue;
            }
        }

        /// <summary>
        /// Gets the group.
        /// </summary>
        public EntityGroup Group
        {
            get { return this.group; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetProperty(ref this.isEnabled, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is selected.
        /// </summary>
        public bool IsSelected
        {
            get 
            { 
                return this.isSelected; 
            }

            set
            {
                if (this.SetProperty(ref this.isSelected, value))
                {
                    this.OnIsSelectedChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public ObservableCollection<object> Items
        {
            get { return this.items; }
        }

        /// <summary>
        /// Gets or sets the total.
        /// </summary>
        public double Total
        {
            get { return this.total; }
            set { this.SetProperty(ref this.total, value, "Total", "DisplayValue"); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value
        {
            get { return this.value; }
            set { this.SetProperty(ref this.value, value, "Total", "DisplayValue"); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The on is selected changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnIsSelectedChanged(object sender, EventArgs e)
        {
            EventHandler eventHandler = this.IsSelectedChanged;

            if (eventHandler != null)
            {
                eventHandler(sender, e);
            }
        }

        #endregion
    }
}
