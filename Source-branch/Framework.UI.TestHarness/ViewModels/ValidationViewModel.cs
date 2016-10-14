namespace Framework.UI.TestHarness.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Framework.ComponentModel;
    using Framework.ComponentModel.Rules;

    public class ValidationViewModel : NotifyDataErrorInfo<ValidationViewModel>
    {
        private bool boolean;
        private DateTime dateTime;
        private IEnumerable<string> items;
        private int number;
        private string selectedItem;
        private string value;

        static ValidationViewModel()
        {
            Rules.Add(new DelegateRule<ValidationViewModel>(
                "Boolean",
                "Boolean cannot be false",
                x => x.Boolean != false));
            Rules.Add(new DelegateRule<ValidationViewModel>(
                "DateTime",
                "DateTime cannot be today",
                x => x.DateTime != DateTime.Today));
            Rules.Add(new DelegateRule<ValidationViewModel>(
                "Number",
                "Number cannot be less than or equal to zero",
                x => x.Number > 0));
            Rules.Add(new DelegateRule<ValidationViewModel>(
                "SelectedItem",
                "SelectedItem cannot be 'One'",
                x => x.SelectedItem != "One"));
            Rules.Add(new DelegateRule<ValidationViewModel>(
                "Value",
                "Value cannot be empty",
                x => !string.IsNullOrWhiteSpace(x.Value)));
        }

        public ValidationViewModel()
        {
            this.dateTime = DateTime.Today;
            this.items = new List<string>()
            {
                "One",
                "Two",
                "Three"
            };
            this.selectedItem = this.items.First();
        }

        public bool Boolean
        {
            get { return this.boolean; }
            set { this.SetProperty(ref this.boolean, value); }
        }

        public DateTime DateTime
        {
            get { return this.dateTime; }
            set { this.SetProperty(ref this.dateTime, value); }
        }

        public IEnumerable<string> Items
        {
            get { return this.items; }
        }

        public int Number
        {
            get { return this.number; }
            set { this.SetProperty(ref this.number, value); }
        }

        public string SelectedItem
        {
            get { return this.selectedItem; }
            set { this.SetProperty(ref this.selectedItem, value); }
        }

        public string Value
        {
            get { return this.value; }
            set { this.SetProperty(ref this.value, value); }
        }
    }
}
