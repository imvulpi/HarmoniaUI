using Godot;
using HarmoniaUI.Library.style.parsing;
using System;

namespace HarmoniaUI.Library.style
{
    /// <summary>
    /// Base style for harmonia nodes, parsed version of <see cref="StyleResource"/>
    /// </summary>
    /// <remarks>
    /// Describes the visual and layout style for Harmonia nodes
    /// </remarks>
    public class ParsedStyle
    {
        #region Events

        public event Action<StyleChangeType> Changed;

        #endregion

        #region Behaviour

        /// <inheritdoc cref="Visibility"/>
        private VisibilityType _visibility;

        /// <summary>
        /// Defines visibility of the element
        /// </summary>
        public VisibilityType Visibility { get => _visibility; set => Set(ref _visibility, value, StyleChangeType.Visibility); }

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

        /// <summary>
        /// Sizing type of the element.
        /// </summary>
        public SizingType SizingType { get => _sizingType; set => Set(ref _sizingType, value, StyleChangeType.Layout); }

        /// <summary>
        /// Width of the element (e.g. "100px", "50%", , leave empty for auto).
        /// </summary>
        public StyleValue Width { get => _width; set => Set(ref _width, value, StyleChangeType.Layout); }

        /// <summary>
        /// Height of the element (e.g. "100px", "50%", , leave empty for auto).
        /// </summary>
        public StyleValue Height { get => _height; set => Set(ref _height, value, StyleChangeType.Layout); }

        /// <summary>
        /// Minimum width of the element.
        /// </summary>
        public StyleValue MinWidth { get => _minWidth; set => Set(ref _minWidth, value, StyleChangeType.Layout); }

        /// <summary>
        /// Minimum height of the element.
        /// </summary>
        public StyleValue MinHeight { get => _minHeight; set => Set(ref _minHeight, value, StyleChangeType.Layout); }

        /// <summary>
        /// Maximum width of the element.
        /// </summary>
        public StyleValue MaxWidth { get => MaxWidth; set => Set(ref _maxWidth, value, StyleChangeType.Layout); }

        /// <summary>
        /// Maximum height of the element.
        /// </summary>
        public StyleValue MaxHeight { get => MaxHeight; set => Set(ref _maxHeight, value, StyleChangeType.Layout); }

        #endregion

        #region Spacing

        /// <inheritdoc cref="Padding"/>
        private RectSides _padding;

        /// <inheritdoc cref="Margin"/>
        private RectSides _margin;

        /// <summary>
        /// Padding inside the element (space between content and border).
        /// </summary>
        public RectSides Padding { get => _padding; set => Set(ref _padding, value, StyleChangeType.Layout); }

        /// <summary>
        /// Margin outside the element (space between element and siblings).
        /// </summary>
        public RectSides Margin { get => _margin; set => Set(ref _margin, value, StyleChangeType.Layout); }

        #endregion

        #region Background

        /// <inheritdoc cref="BackgroundColor" />
        private Color _backgroundColor;

        /// <summary>
        /// Background color of the element.
        /// </summary>
        public Color BackgroundColor { get => _backgroundColor; set => Set(ref _backgroundColor, value, StyleChangeType.Background); }

        #endregion

        #region Border

        /// <inheritdoc cref="BorderRadius"/>
        private RectSides _borderRadius;
        /// <inheritdoc cref="BorderWidth"/>
        private StyleValue _borderWidth;
        /// <inheritdoc cref="BorderColor"/>
        private Color _borderColor;

        /// <summary>
        /// Radius for rounding corners (e.g. "8px", "50%").
        /// </summary>
        public RectSides BorderRadius { get => _borderRadius; set => Set(ref _borderRadius, value, StyleChangeType.Border); }

        /// <summary>
        /// Width of the border (e.g. "2px").
        /// </summary>
        public StyleValue BorderWidth { get => _borderWidth; set => Set(ref _borderWidth, value, StyleChangeType.Border); }

        /// <summary>
        /// Color of the border.
        /// </summary>
        public Color BorderColor { get => _borderColor; set => Set(ref _borderColor, value, StyleChangeType.Border); }

        #endregion

        #region Shadow

        /// <inheritdoc cref="ShadowColor"/>
        private Color _shadowColor;

        /// <inheritdoc cref="ShadowOffset"/>
        private Vector2 _shadowOffset;

        /// <summary>
        /// Color of the box shadow.
        /// </summary>
        public Color ShadowColor { get => _shadowColor; set => Set(ref _shadowColor, value, StyleChangeType.Shadow); }

        /// <summary>
        /// Offset for the shadow in local pixels (x, y).
        /// </summary>
        public Vector2 ShadowOffset { get => _shadowOffset; set => Set(ref _shadowOffset, value, StyleChangeType.Shadow); }

        #endregion

        #region Position

        /// <inheritdoc cref="PositioningType" />
        private PositionType _positioningType;

        /// <inheritdoc cref="PositionX" />
        private StyleValue _positionX;

        /// <inheritdoc cref="PositionY" />
        private StyleValue _positionY;

        /// <summary>
        /// Positioning mode: Normal (flow), Relative (offset), or Absolute (manual).
        /// </summary>
        public PositionType PositioningType { get => _positioningType; set => Set(ref _positioningType, value, StyleChangeType.Positioning); }

        /// <summary>
        /// Horizontal position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        public StyleValue PositionX { get => _positionX; set => Set(ref _positionX, value, StyleChangeType.Positioning); }

        /// <summary>
        /// Vertical position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        public StyleValue PositionY { get => _positionY; set => Set(ref _positionY, value, StyleChangeType.Positioning); }

        #endregion

        /// <summary>
        /// Parses <see cref="StyleResource"/> and returns the result.
        /// </summary>
        /// <param name="rawStyle">Unparsed style</param>
        /// <returns>Parsed style</returns>
        public static ParsedStyle From(StyleResource rawStyle)
        {
            return new ParsedStyle()
            {
                Width = StyleValueParser.Parse(rawStyle.Width),
                Height = StyleValueParser.Parse(rawStyle.Height),
                MinWidth = StyleValueParser.Parse(rawStyle.MinWidth),
                MinHeight = StyleValueParser.Parse(rawStyle.MinHeight),
                MaxWidth = StyleValueParser.Parse(rawStyle.MaxWidth),
                MaxHeight = StyleValueParser.Parse(rawStyle.MaxHeight),
                Padding = StyleValueParser.ParseRect(rawStyle.Padding),
                Margin = StyleValueParser.ParseRect(rawStyle.Margin),
                BackgroundColor = rawStyle.BackgroundColor,
                BorderColor = rawStyle.BorderColor,
                BorderRadius = StyleValueParser.ParseRect(rawStyle.BorderRadius),
                BorderWidth = StyleValueParser.Parse(rawStyle.BorderWidth),
                ShadowColor = rawStyle.ShadowColor,
                ShadowOffset = rawStyle.ShadowOffset,
                PositioningType = rawStyle.PositioningType,
                PositionX = StyleValueParser.Parse(rawStyle.PositionX),
                PositionY = StyleValueParser.Parse(rawStyle.PositionY)
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
