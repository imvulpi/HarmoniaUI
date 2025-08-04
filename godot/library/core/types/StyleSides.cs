namespace HarmoniaUI.library.core.types
{
    /// <summary>
    /// Describes size and unit of each sides with <see cref="Value"/> and <see cref="Unit"/>
    /// </summary>
    /// <remarks>
    /// Supports any shape with 4 sides ._. (...)
    /// </remarks>
    public struct StyleSides
    {
        public StyleSides(int top, int right, int bottom, int left)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Sets Top and Bottom to <paramref name="topAndBottom"/> and Right and Left to <paramref name="rightAndLeft"/>
        /// </summary>
        public StyleSides(int topAndBottom, int rightAndLeft)
        {
            Left = rightAndLeft;
            Right = rightAndLeft;
            Top = topAndBottom;
            Bottom = topAndBottom;
        }

        /// <summary>
        /// Sets Right and Left to <paramref name="rightAndLeft"/> and others normally
        /// </summary>
        public StyleSides(int top, int rightAndLeft, int bottom)
        {
            Left = rightAndLeft;
            Right = rightAndLeft;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Sets all values to <paramref name="allSides"/>
        /// </summary>
        public StyleSides(int allSides) {
            Left = allSides;
            Right = allSides;
            Top = allSides;
            Bottom = allSides;
        }

        /// <summary>
        /// Creates an instance with <see cref="int.Default"/> values
        /// </summary>
        public StyleSides() { }

        /// <summary>
        /// Size and unit of the left side
        /// </summary>
        public int Left { get; set; } = 0;

        /// <summary>
        /// Size and unit of the right side
        /// </summary>
        public int Right { get; set; } = 0;

        /// <summary>
        /// Size and unit of the top side
        /// </summary>
        public int Top { get; set; } = 0;

        /// <summary>
        /// Size and unit of the bottom side
        /// </summary>
        public int Bottom { get; set; } = 0;
    }
}
