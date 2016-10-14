namespace Framework.UI.TestHarness.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Data;
    using Framework.ComponentModel;
    using Framework.UI.TestHarness.Models;

    public sealed class DataGridViewModel : NotifyPropertyChanges
    {
        #region Fields

        private int decimalPlaces = 2;
        private FundCollection funds;
        private ICollectionView fundsView;
        private double zoom = 1D; 

        #endregion

        #region Constructors

        public DataGridViewModel()
        {
            this.funds = new FundCollection(true);
            this.fundsView = CollectionViewSource.GetDefaultView(this.funds);
        }

        #endregion

        #region Public Properties

        public List<bool> Bools
        {
            get { return new List<bool>() { true, false }; }
        }

        public int DecimalPlaces
        {
            get { return this.decimalPlaces; }
            set { this.SetProperty(ref this.decimalPlaces, value); }
        }

        public string NumberFormat
        {
            get { return "N" + this.DecimalPlaces; }
        }

        public FundCollection Funds
        {
            get { return this.funds; }
        }

        public ICollectionView FundsView
        {
            get { return this.fundsView; }
        }

        public double Zoom
        {
            get { return this.zoom; }
            set { this.SetProperty(ref this.zoom, value); }
        }

        #endregion
    }
}
