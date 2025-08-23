using Godot;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Types;

namespace HarmoniaUI.Core.Style.Merger
{
    /// <summary>
    /// Merges two parsed style together.
    /// </summary>
    /// <remarks>
    /// Merging is useful with interaction styles that depend on a primary style.
    /// For example the <b>on hover style</b> will depend on the normal style, meaning the 
    /// <b>unset values will default to the normal style</b> values
    /// </remarks>
    public static class StyleMerger
    {
        /// <summary>
        /// Merges <paramref name="secondary"/> with <paramref name="primary"/>. 
        /// Where unset values of <paramref name="secondary"/> will default to <paramref name="primary"/> values
        /// </summary>
        /// <remarks>
        /// <paramref name="result"/> is used in order to save on allocations.
        /// </remarks>
        /// <param name="primary">Primary parsed style</param>
        /// <param name="secondary">Secondary parsed style</param>
        /// <param name="result">Resulting style after merging.</param>
        /// <returns><paramref name="result"/> style</returns>
        public static ParsedStyle Merge(ParsedStyle primary, ParsedStyle secondary, ref ParsedStyle result)
        {
            if (primary == null) return secondary;
            if (secondary == null) return primary;

            result.LayoutResource = (Interfaces.LayoutResource)MergeResource(primary.LayoutResource, secondary.LayoutResource);
            result.VisualResource = (Interfaces.VisualResource)MergeResource(primary.VisualResource, secondary.VisualResource);
            result.InputResource = (Interfaces.InputResource)MergeResource(primary.InputResource, secondary.InputResource);
            result.Visibility = (VisibilityType)MergeEnum((int)primary.Visibility, (int)secondary.Visibility);
            result.PositioningType = (PositionType)MergeEnum((int)primary.PositioningType, (int)secondary.PositioningType);
            result.SizingType = (SizingType)MergeEnum((int)primary.SizingType, (int)secondary.SizingType);
            result.BackgroundColor = MergeColor(primary.BackgroundColor, secondary.BackgroundColor);
            result.BorderColor = MergeColor(primary.BorderColor, secondary.BorderColor);
            result.ShadowColor = MergeColor(primary.ShadowColor, secondary.ShadowColor);
            result.Width = MergeStyleValue(primary.Width, secondary.Width);
            result.Height = MergeStyleValue(primary.Height, secondary.Height);
            result.MinWidth = MergeStyleValue(primary.MinWidth, secondary.MinWidth);
            result.MinHeight = MergeStyleValue(primary.MinHeight, secondary.MinHeight);
            result.MaxWidth = MergeStyleValue(primary.MaxWidth, secondary.MaxWidth);
            result.MaxHeight = MergeStyleValue(primary.MaxHeight, secondary.MaxHeight);
            result.Padding = MergeStyleSides(primary.Padding, secondary.Padding);
            result.Margin = MergeStyleSides(primary.Margin, secondary.Margin);
            result.BorderRadius = MergeStyleSides(primary.BorderRadius, secondary.BorderRadius);
            result.BorderWidth = MergeStyleSides(primary.BorderWidth, secondary.BorderWidth);
            result.ShadowOffsetX = MergeStyleValue(primary.ShadowOffsetX, secondary.ShadowOffsetX);
            result.ShadowOffsetY = MergeStyleValue(primary.ShadowOffsetY, secondary.ShadowOffsetY);
            result.PositionX = MergeStyleValue(primary.PositionX, secondary.PositionX);
            result.PositionY = MergeStyleValue(primary.PositionY, secondary.PositionY);
            return result;
        }

        private static Resource MergeResource(Resource primary, Resource secondary)
        {
            if (secondary == null)
            {
                return primary;
            }
            return secondary;
        }

        private static int MergeEnum(int primary, int secondary)
        {
            if(secondary == StyleDefaults.ENUM_UNSET_VAL)
            {
                return primary;
            }
            return secondary;
        }

        /// <summary>
        /// Merges a color, by checking whether the <paramref name="secondary"/> color is equal to <see cref="StyleDefaults.UnsetColor"/>
        /// </summary>
        /// <remarks>
        /// <see cref="StyleDefaults.UnsetColor"/> is equal to a value being unset.
        /// </remarks>
        /// <param name="primary">Primary color, returned when secondary is unset</param>
        /// <param name="secondary">Secondary color</param>
        /// <returns>Merged color</returns>
        private static Color MergeColor(Color primary, Color secondary)
        {
            if(secondary == StyleDefaults.UnsetColor)
            {
                return primary;
            }
            return secondary;
        }

        /// <summary>
        /// Merges <see cref="StyleValue"/>, if <paramref name="secondary"/> unit is <see cref="Unit.Auto"/> 
        /// it returns <paramref name="primary"/>, otherwise <paramref name="secondary"/>
        /// </summary>
        /// <param name="primary">Primary <see cref="StyleValue"/>, returned when secondary unit is <see cref="Unit.Auto"/></param>
        /// <param name="secondary">Secondary <see cref="StyleValue"/></param>
        /// <returns>Merged <see cref="StyleValue"/></returns>
        private static StyleValue MergeStyleValue(StyleValue primary, StyleValue secondary)
        {
            if (secondary.Unit == Unit.Auto)
            {
                return primary;
            }
            return secondary;
        }

        /// <summary>
        /// Merges <see cref="StyleSides{T}"/> where T is <see cref="StyleValue"/>,
        /// It uses <see cref="MergeStyleValue(StyleValue, StyleValue)"/> on all of the sides.
        /// </summary>
        /// <param name="primary">Primary sides</param>
        /// <param name="secondary">Secondary sides</param>
        /// <returns>Meged sides</returns>
        private static StyleSides<StyleValue> MergeStyleSides(StyleSides<StyleValue> primary, StyleSides<StyleValue> secondary)
        {
            return new() {
                Bottom = MergeStyleValue(primary.Bottom, secondary.Bottom),
                Top = MergeStyleValue(primary.Top, secondary.Top),
                Left = MergeStyleValue(primary.Left, secondary.Left),
                Right = MergeStyleValue(primary.Right, secondary.Right)
            };
        }
    }
}
