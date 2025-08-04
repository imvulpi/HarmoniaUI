using Godot;
using HarmoniaUI.library.core.commons;
using HarmoniaUI.library.core.types;
using HarmoniaUI.library.nodes;
using HarmoniaUI.Library.style;
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
        public void ApplyLayout(UINode node, ParsedStyle style)
        {
            if(style.PositioningType == PositionType.Absolute)
            {
                node.Position = new Vector2(style.PositionX, style.PositionY);
            }

            var children = node.GetChildren();
            float xOffset = node.Position.X + style.Padding.Left + style.BorderWidth.Left;
            float yOffset = node.Position.Y + style.Padding.Left + style.BorderWidth.Top;
            foreach (var child in children)
            {
                Vector2 childPosition = new(xOffset, yOffset);
                if (child is UINode harmoniaNode)
                {
                    var childStyle = harmoniaNode.ParsedStyle;
                    if (childStyle.PositioningType == PositionType.Relative)
                    {
                        childPosition.X += childStyle.PositionX;
                        childPosition.Y += childStyle.PositionY;
                    }

                    childPosition.X += childStyle.Margin.Left;
                    childPosition.Y += childStyle.Margin.Top;
                    harmoniaNode.Position = childPosition;
                    yOffset += childStyle.Margin.Top
                        + childStyle.Margin.Bottom
                        + childStyle.Padding.Top
                        + childStyle.Padding.Bottom
                        + harmoniaNode.Size.Y;
                }
            }
        }


        /// <inheritdoc cref="ILayoutEngine.ComputeSize(UINode, ParsedStyle)"/>
        public void ComputeSize(UINode node, ParsedStyle style)
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
        protected void ComputeNodeSize(UINode node, ParsedStyle style, Vector2 viewportSize, Vector2 parentSize)
        {
            float tempWidth = ComputeDimension(style.Width, parentSize.X, viewportSize);
            float tempHeight = ComputeDimension(style.Height, parentSize.Y, viewportSize);
            float minWidth = style.MinWidth;
            float minHeight = style.MinHeight;
            float maxWidth = ComputeMaxDimension(style.MaxWidth, parentSize.X, viewportSize);
            float maxHeight = ComputeMaxDimension(style.MaxHeight, parentSize.Y, viewportSize);

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
                    tempWidth += GetBorderWidthX(style, viewportSize, parentSize.X);
                    tempHeight += GetBorderWidthY(style, viewportSize, parentSize.Y);
                    break;
                case SizingType.Content:
                    tempWidth = tempWidth
                        + GetPaddingX(style, viewportSize, parentSize.X)
                        + GetBorderWidthX(style, viewportSize, parentSize.X);

                    tempHeight = tempHeight
                        + GetPaddingY(style, viewportSize, parentSize.Y)
                        + GetBorderWidthY(style, viewportSize, parentSize.Y);
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
        protected void ComputeContentSize(UINode node, ParsedStyle style, Vector2 viewportSize, Vector2 parentSize)
        {
            var width = node.Size.X;
            var height = node.Size.Y;

            switch (style.SizingType)
            {
                case SizingType.Border:
                    node.ContentWidth = width 
                        - GetPaddingX(style, viewportSize, parentSize.X)
                        - GetBorderWidthX(style, viewportSize, parentSize.X);
                    node.ContentHeight = height
                        - GetPaddingY(style, viewportSize, parentSize.Y)
                        - GetBorderWidthY(style, viewportSize, parentSize.Y);
                    break;
                case SizingType.Padding:
                    node.ContentWidth = width - GetPaddingX(style, viewportSize, parentSize.X);
                    node.ContentWidth = width - GetPaddingY(style, viewportSize, parentSize.Y);
                    break;
                case SizingType.Content:
                    node.ContentWidth = width;
                    node.ContentHeight = height;
                    break;
                default:
                    break;
            }
        }
    }
}
