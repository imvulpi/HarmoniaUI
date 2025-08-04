namespace HarmoniaUI.library.core.types
{
    /// <summary>
    /// Defines what composes the size (width/height) of an element
    /// </summary>
    public enum SizingType
    {
        /// <summary>
        /// Size includes the content, padding and border.
        /// </summary>
        Border,

        /// <summary>
        /// Size includes the content and the padding.
        /// </summary>
        /// <remarks>
        /// Excludes border
        /// </remarks>
        Padding,

        /// <summary>
        /// Size includes only the content
        /// </summary>
        /// <remarks>
        /// Excludes padding or border. Content is width/height directly
        /// </remarks>
        Content,
    }
}
