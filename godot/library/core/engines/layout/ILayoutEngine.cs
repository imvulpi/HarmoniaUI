using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.Core.Engines.Layout
{
    /// <summary>
    /// Defines the contract for layout engines in the HarmoniaUI framework,
    /// responsible for measuring and positioning <see cref="UINode"/> instances
    /// according to style and layout rules.
    /// </summary>
    /// <remarks>
    /// Layout engines are a core part of HarmoniaUI's rendering,
    /// transforming style and layout resources into concrete sizes and positions
    /// for UI nodes and their children
    ///
    /// Implementations of <see cref="ILayoutEngine"/> are typically assigned
    /// to a node via its <see cref="StyleResource.LayoutResource"/>. If no
    /// custom engine is provided, a default implementation (ex., 
    /// <see cref="BaseLayoutEngine"/>) is used.
    /// </remarks>
    public interface ILayoutEngine
    {
        /// <summary>
        /// Calculates and sets the size of a Harmonia <see cref="UINode"/> based on the provided style and layout resource.
        /// </summary>
        /// <param name="node">The Harmonia UI node whose size is to be computed and set.</param>
        /// <param name="style">The <see cref="ComputedStyle"/> instance containing style properties to apply to the node.</param>
        /// <param name="layout">
        /// The <see cref="LayoutResource"/> providing layout specific parameters and constraints
        /// used during size calculation. This resource defines custom layout rules like spacing, alignment, or sizing policies. Not
        /// all engines handle all resources, most engines handle 1 or 2.
        /// </param>
        /// <remarks>
        /// This method uses style information combined with layout rules to determine the node's size AND content size,
        /// which may differ from the size. The computed content size influences children layout passes
        /// and positioning of child nodes.
        /// </remarks>
        public void ComputeSize(UINode node, ComputedStyle style, LayoutResource layout);

        /// <summary>
        /// Applies the computed positions to the specified <paramref name="node"/> and its direct children
        /// based on the given style and layout resource.
        /// </summary>
        /// <param name="node">The Harmonia UI node to position along with its immediate child nodes.</param>
        /// <param name="style">The <see cref="ComputedStyle"/> that defines layout style properties that are used during positioning.</param>
        /// <param name="layout">
        /// The <see cref="LayoutResource"/> that provides layout specific rules and parameters
        /// for positioning and arrangement of the node and its children.
        /// </param>
        /// <remarks>
        /// This method assigns the final layout positions to the node and its children, completing the layout pass.
        /// It respects constraints and alignment rules defined in the style and layout resource.
        /// </remarks>
        public void ApplyLayout(UINode node, ComputedStyle style, LayoutResource layout);
    }
}
