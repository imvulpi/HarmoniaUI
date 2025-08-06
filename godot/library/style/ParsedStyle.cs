using Godot;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.core.types.parser;
using HarmoniaUI.library.style.interfaces;
using System;

namespace HarmoniaUI.library.style
{
    /// <summary>
    /// Style of which all values are pixels - parsed <see cref="StyleResource"/>
    /// </summary>
    /// <remarks>
    /// Needs to be updated/reparsed when viewport size changes
    /// </remarks>
    public class ParsedStyle : IStyle<StyleValue, StyleSides<StyleValue>>
    {
        public event Action<StyleChangeType> Changed;

        #region Behaviour

        /// <inheritdoc cref="Visibility"/>
        private VisibilityType _visibility;

        public VisibilityType Visibility { get => _visibility; set => Set(ref _visibility, value, StyleChangeType.Other); }

        #endregion

        #region Size

        /// <inheritdoc cref="SizingType"/>
        private SizingType _sizingType;

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

        public SizingType SizingType { get => _sizingType; set => Set(ref _sizingType, value, StyleChangeType.Relayout); }
        public StyleValue Width { get => _width; set => Set(ref _width, value, StyleChangeType.Relayout); }
        public StyleValue Height { get => _height; set => Set(ref _height, value, StyleChangeType.Relayout); }
        public StyleValue MinWidth { get => _minWidth; set => Set(ref _minWidth, value, StyleChangeType.Relayout); }
        public StyleValue MinHeight { get => _minHeight; set => Set(ref _minHeight, value, StyleChangeType.Relayout); }
        public StyleValue MaxWidth { get => _maxWidth; set => Set(ref _maxWidth, value, StyleChangeType.Relayout); }
        public StyleValue MaxHeight { get => _maxHeight; set => Set(ref _maxHeight, value, StyleChangeType.Relayout); }

        #endregion

        #region Spacing

        /// <inheritdoc cref="Padding"/>
        private StyleSides<StyleValue> _padding;

        /// <inheritdoc cref="Margin"/>
        private StyleSides<StyleValue> _margin;

        public StyleSides<StyleValue> Padding { get => _padding; set => Set(ref _padding, value, StyleChangeType.Relayout); }
        public StyleSides<StyleValue> Margin { get => _margin; set => Set(ref _margin, value, StyleChangeType.Relayout); }

        #endregion

        #region Background

        /// <inheritdoc cref="BackgroundColor" />
        private Color _backgroundColor;

        public Color BackgroundColor { get => _backgroundColor; set => Set(ref _backgroundColor, value, StyleChangeType.Redraw); }

        #endregion

        #region Border

        /// <inheritdoc cref="BorderRadius"/>
        private StyleSides<StyleValue> _borderRadius;
        /// <inheritdoc cref="BorderWidth"/>
        private StyleSides<StyleValue> _borderWidth;
        /// <inheritdoc cref="BorderColor"/>
        private Color _borderColor;

        public StyleSides<StyleValue> BorderRadius { get => _borderRadius; set => Set(ref _borderRadius, value, StyleChangeType.Redraw | StyleChangeType.Relayout); }

        public StyleSides<StyleValue> BorderWidth { get => _borderWidth; set => Set(ref _borderWidth, value, StyleChangeType.Redraw | StyleChangeType.Relayout); }

        public Color BorderColor { get => _borderColor; set => Set(ref _borderColor, value, StyleChangeType.Redraw); }

        #endregion

        #region Shadow

        /// <inheritdoc cref="ShadowColor"/>
        private Color _shadowColor;
        /// <inheritdoc cref="ShadowOffset"/>
        private Vector2 _shadowOffset;

        public Color ShadowColor { get => _shadowColor; set => Set(ref _shadowColor, value, StyleChangeType.Redraw); }
        public Vector2 ShadowOffset { get => _shadowOffset; set => Set(ref _shadowOffset, value, StyleChangeType.Redraw | StyleChangeType.Relayout); }

        #endregion

        #region Position

        /// <inheritdoc cref="PositioningType" />
        private PositionType _positioningType;

        /// <inheritdoc cref="PositionX" />
        private StyleValue _positionX;

        /// <inheritdoc cref="PositionY" />
        private StyleValue _positionY;

        public PositionType PositioningType { get => _positioningType; set => Set(ref _positioningType, value, StyleChangeType.Relayout); }
        public StyleValue PositionX { get => _positionX; set => Set(ref _positionX, value, StyleChangeType.Relayout); }
        public StyleValue PositionY { get => _positionY; set => Set(ref _positionY, value, StyleChangeType.Relayout); }

        #endregion

        /// <summary>
        /// Updates all fields by parsing the <paramref name="rawStyle"/>
        /// </summary>
        /// <param name="rawStyle">Raw style resource</param>
        /// <param name="viewportSize">Size of the viewport (width, height)</param>
        /// <param name="parentSize">Size of the parent content (width, height)</param>
        public static ParsedStyle From(StyleResource rawStyle)
        {
            return new() {
                SizingType = rawStyle.SizingType,
                Visibility = rawStyle.Visibility,
                Width = StyleValueParser.ParseValue(rawStyle.Width),
                Height = StyleValueParser.ParseValue(rawStyle.Height),
                MinWidth = StyleValueParser.ParseValue(rawStyle.MinWidth),
                MinHeight = StyleValueParser.ParseValue(rawStyle.MinHeight),
                MaxWidth = StyleValueParser.ParseValue(rawStyle.MaxWidth),
                MaxHeight = StyleValueParser.ParseValue(rawStyle.MaxHeight),
                Padding = StyleValueParser.ParseSides(rawStyle.Padding),
                Margin = StyleValueParser.ParseSides(rawStyle.Margin),
                BackgroundColor = rawStyle.BackgroundColor,
                BorderColor = rawStyle.BorderColor,
                BorderRadius = StyleValueParser.ParseSides(rawStyle.BorderRadius),
                BorderWidth = StyleValueParser.ParseSides(rawStyle.BorderWidth),
                ShadowColor = rawStyle.ShadowColor,
                ShadowOffset = rawStyle.ShadowOffset,
                PositioningType = rawStyle.PositioningType,
                PositionX = StyleValueParser.ParseValue(rawStyle.PositionX),
                PositionY = StyleValueParser.ParseValue(rawStyle.PositionY)
            };
        }

        /// <summary>
        /// Sets a parameter to a value if they are not equal. Invokes an event that the style changed.
        /// </summary>
        /// <typeparam name="T">Type of the value and parameter</typeparam>
        /// <param name="parameter">Parameter to change</param>
        /// <param name="value">Value of which to change</param>
        /// <param name="styleChanged">Style that was changed</param>
        private void Set<T>(ref T parameter, T value, StyleChangeType styleChanged)
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
