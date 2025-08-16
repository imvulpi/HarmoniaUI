namespace HarmoniaUI.Core.Style.Types
{
    /// <summary>
    /// Describes the positioning type that will be used for a node
    /// </summary>
    public enum PositionType
    {
        /// <summary>
        /// Positioning defined by the Harmonia node used.
        /// </summary>
        Normal,
        /// <summary>
        /// Position offset fron the <see cref="Normal"/> flow position.
        /// </summary>
        Relative,
        /// <summary>
        /// Position independent of the Harmonia node from the (0, 0)
        /// </summary>
        Absolute
    }
}