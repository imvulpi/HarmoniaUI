using Godot;
using HarmoniaUI.Core.Engines.Layout;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Nodes;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

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
                bool noWrapping = flexLayout.Wrap == FlexWrap.NoWrap;

                Vector2 contentSize = new(node.ContentWidth, node.ContentHeight);
                Vector2 currentLine = new();
                float biggest = 0;

                int currentLineIndex = 0;
                List<Vector2> lineSizes = [new Vector2()];
                List<List<Control>> lineNodes = [new()];

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
                        if (flexLayout.Direction == FlexDirection.Column)
                        {
                            bool childFitsY = harmoniaNode.Size.Y <= (contentSize.Y - currentLine.Y);
                            if (wrapping)
                            {
                                if (childFitsY)
                                {
                                    currentLine.Y += harmoniaNode.Size.Y;
                                    biggest = MathF.Max(harmoniaNode.Size.X, biggest);
                                    yOffset += harmoniaNode.Size.Y;
                                }
                                else
                                {
                                    // New X line
                                    xOffset += biggest;
                                    childPosition.X = xOffset;
                                    childPosition.Y = nodeOffsetY; // As well?
                                    yOffset = nodeOffsetY + harmoniaNode.Size.Y;
                                    currentLine.Y = harmoniaNode.Size.Y;

                                    lineSizes.Add(new Vector2());
                                    lineNodes.Add(new());
                                    currentLineIndex++;
                                }
                            }
                            else
                            {
                                yOffset += harmoniaNode.Size.Y;
                                biggest = MathF.Max(harmoniaNode.Size.X, biggest);
                            }
                            lineSizes[currentLineIndex] = new(biggest, lineSizes[currentLineIndex].Y + harmoniaNode.Size.Y);
                        }
                        else if (flexLayout.Direction == FlexDirection.Row)
                        {
                            bool childFitsX = harmoniaNode.Size.X <= (contentSize.X - currentLine.X);
                            if(harmoniaNode.Name == "UINode5") GD.Print($"{harmoniaNode.Size.X} <= {(contentSize.X - currentLine.X)}");
                            if (wrapping)
                            {
                                if (childFitsX)
                                {
                                    currentLine.X += harmoniaNode.Size.X;
                                    biggest = MathF.Max(harmoniaNode.Size.Y, biggest);
                                    xOffset += harmoniaNode.Size.X;
                                }
                                else
                                {
                                    // New Y line
                                    yOffset += biggest; // This should be the biggest Y in the previous line
                                    childPosition.Y = yOffset; // This should be the biggest Y in the previous line
                                    childPosition.X = nodeOffsetX;
                                    xOffset = nodeOffsetX + harmoniaNode.Size.X;
                                    currentLine.X = harmoniaNode.Size.X;
                                    lineSizes[currentLineIndex] = new(lineSizes[currentLineIndex].X, biggest);

                                    lineSizes.Add(new Vector2());
                                    lineNodes.Add(new());
                                    currentLineIndex++;
                                }
                            }
                            else
                            {
                                xOffset += harmoniaNode.Size.X;
                                biggest = MathF.Max(harmoniaNode.Size.Y, biggest);
                            }
                            lineSizes[currentLineIndex] = new(lineSizes[currentLineIndex].X + harmoniaNode.Size.X, biggest);
                        }

                        harmoniaNode.GlobalPosition = childPosition;
                        lineNodes[currentLineIndex].Add(harmoniaNode);
                        // For line sizes WE DON'T want to sum up size of every harmonia node,
                        // We want to sum one axis size based on direction, and the other axis size based on the biggest element.
                        // For example if it's Row direction, we want to sum up the Size.X and get the biggest Size.Y element.
                        //lineSizes[currentLineIndex] = new(lineSizes[currentLineIndex].X + harmoniaNode.Size.X, biggest);
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
            Vector2 contentSize = new(node.ContentWidth, node.ContentHeight);

            float sum = 0;

            if (flexLayout.Direction == FlexDirection.Row)
            {
                for (int i = 0; i < lineSizes.Length; i++)
                {
                    sum += lineSizes[i].Y;
                }
            }
            else
            {
                for (int i = 0; i < lineSizes.Length; i++)
                {
                    sum += lineSizes[i].X;
                }
            }

            GD.Print($"{node.Name} (sum: {sum})");
            for (int i = 0; i < lineSizes.Length; i++)
            {
                Vector2 availableSpace;
                if (flexLayout.Direction == FlexDirection.Row)
                {
                    availableSpace = contentSize - new Vector2(lineSizes[i].X, sum);
                }
                else
                {
                    availableSpace = contentSize - new Vector2(sum, lineSizes[i].Y);
                }
                    
                List<Control> children = lineNodes[i];
                for (int j = 0; j < children.Count; j++)
                {
                    Control child = children[j];

                    float spacingX = GetSpacing(flexLayout.JustifyX, child.GlobalPosition.X, availableSpace.X, children.Count, j);
                    float spacingY = GetSpacing(flexLayout.JustifyY, child.GlobalPosition.Y, availableSpace.Y, children.Count, j);

                    float positionX = availableSpace.X <= 0 ? child.GlobalPosition.X : spacingX;
                    float positionY = availableSpace.Y <= 0 ? child.GlobalPosition.Y : spacingY;

                    child.GlobalPosition = new(
                        positionX,
                        positionY
                    );
                }
            }
        }

        private float GetSpacing(FlexJustifyContent justifyContentType, float position, float availableSpace, float childCount, int currentIndex)
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
