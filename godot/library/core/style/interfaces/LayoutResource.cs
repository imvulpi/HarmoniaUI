using Godot;

namespace HarmoniaUI.Core.Style.Interfaces
{
    /// <summary>
    /// Defines the layout specific configuration for nodes in HarmoniaUI.
    /// </summary>
    /// <remarks>
    /// <see cref="LayoutResource"/> acts as an addon configuration object for a specific
    /// <see cref="Engines.Layout.ILayoutEngine"/>, The <see cref="LayoutResource"/> is also
    /// used to retrieve the engine from <see cref="Engines.Registry.UIEngines"/>
    /// 
    /// <para>
    /// This resource can be assigned in the editor or via code through 
    /// <see cref="Raw.StyleResource.LayoutResource"/>. If no layout resource is assigned, 
    /// or the assigned layout resource is not supported by any custom engines the
    /// HarmoniaUI falls back to the default layout engine and its default configuration.
    /// </para>
    /// 
    /// Typical usage:
    /// <list type="bullet">
    ///   <item>Assigning a custom layout strategy to a node.</item>
    ///   <item>Configuring spacing, alignment, and flow rules for independent layout engines.</item>
    ///   <item>Switching between different layout modes without modifying node code.</item>
    /// </list>
    /// 
    /// The resource will be handled by the engines that are registered supporting them.
    /// </remarks>
    [Tool]
    [GlobalClass]
    public partial class LayoutResource : Resource
    {

    }
}
