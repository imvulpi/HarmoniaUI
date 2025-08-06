using Godot;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.style;
using HarmoniaUI.library.style.interfaces;

namespace HarmoniaUI.library.style
{
    /// <summary>
    /// Describes the visual and layout style for a <see cref="nodes.UINode"/>.
    /// Supports box model, background, border, shadow, and positioning.
    /// Values can use units like 'px', '%', 'vh', 'vw'.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class StyleResource : Resource, IStyle<string, string>
    {
        #region Behaviour

        [ExportSubgroup("Behaviour")]
        [Export]
        public VisibilityType Visibility { get; set; } = VisibilityType.Visible;

        [Export] public LayoutResource LayoutResource { get; set; } = null;
        [Export] public VisualResource VisualResource { get; set; } = null;

        #endregion

        #region Size

        [ExportSubgroup("Size")]
        [Export] public SizingType SizingType { get; set; } = SizingType.Border;
        [Export] public string Width { get; set; }
        [Export] public string Height { get; set; }
        [Export] public string MinWidth { get; set; }
        [Export] public string MinHeight { get; set; }
        [Export] public string MaxWidth { get; set; }
        [Export] public string MaxHeight { get; set; }

        #endregion

        #region Spacing

        [ExportSubgroup("Spacing")]
        [Export] public string Padding { get; set; }
        [Export] public string Margin { get; set; }

        #endregion

        #region Background

        [ExportSubgroup("Background")]
        [Export] public Color BackgroundColor { get; set; } = new Color(0f, 0f, 0f, 0f);

        #endregion

        #region Border

        [ExportSubgroup("Border")]
        [Export] public string BorderRadius { get; set; }
        [Export] public string BorderWidth { get; set; }
        [Export] public Color BorderColor { get; set; }

        #endregion

        #region Shadow

        [ExportSubgroup("Shadow")]
        [Export] public Color ShadowColor { get; set; } = Colors.Transparent;
        [Export] public Vector2 ShadowOffset { get; set; } = Vector2.Zero;

        #endregion

        #region Position

        [ExportSubgroup("Position")]
        [Export] public PositionType PositioningType { get; set; } = PositionType.Normal;
        [Export] public string PositionX { get; set; } = "";
        [Export] public string PositionY { get; set; } = "";

        #endregion
    }
}
