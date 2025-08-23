using Godot;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.Core.Engines.Input
{
    /// <summary>
    /// Provides a base implementation of the <see cref="IInputEngine"/> interface,
    /// it handles the _GuiInput and _Input of Harmonia nodes.
    /// </summary>
    /// <remarks>
    /// WARNING: <see cref="BaseInputEngine"/> Isn't doing anything as of right now, but it can be expanded
    /// in the future.
    /// <para>
    /// For developers <see cref="IInputEngine"/> is a nice way to add some custom behaviour on special 
    /// inputs they desire to handle.
    /// </para>
    /// </remarks>
    public class BaseInputEngine : IInputEngine
    {
        public void GuiInput(UINode node, InputEvent input, InputResource inputResource)
        {
            return;
        }

        public void Input(UINode node, InputEvent input, InputResource inputResource)
        {
            return;
        }
    }
}
