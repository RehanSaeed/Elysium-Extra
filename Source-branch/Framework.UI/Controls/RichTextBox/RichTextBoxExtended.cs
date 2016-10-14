namespace Framework.UI.Controls
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Framework.IO;

    /// <summary>
    /// An extended <see cref="RichTextBox"/> with a bind-able RTF Text property.
    /// </summary>
    public class RichTextBoxExtended : RichTextBox
    {
        #region Dependency Properties

        public static readonly DependencyProperty IsPlainTextCopyEnabledProperty = DependencyProperty.Register(
            "IsPlainTextCopyEnabled", 
            typeof(bool), 
            typeof(RichTextBoxExtended),
            new PropertyMetadata(false, OnIsPlainTextCopyEnabledChanged));

        public static readonly DependencyProperty IsPlainTextCutEnabledProperty = DependencyProperty.Register(
            "IsPlainTextCutEnabled",
            typeof(bool),
            typeof(RichTextBoxExtended),
            new PropertyMetadata(false, OnIsPlainTextCutEnabledChanged));

        public static readonly DependencyProperty IsPlainTextPasteEnabledProperty = DependencyProperty.Register(
            "IsPlainTextPasteEnabled", 
            typeof(bool), 
            typeof(RichTextBoxExtended),
            new PropertyMetadata(false, OnIsPlainTextPasteEnabledChanged));

        public static readonly DependencyProperty RtfTextProperty = DependencyProperty.Register(
            "RtfText",
            typeof(string),
            typeof(RichTextBoxExtended),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.Journal,
                OnRtfTextPropertyChanged,
                OnCoerceRtfText,
                true,
                UpdateSourceTrigger.LostFocus));

        public static readonly DependencyProperty SelectionFontFamilyProperty = DependencyProperty.Register(
            "SelectionFontFamily",
            typeof(FontFamily),
            typeof(RichTextBoxExtended),
            new PropertyMetadata(null, OnSelectionFontFamilyChanged));

        public static readonly DependencyProperty SelectionFontSizeProperty = DependencyProperty.Register(
            "SelectionFontSize", 
            typeof(double?), 
            typeof(RichTextBoxExtended),
            new PropertyMetadata(12D, OnSelectionFontSizeChanged));

        public static readonly DependencyProperty SelectionForegroundProperty = DependencyProperty.Register(
            "SelectionForeground",
            typeof(Brush),
            typeof(RichTextBoxExtended),
            new PropertyMetadata(null, OnSelectionForegroundChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(RichTextBoxExtended),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.Journal,
                OnTextPropertyChanged,
                OnCoerceText,
                true,
                UpdateSourceTrigger.LostFocus));

        public static readonly DependencyProperty XamlTextProperty = DependencyProperty.Register(
            "XamlText",
            typeof(string),
            typeof(RichTextBoxExtended),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault |
                FrameworkPropertyMetadataOptions.Journal,
                OnXamlTextPropertyChanged,
                OnCoerceXamlText,
                true,
                UpdateSourceTrigger.LostFocus));

        #endregion

        #region Fields

        private bool isRtfTextChanging;
        private bool isTextChanging;
        private bool isXamlTextChanging;
        private bool suppressSetSelectionProperties; 

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="RichTextBoxExtended"/> class.
        /// </summary>
        public RichTextBoxExtended()
        {
            this.SelectionChanged += this.OnSelectionChanged;
            this.TextChanged += this.OnTextChanged;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether plain text copy is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if plain text copy is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlainTextCopyEnabled
        {
            get { return (bool)this.GetValue(IsPlainTextCopyEnabledProperty); }
            set { this.SetValue(IsPlainTextCopyEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether plain text cut is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if plain text cut is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlainTextCutEnabled
        {
            get { return (bool)this.GetValue(IsPlainTextCutEnabledProperty); }
            set { this.SetValue(IsPlainTextCutEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether plain text paste is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if plain text paste is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlainTextPasteEnabled
        {
            get { return (bool)this.GetValue(IsPlainTextPasteEnabledProperty); }
            set { this.SetValue(IsPlainTextPasteEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the RTF text.
        /// </summary>
        /// <value>
        /// The RTF text.
        /// </value>
        public string RtfText
        {
            get { return (string)this.GetValue(RtfTextProperty); }
            set { this.SetValue(RtfTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family of the current selection.
        /// </summary>
        /// <value>
        /// The selection font family.
        /// </value>
        public FontFamily SelectionFontFamily
        {
            get { return (FontFamily)this.GetValue(SelectionFontFamilyProperty); }
            set { this.SetValue(SelectionFontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size of the current selection.
        /// </summary>
        /// <returns>The size of the text in the <see cref="T:System.Windows.Controls.Control" />. The default is <see cref="P:System.Windows.SystemFonts.MessageFontSize" />. The font size must be a positive number.</returns>
        public double? SelectionFontSize
        {
            get { return (double?)this.GetValue(SelectionFontSizeProperty); }
            set { this.SetValue(SelectionFontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the foreground of the current selection.
        /// </summary>
        /// <value>
        /// The selection foreground.
        /// </value>
        public Brush SelectionForeground
        {
            get { return (Brush)this.GetValue(SelectionForegroundProperty); }
            set { this.SetValue(SelectionForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the plain text.
        /// </summary>
        /// <value>
        /// The plain text.
        /// </value>
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the XAML text.
        /// </summary>
        /// <value>
        /// The XAML text.
        /// </value>
        public string XamlText
        {
            get { return (string)this.GetValue(XamlTextProperty); }
            set { this.SetValue(XamlTextProperty, value); }
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// This override is a workaround to fix problem setting spell checker language.
        /// From <![CDATA[http://stackoverflow.com/questions/18982057/spell-checking-doesnt-work-with-wpf-richtextbox]]>
        /// </summary>
        /// <param name="e">The text changed event arguments.</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            var changeList = e.Changes.ToList();
            if (changeList.Count > 0)
            {
                foreach (var change in changeList)
                {
                    TextPointer start = null;
                    TextPointer end = null;
                    if (change.AddedLength > 0)
                    {
                        start = this.Document.ContentStart.GetPositionAtOffset(change.Offset);
                        end = this.Document.ContentStart.GetPositionAtOffset(change.Offset + change.AddedLength);
                    }
                    else
                    {
                        int startOffset = Math.Max(change.Offset - change.RemovedLength, 0);
                        start = this.Document.ContentStart.GetPositionAtOffset(startOffset);
                        end = this.Document.ContentStart.GetPositionAtOffset(change.Offset);
                    }

                    if (start != null && end != null)
                    {
                        var range = new TextRange(start, end);
                        range.ApplyPropertyValue(FrameworkElement.LanguageProperty, Document.Language);
                    }
                }
            }

            base.OnTextChanged(e);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// CanExecute event handler for ApplicationCommands.Copy.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="args">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnCanExecuteCopy(object target, CanExecuteRoutedEventArgs args)
        {
            RichTextBoxExtended myRichTextBox = (RichTextBoxExtended)target;
            args.CanExecute = myRichTextBox.IsEnabled && !myRichTextBox.Selection.IsEmpty;
        }

        /// <summary>
        /// Event handler for ApplicationCommands.Copy command.
        /// <remarks>
        /// We want to enforce that data can be set on the clipboard
        /// only in plain text format from this RichTextBox.
        /// </remarks>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnCopy(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextBoxExtended myRichTextBox = (RichTextBoxExtended)sender;
            string selectionText = myRichTextBox.Selection.Text;
            Clipboard.SetText(selectionText);
            e.Handled = true;
        }

        /// <summary>
        /// CanExecute event handler for ApplicationCommands.Cut.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="args">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnCanExecuteCut(object target, CanExecuteRoutedEventArgs args)
        {
            RichTextBoxExtended myRichTextBox = (RichTextBoxExtended)target;
            args.CanExecute = myRichTextBox.IsEnabled && !myRichTextBox.IsReadOnly && !myRichTextBox.Selection.IsEmpty;
        }

        /// <summary>
        /// Event handler for ApplicationCommands.Cut command.
        /// <remarks>
        /// We want to enforce that data can be set on the clipboard
        /// only in plain text format from this RichTextBox.
        /// </remarks>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnCut(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextBoxExtended myRichTextBox = (RichTextBoxExtended)sender;
            string selectionText = myRichTextBox.Selection.Text;
            myRichTextBox.Selection.Text = String.Empty;
            Clipboard.SetText(selectionText);
            e.Handled = true;
        }

        /// <summary>
        /// CanExecute event handler for ApplicationCommand.Paste.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="args">The <see cref="CanExecuteRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnCanExecutePaste(object target, CanExecuteRoutedEventArgs args)
        {
            RichTextBoxExtended myRichTextBox = (RichTextBoxExtended)target;
            args.CanExecute = myRichTextBox.IsEnabled && !myRichTextBox.IsReadOnly && Clipboard.ContainsText();
        }

        /// <summary>
        /// Event handler for ApplicationCommands.Paste command.
        /// <remarks>
        /// We want to allow paste only in plain text format.
        /// </remarks>
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecutedRoutedEventArgs"/> instance containing the event data.</param>
        private static void OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            RichTextBoxExtended myRichTextBox = (RichTextBoxExtended)sender;

            // Handle paste only if clipboard supports text format.
            if (Clipboard.ContainsText())
            {
                myRichTextBox.Selection.Text = Clipboard.GetText();
            }

            e.Handled = true;
        }

        /// <summary>
        /// Called when plain text copy enabled is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsPlainTextCopyEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended richTextBox = (RichTextBoxExtended)dependencyObject;

            if (richTextBox.IsPlainTextCopyEnabled)
            {
                richTextBox.CommandBindings.Add(new CommandBinding(
                    ApplicationCommands.Copy,
                    new ExecutedRoutedEventHandler(OnCopy),
                    new CanExecuteRoutedEventHandler(OnCanExecuteCopy)));
            }
            else
            {
                CommandBinding commandBinding = richTextBox.CommandBindings
                    .OfType<CommandBinding>()
                    .FirstOrDefault(x => x.Command == ApplicationCommands.Copy);
                if (commandBinding != null)
                {
                    richTextBox.CommandBindings.Remove(commandBinding);
                }
            }
        }

        /// <summary>
        /// Called when plain text cut enabled is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsPlainTextCutEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended richTextBox = (RichTextBoxExtended)dependencyObject;

            if (richTextBox.IsPlainTextCutEnabled)
            {
                richTextBox.CommandBindings.Add(new CommandBinding(
                    ApplicationCommands.Cut,
                    new ExecutedRoutedEventHandler(OnCut),
                    new CanExecuteRoutedEventHandler(OnCanExecuteCut)));
            }
            else
            {
                CommandBinding commandBinding = richTextBox.CommandBindings
                    .OfType<CommandBinding>()
                    .FirstOrDefault(x => x.Command == ApplicationCommands.Cut);
                if (commandBinding != null)
                {
                    richTextBox.CommandBindings.Remove(commandBinding);
                }
            }
        }

        /// <summary>
        /// Called when plain text paste enabled is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsPlainTextPasteEnabledChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended richTextBox = (RichTextBoxExtended)dependencyObject;

            if (richTextBox.IsPlainTextPasteEnabled)
            {
                richTextBox.CommandBindings.Add(new CommandBinding(
                    ApplicationCommands.Paste,
                    new ExecutedRoutedEventHandler(OnPaste),
                    new CanExecuteRoutedEventHandler(OnCanExecutePaste)));
            }
            else
            {
                CommandBinding commandBinding = richTextBox.CommandBindings
                    .OfType<CommandBinding>()
                    .FirstOrDefault(x => x.Command == ApplicationCommands.Paste);
                if (commandBinding != null)
                {
                    richTextBox.CommandBindings.Remove(commandBinding);
                }
            }
        }

        /// <summary>
        /// Called when the RTF text property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnRtfTextPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended textBox = (RichTextBoxExtended)dependencyObject;
            string value = (string)e.NewValue;

            if (string.IsNullOrWhiteSpace(value))
            {
                textBox.isRtfTextChanging = true;
                textBox.SetText(string.Empty);
                textBox.isRtfTextChanging = false;

                textBox.ClearUndo();
            }
            else
            {
                if (textBox.isRtfTextChanging)
                {
                    return;
                }

                textBox.isRtfTextChanging = true;
                textBox.SetRtf(value);
                textBox.isRtfTextChanging = false;

                textBox.ClearUndo();
            }
        }

        /// <summary>
        /// Called when the RTF Text property is coerced. Text cannot be null.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceRtfText(DependencyObject dependencyObject, object value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Called when the font family of the current selection is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void OnSelectionFontFamilyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended richTextBoxExtended = (RichTextBoxExtended)dependencyObject;
            if (!richTextBoxExtended.suppressSetSelectionProperties)
            {
                richTextBoxExtended.Focus();
                richTextBoxExtended.Selection.ApplyPropertyValue(RichTextBox.FontFamilyProperty, richTextBoxExtended.SelectionFontFamily);
            }
        }

        /// <summary>
        /// Called when the font size of the current selection is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void OnSelectionFontSizeChanged(
            DependencyObject dependencyObject, 
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended richTextBoxExtended = (RichTextBoxExtended)dependencyObject;
            if (!richTextBoxExtended.suppressSetSelectionProperties)
            {
                richTextBoxExtended.Focus();
                richTextBoxExtended.Selection.ApplyPropertyValue(RichTextBox.FontSizeProperty, richTextBoxExtended.SelectionFontSize);
            }
        }

        /// <summary>
        /// Called when the foreground of the current selection is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        private static void OnSelectionForegroundChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended richTextBoxExtended = (RichTextBoxExtended)dependencyObject;
            if (!richTextBoxExtended.suppressSetSelectionProperties)
            {
                richTextBoxExtended.Focus();
                richTextBoxExtended.Selection.ApplyPropertyValue(RichTextBox.ForegroundProperty, richTextBoxExtended.SelectionForeground);
            }
        }

        /// <summary>
        /// Called when the text property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnTextPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended textBox = (RichTextBoxExtended)dependencyObject;
            string value = (string)e.NewValue;

            if (textBox.isTextChanging)
            {
                return;
            }

            textBox.isTextChanging = true;
            textBox.SetText(value);
            textBox.isTextChanging = false;

            textBox.ClearUndo();
        }

        /// <summary>
        /// Called when the Text property is coerced. Text cannot be null.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceText(DependencyObject dependencyObject, object value)
        {
            return value ?? string.Empty;
        }

        /// <summary>
        /// Called when the XAML text property is changed.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnXamlTextPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            RichTextBoxExtended textBox = (RichTextBoxExtended)dependencyObject;
            string value = (string)e.NewValue;

            if (string.IsNullOrWhiteSpace(value))
            {
                textBox.isXamlTextChanging = true;
                textBox.SetText(string.Empty);
                textBox.isXamlTextChanging = false;

                textBox.ClearUndo();
            }
            else
            {
                if (textBox.isXamlTextChanging)
                {
                    return;
                }

                textBox.isXamlTextChanging = true;
                textBox.SetXaml(value);
                textBox.isXamlTextChanging = false;

                textBox.ClearUndo();
            }
        }

        /// <summary>
        /// Called when the XAML Text property is coerced. Text cannot be null.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="value">The value.</param>
        /// <returns>The coerced value.</returns>
        private static object OnCoerceXamlText(DependencyObject dependencyObject, object value)
        {
            return value ?? string.Empty;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when the selection is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            this.suppressSetSelectionProperties = true;

            object fontFamily = this.Selection.GetPropertyValue(RichTextBox.FontFamilyProperty);
            if (fontFamily is FontFamily)
            {
                this.SelectionFontFamily = (FontFamily)fontFamily;
            }
            else
            {
                this.SelectionFontFamily = null;
            }

            object fontSize = this.Selection.GetPropertyValue(RichTextBox.FontSizeProperty);
            if (fontSize is double)
            {
                this.SelectionFontSize = (double)fontSize;
            }
            else
            {
                this.SelectionFontSize = null;
            }

            object foreground = this.Selection.GetPropertyValue(RichTextBox.ForegroundProperty);
            if (fontSize is Brush)
            {
                this.SelectionForeground = (Brush)foreground;
            }
            else
            {
                this.SelectionForeground = null;
            }

            this.suppressSetSelectionProperties = false;
        } 

        /// <summary>
        /// Called when the text property is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextChangedEventArgs"/> instance containing the event data.</param>
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.isRtfTextChanging)
            {
                this.isRtfTextChanging = true;
                this.RtfText = this.GetRtf();
                this.isRtfTextChanging = false;
            }

            if (!this.isTextChanging)
            {
                this.isTextChanging = true;
                this.Text = this.GetText();
                this.isTextChanging = false;
            }

            if (!this.isXamlTextChanging)
            {
                this.isXamlTextChanging = true;
                this.XamlText = this.GetXaml();
                this.isXamlTextChanging = false;
            }
        }

        /// <summary>
        /// Clears the undo limit.
        /// </summary>
        private void ClearUndo()
        {
            int limit = this.UndoLimit;
            this.UndoLimit = 0;
            this.UndoLimit = limit;
        }

        /// <summary>
        /// Gets the content with the specified data format.
        /// </summary>
        /// <param name="dataFormat">The data format.</param>
        /// <returns>The content with the specified data format.</returns>
        private string GetContent(string dataFormat)
        {
            TextRange textRange = new TextRange(this.Document.ContentStart, this.Document.ContentEnd);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                textRange.Save(memoryStream, dataFormat);
                memoryStream.Position = 0;
                return memoryStream.ReadAllText();
            }
        }

        /// <summary>
        /// Sets the content with the specified data format.
        /// </summary>
        /// <param name="dataFormat">The data format.</param>
        /// <param name="text">The text.</param>
        private void SetContent(string dataFormat, string text)
        {
            TextRange textRange = new TextRange(this.Document.ContentStart, this.Document.ContentEnd);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.WriteAllText(text);
                memoryStream.Position = 0;
                textRange.Load(memoryStream, dataFormat);
            }
        }

        /// <summary>
        /// Gets the RTF text.
        /// </summary>
        /// <returns>The RTF text.</returns>
        private string GetRtf()
        {
            return this.GetContent(DataFormats.Rtf);
        }

        /// <summary>
        /// Sets the RTF text.
        /// </summary>
        /// <param name="text">The RTF text.</param>
        private void SetRtf(string text)
        {
            this.SetContent(DataFormats.Rtf, text);
        }

        /// <summary>
        /// Gets the plain text.
        /// </summary>
        /// <returns>The plain text.</returns>
        private string GetText()
        {
            return new TextRange(this.Document.ContentStart, this.Document.ContentEnd).Text;
        }

        /// <summary>
        /// Sets the plain text.
        /// </summary>
        /// <param name="text">The plain text.</param>
        private void SetText(string text)
        {
            new TextRange(this.Document.ContentStart, this.Document.ContentEnd).Text = text;
        }

        /// <summary>
        /// Gets the XAML text.
        /// </summary>
        /// <returns>The XAML text.</returns>
        private string GetXaml()
        {
            return this.GetContent(DataFormats.Xaml);
        }

        /// <summary>
        /// Sets the XAML text.
        /// </summary>
        /// <param name="text">The XAML text.</param>
        private void SetXaml(string text)
        {
            this.SetContent(DataFormats.Xaml, text);
        }

        #endregion
    }
}
