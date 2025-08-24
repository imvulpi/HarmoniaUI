using Godot;
using HarmoniaUI.Core.Engines.Layout;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.library.core.engines.layout.flex
{
    public class FlexLayoutEngine : BaseLayoutEngine
    {
        public override void ApplyLayout(UINode node, ComputedStyle style, LayoutResource layout)
        {
            if (!HandleVisibility(node, style, layout)) return;
            HandleAbsolutePositioning(node, style, layout);

            var children = node.GetChildren();

            float nodeOffsetX = node.GlobalPosition.X + style.Padding.Left + style.BorderWidth.Left;
            float nodeOffseY = node.GlobalPosition.Y + style.Padding.Top + style.BorderWidth.Top;
            float xOffset = nodeOffsetX;
            float yOffset = nodeOffseY;

            if (layout is FlexLayoutResource flexLayout) {

                bool canGrowY = (flexLayout.Wrap == FlexWrap.Wrap || flexLayout.Wrap == FlexWrap.WrapReverse);
                bool canGrowX = flexLayout.Wrap == FlexWrap.NoWrap;

                Vector2 contentSize = new(node.ContentWidth, node.ContentHeight);
                Vector2 newContentSize = contentSize;

                int start = 0;
                int end = children.Count;
                bool reverse = (flexLayout.Wrap == FlexWrap.WrapReverse);
                for (int i = reverse ? end-1 : start;
                     reverse ? i >= start : i < end;
                     i += reverse ? -1 : 1) 
                {
                    var child = children[i];
                    Vector2 childPosition = new(xOffset, yOffset);
                    if (child is UINode harmoniaNode)
                    {
                        if (canGrowX)
                        {
                            xOffset += harmoniaNode.Size.X;
                            newContentSize.X = childPosition.X + harmoniaNode.Size.X;
                        }
                        else if (canGrowY && childPosition.X + harmoniaNode.Size.X > newContentSize.X)
                        {
                            yOffset += harmoniaNode.Size.Y;
                            xOffset = harmoniaNode.Size.X;
                            childPosition = new(0, yOffset);
                            newContentSize.Y += harmoniaNode.Size.Y;
                            childPosition.X = 0;
                        }
                        else
                        {
                            xOffset += harmoniaNode.Size.X;
                        }

                        harmoniaNode.GlobalPosition = childPosition;
                    }
                    else if (child is Control godotNode)
                    {

                    }
                }

                if(!Engine.IsEditorHint()) GD.Print($"Removed: {node.Size - contentSize} | added: {(node.Size - contentSize) + newContentSize}");
                node.Size -= contentSize;
                node.Size += newContentSize;
            }
        }
    }
}
