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

        [ExportCategory("Editor preview - Debug only")]

        private bool _hoverPreview;
        private bool _focusPreview;
        
        [Export]
        private bool Preview_OnHover { get => _hoverPreview; set
            {
                if (!Engine.IsEditorHint()) return;
                _hoverPreview = value;
                IsHovered = value;
                Preview_UpdateStyle();
            }
        }
        [Export]
        private bool Preview_OnFocus { get => _focusPreview; set
            {
                if (!Engine.IsEditorHint()) return;
                _focusPreview = value;
                IsFocused = value;
                Preview_UpdateStyle();
            }
        }

        private void Preview_UpdateStyle()
        {
            HandleInteractions();
            RawCurrentStyle ??= RawNormalStyle;
        }


        private float preview_intervalMs = 50;
        private float preview_elapsedMs = 0;

        // FOR EDITOR PREVIEW
        public override void _Process(double delta)
        {
            if (!Engine.Singleton.IsEditorHint()) return;
            preview_elapsedMs += (float)delta * 1000;
            if (preview_elapsedMs > preview_intervalMs)
            {
                Preview_UpdateStyle();

                // Weird situation in here, but when rebuilding all of the properties are nullified,
                // We have to return in here because all children need to refresh before doing any layout stuff.
                // DON'T DELETE!!! Only happens in editor
                if (VisualEngine == null)
                {
                    Preview_EnterTree();
                    Preview_UpdateStyle();
                }
                else
                {
                    Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
                    Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);

                    NormalStyle = StyleParser.Parse(NormalStyle, RawNormalStyle);
                    HoveredStyle = StyleParser.Parse(HoveredStyle, RawHoveredStyle);
                    FocusedStyle = StyleParser.Parse(FocusedStyle, RawFocusedStyle);
                    StyleComputer.Compute(ComputedStyle, CurrentStyle, viewportSize, parentSize);
                    LayoutEngine.ComputeSize(this, ComputedStyle, RawCurrentStyle.LayoutResource);
                    LayoutEngine.ApplyLayout(this, ComputedStyle, RawCurrentStyle.LayoutResource);
                    QueueRedraw();
                    preview_elapsedMs = 0;
                }
            }
        }

        private void Preview_EnterTree()
        {
            SetLayoutToPosition();
            Parent = FindParent();
            RawNormalStyle ??= new();
            NormalStyle = new();
            ComputedStyle = new();

            NormalStyle = StyleParser.Parse(NormalStyle, RawNormalStyle);
            HoveredStyle = StyleParser.Parse(HoveredStyle, RawHoveredStyle);
            FocusedStyle = StyleParser.Parse(FocusedStyle, RawFocusedStyle);

            Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
            Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);

            CurrentStyle = NormalStyle;
            RawCurrentStyle = RawNormalStyle;

            StyleComputer.Compute(ComputedStyle, NormalStyle, viewportSize, parentSize);
            VisualEngine = UIEngines.Visual.GetEngine(RawNormalStyle.VisualResource);
            LayoutEngine = UIEngines.Layout.GetEngine(RawNormalStyle.LayoutResource);
            LayoutEngine.ComputeSize(this, ComputedStyle, RawNormalStyle.LayoutResource);
        }
#endif
        #endregion
    }
}
