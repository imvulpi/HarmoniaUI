using Godot;
using HarmoniaUI.Commons;
using HarmoniaUI.Core.Engines.Layout;
using HarmoniaUI.Core.Engines.Registry;
using HarmoniaUI.Core.Engines.Visual;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Raw;
using HarmoniaUI.Core.Style.Merger;
using System;
using HarmoniaUI.Core.Engines.Input;

namespace HarmoniaUI.Nodes
{
    /// <summary>
    /// Base class for all HarmoniaUI nodes, extending Godot's <see cref="Control"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="UINode"/> serves as the foundation for all HarmoniaUI UI elements
    /// and can be applied to any Godot <see cref="Control"/> node.  
    /// It provides a unified styling, layout, and rendering system on top of Godot's
    /// native UI, making it possible to use HarmoniaUI features with both Harmonia
    /// and native Godot controls.
    /// </para>
    /// 
    /// <para>
    /// A <see cref="UINode"/> can be used directly or subclassed
    /// The <see cref="Parent"/> property ensures parent references always point to
    /// a <see cref="UINode"/> instance or <c>null</c>.
    /// </para>
    /// 
    /// <para>
    /// Key features:
    /// <list type="bullet">
    /// <item><description>CSS styling system with <see cref="StyleResource"/>. (But with Types)</description></item>
    /// <item><description>Parsed and computed styles.</description></item>
    /// <item><description>Separate content size (<see cref="ContentWidth"/>, <see cref="ContentHeight"/>) from actual node size.</description></item>
    /// <item><description>Customizable layout via <see cref="ILayoutEngine"/>.</description></item>
    /// <item><description>Customizable rendering via <see cref="IVisualEngine"/>.</description></item>
    /// <item><description>Designed for extensibility (custom input and animations are planned).</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// The <see cref="StyleResource"/> is also exported to the Godot editor, giving designers
    /// easy, direct control over visuals and layout without code changes.
    /// </para>
    /// <para>
    /// <see cref="UINode"/> supports an almost instant live preview in the editor,
    /// updating at 50 ms intervals by default (<c><see cref="preview_intervalMs"/> = 50</c>).  
    /// This preview is only active in Debug mode and inside the editor, and is disabled in
    /// Release builds for performance and reduced overhead.
    /// </para>
    /// </remarks>
    /// <h2>Creating Custom Nodes</h2>
    /// <para>
    /// Extending <see cref="UINode"/> is straightforward, but you should ensure that
    /// overridden methods either call their base implementation or provide equivalent
    /// functionality, so that the styling, layout, and rendering systems remain functional.
    /// </para>
    /// <para>
    /// The following members can be overridden:
    /// <list type="bullet">
    /// <item><description><c><see cref="UpdateLayout"/></c></description></item>
    /// <item><description><c><see cref="ApplyStyle"/></c></description></item>
    /// <item><description><c><see cref="_Draw"/></c></description></item>
    /// <item><description><c><see cref="_Ready"/></c></description></item>
    /// <item><description><c><see cref="_EnterTree"/></c></description></item>
    /// <item><description><c><see cref="_ExitTree"/></c></description></item>
    /// <item><description><see cref="ContentWidth"/></description></item>
    /// <item><description><see cref="ContentHeight"/></description></item>
    /// <item><description>And more...</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Remember: <see cref="ContentWidth"/> and <see cref="ContentHeight"/> are <c>virtual</c>, 
    /// They're used to set Size for children nodes, <c>100%</c> in children style is 100% of 
    /// <see cref="ContentWidth"/> or <see cref="ContentHeight"/>
    /// </para>
    /// <para>
    /// To extend the behavior of <see cref="UINode"/>, you can create custom <see cref="LayoutResource"/>
    /// or <see cref="VisualResource"/> classes and implement corresponding <see cref="ILayoutEngine"/>
    /// or <see cref="IVisualEngine"/> instances.  
    /// This allows you to define entirely new layouts or rendering that integrate
    /// seamlessly with the HarmoniaUI styling and node system.
    /// </para>
    /// <h2>When to Add New Nodes vs. Extend Layout/Visual Engines</h2>
    /// <para>
    /// In HarmoniaUI, deciding whether to create a new <see cref="UINode"/> subclass
    /// or extend layout/visual engines depends on your customization:
    /// </para>
    /// <list type="bullet">
    /// <item><description>
    /// <b>Add a new node subclass</b> when you need new interactive behaviors,
    /// unique properties, or specific control logic tied to a UI element.
    /// For example, buttons, sliders, or custom containers.
    /// </description></item>
    /// <item><description>
    /// <b>Extend layout engines</b> to implement new ways of measuring,
    /// positioning, or sizing nodes. This is ideal when you want to change
    /// layout behavior but not the individual node's logic. it can apply to all <see cref="UINode"/>
    /// </description></item>
    /// <item><description>
    /// <b>Extend visual engines</b> when you want to change how nodes are rendered,
    /// such as custom drawings, effects, or new styles that apply across
    /// multiple node types, beause it can apply to all <see cref="UINode"/>
    /// </description></item>
    /// </list>

