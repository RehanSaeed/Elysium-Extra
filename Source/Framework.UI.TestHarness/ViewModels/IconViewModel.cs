namespace Framework.UI.TestHarness.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using Framework.ComponentModel;

    public sealed class IconViewModel : NotifyPropertyChanges
    {
        private readonly List<StyleViewModel> styles;
        private readonly ICollectionView stylesView;

        private string filterText;
        private Style overlayStyle;
        private string overlayStyleName;

        public IconViewModel()
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri("/Framework.UI;component/Themes/ElysiumExtra/GeometryIcon.xaml", UriKind.RelativeOrAbsolute)
            };

            this.styles = new List<StyleViewModel>();
            foreach (string key in resourceDictionary.Keys)
            {
                this.styles.Add(new StyleViewModel(key, (Style)resourceDictionary[key]));
            }

            this.stylesView = CollectionViewSource.GetDefaultView(this.styles);
            this.stylesView.SortDescriptions.Add(new SortDescription("Key", ListSortDirection.Ascending));
            this.stylesView.Filter =
                x =>
                {
                    StyleViewModel styleViewModel = (StyleViewModel)x;
                    return string.IsNullOrWhiteSpace(this.FilterText) ||
                        styleViewModel.Name.ToLower().Contains(this.FilterText.ToLower()) ||
                        (styleViewModel.Tags != null && styleViewModel.Tags.Any(y => y.ToLower().Contains(this.FilterText.ToLower())));
                };
        }

        public string FilterText
        {
            get
            {
                return this.filterText;
            }

            set
            {
                this.SetProperty(ref this.filterText, value);
                this.StylesView.Refresh();
            }
        }

        public Style OverlayStyle
        {
            get { return this.overlayStyle; }
            set { this.SetProperty(ref this.overlayStyle, value); }
        }

        public string OverlayStyleName
        {
            get
            {
                return this.overlayStyleName;
            }

            set
            {
                this.SetProperty(ref this.overlayStyleName, value);
                if (!string.IsNullOrWhiteSpace(this.OverlayStyleName))
                {
                    this.OverlayStyle = (Style)Application.Current.TryFindResource(this.OverlayStyleName);
                }
            }
        }

        public ICollectionView StylesView
        {
            get { return this.stylesView; }
        }
    }
}
