using Godot;

namespace HarmoniaUI.library.style
{

    /// <summary>
    /// A resource for the layout engine. Gets passed to the Layout Engine that's registered with the type.
    /// </summary>
    /// <remarks>
    /// Can be used for passing data to different layout engines.
    /// </remarks>
    [Tool]
    [GlobalClass]
    public partial class LayoutResource : Resource
    {
        // Possibly a service key to differentiate same resources between many engines?
    }
}