    [Tool]
    [GlobalClass]
    public partial class UINode : Control
    {
        /// <summary>
        /// The style resource applied to a <see cref="UINode"/>, contains layout and visual style properties.
        /// Exported to the Godot editor for easy editing and preview.
        /// </summary>
        [Export] private StyleResource RawNormalStyle { get; set; } = StyleResource.GetDefault();
        
        /// <summary>
        /// The style resource applied to a <see cref="UINode"/> <b>during mouse hovering</b>, contains layout and visual style properties.
        /// Exported to the Godot editor for easy editing and preview.
        /// </summary>
        [Export] private StyleResource RawHoveredStyle { get; set; } = null;

        /// <summary>
        /// The style resource applied to a <see cref="UINode"/> <b>during node focus</b>, contains layout and visual style properties.
        /// Exported to the Godot editor for easy editing and preview.
        /// </summary>
        [Export] private StyleResource RawFocusedStyle { get; set; } = null;

        /// <summary>
        /// The style resource applied to a <see cref="UINode"/> <b>during node press</b>, contains layout and visual style properties.
        /// Exported to the Godot editor for easy editing and preview.
        /// </summary>
        [Export] private StyleResource RawPressedStyle { get; set; } = null;


        /// <summary>
        /// The parsed representation of <see cref="RawNormalStyle"/>, <b>used for setting styles in code</b>b>.
        /// </summary>
        public ParsedStyle NormalStyle { get; set; }

        /// <summary>
        /// The parsed representation of <see cref="RawHoveredStyle"/>, <b>used for setting styles in code</b> when node is hovered on.
        /// </summary>
        public ParsedStyle HoveredStyle { get; set; }

        /// <summary>
        /// The parsed representation of <see cref="RawFocusedStyle"/>, <b>used for setting styles in code</b> when node is focused on.
        /// </summary>
        public ParsedStyle FocusedStyle { get; set; }

        /// <summary>
        /// The parsed representation of <see cref="RawPressedStyle"/>, <b>used for setting styles in code</b> when node is pressed on.
        /// </summary>
        public ParsedStyle PressedStyle { get; set; }

        /// <summary>
        /// The current parsed style, can be multiple parsed styles merged. Mostly for reading, but can be changed. 
        /// </summary>
        /// <remarks>
        /// <b>Changing this won't apply styles long term</b>, change other style properties for applying new styles.
        /// </remarks>
        public ParsedStyle CurrentStyle { get; set; }
        
        /// <summary>
        /// The current Raw style (with strings), mostly for preview and accesing custom Resources.
        /// </summary>
        /// <remarks>
        /// <b>DON'T</b> use to set values.
        /// </remarks>
        public StyleResource RawCurrentStyle { get; set; }

        /// <summary>
        /// The fully computed style after resolving sizing from parent nodes. Values are in pixels
        /// </summary>
        public ComputedStyle ComputedStyle { get; set; }

        /// <summary>
        /// The content width of the node's content, the max width children can occupy (without overflowing)
        /// </summary>
        public virtual float ContentWidth { get; set; }

        /// <summary>
        /// The content height of the node's content, the max height children can occupy (without overflowing)
        /// </summary>
        public virtual float ContentHeight { get; set; }

        /// <summary>
        /// The <see cref="UINode"/> parent of this node in the hierarchy.
        /// Can be <c>null</c> if this node is a root or detached.
        /// </summary>
        public UINode Parent { get; set; }

