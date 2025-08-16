using Godot;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Core.Style.Parsed;

namespace HarmoniaUI.Core.Style.Computed
{
    /// <summary>
    /// Responsible for computing <see cref="ParsedStyle"/> to <see cref="ComputedStyle"/> 
    /// that can be used in visual and layout systems
    /// <para>
    /// Turns all <see cref="StyleValue"/> to pixels, with regard to the parent size, ratios and percentages.
    /// </para>
    /// </summary>
    public static class StyleComputer
    {
        /// <summary>
        /// Updates the provided <paramref name="target"/> style with values from the parsed <paramref name="style"/>
        /// <para>
        /// Uses the parsed style and sizes to compute the <paramref name="target"/>
        /// </para>
        /// </summary>
        /// <param name="target"><see cref="ComputedStyle"/> to compute from <paramref name="style"/></param>
        /// <param name="style">Parsed style which contains the unit information used in order to compute <paramref name="target"/></param>
        /// <param name="viewportSize">Size of the viewport the target should apply to.</param>
        /// <param name="parentSize">The parent size of the node containing <paramref name="target"/></param>
        public static void Compute(ComputedStyle target, ParsedStyle style, Vector2 viewportSize, Vector2 parentSize)
        {
            target.SizingType = style.SizingType;
            target.Visibility = style.Visibility;
            target.Width = GetPixel(style.Width, viewportSize, parentSize, parentSize.X, parentSize.X);
            target.Height = GetPixel(style.Height, viewportSize, parentSize, parentSize.Y, parentSize.Y);
            target.MinWidth = GetPixel(style.MinWidth, viewportSize, parentSize, parentSize.X, 0);
            target.MinHeight = GetPixel(style.MinHeight, viewportSize, parentSize, parentSize.Y, 0);
            target.MaxWidth = GetPixel(style.MaxWidth, viewportSize, parentSize, parentSize.X, float.MaxValue);
            target.MaxHeight = GetPixel(style.MaxHeight, viewportSize, parentSize, parentSize.Y, float.MaxValue);
            target.Padding = GetSides(style.Padding, viewportSize, parentSize);
            target.Margin = GetSides(style.Margin, viewportSize, parentSize);
            target.BackgroundColor = style.BackgroundColor;
            target.BorderColor = style.BorderColor;
            target.BorderRadius = GetSides(style.BorderRadius, viewportSize, parentSize);
            target.BorderWidth = GetSides(style.BorderWidth, viewportSize, parentSize);
            target.ShadowColor = style.ShadowColor;
            target.PositioningType = style.PositioningType;
            target.PositionX = GetPixel(style.PositionX, viewportSize, parentSize, parentSize.X, 0);
            target.PositionY = GetPixel(style.PositionY, viewportSize, parentSize, parentSize.Y, 0);
            target.ShadowOffsetX = GetPixel(style.ShadowOffsetX, viewportSize, new Vector2(target.Width, target.Height), target.Width, 0);
            target.ShadowOffsetY = GetPixel(style.ShadowOffsetY, viewportSize, new Vector2(target.Width, target.Height), target.Height, 0);
        }

        /// <summary>
        /// Returns computed sides from the sides containing units.
        /// </summary>
        /// <param name="sides">Sides that contain units, uncomputed, usually parsed</param>
        /// <param name="viewportSize">Size of the viewport which applies to <paramref name="sides"/></param>
        /// <param name="parentSize">Parent size of the node which applies to <paramref name="sides"/></param>
        /// <returns></returns>
        private static StyleSides<float> GetSides(StyleSides<StyleValue> sides, Vector2 viewportSize, Vector2 parentSize)
        {
            return new StyleSides<float>()
            {
                Left = GetPixel(sides.Left, viewportSize, parentSize, parentSize.X, 0),
                Right = GetPixel(sides.Right, viewportSize, parentSize, parentSize.X, 0),
                Top = GetPixel(sides.Top, viewportSize, parentSize, parentSize.Y, 0),
                Bottom = GetPixel(sides.Bottom, viewportSize, parentSize, parentSize.Y, 0)
            };
        }

        /// <summary>
        /// Computes the <paramref name="value"/> to pixels.
        /// </summary>
        /// <remarks>
        /// <paramref name="parentSide"/> is preferred parent side that's used to compute % values.
        /// Usually it's the values of the same direction. Most often Top/Bottom will be set to Parent.Y,
        /// Left/Right to the parent.X
        /// 
        /// </remarks>
        /// <param name="value">Value which should get computed to pixels</param>
        /// <param name="viewportSize">The size of the viewport that's related to <paramref name="value"/></param>
        /// <param name="parentSize">The size of the parent of a node which <paramref name="value"/> relates to</param>
        /// <param name="parentSide">Preferred parent side that's used to compute % values.</param>
        /// <param name="autoValue">Value that should be set when unit is set to <see cref="Unit.Auto"/></param>
        /// <returns>Pixel values of <paramref name="value"/></returns>
        public static float GetPixel(StyleValue value, Vector2 viewportSize, Vector2 parentSize, float parentSide, float autoValue)
        {
            return value.Unit switch
            {
                Unit.Auto => autoValue,
                Unit.Pixel => value.Value,
                Unit.Percent => parentSide * value.Value,
                Unit.ViewportWidth => viewportSize.X * value.Value,
                Unit.ViewportHeight => viewportSize.Y * value.Value,
                Unit.WidthPercent => parentSize.X * value.Value,
                Unit.HeightPercent => parentSize.Y * value.Value,
                _ => value.Value,
            };
        }
    }
}
