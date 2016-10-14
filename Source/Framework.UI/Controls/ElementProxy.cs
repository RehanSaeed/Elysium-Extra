namespace Framework.UI
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Threading;

    /// <summary>
    /// When placed into an element's Resources collection the proxy's Element property returns 
    /// that containing element. Use the NameScopeSource attached property to bridge an element's 
    /// NameScope to other elements.
    /// </summary>
    /// <remarks>
    /// Adapted from <![CDATA[http://joshsmithonwpf.wordpress.com/2008/07/22/enable-elementname-bindings-with-elementspy/]]>. 
    /// </remarks>
    public class ElementProxy : Freezable
    {
        #region Dependency Properties

        /// <summary>
        /// Identifies the NameScopeSource attached property.
        /// </summary>
        public static readonly DependencyProperty NameScopeSourceProperty = DependencyProperty.RegisterAttached(
            "NameScopeSource",
            typeof(ElementProxy),
            typeof(ElementProxy),
            new UIPropertyMetadata(null, OnNameScopeSourceChanged)); 

        #endregion

        #region Public Properties

        /// <summary>
        /// Backing field for <see cref="ElementProxy.Element"/> property.
        /// </summary>
        private DependencyObject element;

        /// <summary>
        /// Gets the element that the proxy provides access to.
        /// </summary>
        public DependencyObject Element
        {
            get
            {
                if (this.element == null)
                {
                    var prop = typeof(Freezable).GetProperty("InheritanceContext", BindingFlags.Instance | BindingFlags.NonPublic);

                    this.element = prop.GetValue(this, null) as DependencyObject;

                    if (this.element != null)
                    {
                        this.Freeze();
                    }
                }

                return this.element;
            }
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Sets the value of the NameScopeSource attached property.
        /// </summary>
        /// <param name="element">The object from which the property value is read.</param>
        /// <returns>The object's NameScopeSource property value.</returns>
        public static ElementProxy GetNameScopeSource(DependencyObject element)
        {
            return (ElementProxy)element.GetValue(NameScopeSourceProperty);
        }

        /// <summary>
        /// Sets the value of the NameScopeSource attached property for an object.
        /// </summary>
        /// <param name="element">The object to which the attached property is written.</param>
        /// <param name="value">The value to set.</param>
        public static void SetNameScopeSource(DependencyObject element, ElementProxy value)
        {
            element.SetValue(NameScopeSourceProperty, value);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// When implemented in a derived class, creates a new instance of the <see cref="T:System.Windows.Freezable"/> derived class.
        /// </summary>
        /// <returns>The new instance.</returns>
        protected override Freezable CreateInstanceCore()
        {
            // We are required to override this abstract method.
            throw new NotSupportedException();
        }

        #endregion

        #region Private Static Properties

        /// <summary>
        /// Called when the NameScopeSource attached property changes.
        /// </summary>
        /// <param name="element">The object for which the property is changing.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnNameScopeSourceChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            ElementProxy proxy = e.NewValue as ElementProxy;
            if (proxy == null || proxy.Element == null)
            {
                return;
            }

            INameScope scope = NameScope.GetNameScope(proxy.Element);
            if (scope == null)
            {
                return;
            }

            element.Dispatcher.BeginInvoke(
                DispatcherPriority.Normal, 
                (Action)(() => NameScope.SetNameScope(element, scope)));
        }

        #endregion
    }
}
