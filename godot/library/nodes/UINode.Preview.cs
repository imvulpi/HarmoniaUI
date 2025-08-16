using Godot;
using HarmoniaUI.Commons;
using HarmoniaUI.Core.Engines.Registry;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Parsed;

namespace HarmoniaUI.Nodes
{
    public partial class UINode : Control
    {
        // This partial class contains code that is only used in Debug and Editor hint.
        // For releases it will get removed by the compilation.

        #region Editor Preview
#if DEBUG
        float intervalMs = 50;
        float elapsedMs = 0;

        // FOR EDITOR PREVIEW
        public override void _Process(double delta)
        {
            if (!Engine.Singleton.IsEditorHint()) return;
            elapsedMs += (float)delta * 1000;
            if (elapsedMs > intervalMs)
            {
                // Weird situation in here, but when rebuilding all of the properties are nullified,
                // We have to return in here because all children need to refresh before doing any layout stuff.
                // DON'T DELETE!!! Only happens in editor
                if (VisualEngine == null)
                {
                    Parent = FindParent();
                    VisualEngine ??= UIEngines.Visual.GetEngine(StyleResource.VisualResource);
                    LayoutEngine ??= UIEngines.Layout.GetEngine(StyleResource.LayoutResource);
                    ParsedStyle ??= StyleParser.Parse(ParsedStyle, StyleResource);
                    ComputedStyle ??= new();
                    Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
                    Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);
                    StyleComputer.Compute(ComputedStyle, ParsedStyle, viewportSize, parentSize);
                }
                else
                {
                    Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
                    Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);

                    ParsedStyle = StyleParser.Parse(ParsedStyle, StyleResource);
                    StyleComputer.Compute(ComputedStyle, ParsedStyle, viewportSize, parentSize);
                    LayoutEngine.ComputeSize(this, ComputedStyle, StyleResource.LayoutResource);
                    LayoutEngine.ApplyLayout(this, ComputedStyle, StyleResource.LayoutResource);
                    QueueRedraw();
                    elapsedMs = 0;
                }
            }
        }
#endif
        #endregion
    }
}
