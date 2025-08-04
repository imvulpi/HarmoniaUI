using Godot;
using System;
using System.Globalization;

namespace HarmoniaUI.library.core.types.parser
{
    public static class StyleValueParser
    {
        /// <summary>
        /// Parses a Single string representing a value and a unit.
        /// </summary>
        /// <param name="valueAndUnit">Single string representing a tuple of a value and unit</param>
        /// <returns>Parsed style value</returns>
        public static int Parse(string valueAndUnit, Vector2 viewportSize, float parentSide)
        {
            if (valueAndUnit == "auto" || valueAndUnit.Length < 2) return UnitConstants.AUTO;

            valueAndUnit = valueAndUnit.Trim();
            string last2Chars = valueAndUnit.Substring(valueAndUnit.Length - 2, 2); // most units are 2 char long

            if (last2Chars.EndsWith('%'))
            {
                if (float.TryParse(valueAndUnit[..^1], CultureInfo.InvariantCulture, out float result))
                {
                    return GetPixel(result / 100, Unit.Percent, viewportSize, parentSide);
                }
                else
                {
                    GD.PushWarning("Couldn't parse the value. using 'auto' instead");
                    return UnitConstants.AUTO;
                }
            }
            else if (float.TryParse(valueAndUnit[..^2], CultureInfo.InvariantCulture, out float result))
            {
                switch (last2Chars)
                {
                    case "px":
                        return GetPixel(result, Unit.Pixel, viewportSize, parentSide);
                    case "vw":
                        return GetPixel(result / 100, Unit.ViewportWidth, viewportSize, parentSide);
                    case "vh":
                        return GetPixel(result / 100, Unit.ViewportHeight, viewportSize, parentSide);
                    default:
                        GD.PushWarning("Failed to detect unit. Using 'auto' instead");
                        return UnitConstants.AUTO;
                }
            }
            else
            {
                GD.PushWarning("Couldn't parse the style value. using 'auto' instead");
                return UnitConstants.AUTO;
            }
        }

        /// <summary>
        /// Parses values and units stored in a string in a format:
        /// [top+Unit] [right+..] [bottom] [left] separated by a space " "
        /// </summary>
        /// <param name="valuesAndUnits"></param>
        /// <returns>Parsed sides</returns>
        /// <exception cref="ArgumentException">When number of values is bigger than 4</exception>
        /// <remarks>
        /// Supports number of values from 0 to 4
        /// </remarks>
        public static StyleSides ParseSides(string valuesAndUnits, Vector2 viewportSize, Vector2 parentSize)
        {
            string[] values = valuesAndUnits.Split(' ');

            return values.Length switch
            {
                4 => new StyleSides(Parse(values[0], viewportSize, parentSize.Y), Parse(values[1], viewportSize, parentSize.X), Parse(values[2], viewportSize, parentSize.Y), Parse(values[3], viewportSize, parentSize.X)),
                3 => new StyleSides(Parse(values[0], viewportSize, parentSize.Y), Parse(values[1], viewportSize, parentSize.X), Parse(values[2], viewportSize, parentSize.X)),
                2 => new StyleSides(Parse(values[0], viewportSize, parentSize.X), Parse(values[1], viewportSize, parentSize.Y)),
                1 => new StyleSides(Parse(values[0], viewportSize, parentSize.Y), Parse(values[0], viewportSize, parentSize.X)),
                0 => new StyleSides(),
                _ => throw new ArgumentException("Invalid number of values for RectSides")
            };
        }

        public static int GetPixel(float value, Unit unit, Vector2 viewportSize, float parentSide)
        {
            return unit switch
            {
                Unit.Auto => (int)value,
                Unit.Pixel => (int)value,
                Unit.Percent => (int)MathF.Ceiling(parentSide * value),
                Unit.ViewportWidth => (int)MathF.Ceiling(viewportSize.X * value),
                Unit.ViewportHeight => (int)MathF.Ceiling(viewportSize.Y * value),
                _ => (int)value,
            };
        }
    }
}
