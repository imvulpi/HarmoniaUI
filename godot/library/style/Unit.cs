namespace HarmoniaUI.Library.style
{
    /// <summary>
    /// Units of length that are supported by Harmonia styles
    /// </summary>
    public enum Unit
    {
        /// <summary>
        /// Automatic - chosen automatically
        /// </summary>
        Auto,

        /// <summary>
        /// Pixels (px)
        /// </summary>
        Pixel,
        
        /// <summary>
        /// Percentage (%)
        /// </summary>
        /// <remarks>
        /// Usually size is calculated based on the parent size - ex. 100% is the parent width
        /// </remarks>
        Percent,

        /// <summary>
        /// Viewport width (vw) - the percentage of viewport width
        /// </summary>
        ViewportWidth,

        /// <summary>
        /// Viewport height (vh) - the percentage of viewport height
        /// </summary>
        ViewportHeight
    }
}
