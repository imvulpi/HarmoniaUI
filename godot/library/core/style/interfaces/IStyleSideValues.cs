namespace HarmoniaUI.Core.Style.Interfaces
{
    /// <summary>
    /// Specifies the values of a style that describe rectangle sides Top, Bottom, Left, Right
    /// </summary>
    /// <remarks>
    /// Depending on the style stage the <typeparamref name="T"/> might be different:
    /// <list type="bullet">
    /// <item>Raw style -> strings</item>
    /// <item>Parsed style -> typed structure (Usually <seealso cref="Types.StyleSides{T}"/>)</item>
    /// <item>Computed style -> pixel values sides</item>
    /// </list>
    /// </remarks>
    /// <typeparam name="T">Type of the sides depending on the style stage.</typeparam>
    public interface IStyleSideValues<T>
    {
        /// <summary>
        /// The specified radius for rounding of the corners in an element
        /// </summary>
        public T BorderRadius { get; set; }

        /// <summary>
        /// The specified border width for an element
        /// </summary>
        public T BorderWidth { get; set; }

        /// <summary>
        /// Padding inside the element (space between content and border).
        /// </summary>
        public T Padding { get; set; }

        /// <summary>
        /// Margin outside the element (space between element and siblings).
        /// </summary>
        public T Margin { get; set; }
    }
}
