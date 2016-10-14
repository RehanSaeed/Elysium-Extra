namespace Framework.UI.TestHarness.Models
{
    using System;
    using System.Collections.ObjectModel;

    /// <summary>
    /// The fund collection.
    /// </summary>
    public sealed class FundCollection : ObservableCollection<Fund>
    {
        public FundCollection()
            : this(false)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="FundCollection"/> class.
        /// </summary>
        public FundCollection(bool hasErrors)
        {
            this.Add(
                new Fund(this)
                {
                    Date = DateTime.Today.AddDays(2),
                    Icon = "Icon 1",
                    IsActive = true,
                    Name = "Name 1",
                    Url = "http://www.google.com",
                    Priority = 1,
                    Value = 2132332
                });
            this.Add(
                new Fund(this)
                {
                    Date = DateTime.Today.AddDays(1),
                    Icon = "Icon 2",
                    IsActive = true,
                    Name = "Name 2",
                    Url = "http://www.google.com",
                    Priority = 2,
                    Value = 423434
                });
            this.Add(
                new Fund(this)
                {
                    Date = DateTime.Today.AddDays(2),
                    Icon = "Icon 3",
                    IsActive = true,
                    Name = "Name 3",
                    Url = "http://www.google.com",
                    Priority = 3,
                    Value = 2342
                });
            this.Add(
                new Fund(this)
                {
                    Date = DateTime.Today.AddDays(3),
                    Icon = "Icon 4",
                    IsActive = true,
                    Name = "Name 4",
                    Url = "http://www.google.com",
                    Priority = 4,
                    Value = 32423
                });
            this.Add(
                new Fund(this)
                {
                    Date = DateTime.Today.AddDays(4),
                    Icon = "Icon 5",
                    IsActive = true,
                    Name = "Name 5",
                    Url = "http://www.google.com",
                    Priority = 5,
                    Value = 324235
                });

            if (hasErrors)
            {
                this.Add(
                    new Fund(this)
                    {
                        Date = DateTime.Today,
                        Icon = "Icon 6",
                        IsActive = false,
                        Name = "Name 6",
                        Url = "http://www.google.com",
                        Priority = 6,
                        Value = -1
                    });
                this.Add(
                   new Fund(this)
                   {
                       Date = DateTime.Today.AddDays(-1),
                       Priority = 7,
                   });
            }
        }
    }
}