        /// <summary>
        /// The visual engine responsible for drawing this node during Godot's <c>_Draw</c> phase.
        /// Can be replaced to customize rendering behavior.
        /// </summary>
        public IVisualEngine VisualEngine { get; set; }

        /// <summary>
        /// The layout engine responsible for measuring and arranging this node and its children.
        /// Can be replaced to customize layout algorithms.
        /// </summary>
        public ILayoutEngine LayoutEngine { get; set; }

        /// <summary>
        /// The layout engine responsible for measuring and arranging this node and its children.
        /// Can be replaced to customize layout algorithms.
        /// </summary>
        public IInputEngine InputEngine { get; set; }

        /// <summary>
        /// Whether the node is currently focused on.
        /// </summary>
        public bool IsFocused { get; private set; } = false;

        /// <summary>
        /// Whether the node is currently hovered on.
        /// </summary>
        public bool IsHovered { get; private set; } = false;

        /// <summary>
        /// Whether the node is currently pressed.
        /// </summary>
        public bool IsPressed { get; private set; } = false;

        /// <summary>
        /// Event that invokes whenever the node is pressed.
        /// </summary>
        public event Action<UINode, InputEvent> Pressed;

        /// <summary>
        /// Event that invokes whenever the node is released (not pressed anymore)
        /// </summary>
        public event Action<UINode, InputEvent> Released;

        /// <summary>
        /// Cached style for merging, used to limit allocations.
        /// </summary>
        private ParsedStyle _mergingCache = new();

        /// <summary>
        /// In the <see cref="UINode"/> it sets defaults, creates, parses and computes styles, gets engines and computes sizes.
        /// </summary>
        public override void _EnterTree()
        {
#if DEBUG
            if (Engine.IsEditorHint())
            {
                Preview_EnterTree();
                return;
            }
#endif
            SetLayoutToPosition();
            Parent = FindParent();
            RawNormalStyle ??= new();
            NormalStyle = new();
            ComputedStyle = new();
            
            NormalStyle = StyleParser.Parse(NormalStyle, RawNormalStyle);
            HoveredStyle = StyleParser.Parse(HoveredStyle, RawHoveredStyle);
            FocusedStyle = StyleParser.Parse(FocusedStyle, RawFocusedStyle);
            PressedStyle = StyleParser.Parse(PressedStyle, RawPressedStyle);

            Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
            Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);

            CurrentStyle = NormalStyle;
            RawCurrentStyle = RawNormalStyle;

            StyleComputer.Compute(ComputedStyle, NormalStyle, viewportSize, parentSize);
            VisualEngine = UIEngines.Visual.GetEngine(CurrentStyle.VisualResource);
            LayoutEngine = UIEngines.Layout.GetEngine(CurrentStyle.LayoutResource);
            InputEngine = UIEngines.Input.GetEngine(CurrentStyle.InputResource);
            GD.Print($"{LayoutEngine.GetType()}");
            LayoutEngine.ComputeSize(this, ComputedStyle, RawNormalStyle.LayoutResource);

            NormalStyle.Changed += StyleChanged;
            if(HoveredStyle != null) HoveredStyle.Changed += StyleChanged;
            if(FocusedStyle != null) FocusedStyle.Changed += StyleChanged;
            if(PressedStyle != null) PressedStyle.Changed += StyleChanged;

            if (IsRoot()) GetViewport().SizeChanged += ViewportSizeChange;

            MouseEntered += HandleMouseEntered;
            MouseExited += HandleMouseExited;
            FocusEntered += HandleFocusEntered;
            FocusExited += HandleFocusExited;
            Pressed += UINode_Pressed;
            Released += UINode_Released;
        }

        /// <summary>
        /// Handles the hover, focus and press interactions, merges those styles when active.
        /// </summary>
        private void HandleInteractions()
        {
            CurrentStyle = NormalStyle;

            if (IsHovered) 
            {
                CurrentStyle = StyleMerger.Merge(CurrentStyle, HoveredStyle, ref _mergingCache);
            }

            if (IsFocused)
            {
                CurrentStyle = StyleMerger.Merge(CurrentStyle, FocusedStyle, ref _mergingCache);
            }

            if (IsPressed)
            {
                CurrentStyle = StyleMerger.Merge(CurrentStyle, PressedStyle, ref _mergingCache);
            }
        }

