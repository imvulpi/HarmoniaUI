using Godot;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.core.types.parser;
using System;

namespace HarmoniaUI.Library.style
{
    /// <summary>
    /// Style of which all values are pixels - parsed <see cref="StyleResource"/>
    /// </summary>
    /// <remarks>
    /// Needs to be updated/reparsed when viewport size changes
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
        private int _width;

        /// <inheritdoc cref="Height"/>
        private int _height;

        /// <inheritdoc cref="MinHeight"/>
        private int _minHeight;
        
        /// <inheritdoc cref="MinWidth"/>
        private int _minWidth;
        
        /// <inheritdoc cref="MaxHeight"/>
        private int _maxHeight;

        /// <inheritdoc cref="MaxWidth"/>
        private int _maxWidth;

        /// <summary>
        /// Sizing type of the element.
        /// </summary>
        public SizingType SizingType { get => _sizingType; set => Set(ref _sizingType, value, StyleChangeType.Size); }

        /// <summary>
        /// Width of the element
        /// </summary>
        public int Width { get => _width; set => Set(ref _width, value, StyleChangeType.Size); }

        /// <summary>
        /// Height of the element
        /// </summary>
        public int Height { get => _height; set => Set(ref _height, value, StyleChangeType.Size); }

        /// <summary>
        /// Minimum width of the element.
        /// </summary>
        public int MinWidth { get => _minWidth; set => Set(ref _minWidth, value, StyleChangeType.Size); }

        /// <summary>
        /// Minimum height of the element.
        /// </summary>
        public int MinHeight { get => _minHeight; set => Set(ref _minHeight, value, StyleChangeType.Size); }

        /// <summary>
        /// Maximum width of the element.
        /// </summary>
        public int MaxWidth { get => _maxWidth; set => Set(ref _maxWidth, value, StyleChangeType.Size); }

        /// <summary>
        /// Maximum height of the element.
        /// </summary>
        public int MaxHeight { get => _maxHeight; set => Set(ref _maxHeight, value, StyleChangeType.Size); }

        #endregion

        #region Spacing

        /// <inheritdoc cref="Padding"/>
        private StyleSides _padding;

        /// <inheritdoc cref="Margin"/>
        private StyleSides _margin;

        /// <summary>
        /// Padding inside the element (space between content and border).
        /// </summary>
        public StyleSides Padding { get => _padding; set => Set(ref _padding, value, StyleChangeType.Size); }

        /// <summary>
        /// Margin outside the element (space between element and siblings).
        /// </summary>
        public StyleSides Margin { get => _margin; set => Set(ref _margin, value, StyleChangeType.Size); }

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
        private StyleSides _borderRadius;
        /// <inheritdoc cref="BorderWidth"/>
        private StyleSides _borderWidth;
        /// <inheritdoc cref="BorderColor"/>
        private Color _borderColor;

        /// <summary>
        /// Radius for rounding corners (e.g. "8px", "50%").
        /// </summary>
        public StyleSides BorderRadius { get => _borderRadius; set => Set(ref _borderRadius, value, StyleChangeType.Border); }

        /// <summary>
        /// Width of the border (e.g. "2px").
        /// </summary>
        public StyleSides BorderWidth { get => _borderWidth; set => Set(ref _borderWidth, value, StyleChangeType.Border); }

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
        private int _positionX;

        /// <inheritdoc cref="PositionY" />
        private int _positionY;

        /// <summary>
        /// Positioning mode: Normal (flow), Relative (offset), or Absolute (manual).
        /// </summary>
        public PositionType PositioningType { get => _positioningType; set => Set(ref _positioningType, value, StyleChangeType.Position); }

        /// <summary>
        /// Horizontal position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        public int PositionX { get => _positionX; set => Set(ref _positionX, value, StyleChangeType.Position); }

        /// <summary>
        /// Vertical position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        public int PositionY { get => _positionY; set => Set(ref _positionY, value, StyleChangeType.Position); }

        #endregion

        /// <summary>
        /// Updates all fields by parsing the <paramref name="rawStyle"/>
        /// </summary>
        /// <param name="rawStyle">Raw style resource</param>
        /// <param name="viewportSize">Size of the viewport (width, height)</param>
        /// <param name="parentSize">Size of the parent content (width, height)</param>
        public void Update(StyleResource rawStyle, Vector2 viewportSize, Vector2 parentSize)
        {
            SizingType = rawStyle.SizingType;
            Visibility = rawStyle.Visibility;
            Width = StyleValueParser.Parse(rawStyle.Width, viewportSize, parentSize.X);
            Height = StyleValueParser.Parse(rawStyle.Height, viewportSize, parentSize.Y);
            MinWidth = StyleValueParser.Parse(rawStyle.MinWidth, viewportSize, parentSize.X);
            MinHeight = StyleValueParser.Parse(rawStyle.MinHeight, viewportSize, parentSize.Y);
            MaxWidth = StyleValueParser.Parse(rawStyle.MaxWidth, viewportSize, parentSize.X);
            MaxHeight = StyleValueParser.Parse(rawStyle.MaxHeight, viewportSize, parentSize.Y);
            Padding = StyleValueParser.ParseSides(rawStyle.Padding, viewportSize, parentSize);
            Margin = StyleValueParser.ParseSides(rawStyle.Margin, viewportSize, parentSize);
            BackgroundColor = rawStyle.BackgroundColor;
            BorderColor = rawStyle.BorderColor;
            BorderRadius = StyleValueParser.ParseSides(rawStyle.BorderRadius, viewportSize, parentSize);
            BorderWidth = StyleValueParser.ParseSides(rawStyle.BorderWidth, viewportSize, parentSize);
            ShadowColor = rawStyle.ShadowColor;
            ShadowOffset = rawStyle.ShadowOffset;
            PositioningType = rawStyle.PositioningType;
            PositionX = StyleValueParser.Parse(rawStyle.PositionX, viewportSize, parentSize.X);
            PositionY = StyleValueParser.Parse(rawStyle.PositionY, viewportSize, parentSize.Y);

            // Maybe parser? \/
            if(BorderWidth.Left == UnitConstants.AUTO)
            {
                BorderWidth = new StyleSides(0);
            }

            if (BorderRadius.Left == UnitConstants.AUTO)
            {
                BorderRadius = new StyleSides(0);
            }
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
