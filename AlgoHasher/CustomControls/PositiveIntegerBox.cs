using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlgoHasher.CustomControls
{
    public class PositiveIntegerBox : TextBox
    {
        private bool _updatingFromValue;
        private static readonly Regex DigitsOnly = new Regex("^[0-9]+$", RegexOptions.Compiled);

        static PositiveIntegerBox()
        {
        }

        public PositiveIntegerBox()
        {
            DataObject.AddPastingHandler(this, OnPaste);
            PreviewTextInput += OnPreviewTextInput;
            PreviewKeyDown += OnPreviewKeyDown;
            TextChanged += OnTextChangedUpdateValue;

            // Disables IME to avoid non-Latin digit input via IMEs
            InputMethod.SetIsInputMethodEnabled(this, false);
        }

        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(int), typeof(PositiveIntegerBox),
                new FrameworkPropertyMetadata(1, OnRangeChanged, CoerceMinimum));

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(int), typeof(PositiveIntegerBox),
                new FrameworkPropertyMetadata(int.MaxValue, OnRangeChanged, CoerceMaximum));

        public int? Value
        {
            get => (int?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int?), typeof(PositiveIntegerBox),
                new FrameworkPropertyMetadata(null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged, CoerceValue));

        private static object CoerceMinimum(DependencyObject d, object baseValue)
        {
            var ctl = (PositiveIntegerBox)d;
            var min = Math.Max(1, (int)baseValue); // positive only
            if (min > ctl.Maximum) ctl.Maximum = min;
            return min;
        }

        private static object CoerceMaximum(DependencyObject d, object baseValue)
        {
            var ctl = (PositiveIntegerBox)d;
            var max = Math.Max(ctl.Minimum, (int)baseValue);
            return max;
        }

        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (PositiveIntegerBox)d;
            ctl.CoerceValue(ValueProperty);
            ctl.SyncTextWithValue();
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            var ctl = (PositiveIntegerBox)d;
            if (baseValue is int v)
            {
                if (v < ctl.Minimum) return ctl.Minimum;
                if (v > ctl.Maximum) return ctl.Maximum;
                return v;
            }
            return baseValue; // allow null
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctl = (PositiveIntegerBox)d;
            ctl.SyncTextWithValue();
        }

        private void SyncTextWithValue()
        {
            if (_updatingFromValue) return;
            try
            {
                _updatingFromValue = true;
                var desired = Value?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;
                if (!string.Equals(Text, desired, StringComparison.Ordinal))
                {
                    Text = desired;
                    CaretIndex = Text.Length;
                }
            }
            finally
            {
                _updatingFromValue = false;
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Only allow digits
            if (!DigitsOnly.IsMatch(e.Text))
            {
                e.Handled = true;
                return;
            }

            // Simulate resulting text and validate range and leading zero rule
            var proposed = GetProposedText(e.Text);
            if (!IsValidProposedText(proposed))
            {
                e.Handled = true;
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Allow control keys, navigation, editing
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab ||
                e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Home || e.Key == Key.End)
                return;

            // Block space
            if (e.Key == Key.Space)
            {
                e.Handled = true;
                return;
            }

            // Allow Ctrl combos (Copy/Cut/Paste/SelectAll); paste is validated separately
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                return;
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }
            var text = e.SourceDataObject.GetData(DataFormats.Text) as string ?? string.Empty;
            var proposed = GetProposedText(text);
            if (!IsValidProposedText(proposed))
            {
                e.CancelCommand();
            }
        }

        private void OnTextChangedUpdateValue(object sender, TextChangedEventArgs e)
        {
            if (_updatingFromValue) return;

            // Normalize and coerce
            if (string.IsNullOrEmpty(Text))
            {
                // Keep Value = null when empty; you can change to Minimum if you prefer
                SetCurrentValue(ValueProperty, null);
                return;
            }

            if (!int.TryParse(Text, NumberStyles.None, CultureInfo.InvariantCulture, out var parsed))
            {
                // Revert to last valid Value
                SyncTextWithValue();
                return;
            }

            // Coerce to range and push back if needed
            if (parsed < Minimum) parsed = Minimum;
            if (parsed > Maximum) parsed = Maximum;

            if (Value != parsed)
            {
                SetCurrentValue(ValueProperty, parsed);
            }

            // Remove leading zeros (e.g., "007" -> "7")
            var normalized = parsed.ToString(CultureInfo.InvariantCulture);
            if (!string.Equals(Text, normalized, StringComparison.Ordinal))
            {
                var caret = CaretIndex;
                Text = normalized;
                CaretIndex = Math.Min(caret, Text.Length);
            }
        }

        private string GetProposedText(string incoming)
        {
            // Simulate what the text would be after this input (typing or paste)
            var selectionStart = SelectionStart;
            var selectionLength = SelectionLength;

            var before = Text?.Substring(0, selectionStart) ?? string.Empty;
            var after = Text is { Length: > 0 } t && selectionStart + selectionLength <= t.Length
                ? t.Substring(selectionStart + selectionLength)
                : string.Empty;

            return before + incoming + after;
        }

        private bool IsValidProposedText(string proposed)
        {
            if (string.IsNullOrEmpty(proposed))
                return true; // allow clearing; Value becomes null

            if (!DigitsOnly.IsMatch(proposed))
                return false;

            // No leading zeros for positive numbers (except single "0" if Minimum == 0; here Minimum >= 1 by default)
            if (proposed.Length > 1 && proposed.StartsWith("0", StringComparison.Ordinal))
                return false;

            if (!int.TryParse(proposed, NumberStyles.None, CultureInfo.InvariantCulture, out var v))
                return false;

            // Enforce range
            return v >= Minimum && v <= Maximum;
        }
    }
}
