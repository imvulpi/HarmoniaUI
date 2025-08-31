namespace HarmoniaUI.Core.Engines.Layout.Flex
{
    /// <summary>
    /// Describes the wrapping style in the flex layouts
    /// </summary>
    public enum FlexWrap
    {
        /// <summary>
        /// No wrapping occurs.
        /// </summary>
        NoWrap,

        /// <summary>
        /// Children elements are wrapped when no space is left
        /// </summary>
        Wrap,

        /// <summary>
        /// <see cref="Wrap"/> but children are reversed.
        /// </summary>
        WrapReverse
    }
}
