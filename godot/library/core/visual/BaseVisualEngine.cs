using Godot;
using HarmoniaUI.library.nodes;
using HarmoniaUI.library.style;

namespace HarmoniaUI.library.core.visual
{
    /// <summary>
    /// Renders the visuals of a Harmonia node using a <see cref="StyleBoxFlat"/>, node and style
    /// </summary>
    public class BaseVisualEngine : IVisualEngine
    {
        protected StyleBoxFlat CurrentStyleBox;
        public virtual void Draw(UINode node, ComputedStyle style)
        {
            if (style == null) return;
            CurrentStyleBox = HarmoniaDefaults.GetEmptyStylebox();

            DrawBackground(node, style);
            DrawBorder(node, style);

            node.DrawStyleBox(CurrentStyleBox, new Rect2(0,0, node.Size));
        }

        /// <summary>
        /// Draws the background of the harmonia node
        /// </summary>
        protected virtual void DrawBackground(UINode node, ComputedStyle style)
        {
            CurrentStyleBox.BgColor = style.BackgroundColor;
        }

        /// <summary>
        /// Draws the border of the harmonia node using border radius and width
        /// </summary>
        protected virtual void DrawBorder(UINode node, ComputedStyle style)
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
