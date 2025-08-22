using GdUnit4;
using Godot;
using HarmoniaUI.Core.Style.Merger;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Types;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="StyleMerger"/> sizes.
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class StyleMergerTests
    {

        /// <summary>
        /// Tests merging of two <see cref="ParsedStyle"/> primary and secondary.
        /// </summary>
        [TestCase]
        public void SingleMergingTest()
        {
            ParsedStyle primary = new ParsedStyle()
            {
                BackgroundColor = Colors.Black,
                BorderColor = Colors.Blue,
                Height = new(0.5f, Unit.Percent),
                Width = new(0.25f, Unit.Percent),
                PositioningType = PositionType.Relative,
                SizingType = SizingType.Padding,
            };

            ParsedStyle secondary = new ParsedStyle()
            {
                BackgroundColor = Colors.White,
                Width = new(0.5f, Unit.Percent),
                PositioningType = PositionType.Normal
            };

            ParsedStyle result = new();
            StyleMerger.Merge(primary, secondary, ref result);

            Assertions.AssertObject(result.BackgroundColor).IsEqual(Colors.White);
            Assertions.AssertObject(result.BorderColor).IsEqual(Colors.Blue);
            Assertions.AssertInt((int)result.PositioningType).IsEqual((int)PositionType.Normal);
            Assertions.AssertInt((int)result.SizingType).IsEqual((int)SizingType.Padding);
            Assertions.AssertObject(result.Width).IsEqual(new StyleValue(0.5f, Unit.Percent));
        }


        /// <summary>
        /// Tests merging of two <see cref="ParsedStyle"/> primary and secondary.
        /// and then additional merging of the result and tertiary style
        /// </summary>
        [TestCase]
        public void DoubleMergingTest()
        {
            ParsedStyle primary = new ParsedStyle()
            {
                BackgroundColor = Colors.Black,
                BorderColor = Colors.Blue,
                Height = new(0.5f, Unit.Percent),
                Width = new(0.25f, Unit.Percent),
                PositioningType = PositionType.Relative,
                SizingType = SizingType.Padding,
                Margin = new(new StyleValue(10, Unit.Pixel)),
                Padding = new(new StyleValue(50, Unit.Pixel)),
                Visibility = VisibilityType.Visible,
            };

            ParsedStyle secondary = new ParsedStyle()
            {
                BackgroundColor = Colors.White,
                Width = new(0.5f, Unit.Percent),
                PositioningType = PositionType.Normal,
                Margin = new(new StyleValue(50, Unit.Pixel)),
                Visibility = VisibilityType.Hidden
            };

            ParsedStyle tertiary = new ParsedStyle()
            {
                BackgroundColor = Colors.Red,
                Width = new(0.75f, Unit.Percent),
                Padding = new(new(10, Unit.Pixel))
            };

            ParsedStyle result = new();
            StyleMerger.Merge(primary, secondary, ref result);
            StyleMerger.Merge(result, tertiary, ref result);

            Assertions.AssertObject(result.BackgroundColor).IsEqual(Colors.Red);
            Assertions.AssertObject(result.BorderColor).IsEqual(Colors.Blue);
            Assertions.AssertInt((int)result.PositioningType).IsEqual((int)PositionType.Normal);
            Assertions.AssertInt((int)result.SizingType).IsEqual((int)SizingType.Padding);
            Assertions.AssertInt((int)result.Visibility).IsEqual((int)VisibilityType.Hidden);
            Assertions.AssertObject(result.Width).IsEqual(new StyleValue(0.75f, Unit.Percent));
            Assertions.AssertObject(result.Margin).IsEqual(new StyleSides<StyleValue>(new(50, Unit.Pixel)));
            Assertions.AssertObject(result.Padding).IsEqual(new StyleSides<StyleValue>(new(10, Unit.Pixel)));
        }
    }
}
