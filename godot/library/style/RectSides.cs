namespace HarmoniaUI.Library.style
{
    /// <summary>
    /// Describes size and unit of each sides with <see cref="StyleValue.Value"/> and <see cref="StyleValue.Unit"/>
    /// </summary>
    /// <remarks>
    /// Supports any shape with 4 sides ._. (...)
    /// </remarks>
    public struct RectSides
    {
        public RectSides(StyleValue top, StyleValue right, StyleValue bottom, StyleValue left)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Sets Top and Bottom to <paramref name="topAndBottom"/> and Right and Left to <paramref name="rightAndLeft"/>
        /// </summary>
        public RectSides(StyleValue topAndBottom, StyleValue rightAndLeft)
        {
            Left = rightAndLeft;
            Right = rightAndLeft;
            Top = topAndBottom;
            Bottom = topAndBottom;
        }

        /// <summary>
        /// Sets Right and Left to <paramref name="rightAndLeft"/> and others normally
        /// </summary>
        public RectSides(StyleValue top, StyleValue rightAndLeft, StyleValue bottom)
        {
            Left = rightAndLeft;
            Right = rightAndLeft;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Sets all values to <paramref name="allSides"/>
        /// </summary>
        public RectSides(StyleValue allSides) {
            Left = allSides;
            Right = allSides;
            Top = allSides;
            Bottom = allSides;
        }

        /// <summary>
        /// Creates an instance with <see cref="StyleValue.Default"/> values
        /// </summary>
        public RectSides() { }

        /// <summary>
        /// Size and unit of the left side
        /// </summary>
        public StyleValue Left { get; set; } = StyleValue.Default;

        /// <summary>
        /// Size and unit of the right side
        /// </summary>
        public StyleValue Right { get; set; } = StyleValue.Default;

        /// <summary>
        /// Size and unit of the top side
        /// </summary>
        public StyleValue Top { get; set; } = StyleValue.Default;

        /// <summary>
        /// Size and unit of the bottom side
        /// </summary>
        public StyleValue Bottom { get; set; } = StyleValue.Default;
    }
}
