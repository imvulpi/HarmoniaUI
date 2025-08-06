using Godot;
using HarmoniaUI.library.core.commons;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.nodes;
using HarmoniaUI.library.style.interfaces;

namespace HarmoniaUI.library.style
{
    /// <summary>
    /// Computed style with pixel values. Usually computed from parsed styles.
    /// </summary>
    public class ComputedStyle : IStyle<float, StyleSides<float>>
    {
        public VisibilityType Visibility { get; set; }
        public SizingType SizingType { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float MinWidth { get; set; }
        public float MinHeight { get; set; }
        public float MaxWidth { get; set; }
        public float MaxHeight { get; set; }
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Color ShadowColor { get; set; }
        public Vector2 ShadowOffset { get; set; }
        public PositionType PositioningType { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public StyleSides<float> BorderRadius { get; set; }
        public StyleSides<float> BorderWidth { get; set; }
        public StyleSides<float> Padding { get; set; }
        public StyleSides<float> Margin { get; set; }

        /// <summary>
        /// Updates current instance from parsed style and using node as a reference for values
        /// </summary>
        /// <param name="style">Parsed style to compute</param>
        /// <param name="node">Node used as reference for % values</param>
        public void UpdateFrom(ParsedStyle style, UINode node)
        {
            Vector2 viewportSize = ViewportHelper.GetViewportSize(node);
            Vector2 parentSize = node.Parent == null ? viewportSize : new(node.Parent.ContentWidth, node.Parent.ContentHeight);

            SizingType = style.SizingType;
            Visibility = style.Visibility;
            Width = style.Width.GetPixel(viewportSize, parentSize, parentSize.X, parentSize.X);
            Height = style.Height.GetPixel(viewportSize, parentSize, parentSize.Y, parentSize.Y);
            MinWidth = style.MinWidth.GetPixel(viewportSize, parentSize, parentSize.X, 0);
            MinHeight = style.MinHeight.GetPixel(viewportSize, parentSize, parentSize.X, 0);
            MaxWidth = style.MaxWidth.GetPixel(viewportSize, parentSize, parentSize.X, float.MaxValue);
            MaxHeight = style.MaxHeight.GetPixel(viewportSize, parentSize, parentSize.X, float.MaxValue);
            Padding = GetSides(style.Padding, viewportSize, parentSize);
            Margin = GetSides(style.Margin, viewportSize, parentSize); ;
            BackgroundColor = style.BackgroundColor;
            BorderColor = style.BorderColor;
            BorderRadius = GetSides(style.BorderRadius, viewportSize, parentSize);
            BorderWidth = GetSides(style.BorderWidth, viewportSize, parentSize);
            ShadowColor = style.ShadowColor;
            ShadowOffset = style.ShadowOffset;
            PositioningType = style.PositioningType;
            PositionX = style.PositionX.GetPixel(viewportSize, parentSize, parentSize.X, 0);
            PositionY = style.PositionY.GetPixel(viewportSize, parentSize, parentSize.Y, 0);
        }

        /// <summary>
        /// Computes parsed style sides from to float sides
        /// </summary>
        /// <param name="sides">The parsed sides with style values</param>
        /// <param name="viewportSize">Size of the viewport</param>
        /// <param name="parentSize">Size of the parent</param>
        /// <returns>Sides with pixel values</returns>
        private static StyleSides<float> GetSides(StyleSides<StyleValue> sides, Vector2 viewportSize, Vector2 parentSize)
        {
            return new StyleSides<float>()
            {
                Left = sides.Left.GetPixel(viewportSize, parentSize, parentSize.X, 0),
                Right = sides.Right.GetPixel(viewportSize, parentSize, parentSize.X, 0),
                Top = sides.Top.GetPixel(viewportSize, parentSize, parentSize.Y, 0),
                Bottom = sides.Bottom.GetPixel(viewportSize, parentSize, parentSize.Y, 0)
            };
        }
    }
}
