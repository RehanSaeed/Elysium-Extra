namespace Framework.UI
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using Elysium;
    using Framework.UI.Controls;

    public static class ThemeManager
    {
        #region Dependency Properties

        public static readonly DependencyProperty ThemeProperty = DependencyProperty.RegisterAttached(
            "Theme",
            typeof(Theme),
            typeof(ThemeManager),
            new PropertyMetadata(Theme.Light, OnThemePropertyChanged));

        #endregion

        #region Private Static Fields

        private static readonly Uri ElysiumExtraDictionaryUri = new Uri("/Framework.UI;component/Themes/Generic.xaml", UriKind.Relative);
        private static readonly Uri BrushDictionaryUri = new Uri("/Framework.UI;component/Themes/WPF/Base/Brush.xaml", UriKind.Relative);
        private static readonly Uri ColorDictionaryUri = new Uri("/Framework.UI;component/Themes/WPF/Base/Color.xaml", UriKind.Relative);
        private static readonly Uri LightColorDictionaryUri = new Uri("/Framework.UI;component/Themes/WPF/Base/Theme/LightColor.xaml", UriKind.Relative);
        private static readonly Uri DarkColorDictionaryUri = new Uri("/Framework.UI;component/Themes/WPF/Base/Theme/DarkColor.xaml", UriKind.Relative);

        #endregion

        #region Public Static Methods

        public static Theme GetTheme(DependencyObject obj)
        {
            return (Theme)obj.GetValue(ThemeProperty);
        }

        public static void SetTheme(DependencyObject obj, Theme value)
        {
            obj.SetValue(ThemeProperty, value);
        }

        #endregion

        #region Internal Static Methods

        internal static void AddExtraResourceDictionary(
            ResourceDictionary resourceDictionary,
            Theme theme,
            Color accentColor,
            Color contrastColor,
            Color semitransparentContrastColor)
        {
            if (!resourceDictionary.MergedDictionaries.Any(x => x.Source == ElysiumExtraDictionaryUri))
            {
                SharedResourceDictionary sharedResourceDictionary = new SharedResourceDictionary()
                {
                    Source = ElysiumExtraDictionaryUri.ToString()
                };

                UpdateTheme(sharedResourceDictionary, theme);
                UpdateAccentColor(sharedResourceDictionary, accentColor);
                UpdateContrastColor(sharedResourceDictionary, contrastColor);
                UpdateSemitransparentContrastColor(sharedResourceDictionary, semitransparentContrastColor);

                resourceDictionary.MergedDictionaries.Add(sharedResourceDictionary);
            }
        }

        internal static void UpdateTheme(ResourceDictionary resourceDictionary, Theme theme)
        {
            ResourceDictionary brushResourceDictionary = resourceDictionary.FindDictionary(BrushDictionaryUri.ToString());
            if (brushResourceDictionary != null)
            {
                if (theme == Theme.Dark)
                {
                    brushResourceDictionary.ReplaceDictionary(LightColorDictionaryUri, DarkColorDictionaryUri);
                }
                else
                {
                    brushResourceDictionary.ReplaceDictionary(DarkColorDictionaryUri, LightColorDictionaryUri);
                }
            }
            else
            {
                if (theme == Theme.Dark)
                {
                    resourceDictionary.MergedDictionaries.Add(
                        new ResourceDictionary()
                        {
                            Source = DarkColorDictionaryUri
                        });
                }
                else
                {
                    resourceDictionary.MergedDictionaries.Add(
                        new ResourceDictionary()
                        {
                            Source = LightColorDictionaryUri
                        });
                }
            }
        }

        internal static void UpdateAccentColor(ResourceDictionary resourceDictionary, Color accentColor)
        {
            UpdateColor(resourceDictionary, "AccentColor", accentColor);
        }

        internal static void UpdateContrastColor(ResourceDictionary resourceDictionary, Color contrastColor)
        {
            UpdateColor(resourceDictionary, "ContrastColor", contrastColor);
        }

        internal static void UpdateSemitransparentContrastColor(ResourceDictionary resourceDictionary, Color semitransparentContrastColor)
        {
            UpdateColor(resourceDictionary, "SemitransparentContrastColor", semitransparentContrastColor);
        }

        #endregion

        #region Private Static Methods

        private static void UpdateColor(ResourceDictionary resourceDictionary, string key, Color color)
        {
            if (color != null && !((color.A == 0) && (color.R == 0) && (color.G == 0) && (color.B == 0)))
            {
                ResourceDictionary brushResourceDictionary = resourceDictionary.FindDictionary(BrushDictionaryUri.ToString());
                if (brushResourceDictionary != null)
                {
                    ResourceDictionary oldColorResourceDictionary = brushResourceDictionary.FindDictionary(ColorDictionaryUri.ToString());
                    ResourceDictionary colorResourceDictionary = new ResourceDictionary()
                        {
                            Source = ColorDictionaryUri
                        };
                    colorResourceDictionary["AccentColor"] = oldColorResourceDictionary["AccentColor"];
                    colorResourceDictionary["ContrastColor"] = oldColorResourceDictionary["ContrastColor"];
                    colorResourceDictionary["SemitransparentContrastColor"] = oldColorResourceDictionary["SemitransparentContrastColor"];
                    colorResourceDictionary[key] = color;
                    brushResourceDictionary.ReplaceDictionary(ColorDictionaryUri, colorResourceDictionary);
                }
            }
        }

        private static void OnThemePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement frameworkElement = (FrameworkElement)dependencyObject;
            ThemeManager.UpdateTheme(frameworkElement.Resources, GetTheme(frameworkElement));
        }

        #endregion
    }
}
