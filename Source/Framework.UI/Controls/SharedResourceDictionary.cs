namespace Framework.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Loads singleton instance of ResourceDictionary to current scope;
    /// </summary>
    public class SharedResourceDictionary : ResourceDictionary
    {
        #region Fields

        private static readonly Dictionary<string, ResourceDictionary> SharedResources = new Dictionary<string, ResourceDictionary>();

        private string source;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public new string Source
        {
            get
            {
                return this.source;
            }

            set
            {
                if (this.source != value)
                {
                    this.source = value;
                    this.SourceChanged(value);
                }
            }
        }

        #endregion

        #region Public Methods

        #region Resource Dictionary

        /// <summary>
        /// Return all resource dictionaries that are in memory.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<ResourceDictionary> GetResourceDictionaries()
        {
            foreach (KeyValuePair<string, ResourceDictionary> weakReference in SharedResources)
            {
                yield return weakReference.Value;
            }
        }

        /// <summary>
        /// Get the resource dictionary specified by the source uri.
        /// If the dictionary is not loaded add a weak reference to the list
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="ResourceDictionary"/>.
        /// </returns>
        public static ResourceDictionary GetResourceDictionary(string source)
        {
            // Return the resource dictionary if it could be found
            var foundDictionary = TryFindResourceDictionary(source);
            if (foundDictionary != null)
            {
                return foundDictionary;
            }

            // Not found so remove the weak reference
            if (SharedResources.ContainsKey(source))
            {
                SharedResources.Remove(source);
            }

            // Load the resource dictionary and hold a weak reference to it
            var newDictionary = new ResourceDictionary { Source = new Uri(source, UriKind.RelativeOrAbsolute) };
            SharedResources.Add(source, newDictionary);
            return newDictionary;
        }

        /// <summary>
        /// Find the resource dictionary by recursively looking in the merged dictionaries
        /// Throw an exceptionReturn if the dictionary could not be found
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="ResourceDictionary"/>.
        /// </returns>
        public static ResourceDictionary FindResourceDictionary(string source)
        {
            var foundDictionary = TryFindResourceDictionary(source);
            if (foundDictionary == null)
            {
                throw new Exception(string.Format(
                    "Could not find resource dictionary {0} in SharedResourceDictionary",
                    source));
            }

            return foundDictionary;
        }

        /// <summary>
        /// Find the resource dictionary by recursively looking in the merged dictionaries
        /// Return null if the dictionary could not be found
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="ResourceDictionary"/>.
        /// </returns>
        public static ResourceDictionary TryFindResourceDictionary(string source)
        {
            var foundDictionary = GetResourceDictionaries()
                .Select(dictionary => dictionary.FindDictionary(source))
                .FirstOrDefault(founddictionary => founddictionary != null);
            return foundDictionary;
        }

        #endregion

        #region Resource

        /// <summary>
        /// The try find resource.
        /// </summary>
        /// <param name="resourceKey"> The resource key. </param>
        /// <returns>The <see cref="object"/>.</returns>
        public static object TryFindResource(object resourceKey)
        {
            var foundResource = GetResourceDictionaries()
                                    .Select(resourceDictionary => resourceDictionary.FindResource(resourceKey))
                                    .FirstOrDefault(value => value != null);
            return foundResource;
        }

        /// <summary>
        /// The find resource.
        /// </summary>
        /// <param name="resourceKey">
        /// The resource key.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object FindResource(object resourceKey)
        {
            var foundResource = TryFindResource(resourceKey);
            if (foundResource == null)
            {
                throw new Exception(string.Format(
                    "Could not find resource with resourceKey {0} in SharedResourceDictionary",
                    resourceKey));
            }

            return foundResource;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// The source changed.
        /// </summary>
        /// <param name="newSource">
        /// The new source.
        /// </param>
        private void SourceChanged(string newSource)
        {
            var resource = GetResourceDictionary(newSource);
            if (resource != null)
            {
                this.MergedDictionaries.Add(resource);
            }
        }

        /// <summary>
        /// The is in application scope.
        /// </summary>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsInApplicationScope(ResourceDictionary resource)
        {
            if (resource == null || resource.Source == null)
            {
                return false;
            }

            // Try and find the resource dictionary in the application scope 
            if ((Application.Current != null) && Application.Current.Resources.ContainsDictionary(resource.Source.OriginalString))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
