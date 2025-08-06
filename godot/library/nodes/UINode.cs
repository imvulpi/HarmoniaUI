using Godot;
using HarmoniaUI.library.core.commons;
using HarmoniaUI.library.core.layout;
using HarmoniaUI.library.core.visual;
using HarmoniaUI.library.nodes.godot;
using HarmoniaUI.library.style;
using System.Text.Json.Nodes;

namespace HarmoniaUI.library.nodes
{
    [Tool]
    [GlobalClass]
    public partial class UINode : Control
    {
        [Export] private StyleResource StyleResource { get; set; } = new();
        public ParsedStyle ParsedStyle { get; set; }
        public ComputedStyle ComputedStyle { get; set; }

        public virtual float ContentWidth { get; set; }
        public virtual float ContentHeight { get; set; }
        
        public UINode Parent { get; set; }

        public IVisualEngine VisualEngine { get; set; }
        public ILayoutEngine LayoutEngine { get; set; }

        public override void _EnterTree()
        {
            SetLayoutToPosition();
            Parent = FindParent();
            Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
            Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);
            ParsedStyle = ParsedStyle.From(StyleResource);
            ComputedStyle = new();
            ComputedStyle.UpdateFrom(ParsedStyle, this);
            ParsedStyle.Changed += StyleChanged;
            VisualEngine = VisualEngineRegistry.GetEngine(StyleResource.VisualResource);
            LayoutEngine = LayoutEngineRegistry.GetEngine(StyleResource.LayoutResource);
            LayoutEngine.ComputeSize(this, ComputedStyle);

            if (IsRoot())
            {
                GetViewport().SizeChanged += ViewportSizeChange;
            }
        }

        public override void _Ready() => Layout();
        public override void _Draw() => VisualEngine.Draw(this, ComputedStyle);
        public override void _ExitTree()
        {
            if (IsRoot())
                GetViewport().SizeChanged -= ViewportSizeChange;
        }

        private void ViewportSizeChange() {
            GD.Print($"{Name} Updating because of viewport change");
            Layout();
        }
        private void StyleChanged(StyleChangeType obj)
        {
            if (obj.HasFlag(StyleChangeType.Relayout))
            {
                Layout();
                return; // Layout queues redraw that's why we ignore Redraw
            }
            if (obj.HasFlag(StyleChangeType.Redraw)) QueueRedraw();
        }

        public void Layout()
        {
            ComputedStyle.UpdateFrom(ParsedStyle, this);
            LayoutEngine.ComputeSize(this, ComputedStyle);
            foreach (var child in GetChildren())
            {
                if(child is UINode harmonia)
                {
                    harmonia.Layout();
                }
            }

            LayoutEngine.ApplyLayout(this, ComputedStyle);
            QueueRedraw();
        }

        public UINode FindParent()
        {
            Node parent = GetParent();
            if (parent != null)
            {
                if (parent is UINode harmoniaNode)
                {
                    return harmoniaNode;
                }
                else if (parent is Control controlNode)
                {
                    return new ControlUIWrapper(controlNode);
                }
            }
            return null;
        }

        public bool IsRoot()
        {
            Node current = GetParent();

            while (current != null)
            {
                if (current is CanvasLayer)
                    break;

                if (current is UINode)
                    return false;

                current = current.GetParent();
            }

            return true;
        }

        public static UINode FindRoot(UINode node)
        {
            Node current = node;
            UINode lastUINode = node;

            while (current.GetParent() != null)
            {
                current = current.GetParent();

                if (current is CanvasLayer)
                    break;

                if (current is UINode uiBase)
                    lastUINode = uiBase;
            }

            return lastUINode;
        }

        private void SetLayoutToPosition()
        {
            SetAnchor(Side.Left, 0);
            SetAnchor(Side.Right, 0);
            SetAnchor(Side.Top, 0);
            SetAnchor(Side.Bottom, 0);
        }


#if DEBUG
        float intervalMs = 50;
        float elapsedMs = 0;

        // FOR EDITOR PREVIEW
        public override void _Process(double delta)
        {
            if (!Engine.Singleton.IsEditorHint()) return;
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
                    ParsedStyle ??= ParsedStyle.From(StyleResource);
                    ComputedStyle ??= new();
                    ComputedStyle.UpdateFrom(ParsedStyle, this);
                    return;
                }

                ParsedStyle = ParsedStyle.From(StyleResource);
                ComputedStyle.UpdateFrom(ParsedStyle, this);
                LayoutEngine.ComputeSize(this, ComputedStyle);
                LayoutEngine.ApplyLayout(this, ComputedStyle);
                QueueRedraw();
                elapsedMs = 0;
            }
        }
#endif
    }
}