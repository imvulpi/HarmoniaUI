using HarmoniaUI.library.nodes;
using HarmoniaUI.Library.style;

namespace HarmoniaUI.library.core.visual
{
    /// <summary>
    /// Renders the visual representation of Harmonia Nodes based on computer layout and size
    /// </summary>
    /// <remarks>
    /// It renders stuff like colors, borders etc.
    /// </remarks>
    public interface IVisualEngine
    {
        /// <summary>
        /// Renders the visual representations of Harmonia nodes
        /// </summary>
        /// <param name="node">Node to render</param>
        /// <param name="style">Style to follow when rendering</param>
        public void Draw(UINode node, ParsedStyle style);
    }
}