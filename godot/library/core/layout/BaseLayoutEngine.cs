using Godot;
using HarmoniaUI.library.core.commons;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.nodes;
using HarmoniaUI.library.style;
using System;
using static HarmoniaUI.library.core.commons.StyleHelper;

namespace HarmoniaUI.library.core.layout
{
    /// <summary>
    /// Base for layout engines, positions nodes, sets sizes and more
    /// </summary>
    public class BaseLayoutEngine : ILayoutEngine
    {
        /// <inheritdoc cref="ILayoutEngine.ApplyLayout(UINode, ParsedStyle)"/>
        public void ApplyLayout(UINode node, ComputedStyle style)
        {
            if(style.PositioningType == PositionType.Absolute)
            {
                node.Position = new Vector2(style.PositionX, style.PositionY);
            }

            var children = node.GetChildren();
            float xOffset = node.GlobalPosition.X + style.Padding.Left + style.BorderWidth.Left;
            float yOffset = node.GlobalPosition.Y + style.Padding.Top + style.BorderWidth.Top;
            foreach (var child in children)
            {
                Vector2 childPosition = new(xOffset, yOffset);
                if (child is UINode harmoniaNode)
                {
                    var childStyle = harmoniaNode.ComputedStyle;
                    if (childStyle.PositioningType == PositionType.Relative)
                    {
                        childPosition.X += childStyle.PositionX;
                        childPosition.Y += childStyle.PositionY;
                    }

                    childPosition.X += childStyle.Margin.Left;
                    childPosition.Y += childStyle.Margin.Top;
                    harmoniaNode.GlobalPosition = childPosition;
                    yOffset += childStyle.Margin.Top
                        + childStyle.Margin.Bottom
                        + harmoniaNode.Size.Y;
                }
                else if(child is Control godotNode)
                {
                    Vector2 parentSize = new(node.ContentWidth, node.ContentHeight);
                    godotNode.Size = parentSize;
                    godotNode.GlobalPosition = new(xOffset, yOffset);

                    yOffset += godotNode.Size.Y;
                }
            }
        }


        /// <inheritdoc cref="ILayoutEngine.ComputeSize(UINode, ParsedStyle)"/>
        public void ComputeSize(UINode node, ComputedStyle style)
        {
            Vector2 viewportSize = ViewportHelper.GetViewportSize(node);
            Vector2 parentSize = node.Parent == null ? viewportSize : new(node.Parent.ContentWidth, node.Parent.ContentHeight);

            ComputeNodeSize(node, style, viewportSize, parentSize);
            ComputeContentSize(node, style, viewportSize, parentSize);
        }

        /// <summary>
        /// Computes and sets <paramref name="node"/> Size from provided style.
        /// </summary>
        /// <remarks>
        /// Respects min/max sizes and sizing type.
        /// </remarks>
        /// <param name="node">Harmonia node to edit</param>
        /// <param name="style">Style used in computations</param>
        /// <param name="viewportSize">Size of the current viewport</param>
        /// <param name="parentSize">Size of the parent of <paramref name="node"/></param>
        protected void ComputeNodeSize(UINode node, ComputedStyle style, Vector2 viewportSize, Vector2 parentSize)
        {
            float tempWidth = ComputeDimension(style.Width, parentSize.X);
            float tempHeight = ComputeDimension(style.Height, parentSize.Y);
            float minWidth = style.MinWidth;
            float minHeight = style.MinHeight;
            float maxWidth = ComputeMaxDimension(style.MaxWidth);
            float maxHeight = ComputeMaxDimension(style.MaxHeight);

            tempWidth = Math.Max(tempWidth, minWidth);
            tempHeight = Math.Max(tempHeight, minHeight);
            tempWidth = Math.Min(tempWidth, maxWidth);
            tempHeight = Math.Min(tempHeight, maxHeight);

            switch (style.SizingType)
            {
                case SizingType.Border:
                    node.Size = new Vector2(tempWidth, tempHeight);
                    return;
                case SizingType.Padding:
                    tempWidth += GetBorderWidthX(style);
                    tempHeight += GetBorderWidthY(style);
                    break;
                case SizingType.Content:
                    tempWidth = tempWidth
                        + GetPaddingX(style)
                        + GetBorderWidthX(style);

                    tempHeight = tempHeight
                        + GetPaddingY(style)
                        + GetBorderWidthY(style);
                    break;
            }
            node.Size = new Vector2(tempWidth, tempHeight);
        }

        /// <summary>
        /// Computes and sets <paramref name="node"/> Content size.
        /// </summary>
        /// <param name="node">Harmonia node to edit</param>
        /// <param name="style">Style used in computations</param>
        /// <param name="viewportSize">Size of the current viewport</param>
        /// <param name="parentSize">Size of the parent of <paramref name="node"/></param>
        protected void ComputeContentSize(UINode node, ComputedStyle style, Vector2 viewportSize, Vector2 parentSize)
        {
            var width = node.Size.X;
            var height = node.Size.Y;

            node.ContentWidth = width
                - GetPaddingX(style)
                - GetBorderWidthX(style);
            node.ContentHeight = height
                - GetPaddingY(style)
                - GetBorderWidthY(style);
        }
    }
}
