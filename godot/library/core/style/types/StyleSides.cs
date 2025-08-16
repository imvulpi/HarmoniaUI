namespace HarmoniaUI.Core.Style.Types
{
    /// <summary>
    /// Specifies the size of all sides of a rectangle.
    /// </summary>
    /// <remarks>
    /// The specific type will depend on the usage.
    /// For styles the type will differ based on style stage
    /// (Raw, Parsed, Computed)
    /// </remarks>
    public struct StyleSides<T> where T : struct
    {
        /// <summary>
        /// Sets all values to the specified sides.
        /// </summary>
        /// <param name="top">Top value side</param>
        /// <param name="right">Right value side</param>
        /// <param name="bottom">Bottom value side</param>
        /// <param name="left">Left value side</param>
        public StyleSides(T top, T right, T bottom, T left)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Sets Top and Bottom to <paramref name="topAndBottom"/> and Right and Left to <paramref name="rightAndLeft"/>
        /// </summary>
        /// <param name="rightAndLeft">Right and left side values</param>
        /// <param name="topAndBottom">Top and bottom side values</param>
        public StyleSides(T topAndBottom, T rightAndLeft)
        {
            Left = rightAndLeft;
            Right = rightAndLeft;
            Top = topAndBottom;
            Bottom = topAndBottom;
        }

        /// <summary>
        /// Sets Right and Left to <paramref name="rightAndLeft"/> and others normally
        /// </summary>
        /// <param name="top">Top value side</param>
        /// <param name="rightAndLeft">Right and left side values</param>
        /// <param name="bottom">Bottom value side</param>
        public StyleSides(T top, T rightAndLeft, T bottom)
        {
            Left = rightAndLeft;
            Right = rightAndLeft;
            Top = top;
            Bottom = bottom;
        }

        /// <summary>
        /// Sets all values to <paramref name="allSides"/>
        /// </summary>
        /// <param name="allSides">All sides value</param>
        public StyleSides(T allSides) {
            Left = allSides;
            Right = allSides;
            Top = allSides;
            Bottom = allSides;
        }

        /// <summary>
        /// Creates an instance with <see cref="T.Default"/> values
        /// </summary>
        public StyleSides() { }

        /// <summary>
        /// Value of the left side
        /// </summary>
        public T Left { get; set; }

        /// <summary>
        /// Value of the right side
        /// </summary>
        public T Right { get; set; }

        /// <summary>
        /// Value of the top side
        /// </summary>
        public T Top { get; set; }

        /// <summary>
        /// Value of the bottom side
        /// </summary>
        public T Bottom { get; set; }

        /// <summary>
        /// Displays all sides in a pretty style
        /// </summary>
        /// <returns>Pretty sides string</returns>
        public override string ToString()
        {
            return $"(Top: {Top}) (Right: {Right}) (Bottom: {Bottom}) (Left: {Left})";
        }
    }
}
