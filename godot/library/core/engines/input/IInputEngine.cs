using Godot;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.Core.Engines.Input
{
    /// <summary>
    /// Defines the contract for input engines in HarmoniaUI, it allows for handling
    /// of events in Harmonia nodes.
    /// </summary>
    /// <remarks>
    ///
    /// Implementations of <see cref="IInputEngine"/> are typically assigned
    /// to a node via its <see cref="InputResource"/>. If no
    /// custom engine is provided, a default implementation (ex., 
    /// <see cref="BaseInputEngine"/>) is used.
    /// </remarks>
    public interface IInputEngine
    {
        /// <summary>
        /// Extends the <see cref="Node._Input(InputEvent)"/> handling in HarmoniaUI Nodes
        /// </summary>
        /// <param name="node">Node of which event is to be handled</param>
        /// <param name="input">Event to handled</param>
        /// <param name="inputResource">Custom input resource in the current node style</param>
        public void Input(UINode node, InputEvent input, InputResource inputResource);

        /// <summary>
        /// Extends the <see cref="Control._GuiInput(InputEvent)"/> handling in HarmoniaUI Nodes
        /// </summary>
        /// <param name="node">Node of which event is to be handled</param>
        /// <param name="input">Event to handled</param>
        /// <param name="inputResource">Custom input resource in the current node style</param>
        public void GuiInput(UINode node, InputEvent input, InputResource inputResource);
    }
}
