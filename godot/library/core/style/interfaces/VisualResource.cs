using Godot;

namespace HarmoniaUI.Core.Style.Interfaces
{
    /// <summary>
    /// Defines the visual styling configuration for nodes in HarmoniaUI.
    /// </summary>
    /// <remarks>
    /// <see cref="VisualResource"/> contains rendering settings that determine how
    /// the node is drawn by an <see cref="Engines.Visual.IVisualEngine"/>.  
    /// 
    /// Unlike <see cref="LayoutResource"/>, which controls positioning and sizing,  
    /// <see cref="VisualResource"/> focuses solely on appearance, including background fills,
    /// borders, shadows, gradients, and other visual decorations.
    /// 
    /// This resource can be assigned in the editor or via code through 
    /// <see cref="Raw.StyleResource.VisualResource"/>. If no visual resource is assigned, 
    /// or the assigned visual resource is not supported by any custom engines the
    /// HarmoniaUI falls back to the default visual engine with its default settings.
    /// 
    /// Typical usage:
    /// <list type="bullet">
    ///   <item>Defining background and border rendering styles.</item>
    ///   <item>Applying shadows, gradients, and other effects.</item>
    ///   <item>Creating reusable visual presets for multiple nodes.</item>
    /// </list>
    /// 
    /// Visual resources are consumed during the node's <see cref="CanvasItem._Draw"/> phase,  
    /// where the active <see cref="IVisualEngine"/> applies the configured appearance.
    /// 
    /// <para>
    /// <b>Note:</b> Visual resources should contain only data - the actual drawing is
    /// handled entirely by the assigned visual engine.
    /// </para>
    /// </remarks>
    [Tool]
    [GlobalClass]
    public partial class VisualResource : Resource
    {
    }
}
