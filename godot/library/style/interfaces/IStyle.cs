using Godot;
using HarmoniaUI.library.core.types;

namespace HarmoniaUI.library.style.interfaces
{
    /// <summary>
    /// Style for harmonia nodes, interface for raw, parsed and computed styles.
    /// <para>
    /// This interface itself contains values not requiring parsing or computing into other structures
    /// </para>
    /// </summary>
    /// <remarks>
    /// The types should be choosen based on whether its a raw/parsed/computed style.
    /// </remarks>
    /// <typeparam name="T1">Single values that are parsed and computed</typeparam>
    /// <typeparam name="T2">Side (4) values that are parsed and computed</typeparam>
    public interface IStyle<T1, T2> : IStyleValues<T1>, IStyleSideValues<T2>
    {
        /// <summary>
        /// Defines visibility of the element
        /// </summary>
        public VisibilityType Visibility { get; set; }

        /// <summary>
        /// Sizing type of the element.
        /// </summary>
        public SizingType SizingType { get; set; }

        /// <summary>
        /// Background color of the element.
        /// </summary>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Color of the border.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Color of the box shadow.
        /// </summary>
        public Color ShadowColor { get; set; }

        /// <summary>
        /// Offset for the shadow in local pixels (x, y).
        /// </summary>
        public Vector2 ShadowOffset { get; set; }

        /// <summary>
        /// Positioning mode: Normal (flow), Relative (offset), or Absolute (manual).
        /// </summary>
        public PositionType PositioningType { get; set; }
    }
}
