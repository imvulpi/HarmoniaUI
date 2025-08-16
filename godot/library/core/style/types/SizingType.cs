namespace HarmoniaUI.Core.Style.Types
{
    /// <summary>
    /// Specifies what should happen to the size and content size of an element
    /// </summary>
    public enum SizingType
    {
        /// <summary>
        /// Set size will include the content, padding and border.
        /// </summary>
        /// <remarks>
        /// The set size will not become larger with padding or border,
        /// but it also <b>shrinks</b> the <b>content</b> size.
        /// </remarks>
        Border,

        /// <summary>
        /// Set size will include the content and padding
        /// </summary>
        /// <remarks>
        /// The set size will not become larger with padding, but will with border.
        /// it will shrink the <b>content</b> size by the padding size
        /// </remarks>
        Padding,

        /// <summary>
        /// Set size if directly the content size
        /// </summary>
        /// <remarks>
        /// This means the content size will not shrink but the <b>overall size</b>
        /// will become larger with bigger padding or border.
        /// </remarks>
        Content,
    }
}