        /// <summary>
        /// Triggers the rendering of visuals and positioning of children
        /// </summary>
        public override void _Ready() => UpdateLayout();

        /// <summary>
        /// Overrides Godot's <c>_Draw</c> method to perform custom drawing logic.
        /// </summary>
        /// <remarks>
        /// The actual rendering is delegated to the assigned <see cref="VisualEngine"/>, which
        /// uses the regular and visual styles defined in the <see cref="VisualResource"/>.
        /// This ensures that the node's appearance reflects its current computed styles.
        /// </remarks>
        public override void _Draw()
        {
            VisualEngine.Draw(this, ComputedStyle, CurrentStyle.VisualResource);
        }

        /// <summary>
        /// Unlinks from events and prepares the node for hierachy exit.
        /// </summary>
        public override void _ExitTree()
        {
#if DEBUG
            if (Engine.IsEditorHint()) return;
#endif
            if (IsRoot())
                GetViewport().SizeChanged -= ViewportSizeChange;
            MouseEntered -= HandleMouseEntered;
            MouseExited -= HandleMouseExited;
            FocusEntered -= HandleFocusEntered;
            FocusExited -= HandleFocusExited;
        }

        /// <summary>
        /// Handles the gui press event and invokes the <see cref="InputEngine"/>.
        /// </summary>
        /// <param name="event">Event to be handled</param>
        public override void _GuiInput(InputEvent @event)
        {
#if DEBUG
            if (Engine.IsEditorHint()) return;
#endif
            InputEngine.GuiInput(this, @event, CurrentStyle.InputResource);

            if(TryGetPress(@event, out bool pressed))
            {
                if (pressed) Click(@event);
                else Unclick(@event);
            }
        }

        /// <summary>
        /// Handles the press event and invokes the <see cref="InputEngine"/>.
        /// </summary>
        /// <param name="event">Event to be handled</param>
        public override void _Input(InputEvent @event)
        {
#if DEBUG
            if (Engine.IsEditorHint()) return;
#endif
            InputEngine.Input(this, @event, CurrentStyle.InputResource);
            if (IsPressed)
            {
                if(TryGetPress(@event, out bool _))
                {
                    Unclick(@event);
                }
            }
            else
            {
                if (IsFocused && TryGetFocusedPress(@event, out var pressed))
                {
                    Click(@event);
                }
            }
        }

        /// <summary>
        /// Recomputes the node's style and layout, applies size and position changes,
        /// and triggers a redraw of the node and its children.
        /// </summary>
        /// <remarks>
        /// This method computes the current <see cref="CurrentStyle"/>, invokes the
        /// configured <see cref="ILayoutEngine"/> to compute sizes and arrange the node
        /// and its children, to finally call for a redraw
        /// 
        /// <para>
        /// Updates layouts of all direct children which essentially means it
        /// updates all children below this <see cref="UINode"/> in the hierarchy
        /// </para>
        /// 
        /// <para>
        /// Since layout changes affect rendering, <see cref="UpdateLayout"/> also
        /// calls for a redraw via Godot's <c>QueueRedraw();</c> call.
        /// </para>
        /// 
        /// <para>
        /// Typically called after style changes, adding/removing children, viewport change 
        /// or when a manual layout refresh is needed.
        /// </para>
        /// </remarks>
        public virtual void UpdateLayout()
        {
            if(IsRoot()) ApplyStyle();

            foreach (var child in GetChildren())
            {
                if (child is UINode harmonia)
                {
                    harmonia.ApplyStyle();
                    harmonia.UpdateLayout();
                }
            }

            LayoutEngine.ApplyLayout(this, ComputedStyle, CurrentStyle.LayoutResource);
            QueueRedraw();
        }

