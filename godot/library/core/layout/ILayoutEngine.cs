using HarmoniaUI.library.nodes;
using HarmoniaUI.library.style;

namespace HarmoniaUI.library.core.layout
{
    /// <summary>
    /// Calculates layout and size of the Harmonia Nodes, applies the changes to the Harmonia nodes.
    /// /// </summary>
    public interface ILayoutEngine
    {
        /// <summary>
        /// Calculates and sets the size of a Harmonia Node
        /// </summary>
        /// <param name="node">Harmonia node to set size in</param>
        /// <param name="style">Style to apply for the node, usually style of the <paramref name="node"/></param>
        public void ComputeSize(UINode node, ComputedStyle style);

        /// <summary>
        /// Applies positions of the <paramref name="node"/> and its direct children.
        /// </summary>
        /// <param name="node">Harmonia node to position</param>
        /// <param name="style">Style to follow when applying layout, usually style of the <paramref name="node"/></param>
        public void ApplyLayout(UINode node, ComputedStyle style);
    }
}
