using Godot;
using System;

namespace HarmoniaUI.Commons
{
    /// <summary>
    /// Helpful utilities for Godot <see cref="Viewport"/>
    /// </summary>
    public static class ViewportHelper
    {
        /// <summary>
        /// Gets the size of <see cref="Viewport"/> visible rect
        /// </summary>
        /// <remarks>
        /// In the editor when in debug mode it returns the Project settings width and height, 
        /// so that the viewport matches the editor preview window.
        /// </remarks>
        /// <param name="node">Node used to access the viewport methods</param>
        /// <returns>Size in Vector2 where x is width and y is height</returns>
        public static Vector2 GetViewportSize(Node node)
        {
#if DEBUG
            if (Engine.Singleton.IsEditorHint())
            {
                var vwObj = ProjectSettings.Singleton.GetSetting("display/window/size/viewport_width").Obj;
                var vhObj = ProjectSettings.Singleton.GetSetting("display/window/size/viewport_height").Obj;
                float vw = Convert.ToSingle(vwObj);
                float vh = Convert.ToSingle(vhObj);
                return new Vector2(vw, vh);
            }
            else
            {
                return node.GetTree().Root.GetViewport().GetVisibleRect().Size;
            }
#else
                return node.GetTree().Root.GetViewport().GetVisibleRect().Size;
#endif
        }
    }
}
