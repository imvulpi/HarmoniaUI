using System;

namespace HarmoniaUI.Library.style.parsing
{
    public static class StyleValueParser
    {
        /// <summary>
        /// Parses a Single string representing a value and a unit.
        /// </summary>
        /// <param name="valueAndUnit">Single string representing a tuple of a value and unit</param>
        /// <returns>Parsed style value</returns>
        public static StyleValue Parse(string valueAndUnit)
        {
            if (valueAndUnit == "auto") return new(0, Unit.Auto);

            valueAndUnit = valueAndUnit.Trim();
            string last2Chars = valueAndUnit.Substring(valueAndUnit.Length - 2, 2); // most units are 2 char long
            float value = 0;

            if (last2Chars.EndsWith('%'))
            {
                return new(float.Parse(valueAndUnit[..^1])/100, Unit.Percent);
            }

            value = float.Parse(valueAndUnit[..^2]);
            return last2Chars switch
            {
                "px" => new(value, Unit.Pixel),
                "vw" => new(value/100, Unit.ViewportWidth),
                "vh" => new(value/100, Unit.ViewportHeight),
                _ => new(value, Unit.Auto),
            };
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
        public static RectSides ParseRect(string valuesAndUnits)
        {
            string[] values = valuesAndUnits.Split(' ');
            RectSides rectSides = new RectSides();

            return values.Length switch
            {
                4 => new RectSides(Parse(values[0]), Parse(values[1]), Parse(values[2]), Parse(values[3])),
                3 => new RectSides(Parse(values[0]), Parse(values[1]), Parse(values[2])),
                2 => new RectSides(Parse(values[0]), Parse(values[1])),
                1 => new RectSides(Parse(values[0])),
                0 => new RectSides(),
                _ => throw new ArgumentException("Invalid number of values for RectSides")
            };
        }
    }
}
