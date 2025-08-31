namespace HarmoniaUI.Core.Engines.Layout.Flex
{
    /// <summary>
    /// Describes the direction in which children of flex container will get positioned
    /// </summary>
    public enum FlexDirection
    {
        /// <summary>
        /// Children are positioned on the X axis
        /// </summary>
        /// <remarks>
        /// When wrapping it will overflow on Y axis as opposed to X axis
        /// </remarks>
        Row,

        /// <summary>
        /// Children are positioned on the Y axis.
        /// </summary>
        /// <remarks>
        /// When wrapping it will overflow on X axis as opposed to Y axis
        /// </remarks>
        Column,
    }
}
