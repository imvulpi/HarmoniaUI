using Godot;
using HarmoniaUI.Core.Style.Types;

namespace HarmoniaUI.Core.Style.Interfaces
{
    /// <summary>
    /// Defines the contract for style objects in HarmoniaUI, used by 
    /// <see cref="Raw.StyleResource"/>, <see cref="Parsed.ParsedStyle"/>, and <see cref="Computed.ComputedStyle"/>.
    /// </summary>
    /// <typeparam name="T1">
    /// Type used for single value properties (ex. width, height, position X and Y).
    /// The concrete type depends on whether the style is raw, parsed, or computed:
    /// <list type="bullet">
    /// <item><description>Raw style -> string</description></item>
    /// <item><description>Parsed style -> typed structure or enum</description></item>
    /// <item><description>Computed style -> fully resolved, pixel value</description></item>
    /// </list>
    /// </typeparam>
    /// <typeparam name="T2">
    /// Type used for side based (4-value) properties (ex. padding, margin, border widths).
    /// The concrete type depends on the style stage (raw, parsed, computed), similar to <typeparamref name="T1"/>.
    /// </typeparam>
    /// <remarks>
    /// The <see cref="IStyle{T1, T2}"/> interface provides a unified API so that 
    /// style properties can be accessed consistently regardless of whether the 
    /// style is raw, parsed, or computed.
    /// 
    /// <para>
    /// <b>Direct interface properties</b> do not require parsing or computation
    /// and can be read as is from any style stage.
    /// </para>
    /// 
    /// Implementations of this interface should ensure that the chosen types for
    /// <typeparamref name="T1"/> and <typeparamref name="T2"/> match the intended
    /// stage of the style.
    /// </remarks>
    public interface IStyle<T1, T2> : IStyleValues<T1>, IStyleSideValues<T2>
    {
        /// <summary>
        /// Specifies whether an element is visible, hidden or <seealso cref="VisibilityType.Transparent"/>
        /// </summary>
        public VisibilityType Visibility { get; set; }

        /// <summary>
        /// Specifies what happens to the overall size and content size of an element
        /// </summary>
        /// <remarks>
        /// depending on the value it adds or maintains set <b>size</b>, shrinks or keeps the <b>content</b> size.
        /// </remarks>
        public SizingType SizingType { get; set; }

        /// <summary>
        /// Sets the color of the elements background.
        /// </summary>
        /// <remarks>
        /// The background will <b>include</b> padding but exclude <b>border, margin</b>
        /// </remarks>
        Color BackgroundColor { get; set; }

        /// <summary>
        /// Sets the color of the elements border.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Sets the color of the elements shadow.
        /// </summary>
        public Color ShadowColor { get; set; }

        /// <summary>
        /// Specifies the positioning mode of an element.
        /// </summary>
        public PositionType PositioningType { get; set; }
    }
}
