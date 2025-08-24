using Godot;
using HarmoniaUI.Commons;
using HarmoniaUI.Core.Engines.Layout;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;
using System;
using System.Collections.Generic;

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
            float nodeOffsetY = node.GlobalPosition.Y + style.Padding.Top + style.BorderWidth.Top;
            float xOffset = nodeOffsetX;
            float yOffset = nodeOffsetY;

            if (layout is FlexLayoutResource flexLayout)
            {
                bool wrapping = (flexLayout.Wrap == FlexWrap.Wrap || flexLayout.Wrap == FlexWrap.WrapReverse);

                Vector2 contentSize = new(node.ContentWidth * node.Scale.X, node.ContentHeight * node.Scale.Y);
                Vector2 currentLine = new();
                float biggest = 0;

                int currentLineIndex = 0;
                List<Vector2> lineSizes = [new Vector2()];
                List<List<Control>> lineNodes = [[]];

                int start = 0;
                int end = children.Count;
                bool reverse = (flexLayout.Wrap == FlexWrap.WrapReverse);

                for (int i = reverse ? end - 1 : start;
                     reverse ? i >= start : i < end;
                     i += reverse ? -1 : 1)
                {
                    var child = children[i];
                    Vector2 childPosition = new(xOffset, yOffset);
                    if (child is UINode harmoniaNode)
                    {
                        Vector2 harmoniaNodeSize = new(
                            (harmoniaNode.Size.X + harmoniaNode.ComputedStyle.Margin.Left + harmoniaNode.ComputedStyle.Margin.Right) * node.Scale.X,
                            (harmoniaNode.Size.Y + harmoniaNode.ComputedStyle.Margin.Top + harmoniaNode.ComputedStyle.Margin.Bottom) * node.Scale.Y
                        );

                        if (flexLayout.Direction == FlexDirection.Column)
                        {
                            float childFitDiff = (harmoniaNodeSize.Y) - (contentSize.Y - currentLine.Y);
                            bool childFitsY = childFitDiff < 0 || childFitDiff <= 0.5f;
                            if (wrapping)
                            {
                                if (childFitsY)
                                {
                                    currentLine.Y += harmoniaNodeSize.Y;
                                    biggest = MathF.Max(harmoniaNodeSize.X, biggest);
                                    yOffset += harmoniaNodeSize.Y;
                                }
                                else
                                {
                                    xOffset += biggest;
                                    childPosition.X = xOffset;
                                    childPosition.Y = nodeOffsetY;
                                    yOffset = nodeOffsetY + harmoniaNodeSize.Y;
                                    currentLine.Y = harmoniaNodeSize.Y;

                                    lineSizes.Add(new Vector2());
                                    lineNodes.Add([]);
                                    currentLineIndex++;
                                }
                            }
                            else
                            {
                                yOffset += harmoniaNodeSize.Y;
                                biggest = MathF.Max(harmoniaNodeSize.X, biggest);
                            }
                            lineSizes[currentLineIndex] = new(biggest, lineSizes[currentLineIndex].Y + harmoniaNodeSize.Y);
                        }
                        else if (flexLayout.Direction == FlexDirection.Row)
                        {
                            float childFitDiff = (harmoniaNodeSize.X) - (contentSize.X - currentLine.X);
                            bool childFitsX = childFitDiff < 0 || childFitDiff <= 0.5f;
                            if (wrapping)
                            {
                                if (childFitsX)
                                {
                                    currentLine.X += harmoniaNodeSize.X;
                                    biggest = MathF.Max(harmoniaNodeSize.Y, biggest);
                                    xOffset += harmoniaNodeSize.X;
                                }
                                else
                                {
                                    yOffset += biggest;
                                    childPosition.Y = yOffset;
                                    childPosition.X = nodeOffsetX;
                                    xOffset = nodeOffsetX + harmoniaNodeSize.X;
                                    currentLine.X = harmoniaNodeSize.X;
                                    lineSizes[currentLineIndex] = new(lineSizes[currentLineIndex].X, biggest);

                                    lineSizes.Add(new Vector2());
                                    lineNodes.Add([]);
                                    currentLineIndex++;
                                }
                            }
                            else
                            {
                                xOffset += harmoniaNodeSize.X;
                                biggest = MathF.Max(harmoniaNodeSize.Y, biggest);
                            }
                            lineSizes[currentLineIndex] = new(lineSizes[currentLineIndex].X + harmoniaNodeSize.X, biggest);
                        }

                        harmoniaNode.GlobalPosition = new Vector2(
                            childPosition.X + harmoniaNode.ComputedStyle.Margin.Left,
                            childPosition.Y + harmoniaNode.ComputedStyle.Margin.Top
                        );
                        lineNodes[currentLineIndex].Add(harmoniaNode);
                    }
                    else if (child is Control godotNode)
                    {

                    }
                }

                ApplySpacing(node, flexLayout, lineSizes.ToArray(), lineNodes.ToArray());
            }
        }

        private void ApplySpacing(UINode node, FlexLayoutResource flexLayout, Vector2[] lineSizes, List<Control>[] lineNodes)
        {
            Vector2 contentSize = new(node.ContentWidth * node.Scale.X, node.ContentHeight * node.Scale.Y);

            float directionalSum = 0;
            if (flexLayout.Direction == FlexDirection.Row)
            {
                for (int i = 0; i < lineSizes.Length; i++)
                {
                    directionalSum += lineSizes[i].Y;
                }
            }
            else
            {
                for (int i = 0; i < lineSizes.Length; i++)
                {
                    directionalSum += lineSizes[i].X;
                }
            }

            Vector2 viewportSize = ViewportHelper.GetViewportSize(node);
            float gapRow = StyleComputer.GetPixel(flexLayout.GapRow, viewportSize, contentSize, contentSize.X, 0);
            float gapColumn = StyleComputer.GetPixel(flexLayout.GapColumn, viewportSize, contentSize, contentSize.Y, 0);

            for (int i = 0; i < lineSizes.Length; i++)
            {
                Vector2 availableSpace;
                List<Control> children = lineNodes[i];
                if (flexLayout.Direction == FlexDirection.Row)
                {
                    availableSpace = contentSize - new Vector2(
                        lineSizes[i].X + gapRow * (children.Count-1), 
                        directionalSum + gapColumn * (lineSizes.Length - 1)
                    );
                }
                else
                {
                    availableSpace = contentSize - new Vector2(
                        directionalSum + gapRow * (lineSizes.Length - 1),
                        lineSizes[i].Y + gapColumn * (children.Count - 1));
                }

                for (int j = 0; j < children.Count; j++)
                {
                    Control child = children[j];

                    float positionX;
                    float positionY;
                    float spacingX;
                    float spacingY;

                    if (flexLayout.Direction == FlexDirection.Row)
                    {
                        child.GlobalPosition = new()
                        {
                            X = child.GlobalPosition.X + (gapRow * j),
                            Y = child.GlobalPosition.Y + (gapColumn * i)
                        };
                        spacingX = GetPositionWithSpacing(flexLayout.JustifyX, child.GlobalPosition.X, availableSpace.X, children.Count, j);
                        spacingY = GetPositionWithSpacing(flexLayout.JustifyY, child.GlobalPosition.Y, availableSpace.Y, lineSizes.Length, i);
                    }
                    else
                    {
                        child.GlobalPosition = new()
                        {
                            X = child.GlobalPosition.X + (gapRow * i),
                            Y = child.GlobalPosition.Y + (gapColumn * j)
                        };
                        spacingX = GetPositionWithSpacing(flexLayout.JustifyX, child.GlobalPosition.X, availableSpace.X, lineSizes.Length, i);
                        spacingY = GetPositionWithSpacing(flexLayout.JustifyY, child.GlobalPosition.Y, availableSpace.Y, children.Count, j);
                    }

                    positionX = availableSpace.X <= 0 ? child.GlobalPosition.X : spacingX;
                    positionY = availableSpace.Y <= 0 ? child.GlobalPosition.Y : spacingY;

                    child.GlobalPosition = new(
                        positionX,
                        positionY
                    );
                }
            }
        }

        private float GetPositionWithSpacing(FlexJustifyContent justifyContentType, float position, float availableSpace, float childCount, int currentIndex)
        {
            switch (justifyContentType)
            {
                case FlexJustifyContent.Start:
                    return position;
                case FlexJustifyContent.End:
                    return position + availableSpace;
                case FlexJustifyContent.Center:
                    return position + (availableSpace / 2);
                case FlexJustifyContent.SpaceBetween:
                    if (currentIndex == 0) return position;
                    return position + (availableSpace / (childCount - 1) * (currentIndex));
                case FlexJustifyContent.SpaceAround:
                    if (currentIndex == 0) return position + availableSpace / childCount / 2;
                    return position + (availableSpace / childCount * (currentIndex + 0.5f));
                case FlexJustifyContent.SpaceEvenly:
                    return position + (availableSpace / (childCount + 1) * (currentIndex+1));
                default:
                    return position;
            }
        }
    }
}
