using Godot;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Core.Style.Interfaces;

namespace HarmoniaUI.Core.Style.Computed
{
    /// <summary>
    /// Represents the final, fully resolved style for harmonia nodes.
    /// </summary>
    /// <remarks>
    /// <see cref="ComputedStyle"/> contains values from the parsed style that were
    /// converted to pixel values.
    /// 
    /// <para>
    /// It contains all the final visual and layout properties that the
    /// <see cref="ILayoutEngine"/> and <see cref="IVisualEngine"/> use to render and
    /// arrange the nodes in hierarchy.
    /// </para>
    /// 
    /// This is the style state that should be used during layout and drawing passes,
    /// as it reflects the complete visual configuration.
    /// </remarks>
    public class ComputedStyle : IStyle<float, StyleSides<float>>
    {
        public VisibilityType Visibility { get; set; }
        public SizingType SizingType { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float MinWidth { get; set; }
        public float MinHeight { get; set; }
        public float MaxWidth { get; set; }
        public float MaxHeight { get; set; }
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public Color ShadowColor { get; set; }
        public PositionType PositioningType { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public StyleSides<float> BorderRadius { get; set; }
        public StyleSides<float> BorderWidth { get; set; }
        public StyleSides<float> Padding { get; set; }
        public StyleSides<float> Margin { get; set; }
        public float ShadowOffsetX { get; set; }
        public float ShadowOffsetY { get; set; }
    }
}
