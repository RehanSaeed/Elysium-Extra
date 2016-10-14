namespace Framework.UI.TestHarness.ViewModels
{
    using System.Collections.Generic;

    public sealed class ColoursViewModel
    {
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
    }
}
