namespace Framework.UI.Controls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Automation;
    using System.Windows.Controls;

    public struct DataGridColumnInfo
    {
        public DataGridColumnInfo(DataGridColumn column)
            : this()
        {
            this.DisplayIndex = column.DisplayIndex;
            this.Name = AutomationProperties.GetName(column);
            this.SortDirection = column.SortDirection;
            this.Visibility = column.Visibility;
            this.WidthType = column.Width.UnitType;
            this.WidthValue = column.Width.DisplayValue;
        }

        public int DisplayIndex
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ListSortDirection? SortDirection
        {
            get;
            set;
        }

        public Visibility Visibility
        {
            get;
            set;
        }

        public DataGridLengthUnitType WidthType
        {
            get;
            set;
        }

        public double WidthValue
        {
            get;
            set;
        }

        public void Apply(DataGridColumn column, int gridColumnCount)
        {
            if (column.DisplayIndex != this.DisplayIndex)
            {
                var maxIndex = (gridColumnCount == 0) ? 0 : gridColumnCount - 1;
                column.DisplayIndex = (this.DisplayIndex <= maxIndex) ? this.DisplayIndex : maxIndex;
            }

            column.SortDirection = this.SortDirection;
            column.Visibility = this.Visibility;
            column.Width = new DataGridLength(this.WidthValue, this.WidthType);
        }
    }
}
