namespace HarmoniaUI.Core.Style.Types
{
    /// <summary>
    /// Specified the visibility of an element, hiding or showing it.
    /// </summary>
    /// <remarks>
    /// If visibility is set to <see cref="Transparent"/> then it occupies space but is not visible.
    /// It's not rendered, but layout still passes.
    /// </remarks>
    public enum VisibilityType
    {
        /// <summary>
        /// Unset visibility, meaning the visibility will get inherited or defaulted.
        /// </summary>
        Unset = StyleDefaults.ENUM_UNSET_VAL,
        
        /// <summary>
        /// Element is visible.
        /// </summary>
        Visible,

        /// <summary>
        /// Element is not visible, but occupies space.
        /// </summary>
        Transparent,

        /// <summary>
        /// Element is hidden and doesn't occupy any space.
        /// </summary>
        Hidden
    }
}
