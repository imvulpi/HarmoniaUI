using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.Core.Engines.Visual
{
    /// <summary>
    /// Defines the contract for visual engines in the HarmoniaUI framework,
    /// responsible for rendering <see cref="UINode"/> instances according to
    /// their computed styles and visual resources.
    /// </summary>
    /// <remarks>
    /// Visual engines handle all drawing logic for HarmoniaUI nodes, integrating
    /// with Godot’s <c>_Draw</c> method to produce the final on-screen visuals.  
    /// 
    /// <para>
    /// Implementations of <see cref="IVisualEngine"/> are typically assigned to a node
    /// through its <see cref="StyleResource.VisualResource"/>. If no custom engine is
    /// provided, a default implementation such as <see cref="BaseVisualEngine"/> is used.
    /// </para>
    /// 
    /// Developers can implement custom visual engines to support specialized rendering
    /// techniques such as vector drawing, procedural textures, custom effects,
    /// or unique theming systems.
    /// </remarks>
    public interface IVisualEngine
    {
        /// <summary>
        /// Renders the specified <paramref name="node"/> using its computed style and visual resource.
        /// </summary>
        /// <param name="node">The Harmonia UI node to draw.</param>
        /// <param name="style">The <see cref="ComputedStyle"/> defining the node's visual properties.</param>
        /// <param name="visual">
        /// The <see cref="VisualResource"/> containing additional custom visual definitions
        /// such as textures, gradients, or other custom values.
        /// </param>
        public void Draw(UINode node, ComputedStyle style, VisualResource visual);
    }
}