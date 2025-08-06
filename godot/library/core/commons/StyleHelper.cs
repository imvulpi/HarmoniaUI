using Godot;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.style;

namespace HarmoniaUI.library.core.commons
{
    /// <summary>
    /// Helper class to help with <see cref="ParsedStyle"/>
    /// </summary>
    public class StyleHelper
    {

        /// <summary>
        /// Simply gets padding from left and right
        /// </summary>
        /// <returns>Padding from Left + Right</returns>
        public static float GetPaddingX(ComputedStyle style)
        {
            return style.Padding.Left + style.Padding.Right;
        }

        /// <summary>
        /// Simply gets padding from top and bottom
        /// </summary>
        /// <returns>Padding from Top + Bottom</returns>
        public static float GetPaddingY(ComputedStyle style)
        {
            return style.Padding.Top + style.Padding.Bottom;
        }

        /// <summary>
        /// Simply gets border width from left and right
        /// </summary>
        /// <returns>Border width from Left + Right</returns>
        public static float GetBorderWidthX(ComputedStyle style)
        {
            return style.BorderWidth.Left + style.BorderWidth.Right;
        }

        /// <summary>
        /// Simply gets border width from top and bottom
        /// </summary>
        /// <returns>Border width from Top + Bottom</returns>
        public static float GetBorderWidthY(ComputedStyle style)
        {
            return style.BorderWidth.Top + style.BorderWidth.Bottom;
        }

        /// <summary>
        /// Gets pixel value from the <paramref name="measurement"/> or returns <paramref name="parentSide"/> as auto
        /// </summary>
        /// <param name="measurement">Measurement to get value from</param>
        /// <param name="parentSide">Side of the parent related to <paramref name="measurement"/></param>
        /// <returns>Pixel value; <paramref name="parentSide"/> otherwise</returns>
        public static float ComputeDimension(float measurement, float parentSide)
        {
            if (measurement == UnitConstants.AUTO)
                return parentSide;
            else
                return measurement;
        }

        /// <summary>
        /// Gets pixel value from the <paramref name="measurement"/> or returns <see cref="float.MaxValue"/> as auto
        /// </summary>
        /// <param name="measurement">Measurement to get value from</param>
        /// <param name="parentSide">Side of the parent related to <paramref name="measurement"/></param>
        /// <returns>Pixel value; <see cref="float.MaxValue"/> otherwise</returns>
        public static float ComputeMaxDimension(float measurement)
        {
            if (measurement == UnitConstants.AUTO)
                return float.MaxValue;
            else
                return measurement;
        }
    }
}
