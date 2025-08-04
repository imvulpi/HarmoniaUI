using Godot;
using HarmoniaUI.library.core.commons;
using HarmoniaUI.library.core.layout;
using HarmoniaUI.library.core.visual;
using HarmoniaUI.library.nodes.godot;
using HarmoniaUI.Library.style;

namespace HarmoniaUI.library.nodes
{
    [Tool]
    [GlobalClass]
    public partial class UINode : Control
    {
        [Export] private StyleResource StyleResource { get; set; } = new();
        public ParsedStyle ParsedStyle { get; set; }

        public virtual float ContentWidth { get; set; }
        public virtual float ContentHeight { get; set; }
        
        public UINode Parent { get; set; }

        public IVisualEngine VisualEngine { get; set; }
        public ILayoutEngine LayoutEngine { get; set; }

        public override void _EnterTree()
        {
            Parent = FindParent();
            Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
            Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);
            ParsedStyle = new ParsedStyle();
            ParsedStyle.Update(StyleResource, viewportSize, parentSize);
            ParsedStyle.Changed += StyleChanged;
            VisualEngine = VisualEngineRegistry.GetEngine(StyleResource.VisualResource);
            LayoutEngine = LayoutEngineRegistry.GetEngine(StyleResource.LayoutResource);
        }

        private void StyleChanged(StyleChangeType obj)
        {
            if(obj == StyleChangeType.Background || obj == StyleChangeType.Border)
            {
                QueueRedraw();
            }
        }

        public override void _Ready()
        {
            LayoutEngine.ComputeSize(this, ParsedStyle);
            LayoutEngine.ApplyLayout(this, ParsedStyle);
        }

        public override void _Draw()
        {
            VisualEngine.Draw(this, ParsedStyle);
        }

#if DEBUG
        float intervalMs = 100;
        float elapsedMs = 0;

        // FOR EDITOR PREVIEW
        public override void _Process(double delta)
        {
            elapsedMs += (float)delta*1000;
            if (elapsedMs > intervalMs)
            {
                // Weird situation in here, but when rebuilding all of the properties are nullified,
                // We have to return in here because all children need to refresh before doing any layout stuff.
                // DON'T DELETE!!! Only happens in editor
                if (VisualEngine == null)
                {
                    Parent = FindParent();
                    VisualEngine ??= VisualEngineRegistry.GetEngine(StyleResource.VisualResource);
                    LayoutEngine ??= LayoutEngineRegistry.GetEngine(StyleResource.LayoutResource);
                    ParsedStyle ??= new ParsedStyle();
                    return;
                }

                Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
                Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);
                ParsedStyle.Update(StyleResource, viewportSize, parentSize);

                LayoutEngine.ComputeSize(this, ParsedStyle);
                LayoutEngine.ApplyLayout(this, ParsedStyle);
                QueueRedraw();
                elapsedMs = 0;
            }
        }
#endif

        public UINode FindParent()
        {
            Node parent = GetParent();
            if (parent != null)
            {
                if(parent is UINode harmoniaNode)
                {
                    return harmoniaNode;
                }
                else if(parent is Control controlNode)
                {
                    return new ControlUIWrapper(controlNode);
                }
            }
            return null;
        }
    }
}