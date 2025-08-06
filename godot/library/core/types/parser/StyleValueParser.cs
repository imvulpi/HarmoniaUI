using Godot;
using System;
using System.Globalization;

namespace HarmoniaUI.library.core.types.parser
{
    public static class StyleValueParser
    {
        /// <summary>
        /// Parses values from <see cref="StyleSides"/>, sets pixel values relative to the parameters
        /// </summary>
        /// <remarks>
        /// Supports values from ranges of 0 to 4
        /// </remarks>
        /// <param name="sides">Sides represented as string</param>
        /// <param name="viewportSize">Viewport size</param>
        /// <param name="parentSize">Size of the parent node</param>
        /// <param name="autoValue">The value related to Auto unit, setting 0 essentially removes auto.</param>
        /// <returns>Parsed sides with pixel values</returns>
        /// <exception cref="ArgumentException">When the number of values exceeds 4</exception>
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
        /// Parses a single string value
        /// </summary>
        /// <param name="value">Value to be parsed</param>
        /// <param name="viewportSize">Size of the viewport</param>
        /// <param name="parentSize">Size of the parent node</param>
        /// <param name="parentSide">Side of the parent related to the value, ex. Width -> Parent X </param>
        /// <param name="autoValue">The value related to Auto unit, setting 0 essentially removes auto.</param>
        /// <returns>Parsed pixel value</returns>
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
