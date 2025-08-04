using Godot;
using System;

namespace HarmoniaUI.library.core.commons
{
    /// <summary>
    /// Helper class to help with godots <see cref="Viewport"/>
    /// </summary>
    public static class ViewportHelper
    {
        /// <summary>
        /// Gets the viewport size. At DEBUG in the editor hint it gets the editor window size.
        /// </summary>
        /// <param name="control">Node used to retrieve viewport rect</param>
        /// <returns>Viewport size</returns>
        public static Vector2 GetViewportSize(Control control)
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
                return control.GetViewportRect().Size;
            }
#else
                return control.GetViewportRect().Size;
#endif
        }
    }
}