        /// <summary>
        /// Recomputes the style and size
        /// </summary>
        /// <remarks>
        /// Computes the <see cref="CurrentStyle"/> into <see cref="ComputedStyle"/> and recomputes the size
        /// using the <see cref="LayoutEngine"/> and styles.
        /// </remarks>
        public virtual void ApplyStyle()
        {
            Vector2 viewportSize = ViewportHelper.GetViewportSize(this);
            Vector2 parentSize = Parent == null ? viewportSize : new Vector2(Parent.ContentWidth, Parent.ContentHeight);

            StyleComputer.Compute(ComputedStyle, CurrentStyle, viewportSize, parentSize);
            LayoutEngine.ComputeSize(this, ComputedStyle, CurrentStyle.LayoutResource);
        }

        /// <summary>
        /// Finds and returns the nearest parent <see cref="UINode"/> of this node.
        /// </summary>
        /// <returns>
        /// The closest ancestor <see cref="UINode"/>, or <c>null</c> if no such parent exists.
        /// </returns>
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
                    controlNode.SetAnchorsPreset(LayoutPreset.FullRect); // Default, makes the godot node be scaled with viewport
                    return null;
                }
            }
            return null;
        }


        /// <summary>
        /// Determines whether this node is a root node in the HarmoniaUI hierarchy.
        /// </summary>
        /// <remarks>
        /// A root node is defined as a direct child of a <see cref="CanvasLayer"/> node or <see cref="Window"/>.
        /// </remarks>
        /// <returns><c>true</c> if this node is a root; otherwise, <c>false</c>.</returns>
        public bool IsRoot()
        {
            Node current = GetParent();

            while (current != null)
            {
                if (current is CanvasLayer | current is Window)
                    break;

                if (current is UINode)
                    return false;

                current = current.GetParent();
            }

            return true;
        }

        /// <summary>
        /// Finds the root <see cref="UINode"/> of the specified node.
        /// </summary>
        /// <param name="node">The <see cref="UINode"/> to find the root for.</param>
        /// <returns>
        /// The root <see cref="UINode"/> that is an ancestor of <paramref name="node"/>,
        /// or <c>null</c> if no root exists.
        /// </returns>
        public static UINode FindRoot(UINode node)
        {
            Node current = node;
            UINode lastUINode = node;

            while (current.GetParent() != null)
            {
                current = current.GetParent();

                if (current is CanvasLayer | current is Window)
                    break;

                if (current is UINode uiBase)
                    lastUINode = uiBase;
            }

            return lastUINode;
        }

        /// <summary>
        /// Handles the focus exiting
        /// </summary>
        protected virtual void HandleFocusExited()
        {
            IsFocused = false;
            HandleInteractions();
            UpdateParentOrSelf();
        }

        /// <summary>
        /// Handles the focus entering
        /// </summary>
        protected virtual void HandleFocusEntered()
        {
            IsFocused = true;
            HandleInteractions();
            UpdateParentOrSelf();
        }

        /// <summary>
        /// Handles the mouse hovering exiting
        /// </summary>
        protected virtual void HandleMouseExited()
        {
            IsHovered = false;
            HandleInteractions();
            UpdateParentOrSelf();

        }

        /// <summary>
        /// Handles the mouse hovering entering
        /// </summary>
        protected virtual void HandleMouseEntered()
        {
            IsHovered = true;
            HandleInteractions();
            UpdateParentOrSelf();
        }

        /// <summary>
        /// Handles the UINode <see cref="IsPressed"/> being unpressed/released.
        /// </summary>
        /// <param name="node">Node from a which pressed event fires (this)</param>
        /// <param name="event">Event associated with the press</param>
        private void UINode_Released(UINode node, InputEvent @event)
        {
            IsPressed = false;
            HandleInteractions();
            UpdateParentOrSelf();
        }

        /// <summary>
        /// Handles the UINode <see cref="IsPressed"/> being pressed.
        /// </summary>
        /// <param name="node">Node from a which pressed event fires (this)</param>
        /// <param name="event">Event associated with the press</param>
        private void UINode_Pressed(UINode node, InputEvent @event)
        {
            IsPressed = true;
            HandleInteractions();
            UpdateParentOrSelf();
        }

        /// <summary>
        /// Unclicks the node, turning <see cref="IsPressed"/> to false, and invoking <see cref="Released"/>
        /// </summary>
        /// <param name="event">Event associated with release</param>
        private void Unclick(InputEvent @event)
        {
            IsPressed = false;
            Released?.Invoke(this, @event);
        }

        /// <summary>
        /// Clicks the node, turning <see cref="IsPressed"/> to true, and invoking <see cref="Pressed"/>
        /// </summary>
        /// <param name="event">Event associated with press</param>
        private void Click(InputEvent @event)
        {
            IsPressed = true;
            Pressed?.Invoke(this, @event);
        }

        /// <summary>
        /// Gets information whether input was a click-type event, and whether it was released or pressed.
        /// </summary>
        /// <param name="event">Event to be processed</param>
        /// <param name="pressed">Whether the click-type event was pressed (true) or released (false)</param>
        /// <returns>True if the input is a click-type event; false otherwise</returns>
        /// <remarks>
        /// Doesn't account for focus, so events not requiring focus are handled.
        /// for focus required inputs use <see cref="TryGetFocusedPress(InputEvent, out bool)"/>
        /// </remarks>
        private bool TryGetPress(InputEvent @event, out bool pressed)
        {
            pressed = false;

            switch (@event)
            {
                case InputEventMouseButton mouse:
                    if (mouse.ButtonIndex != MouseButton.Left) return false;
                    pressed = mouse.Pressed;
                    return true;

                case InputEventScreenTouch touch:
                    pressed = touch.Pressed;
                    return true;

                case InputEventJoypadButton joypad:
                    if (joypad.ButtonIndex != JoyButton.X) return false;
                    pressed = joypad.Pressed;
                    return true;
                case InputEventKey keyboard:
                    if(keyboard.Keycode == Key.Enter
                    || keyboard.Keycode == Key.Space
                    || keyboard.Keycode == Key.KpEnter)
                    {
                        pressed = keyboard.Pressed;
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets information whether input was a click-type event specifically for focus required
        /// input types and whether it was released or pressed.
        /// </summary>
        /// <param name="event">Event to be processed</param>
        /// <param name="pressed">Whether the click-type event was pressed (true) or released (false)</param>
        /// <returns>True if the input is a click-type event; false otherwise</returns>
        /// <remarks>
        /// Explanation: Some events need to be handled globally in <see cref="_Input(InputEvent)"/> so focused
        /// nodes can be selected, but it can't handle inputs not requiring focus, because it would click all
        /// nodes then.
        /// </remarks>
        private bool TryGetFocusedPress(InputEvent @event, out bool pressed)
        {
            pressed = false;

            switch (@event)
            {
                case InputEventJoypadButton joypad:
                    if (joypad.ButtonIndex != JoyButton.X) return false;
                    pressed = joypad.Pressed;
                    return true;
                case InputEventKey keyboard:
                    if(keyboard.Keycode == Key.Enter
                    || keyboard.Keycode == Key.Space
                    || keyboard.Keycode == Key.KpEnter)
                    {
                        pressed = keyboard.Pressed;
                        return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Updates itself if it's the root otherwise it updates Parent, which in turn will update this node.
        /// </summary>
        /// <remarks>
        /// Updating parent first is useful, so the relative units also update.
        /// </remarks>
        private void UpdateParentOrSelf()
        {
            if (IsRoot())
                UpdateLayout();
            else Parent?.UpdateLayout();
        }

        /// <summary>
        /// Updates the layout whenether a viewport size changes
        /// </summary>
        private void ViewportSizeChange()
        {
            UpdateParentOrSelf();
        }

        /// <summary>
        /// Updates layout or redraws based on the <see cref="UINodeAction"/>
        /// </summary>
        /// <param name="obj">Value which defines what should update</param>
        private void StyleChanged(UINodeAction obj)
        {
            if (obj.HasFlag(UINodeAction.Relayout))
            {
                UpdateParentOrSelf();
                return; // Layout queues redraw that's why we ignore Redraw
            }
            if (obj.HasFlag(UINodeAction.Redraw)) QueueRedraw();
        }

        /// <summary>
        /// Sets all of the anchors of this <see cref="UINode"/> to 0, basically setting the Layout Mode
        /// to position. (Layout mode exists in editor but not in the code)
        /// </summary>
        private void SetLayoutToPosition()
        {
            SetAnchor(Side.Left, 0);
            SetAnchor(Side.Right, 0);
            SetAnchor(Side.Top, 0);
            SetAnchor(Side.Bottom, 0);
        }
    }
}