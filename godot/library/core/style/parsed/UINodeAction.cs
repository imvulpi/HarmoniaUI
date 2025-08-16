using System;

namespace HarmoniaUI.Core.Style.Parsed
{
    /// <summary>
    /// Describes what actions the UI Node can perform. 
    /// Bit fields enum, all values are stored in seperate bits,
    /// and multiple values can be applied at the same time.
    /// </summary>
    /// <remarks>
    /// Often used to suggest what action the node should perform.
    /// </remarks>
    [Flags]
    public enum UINodeAction
    {
        /// <summary>
        /// The node doesn't need to perform any action
        /// </summary>
        None = 0,
        /// <summary>
        /// The node should call for a redraw (using <see cref="Godot.CanvasItem.QueueRedraw"/>
        /// </summary>
        Redraw = 1,
        /// <summary>
        /// The node should call for updating the layout
        /// </summary>
        Relayout = 2,
    }
}