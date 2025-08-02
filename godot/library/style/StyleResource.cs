using Godot;

namespace HarmoniaUI.Library.style
{
    /// <summary>
    /// Describes the visual and layout style for a <see cref="nodes.UINode"/>.
    /// Supports box model, background, border, shadow, and positioning.
    /// Values can use units like 'px', '%', 'vh', 'vw'.
    /// </summary>
    [Tool]
    [GlobalClass]
    public partial class StyleResource : Resource
    {
        #region Behaviour

        /// <summary>
        /// Defines visibility of the element
        /// </summary>
        [ExportSubgroup("Behaviour")]
        [Export]
        public VisibilityType Visibility { get; set; }

        #endregion

        #region Size

        /// <summary>
        /// Sizing type of the element.
        /// </summary>
        [ExportSubgroup("Size")]
        [Export] public SizingType SizingType { get; set; }

        /// <summary>
        /// Width of the element (e.g. "100px", "50%", , leave empty for auto).
        /// </summary>
        [Export] public string Width = "";

        /// <summary>
        /// Height of the element (e.g. "100px", "50%", , leave empty for auto).
        /// </summary>
        [Export] public string Height = "";

        /// <summary>
        /// Minimum width of the element.
        /// </summary>
        [Export] public string MinWidth = "";

        /// <summary>
        /// Minimum height of the element.
        /// </summary>
        [Export] public string MinHeight = "";


        /// <summary>
        /// Maximum width of the element.
        /// </summary>
        [Export] public string MaxWidth = "";

        /// <summary>
        /// Maximum height of the element.
        /// </summary>
        [Export] public string MaxHeight = "";

        #endregion

        #region Spacing

        /// <summary>
        /// Padding inside the element (space between content and border).
        /// </summary>
        [ExportSubgroup("Spacing")]
        [Export] public string Padding = "";

        /// <summary>
        /// Margin outside the element (space between element and siblings).
        /// </summary>
        [Export] public string Margin = "";

        #endregion

        #region Background

        /// <summary>
        /// Background color of the element.
        /// </summary>
        [ExportSubgroup("Background")]
        [Export] public Color BackgroundColor = new Color(0.15f, 0.15f, 0.15f);

        #endregion

        #region Border

        /// <summary>
        /// Radius for rounding corners (e.g. "8px", "50%").
        /// </summary>
        [ExportSubgroup("Border")]
        [Export] public string BorderRadius = "";

        /// <summary>
        /// Width of the border (e.g. "2px").
        /// </summary>
        [Export] public string BorderWidth = "";

        /// <summary>
        /// Color of the border.
        /// </summary>
        [Export] public Color BorderColor = Colors.Transparent;

        #endregion

        #region Shadow

        [ExportSubgroup("Shadow")]
        /// <summary>
        /// Color of the box shadow.
        /// </summary>
        [Export] public Color ShadowColor = Colors.Transparent;

        /// <summary>
        /// Offset for the shadow in local pixels (x, y).
        /// </summary>
        [Export] public Vector2 ShadowOffset = Vector2.Zero;

        #endregion

        #region Position

        [ExportSubgroup("Position")]

        /// <summary>
        /// Positioning mode: Normal (flow), Relative (offset), or Absolute (manual).
        /// </summary>
        [Export] public PositionType PositioningType = PositionType.Normal;

        /// <summary>
        /// Horizontal position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        [Export] public string PositionX { get; set; } = "";

        /// <summary>
        /// Vertical position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        [Export] public string PositionY { get; set; } = "";

        #endregion
    }
}
