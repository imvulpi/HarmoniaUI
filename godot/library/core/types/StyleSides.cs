
using Godot;

namespace HarmoniaUI.library.core.types
{
    /// <summary>
    /// Describes size and unit of each sides with <see cref="Value"/> and <see cref="Unit"/>
    /// </summary>
    /// <remarks>
    /// Supports any shape with 4 sides ._. (...)
    /// </remarks>
    public struct StyleSides<T> where T : struct
    {
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
        /// Size and unit of the left side
        /// </summary>
        public T Left { get; set; }

        /// <summary>
        /// Size and unit of the right side
        /// </summary>
        public T Right { get; set; }

        /// <summary>
        /// Size and unit of the top side
        /// </summary>
        public T Top { get; set; }

        /// <summary>
        /// Size and unit of the bottom side
        /// </summary>
        public T Bottom { get; set; }
    }
}
