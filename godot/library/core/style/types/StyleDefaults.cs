using Godot;

namespace HarmoniaUI.Core.Style.Types
{
    /// <summary>
    /// Default values for <see cref="Raw.StyleResource"/> and other styles like <see cref="Parsed.ParsedStyle"/>
    /// </summary>
    public static class StyleDefaults
    {
        /// <summary>
        /// Instance of color that signifies the exported property is unset.
        /// This should result in a transparent color, but allows unset values.
        /// </summary>
        /// <remarks>
        /// Reason: It's impossible to export nullables to editor.
        /// </remarks>
        public static readonly Color UnsetColor = new Color(-1, -1, -1, -1);

        /// <summary>
        /// Default value for enums .Unset value.
        /// </summary>
        public const int ENUM_UNSET_VAL = -1;

        /// <summary>
        /// Default <see cref="VisibilityType"/> in styles.
        /// </summary>
        public const VisibilityType VISIBILITY_TYPE_DEFAULT = VisibilityType.Visible;

        /// <summary>
        /// Default <see cref="SizingType"/> in styles.
        /// </summary>
        public const SizingType SIZING_TYPE_DEFAULT = SizingType.Border;
        
        /// <summary>
        /// Default <see cref="PositionType"/> in styles.
        /// </summary>
        public const PositionType POSITION_TYPE_DEFAULT = PositionType.Normal;
    }
}
