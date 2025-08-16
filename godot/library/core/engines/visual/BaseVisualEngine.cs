using Godot;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;
using System;

namespace HarmoniaUI.Core.Engines.Visual
{
    /// <summary>
    /// Provides a base implementation of the <see cref="IVisualEngine"/> interface,
    /// offering the default rendering behavior for <see cref="UINode"/> instances
    /// within the HarmoniaUI framework.
    /// </summary>
    /// <remarks>
    /// <see cref="BaseVisualEngine"/> is responsible for drawing UI nodes
    /// by interpreting their computed styles and visual resources.  
    /// It serves as the default visual engine when none is explicitly
    /// assigned via the node's <see cref="StyleResource.VisualResource"/>.
    ///
    /// It renders background colors, borders, and padding according to style rules.
    /// For rendering it utilizes StyleBoxFlat and uses the functions to draw it.
    /// 
    /// <para>
    /// ⚠️ Be aware that the <see cref="BaseVisualEngine"/> doesn't handle the <see cref="VisualResource"/>,
    /// it's handled by an engine associated with it in <see cref="Registry.UIEngines"/>
    /// </para>
    /// 
    /// Developers can extend <see cref="BaseVisualEngine"/> to implement
    /// custom rendering logic, such as advanced effects, procedural visuals,
    /// or specialized theming. For the more advanced rendering logics they might 
    /// want to implement <see cref="IVisualEngine"/> instead.
    /// </remarks>
    public class BaseVisualEngine : IVisualEngine
    {
        /// <summary>
        /// Stylebox currently used for drawing the visuals. Used so it's available between methods.
        /// </summary>
        /// <remarks>
        /// Should be reset each <see cref="Draw(UINode, ComputedStyle, VisualResource)"/>
        /// </remarks>
        protected StyleBoxFlat CurrentStyleBox;

        /// <inheritdoc cref="IVisualEngine.Draw(UINode, ComputedStyle, VisualResource)"/>
        /// <remarks>
        /// This method serves as the main entry point for drawing a node.
        /// It typically calls <see cref="DrawBackground"/> and <see cref="DrawBorder"/> 
        /// internally, followed by any additional visual rendering defined in the 
        /// <paramref name="visual"/> resource.
        /// </remarks>
        public virtual void Draw(UINode node, ComputedStyle style, VisualResource visual)
        {
            if (style == null) return;

            CurrentStyleBox = new()
            {
                BgColor = Color.Color8(0, 0, 0, 0),
                AntiAliasing = true,
                AntiAliasingSize = 1.0f,
                CornerDetail = 16
            };

            DrawBackground(node, style, visual);
            DrawBorder(node, style, visual);

            DrawShadow(node, style, visual);
            var size = new Vector2(MathF.Ceiling(node.Size.X), MathF.Ceiling(node.Size.Y));
            node.DrawStyleBox(CurrentStyleBox, new Rect2(0,0, size));
        }

        /// <summary>
        /// Draws the shadow of the specified <paramref name="node"/> based on its style properties.
        /// </summary>
        /// <param name="node">The Harmonia UI node whose shadow is being rendered.</param>
        /// <param name="style">The <see cref="ComputedStyle"/> containing shadow color and offsets</param>
        /// <param name="visual">Custom engine visual resource</param>
        protected virtual void DrawShadow(UINode node, ComputedStyle style, VisualResource visual)
        {
            if (style.ShadowColor.A == 0) return;
            if (style.ShadowOffsetX == 0 && style.ShadowOffsetY == 0) return;

            StyleBoxFlat duplicate = (StyleBoxFlat)CurrentStyleBox.Duplicate();
            duplicate.BgColor = style.ShadowColor;
            node.DrawStyleBox(duplicate, new Rect2(style.ShadowOffsetX,  style.ShadowOffsetY, node.Size));
        }

        /// <summary>
        /// Draws the background of the specified <paramref name="node"/> based on its style properties.
        /// </summary>
        /// <param name="node">The Harmonia UI node whose background is being rendered.</param>
        /// <param name="style">The <see cref="ComputedStyle"/> containing background color</param>
        /// <param name="visual">Custom engine visual resource</param>
        protected virtual void DrawBackground(UINode node, ComputedStyle style, VisualResource visual)
        {
            CurrentStyleBox.BgColor = style.BackgroundColor;
        }

        /// <summary>
        /// Draws the border of the specified <paramref name="node"/> based on its style properties.
        /// </summary>
        /// <param name="node">The Harmonia UI node whose border is being rendered.</param>
        /// <param name="style">The <see cref="ComputedStyle"/> containing border color, width, and style settings.</param>
        /// <param name="visual">Custom engine visual resource</param>
        /// <remarks>
        /// Borders are drawn within the bounds of the node, respecting padding and corner radius values.
        /// </remarks>
        protected virtual void DrawBorder(UINode node, ComputedStyle style, VisualResource visual)
        {
            var borderRadius = style.BorderRadius;
            CurrentStyleBox.CornerRadiusTopLeft = (int)borderRadius.Top;
            CurrentStyleBox.CornerRadiusTopRight = (int)borderRadius.Right;
            CurrentStyleBox.CornerRadiusBottomLeft = (int)borderRadius.Bottom;
            CurrentStyleBox.CornerRadiusBottomRight = (int)borderRadius.Left;
            CurrentStyleBox.BorderColor = style.BorderColor;

            var borderWidth = style.BorderWidth;
            CurrentStyleBox.BorderWidthTop = (int)borderWidth.Top;
            CurrentStyleBox.BorderWidthLeft = (int)borderWidth.Left;
            CurrentStyleBox.BorderWidthBottom = (int)borderWidth.Bottom;
            CurrentStyleBox.BorderWidthRight = (int)borderWidth.Right;
        }
    }
}
