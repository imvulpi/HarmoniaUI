using Godot;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.nodes;
using HarmoniaUI.Library.style;
using System.Runtime.InteropServices.Marshalling;

namespace HarmoniaUI.library.core.visual
{
    /// <summary>
    /// Renders the visuals of a Harmonia node using a <see cref="StyleBoxFlat"/>, node and style
    /// </summary>
    public class BaseVisualEngine : IVisualEngine
    {
        protected StyleBoxFlat CurrentStyleBox;
        public virtual void Draw(UINode node, ParsedStyle style)
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
        protected virtual void DrawBackground(UINode node, ParsedStyle style)
        {
            CurrentStyleBox.BgColor = style.BackgroundColor;
        }

        /// <summary>
        /// Draws the border of the harmonia node using border radius and width
        /// </summary>
        protected virtual void DrawBorder(UINode node, ParsedStyle style)
        {
            var borderRadius = style.BorderRadius;
            CurrentStyleBox.CornerRadiusTopLeft = borderRadius.Top;
            CurrentStyleBox.CornerRadiusTopRight = borderRadius.Right;
            CurrentStyleBox.CornerRadiusBottomLeft = borderRadius.Bottom;
            CurrentStyleBox.CornerRadiusBottomRight = borderRadius.Left;
            CurrentStyleBox.BorderColor = style.BorderColor;

            var borderWidth = style.BorderWidth;
            CurrentStyleBox.BorderWidthTop = borderWidth.Top;
            CurrentStyleBox.BorderWidthLeft = borderWidth.Left;
            CurrentStyleBox.BorderWidthBottom = borderWidth.Bottom;
            CurrentStyleBox.BorderWidthRight = borderWidth.Right;
        }
    }
}
