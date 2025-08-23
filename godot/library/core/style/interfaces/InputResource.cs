using Godot;

namespace HarmoniaUI.Core.Style.Interfaces
{
    /// <summary>
    /// Defines the input extensions for nodes in HarmoniaUI.
    /// </summary>
    /// <remarks>
    /// <see cref="InputResource"/> acts as an addon object for a specific
    /// <see cref="Engines.Input.IInputEngine"/>, The <see cref="InputResource"/> is also
    /// used to retrieve the engine from <see cref="Engines.Registry.UIEngines"/>
    /// 
    /// <para>
    /// This resource can be assigned in the editor or via code through 
    /// <see cref="Parsed.ParsedStyle.InputResource"/>. If no input resource is assigned, 
    /// or the assigned input resource is not supported by any custom engines the
    /// HarmoniaUI falls back to the default input engine and its default configuration.
    /// </para>
    /// 
    /// Typical usage:
    /// <list type="bullet">
    ///   <item>Assigning a custom input strategy to a node.</item>
    ///   <item>Handling different inputs in a node.</item>
    ///   <item>Doing specific things when a specific input is detected</item>
    /// </list>
    /// 
    /// The resource will be handled by the engines that are registered as supporting them.
    /// </remarks>
    [Tool]
    [GlobalClass]
    public partial class InputResource : Resource
    {

    }
}
