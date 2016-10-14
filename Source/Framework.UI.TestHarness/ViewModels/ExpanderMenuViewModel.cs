namespace Framework.UI.TestHarness.ViewModels
{
    using System.Collections.Generic;
    using Framework.UI.TestHarness.Models;

    public sealed class ExpanderMenuViewModel
    {
        private readonly FundCollection funds;

        public ExpanderMenuViewModel()
        {
            this.funds = new FundCollection();
        }

        public List<string> Colours
        {
            get
            {
                return new List<string>()
                {
                    "Green",
                    "Red",
                    "Blue",
                    "Black",
                    "Yellow",
                    "Brown",
                    "Purple",
                    "Orange"
                };
            }
        }

        public FundCollection Funds
        {
            get { return this.funds; }
        }
    }
}
