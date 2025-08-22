using Godot;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Core.Style.Types;

namespace HarmoniaUI.Core.Style.Raw
{
    /// <summary>
    /// Represents the unprocessed style data for Harmonia nodes
    /// </summary>
    /// <remarks>
    /// <see cref="StyleResource"/> stores the original, unparsed style information 
    /// It contains values, and data in their initial form before 
    /// any type conversion or validation occurs.
    ///
    /// This format is easier to understand and edit in the Godot editor. 
    /// But it requires parsing: <seealso cref="Parsed.ParsedStyle"/>
    /// </remarks>
    [Tool]
    [GlobalClass]
    public partial class StyleResource : Resource, IStyle<string, string>
    {
        #region Behaviour

        [ExportSubgroup("Behaviour")]
        [Export]
        public VisibilityType Visibility { get; set; } = VisibilityType.Unset;

        /// <summary>
        /// A custom layout resource that will be used to retrieve a layout engine.
        /// It's passed 'as is' to the <see cref="Engines.Layout.ILayoutEngine"/> 
        /// associated with the layout resource type
        /// </summary>
        /// <remarks>
        /// You can create a custom <see cref="LayoutResource"/> and a engine supporting it 
        /// by registering it in <see cref="Engines.Registry.UIEngines"/>.
        /// </remarks>
        [Export] public LayoutResource LayoutResource { get; set; } = null;

        /// <summary>
        /// A custom visual resource that will be used to retrieve a visual engine.
        /// It's passed 'as is' to the <see cref="Engines.Visual.IVisualEngine"/>
        /// associated with the visual resource type
        /// </summary>
        /// <remarks>
        /// You can create a custom <see cref="VisualResource"/> and a engine supporting it 
        /// by registering it in <see cref="Engines.Registry.UIEngines"/>.
        /// </remarks>
        [Export] public VisualResource VisualResource { get; set; } = null;

        #endregion

        #region Size

        [ExportSubgroup("Size")]
        [Export] public SizingType SizingType { get; set; } = SizingType.Unset;
        [Export] public string Width { get; set; } = string.Empty;
        [Export] public string Height { get; set; } = string.Empty;
        [Export] public string MinWidth { get; set; } = string.Empty;
        [Export] public string MinHeight { get; set; } = string.Empty;
        [Export] public string MaxWidth { get; set; } = string.Empty;
        [Export] public string MaxHeight { get; set; } = string.Empty;

        #endregion

        #region Spacing

        [ExportSubgroup("Spacing")]
        [Export] public string Padding { get; set; } = string.Empty;
        [Export] public string Margin { get; set; } = string.Empty;

        #endregion

        #region Background

        [ExportSubgroup("Background")]
        [Export] public Color BackgroundColor { get; set; } = StyleDefaults.UnsetColor;

        #endregion

        #region Border

        [ExportSubgroup("Border")]
        [Export] public string BorderRadius { get; set; } = string.Empty;
        [Export] public string BorderWidth { get; set; } = string.Empty;
        [Export] public Color BorderColor { get; set; } = StyleDefaults.UnsetColor;

        #endregion

        #region Shadow

        [ExportSubgroup("Shadow")]
        [Export] public Color ShadowColor { get; set; } = StyleDefaults.UnsetColor;
        [Export] public string ShadowOffsetX { get; set; } = string.Empty;
        [Export] public string ShadowOffsetY { get; set; } = string.Empty; 

        #endregion

        #region Position

        [ExportSubgroup("Position")]
        [Export] public PositionType PositioningType { get; set; } = PositionType.Unset;
        [Export] public string PositionX { get; set; } = string.Empty;
        [Export] public string PositionY { get; set; } = string.Empty; 

        #endregion

        public static StyleResource GetDefault()
        {
            return new()
            {
                Visibility = StyleDefaults.VISIBILITY_TYPE_DEFAULT,
                SizingType = StyleDefaults.SIZING_TYPE_DEFAULT,
                PositioningType = StyleDefaults.POSITION_TYPE_DEFAULT,
                BackgroundColor = Colors.Transparent,
                ShadowColor = Colors.Transparent,
                BorderColor = Colors.Transparent,
            };
        }
    }
}
