namespace Framework.UI.TestHarness.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows;

    public sealed class StyleViewModel
    {
        private readonly string key;
        private readonly Style style;

        public StyleViewModel(string key, Style style)
        {
            this.key = key;
            this.style = style;
        }

        public string Key
        {
            get { return this.key; }
        }

        public string Name
        {
            get { return this.Key.Replace("IconStyle", string.Empty); }
        }

        public Style Style
        {
            get { return this.style; }
        }

        public string[] Tags
        {
            get
            {
                Setter setter = this.style.Setters
                    .OfType<Setter>()
                    .FirstOrDefault(x => string.Equals(x.Property.Name, "", StringComparison.Ordinal));

                if (setter == null)
                {
                    return null;
                }

                return setter.Value
                    .ToString()
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
