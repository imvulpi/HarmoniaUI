using Godot;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Core.Style.Interfaces;
using System;

namespace HarmoniaUI.Core.Style.Parsed
{
    /// <summary>
    /// Represents the parsed style for harmonia nodes after processing raw style data.
    /// </summary>
    /// <remarks>
    /// <see cref="ParsedStyle"/> is created by interpreting and validating the values
    /// from a <see cref="Raw.StyleResource"/>, converting them into strongly typed properties
    /// that can be easily computed to pixel values
    /// <para>
    /// Parsing does not convert values to pixels, nor does it apply inheritance, 
    /// defaults, or cascading rules - those are handled later when producing the 
    /// <see cref="Computed.ComputedStyle"/>.
    /// </para>
    /// 
    /// <para>
    /// Parsing is done to improve performance in recomputations, since there are different <see cref="Unit"/>s,
    /// parsing keeps it clean and fast to check and iterate over, when converting to pixels.
    /// </para>
    /// </remarks>
    public class ParsedStyle : IStyle<StyleValue, StyleSides<StyleValue>>
    {
        /// <summary>
        /// An event that triggers whenether any property changes,
        /// <see cref="UINodeAction"/> suggests what action the node should perform
        /// </summary>
        public event Action<UINodeAction> Changed;

        #region Behaviour

        /// <inheritdoc cref="Visibility"/>
        private VisibilityType _visibility = VisibilityType.Unset;

        public VisibilityType Visibility { get => _visibility; set => Set(ref _visibility, value, UINodeAction.Redraw | UINodeAction.Relayout); }

        #endregion

        #region Size

        /// <inheritdoc cref="SizingType"/>
        private SizingType _sizingType = SizingType.Unset;

        /// <inheritdoc cref="Width"/>
        private StyleValue _width;

        /// <inheritdoc cref="Height"/>
        private StyleValue _height;

        /// <inheritdoc cref="MinHeight"/>
        private StyleValue _minHeight;
        
        /// <inheritdoc cref="MinWidth"/>
        private StyleValue _minWidth;
        
        /// <inheritdoc cref="MaxHeight"/>
        private StyleValue _maxHeight;

        /// <inheritdoc cref="MaxWidth"/>
        private StyleValue _maxWidth;

        public SizingType SizingType { get => _sizingType; set => Set(ref _sizingType, value, UINodeAction.Relayout); }
        public StyleValue Width { get => _width; set => Set(ref _width, value, UINodeAction.Relayout); }
        public StyleValue Height { get => _height; set => Set(ref _height, value, UINodeAction.Relayout); }
        public StyleValue MinWidth { get => _minWidth; set => Set(ref _minWidth, value, UINodeAction.Relayout); }
        public StyleValue MinHeight { get => _minHeight; set => Set(ref _minHeight, value, UINodeAction.Relayout); }
        public StyleValue MaxWidth { get => _maxWidth; set => Set(ref _maxWidth, value, UINodeAction.Relayout); }
        public StyleValue MaxHeight { get => _maxHeight; set => Set(ref _maxHeight, value, UINodeAction.Relayout); }

        #endregion

        #region Spacing

        /// <inheritdoc cref="Padding"/>
        private StyleSides<StyleValue> _padding;

        /// <inheritdoc cref="Margin"/>
        private StyleSides<StyleValue> _margin;

        public StyleSides<StyleValue> Padding { get => _padding; set => Set(ref _padding, value, UINodeAction.Relayout); }
        public StyleSides<StyleValue> Margin { get => _margin; set => Set(ref _margin, value, UINodeAction.Relayout); }

        #endregion

        #region Background

        /// <inheritdoc cref="BackgroundColor" />
        private Color _backgroundColor = StyleDefaults.UnsetColor;

        public Color BackgroundColor { get => _backgroundColor; set => Set(ref _backgroundColor, value, UINodeAction.Redraw); }

        #endregion

        #region Border

        /// <inheritdoc cref="BorderRadius"/>
        private StyleSides<StyleValue> _borderRadius;
        /// <inheritdoc cref="BorderWidth"/>
        private StyleSides<StyleValue> _borderWidth;
        /// <inheritdoc cref="BorderColor"/>
        private Color _borderColor = StyleDefaults.UnsetColor;

        public StyleSides<StyleValue> BorderRadius { get => _borderRadius; set => Set(ref _borderRadius, value, UINodeAction.Redraw | UINodeAction.Relayout); }

        public StyleSides<StyleValue> BorderWidth { get => _borderWidth; set => Set(ref _borderWidth, value, UINodeAction.Redraw | UINodeAction.Relayout); }

        public Color BorderColor { get => _borderColor; set => Set(ref _borderColor, value, UINodeAction.Redraw); }

        #endregion

        #region Shadow

        /// <inheritdoc cref="ShadowColor"/>
        private Color _shadowColor = StyleDefaults.UnsetColor;
        /// <inheritdoc cref="ShadowOffsetX"/>
        private StyleValue _shadowOffsetX;
        /// <inheritdoc cref="ShadowOffsetY"/>
        private StyleValue _shadowOffsetY;

        public Color ShadowColor { get => _shadowColor; set => Set(ref _shadowColor, value, UINodeAction.Redraw); }
        public StyleValue ShadowOffsetX { get => _shadowOffsetX; set => Set(ref _shadowOffsetX, value, UINodeAction.Redraw); }
        public StyleValue ShadowOffsetY { get => _shadowOffsetY; set => Set(ref _shadowOffsetY, value, UINodeAction.Redraw); }

        #endregion

        #region Position

        /// <inheritdoc cref="PositioningType" />
        private PositionType _positioningType = PositionType.Unset;

        /// <inheritdoc cref="PositionX" />
        private StyleValue _positionX;

        /// <inheritdoc cref="PositionY" />
        private StyleValue _positionY;

        public PositionType PositioningType { get => _positioningType; set => Set(ref _positioningType, value, UINodeAction.Relayout); }
        public StyleValue PositionX { get => _positionX; set => Set(ref _positionX, value, UINodeAction.Relayout); }
        public StyleValue PositionY { get => _positionY; set => Set(ref _positionY, value, UINodeAction.Relayout); }

        #endregion

        /// <summary>
        /// Sets a parameter to a value if they are not equal. Invokes an event that the style changed.
        /// </summary>
        /// <typeparam name="T">Type of the value and parameter</typeparam>
        /// <param name="parameter">Parameter to change</param>
        /// <param name="value">Value of which to change</param>
        /// <param name="styleChanged">Style that was changed</param>
        private void Set<T>(ref T parameter, T value, UINodeAction styleChanged)
        {
            if (parameter.Equals(value))
            {
                return;
            }

            parameter = value;
            Changed?.Invoke(styleChanged);
        }
    }
}
