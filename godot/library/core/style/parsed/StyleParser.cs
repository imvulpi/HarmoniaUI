using Godot;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Core.Style.Raw;
using System;
using System.Globalization;

namespace HarmoniaUI.Core.Style.Parsed
{
    /// <summary>
    /// Responsible for converting a <see cref="StyleResource"/> into a 
    /// <see cref="ParsedStyle"/> that can be used by the layout and visual systems.
    /// </summary>
    /// <remarks>
    /// The <see cref="StyleParser"/> reads the raw style data stored in a 
    /// <see cref="StyleResource"/>, validates it, and converts it into strongly typed 
    /// values stored in a <see cref="ParsedStyle"/> instance.  
    /// 
    /// <para>
    /// Parsing does not apply inheritance, or convert values to pixels - these are 
    /// handled later when producing a <see cref="Computed.ComputedStyle"/>.  
    /// </para>
    /// </remarks>
    public static class StyleParser
    {
        /// <summary>
        /// Parses the style information from a <paramref name="style"/> resource 
        /// and writes the results into the specified <paramref name="target"/> parsed style.
        /// </summary>
        /// <param name="target">
        /// The <see cref="ParsedStyle"/> object to populate with typed style values.
        /// </param>
        /// <param name="style">
        /// The <see cref="StyleResource"/> containing raw style definitions to parse.
        /// </param>
        /// <remarks>
        /// This method overwrites the values in the <paramref name="target"/> with 
        /// the parsed equivalents of the raw data found in <paramref name="style"/>.  
        /// It is typically called once in the <c>_EnterTree</c> process 
        /// before computing the final <see cref="Computed.ComputedStyle"/>.
        /// </remarks>
        /// <returns>Style with parsed values</returns>
        public static ParsedStyle Parse(ParsedStyle target, StyleResource style)
        {
            if(style == null) return null;
            target ??= new ParsedStyle();
            target.SizingType = style.SizingType;
            target.Visibility = style.Visibility;
            target.Width = ParseValue(style.Width);
            target.Height = ParseValue(style.Height);
            target.MinWidth = ParseValue(style.MinWidth);
            target.MinHeight = ParseValue(style.MinHeight);
            target.MaxWidth = ParseValue(style.MaxWidth);
            target.MaxHeight = ParseValue(style.MaxHeight);
            target.Padding = ParseSides(style.Padding);
            target.Margin = ParseSides(style.Margin);
            target.BackgroundColor = style.BackgroundColor;
            target.BorderColor = style.BorderColor;
            target.BorderRadius = ParseSides(style.BorderRadius);
            target.BorderWidth = ParseSides(style.BorderWidth);
            target.ShadowColor = style.ShadowColor;
            target.ShadowOffsetX = ParseValue(style.ShadowOffsetX);
            target.ShadowOffsetY = ParseValue(style.ShadowOffsetY);
            target.PositioningType = style.PositioningType;
            target.PositionX = ParseValue(style.PositionX);
            target.PositionY = ParseValue(style.PositionY);
            return target;
        }

        /// <summary>
        /// Parses the string representation of four <see cref="StyleValue"/> sides: (Top, Bottom, Left, Right)
        /// into a <see cref="StyleSides{StyleValue}"/>
        /// </summary>
        /// <remarks>
        /// The string representation is seperated by a space ' '
        /// <para>
        /// In case of an empty string it returns default values.
        /// </para>
        /// </remarks>
        /// <param name="sides">String representation of 4 <see cref="StyleValue"/> sides seperated by a space</param>
        /// <returns>Parsed sides</returns>
        /// <exception cref="ArgumentException">Thrown when number of values in the <paramref name="sides"/> is bigger than 4</exception>
        public static StyleSides<StyleValue> ParseSides(string sides)
        {
            if (sides == null) return new();
            string[] values = sides.Split(' ');
            return values.Length switch
            {
                4 => new StyleSides<StyleValue>(
                    ParseValue(values[0]),
                    ParseValue(values[1]),
                    ParseValue(values[2]),
                    ParseValue(values[3])),
                3 => new StyleSides<StyleValue>(
                    ParseValue(values[0]),
                    ParseValue(values[1]),
                    ParseValue(values[2])),
                2 => new StyleSides<StyleValue>(
                    ParseValue(values[0]),
                    ParseValue(values[1])),
                1 => new StyleSides<StyleValue>(
                    ParseValue(values[0]),
                    ParseValue(values[0])),
                0 => new StyleSides<StyleValue>(),
                _ => throw new ArgumentException("Invalid number of values for RectSides")
            };
        }

        /// <summary>
        /// Parses a <b>string</b> string representation of 
        /// <see cref="StyleValue"/> to an actual <see cref="StyleValue"/>
        /// </summary>
        /// <remarks>
        /// Examples: <c>10px</c>, <c>0.5%</c> and more...
        /// </remarks>
        /// <param name="value">String representation of <see cref="StyleValue"/></param>
        /// <returns>Parsed value; otherwise <see cref="StyleValue.Default"/> otherwise</returns>
        public static StyleValue ParseValue(string value)
        {
            if (value == null || value == "auto") return StyleValue.Default;
            int numericEnd = GetNumberEndIndex(value);
            if(numericEnd == 0) return StyleValue.Default;

            float numericValue = float.Parse(value[..numericEnd], CultureInfo.InvariantCulture);
            string unit = value[numericEnd..];

            switch (unit)
            {
                case "px":
                    return new(numericValue, Unit.Pixel);
                case "%":
                    return new(numericValue / 100, Unit.Percent);
                case "vw":
                    return new(numericValue / 100, Unit.ViewportWidth);
                case "vh":
                    return new(numericValue / 100, Unit.ViewportHeight);
                case "w%":
                    return new(numericValue / 100, Unit.WidthPercent);
                case "h%":
                    return new(numericValue / 100, Unit.HeightPercent);
                default:
                    GD.PushWarning($"Failed to value unit '{unit}'. Using 'auto' instead");
                    return StyleValue.Default;
            }
        }

        /// <summary>
        /// Returns an index where the float number in string representation ends.
        /// </summary>
        /// <param name="input">String value</param>
        /// <returns>0 if no number or invalid; otherwise index where number ends</returns>
        private static int GetNumberEndIndex(string input)
        {
            int i = 0;
            bool hasDecimal = false;
            bool valid = false;

            if (i < input.Length && (input[i] == '+' || input[i] == '-'))    
                i++;
            
            while (i < input.Length)
            {
                char c = input[i];

                if (char.IsDigit(c))
                {
                    valid = true;
                    i++;
                }
                else if (c == '.' && !hasDecimal)
                {
                    hasDecimal = true;
                    i++;
                }
                else
                {
                    break;
                }
            }

            if (!valid) return 0;
            return i;
        }
    }
}
