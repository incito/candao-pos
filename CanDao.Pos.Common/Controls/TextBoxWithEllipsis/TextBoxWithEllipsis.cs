using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CanDao.Pos.Common.Controls
{
    public class TextBoxWithEllipsis : TextBox
    {
        public TextBoxWithEllipsis()
        {
            // Initialize inherited stuff as desired.
            IsReadOnlyCaretVisible = true;

            // Initialize stuff added by this class
            IsEllipsisEnabled = true;
            UseLongTextForToolTip = true;
            _internalEnabled = true;

            LayoutUpdated += TextBoxWithEllipsis_LayoutUpdated;
            //SizeChanged += TextBoxWithEllipsis_SizeChanged;

            BorderThickness = new Thickness(0);
            Background = Brushes.Transparent;
            IsEnabled = false;
        }

        /// <summary>
        /// The underlying text that gets truncated with ellipsis if it doesn't fit.
        /// Setting this and setting Text has the same effect, but getting Text may
        /// get a truncated version of LongText.
        /// </summary>
        public string LongText
        {
            get { return _longText; }

            set
            {
                _longText = value ?? "";
                ToolTip = _longText;
                PrepareForLayout();
            }
        }

        /// <summary>
        /// If true, Text/LongText will be truncated with ellipsis
        /// to fit in the visible area of the TextBox
        /// (except when it has the focus).
        /// </summary>
        public bool IsEllipsisEnabled
        {
            get { return _externalEnabled; }

            set
            {
                _externalEnabled = value;
                PrepareForLayout();

                if (_DoEllipsis)
                {
                    // Since we didn't change Text or Size, layout wasn't performed 
                    // as a side effect.  Pretend that it was.
                    TextBoxWithEllipsis_LayoutUpdated(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// If true, ToolTip will be set to LongText whenever
        /// LongText doesn't fit in the visible area.  
        /// If false, ToolTip will be set to null unless
        /// the user sets it to something other than LongText.
        /// </summary>
        public bool UseLongTextForToolTip
        {
            get { return _useLongTextForToolTip; }

            set
            {
                if (_useLongTextForToolTip != value)
                {
                    _useLongTextForToolTip = value;

                    if (value)
                    {
                        // When turning it on, set ToolTip to
                        // _longText if the current Text is too long.
                        if (ExtentWidth > ViewportWidth ||
                            Text != _longText)
                        {
                            ToolTip = _longText;
                        }
                    }
                    else
                    {
                        // When turning it off, set ToolTip to null
                        // unless user has set it to something other
                        // than _longText;
                        if (_longText.Equals(ToolTip))
                        {
                            ToolTip = null;
                        }
                    }
                }
            }
        }

        // Last length of substring of LongText known to fit.
        // Used while calculating the correct length to fit.
        private int lastFitLen = 0;

        // Last length of substring of LongText known to be too long.
        // Used while calculating the correct length to fit.
        private int lastLongLen;

        // Length of substring of LongText currently assigned to the Text property.
        // Used while calculating the correct length to fit.
        private int curLen;

        // Used to detect whether the OnTextChanged event occurs due to an
        // external change vs. an internal one.
        private bool _externalChange = true;

        // Used to disable ellipsis internally (primarily while
        // the control has the focus).
        private bool _internalEnabled = true;

        // Backer for LongText.
        private string _longText = "";

        // Backer for IsEllipsisEnabled
        private bool _externalEnabled = true;

        // Backer for UseLongTextForToolTip.
        private bool _useLongTextForToolTip;

        // OnTextChanged is overridden so we can avoid 
        // raising the TextChanged event when we change 
        // the Text property internally while searching 
        // for the longest substring that fits.
        // If Text is changed externally, we copy the
        // new Text into LongText before we overwrite Text 
        // with the truncated version (if IsEllipsisEnabled).
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_externalChange)
            {
                _longText = Text ?? "";
                if (UseLongTextForToolTip) ToolTip = _longText;
                PrepareForLayout();
                base.OnTextChanged(e);
            }
        }

        // Makes the entire text available for editing, selecting, and scrolling
        // until focus is lost.
        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            _internalEnabled = false;
            SetText(_longText);
            base.OnGotKeyboardFocus(e);
        }

        // Returns to trimming and showing ellipsis.
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            _internalEnabled = true;
            PrepareForLayout();
            base.OnLostKeyboardFocus(e);
        }

        // Sets the Text property without raising the TextChanged event.
        private void SetText(string text)
        {
            if (Text != text)
            {
                _externalChange = false;
                Text = text; // Will trigger Layout event.
                _externalChange = true;
            }
        }

        // Arranges for the next LayoutUpdated event to trim _longText and add ellipsis.
        // Also triggers layout by setting Text.
        private void PrepareForLayout()
        {
            lastFitLen = 0;
            lastLongLen = _longText.Length;
            curLen = _longText.Length;
            SetText(_longText);
        }

        private void TextBoxWithEllipsis_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_DoEllipsis && e.NewSize.Width != e.PreviousSize.Width)
            {
                // We need to recalculate the longest substring of LongText that will fit (with ellipsis).
                // Prepare for the LayoutUpdated event, which does the recalc and is raised after this.
                PrepareForLayout();
            }
        }

        private bool _DoEllipsis { get { return IsEllipsisEnabled && _internalEnabled; } }

        // Called when Text or Size changes (and maybe at other times we don't care about).
        private void TextBoxWithEllipsis_LayoutUpdated(object sender, EventArgs e)
        {
            if (_DoEllipsis)
            {
                // This does a binary search (bisection) to determine the maximum substring
                // of _longText that will fit in visible area.  Instead of a loop, it
                // uses a type of recursion that happens because this event is raised
                // again if we set the Text property in here.

                if (ExtentWidth - ViewportWidth > 0.000000001)
                {
                    // The current Text (whose length without ellipsis is curLen) is too long.
                    lastLongLen = curLen;
                }
                else
                {
                    // The current Text is not too long.
                    lastFitLen = curLen;
                }

                // Try a new substring whose length is halfway between the last length
                // known to fit and the last length known to be too long.
                int newLen = (lastFitLen + lastLongLen) / 2;

                if (curLen == newLen)
                {
                    // We're done! Usually, _lastLongLen is _lastFitLen + 1.

                    if (UseLongTextForToolTip)
                    {
                        ToolTip = Text == _longText ? null : _longText;
                    }
                }
                else
                {
                    curLen = newLen;

                    // This sets the Text property without raising the TextChanged event.
                    // However it does raise the LayoutUpdated event again, though
                    // not recursively.
                    SetText(_longText.Substring(0, curLen) + "\u2026");
                }
            }
            else if (UseLongTextForToolTip)
            {
                ToolTip = ViewportWidth < ExtentWidth ? _longText : null;
            }
        }
    }
}