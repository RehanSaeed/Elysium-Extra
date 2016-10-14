namespace Framework.UI.Controls
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    public sealed class ElasticPickerGrid : Grid
    {
        /// <summary>
        /// Called when the visual children of a <see cref="T:System.Windows.Controls.Grid" /> element change.
        /// </summary>
        /// <param name="visualAdded">Identifies the visual child that's added.</param>
        /// <param name="visualRemoved">Identifies the visual child that's removed.</param>
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
            Task.Run(() => this.UpdateColumns());
        }

        private void UpdateColumns()
        {
            this.Dispatcher.InvokeAsync(
                () =>
                {
                    this.ColumnDefinitions.Clear();
                    int i = 0;
                    foreach (FrameworkElement item in this.Children.Cast<FrameworkElement>())
                    {
                        EntityGroup entityGroup = (EntityGroup)item.DataContext;

                        ColumnDefinition columnDefinition = new ColumnDefinition()
                        {
                            DataContext = entityGroup
                        };
                        Grid.SetColumn(item, i);

                        if (entityGroup.IsVisible)
                        {
                            columnDefinition.SetBinding(ColumnDefinition.WidthProperty, "Width");
                        }
                        else
                        {
                            columnDefinition.Width = new GridLength(0);
                        }

                        this.ColumnDefinitions.Add(columnDefinition);

                        ++i;
                    }
                });
        }
    }
}
