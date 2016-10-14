namespace Framework.UI.Controls
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// The elastic list box item.
    /// </summary>
    public sealed class ElasticListBoxItem : ListBoxItem
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsHighlightedProperty = DependencyProperty.Register(
            "IsHighlighted", 
            typeof(bool), 
            typeof(ElasticListBoxItem), 
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsHighlightedChanged));

        public static readonly DependencyProperty MinDisabledHeightProperty = DependencyProperty.Register(
            "MinDisabledHeight", 
            typeof(double), 
            typeof(ElasticListBoxItem), 
            new PropertyMetadata(10D));

        public static readonly DependencyProperty MinEnabledHeightProperty = DependencyProperty.Register(
            "MinEnabledHeight", 
            typeof(double), 
            typeof(ElasticListBoxItem), 
            new PropertyMetadata(24D));

        public static readonly DependencyProperty MinOpacityProperty = DependencyProperty.Register(
            "MinOpacity",
            typeof(double),
            typeof(ElasticListBoxItem),
            new PropertyMetadata(0.2D));

        public static readonly DependencyProperty ParentListBoxProperty = DependencyProperty.Register(
            "ParentListBox",
            typeof(ElasticListBox),
            typeof(ElasticListBoxItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PercentageProperty = DependencyProperty.Register(
            "Percentage",
            typeof(double),
            typeof(ElasticListBoxItem),
            new PropertyMetadata(0D));

        public static readonly DependencyProperty PercentageOpacityProperty = DependencyProperty.Register(
            "PercentageOpacity", 
            typeof(double), 
            typeof(ElasticListBoxItem), 
            new PropertyMetadata(0D));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(ElasticListBoxItem),
            new PropertyMetadata(0D, OnValueChanged));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether is highlighted.
        /// </summary>
        public bool IsHighlighted
        {
            get { return (bool)this.GetValue(IsHighlightedProperty); }
            set { this.SetValue(IsHighlightedProperty, value); }
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
        /// Gets or sets the min opacity.
        /// </summary>
        public double MinOpacity
        {
            get { return (double)this.GetValue(MinOpacityProperty); }
            set { this.SetValue(MinOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parent list box.
        /// </summary>
        public ElasticListBox ParentListBox
        {
            get { return (ElasticListBox)this.GetValue(ParentListBoxProperty); }
            set { this.SetValue(ParentListBoxProperty, value); }
        }

        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        public double Percentage
        {
            get { return (double)this.GetValue(PercentageProperty); }
            set { this.SetValue(PercentageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the percentage opacity.
        /// </summary>
        public double PercentageOpacity
        {
            get { return (double)this.GetValue(PercentageOpacityProperty); }
            set { this.SetValue(PercentageOpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// The on is highlighted changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The Event Arguments. </param>
        private static void OnIsHighlightedChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ElasticListBoxItem listBoxItem = (ElasticListBoxItem)dependencyObject;
            ElasticListBox listBox = listBoxItem.ParentListBox;

            listBox.ResizeItems();
        }

        /// <summary>
        /// The on value changed.
        /// </summary>
        /// <param name="dependencyObject"> The dependency object. </param>
        /// <param name="e"> The Event Arguments. </param>
        private static void OnValueChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ElasticListBoxItem listBoxItem = (ElasticListBoxItem)dependencyObject;
            ElasticListBox listBox = listBoxItem.ParentListBox;

            // Update the total.
            var items = listBox.Items
                .Cast<object>()
                .Select(x => listBoxItem.ParentListBox.ItemContainerGenerator.ContainerFromItem(x))
                .Where(x => x != null)
                .Cast<ElasticListBoxItem>();
            listBox.Total = items.Sum(x => x.Value);

            // Update the percentage.
            foreach (var item in items)
            {
                if (listBox.Total == 0D)
                {
                    item.Percentage = 0D;
                    item.PercentageOpacity = listBoxItem.MinOpacity;
                }
                else
                {
                    item.Percentage = item.Value / listBox.Total;
                    item.PercentageOpacity = Math.Max(listBoxItem.MinOpacity, item.Percentage);
                }
            }

            listBox.ResizeItems();
        }

        #endregion
    }
}
