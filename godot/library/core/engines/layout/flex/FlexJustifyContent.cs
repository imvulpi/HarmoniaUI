namespace HarmoniaUI.Core.Engines.Layout.Flex
{
    /// <summary>
    /// Describes the spacing between children if there is available space between them.
    /// </summary>
    /// <remarks>
    /// Depending on the axis/<see cref="FlexDirection"/>, it justifies between lines or children nodes.
    /// </remarks>
    public enum FlexJustifyContent
    {
        /// <summary>
        /// All children are spaced at the start of the parent.
        /// </summary>
        Start,

        /// <summary>
        /// All children are spaced at the end of the parent.
        /// </summary>
        End, 
        
        /// <summary>
        /// All children are positioned to the center, without spacing between them.
        /// </summary>
        Center,
        
        /// <summary>
        /// Children are spaced equally in between, ignoring the left and right sides
        /// </summary>
        SpaceBetween,

        /// <summary>
        /// Children are spaced in between with the most left and right side having a spacing
        /// calculated from: <c>(space in between / 2)</c>
        /// </summary>
        SpaceAround,

        /// <summary>
        /// Children are spaced evenly on all sides, including the left and right side.
        /// </summary>
        SpaceEvenly,
    }
}
