using Godot;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;
using System;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

namespace HarmoniaUI.Core.Engines.Layout
{
    /// <summary>
    /// Provides a base implementation of the <see cref="ILayoutEngine"/> interface,
    /// offers basic size and layout computation logic for arranging UI nodes within
    /// the HarmoniaUI system.
    /// </summary>
    /// <remarks>
    /// <see cref="BaseLayoutEngine"/> handles the core responsibilities of measuring
    /// and positioning child nodes according to style definitions and layout rules.
    /// It serves as a starting point for creating custom layout engines by extending
    /// or overriding its methods.
    ///
    /// This class retrieves layout parameters from associated style resources and
    /// computes sizes and positions accordingly. When no custom layout engine is
    /// specified in the node's style, <see cref="BaseLayoutEngine"/> acts as the
    /// default engine.
    ///
    /// Developers can extend <see cref="BaseLayoutEngine"/> to implement more
    /// specialized layout behaviors, such as grid, flexbox like, or other custom
    /// layouts, by overriding the relevant methods.
    /// </remarks>
    public class BaseLayoutEngine : ILayoutEngine
    {
        public virtual void ApplyLayout(UINode node, ComputedStyle style, LayoutResource layout)
        {
            if (!HandleVisibility(node, style, layout)) return;
            HandleAbsolutePositioning(node, style, layout);

            var children = node.GetChildren();
            float xOffset = node.GlobalPosition.X + style.Padding.Left + style.BorderWidth.Left;
            float yOffset = node.GlobalPosition.Y + style.Padding.Top + style.BorderWidth.Top;
            foreach (var child in children)
            {
                Vector2 childPosition = new(xOffset, yOffset);
                if (child is UINode harmoniaNode)
                {
                    var childStyle = harmoniaNode.ComputedStyle;
                    if (childStyle.Visibility == VisibilityType.Hidden) continue;
                    if (childStyle.PositioningType == PositionType.Absolute)
                    {
                        harmoniaNode.GlobalPosition = new Vector2(childStyle.PositionX, childStyle.PositionY);
                        continue; // Absolutes should not add any offsets.
                    }

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

        protected virtual void HandleAbsolutePositioning(UINode node, ComputedStyle style, LayoutResource layout)
        {
            if (style.PositioningType == PositionType.Absolute)
            {
                node.GlobalPosition = new Vector2(style.PositionX, style.PositionY);
            }
        }

        /// <summary>
        /// Applies the visibility, and returns a bool whether applying layout should continue
        /// </summary>
        /// <param name="node">The Harmonia UI node to update the visibility for.</param>
        /// <param name="style">The <see cref="ComputedStyle"/> that defines node visibilityparam>
        /// <param name="layout">The <see cref="LayoutResource"/> which provides custom layout styling</param>
        /// <returns>True if applying layout should continue; False if it should stop</returns>
        protected virtual bool HandleVisibility(UINode node, ComputedStyle style, LayoutResource layout)
        {
            if (style.Visibility == VisibilityType.Hidden)
            {
                node.Visible = false;
                return false;
            }
            else if (style.Visibility == VisibilityType.Transparent)
                node.Visible = false;
            else
                node.Visible = true;
            return true;
        }

        public virtual void ComputeSize(UINode node, ComputedStyle style, LayoutResource layout)
        {
            float tempWidth = Math.Max(style.Width, style.MinWidth);
            float tempHeight = Math.Max(style.Height, style.MinHeight);
            tempWidth = Math.Min(tempWidth, style.MaxWidth);
            tempHeight = Math.Min(tempHeight, style.MaxHeight);

            switch (style.SizingType)
            {
                case SizingType.Border:
                    node.Size = new Vector2(tempWidth, tempHeight);
                    break;
                case SizingType.Padding:
                    tempWidth += style.BorderWidth.Left + style.BorderWidth.Right;
                    tempHeight += style.BorderWidth.Top + style.BorderWidth.Bottom;
                    break;
                case SizingType.Content:
                    tempWidth = tempWidth
                        + style.Padding.Left + style.Padding.Right
                        + style.BorderWidth.Left + style.BorderWidth.Right;

                    tempHeight = tempHeight
                        + style.Padding.Top + style.Padding.Bottom
                        + style.BorderWidth.Top + style.BorderWidth.Bottom;
                    break;
            }

            node.Size = new Vector2(tempWidth, tempHeight);
            node.ContentWidth = MathF.Max(0, tempWidth
                - (style.Padding.Left + style.Padding.Right)
                - (style.BorderWidth.Left + style.BorderWidth.Right));
            node.ContentHeight = MathF.Max(0, tempHeight
                - (style.Padding.Top + style.Padding.Bottom)
                - (style.BorderWidth.Top + style.BorderWidth.Bottom));
        }
    }
}
