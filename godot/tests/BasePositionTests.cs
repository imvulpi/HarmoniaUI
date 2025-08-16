using GdUnit4;
using Godot;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="UINode"/> positioning.
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class BasePositionTests
    {
        /// <summary>
        /// Tests the <see cref="PositionType.Normal"/> positioning of <see cref="UINode"/>
        /// </summary>
        /// <param name="width">Width of the test subject</param>
        /// <param name="widthUnit">Unit of the test subject width</param>
        /// <param name="height">Height of the test subject</param>
        /// <param name="heightUnit">Unit of the test subject height</param>
        [TestCase(1, Unit.Auto, 1, Unit.Auto)]
        [TestCase(300, Unit.Pixel, 500, Unit.Pixel)]
        [TestCase(0.5f, Unit.Percent, 0.5f, Unit.Percent)]
        [TestCase(0.25f, Unit.WidthPercent, 0.5f, Unit.HeightPercent)]
        [TestCase(0.5f, Unit.HeightPercent, 0.75f, Unit.WidthPercent)]
        [TestCase(500, Unit.Pixel, 0.1f, Unit.ViewportHeight)]
        [TestCase(120, Unit.Pixel, 0.5f, Unit.ViewportWidth)]
        [TestCase(0.5f, Unit.HeightPercent, 0.333f, Unit.ViewportHeight)]
        [TestCase(0.5f, Unit.HeightPercent, 0.125f, Unit.ViewportHeight)]
        public void NormalPosition(float width, Unit widthUnit, float height, Unit heightUnit)
        {
            UINode root = new();
            var testSubject = new UINode();
            var child1 = new UINode();
            var child2 = new UINode();
            root.AddChild(child1);
            root.AddChild(child2);
            root.AddChild(testSubject);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.Width = new StyleValue(1152, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(648, Unit.Pixel);

            child1.ParsedStyle.Width = new StyleValue(0.5f, Unit.WidthPercent);
            child1.ParsedStyle.Height = new StyleValue(400, Unit.Pixel);
            child2.ParsedStyle.Width = new StyleValue(0.25f, Unit.ViewportWidth);
            child2.ParsedStyle.Height = new StyleValue(0.2f, Unit.Percent);
            testSubject.ParsedStyle.Width = new StyleValue(width, widthUnit);
            testSubject.ParsedStyle.Height = new StyleValue(height, heightUnit);
            testSubject.ParsedStyle.PositioningType = PositionType.Normal;

            Vector2 expected = new(
                0,
                child1.Size.Y + child2.Size.Y
            );
            
            Assertions.AssertVector(testSubject.GlobalPosition).IsEqual(expected);
        }

        /// <summary>
        /// Tests the <see cref="PositionType.Relative"/> positioning of <see cref="UINode"/>
        /// </summary>
        /// <param name="posX">Relative position X value</param>
        /// <param name="posXUnit">Unit of the relative position X</param>
        /// <param name="posY">Relative position Y value</param>
        /// <param name="posYUnit">Unit of the relative position Y</param>
        [TestCase(0, Unit.Auto, 0, Unit.Auto)]
        [TestCase(100, Unit.Pixel, 100, Unit.Pixel)]
        [TestCase(0.5f, Unit.Percent, 0.5f, Unit.Percent)]
        [TestCase(0.5f, Unit.WidthPercent, 0.5f, Unit.HeightPercent)]
        [TestCase(0.25f, Unit.HeightPercent, 0.75f, Unit.WidthPercent)]
        [TestCase(500, Unit.Pixel, 0.1f, Unit.ViewportHeight)]
        [TestCase(120, Unit.Pixel, 0.5f, Unit.ViewportWidth)]
        [TestCase(0.333f, Unit.HeightPercent, 0.333f, Unit.ViewportHeight)]
        [TestCase(0.5f, Unit.HeightPercent, 0.125f, Unit.ViewportHeight)]
        public void RelativePositioning(float posX, Unit posXUnit, float posY, Unit posYUnit)
        {
            UINode root = new();
            var testSubject = new UINode();
            var child1 = new UINode();
            root.AddChild(child1);
            root.AddChild(testSubject);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.Width = new StyleValue(1152, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(648, Unit.Pixel);

            child1.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child1.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);
            testSubject.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            testSubject.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);
            testSubject.ParsedStyle.Width = new StyleValue(posX, posXUnit);
            testSubject.ParsedStyle.Height = new StyleValue(posY, posYUnit);
            testSubject.ParsedStyle.PositioningType = PositionType.Relative;

            Vector2 expected = new(
                testSubject.ComputedStyle.PositionX,
                testSubject.ComputedStyle.PositionY + child1.Size.Y
            );
            
            Assertions.AssertVector(testSubject.GlobalPosition).IsEqual(expected);
        }

        /// <summary>
        /// Tests the <see cref="PositionType.Relative"/> positioning of <see cref="UINode"/>
        /// </summary>
        /// <param name="posX">Absolute position X value</param>
        /// <param name="posXUnit">Unit of the absolute position X</param>
        /// <param name="posY">Absolute position Y value</param>
        /// <param name="posYUnit">Unit of the absolute position Y</param>
        [TestCase(0, Unit.Auto, 0, Unit.Auto)]
        [TestCase(100, Unit.Pixel, 100, Unit.Pixel)]
        [TestCase(0.5f, Unit.Percent, 0.5f, Unit.Percent)]
        [TestCase(0.5f, Unit.WidthPercent, 0.5f, Unit.HeightPercent)]
        [TestCase(0.25f, Unit.HeightPercent, 0.75f, Unit.WidthPercent)]
        [TestCase(500, Unit.Pixel, 0.1f, Unit.ViewportHeight)]
        [TestCase(120, Unit.Pixel, 0.5f, Unit.ViewportWidth)]
        [TestCase(0.333f, Unit.HeightPercent, 0.333f, Unit.ViewportHeight)]
        [TestCase(0.5f, Unit.HeightPercent, 0.125f, Unit.ViewportHeight)]
        public void AbsolutePositioning(float posX, Unit posXUnit, float posY, Unit posYUnit)
        {
            UINode root = new();
            var testSubject = new UINode();
            var child1 = new UINode();
            root.AddChild(child1);
            root.AddChild(testSubject);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.Width = new StyleValue(1152, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(648, Unit.Pixel);

            child1.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child1.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);
            testSubject.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            testSubject.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);
            testSubject.ParsedStyle.Width = new StyleValue(posX, posXUnit);
            testSubject.ParsedStyle.Height = new StyleValue(posY, posYUnit);
            testSubject.ParsedStyle.PositioningType = PositionType.Absolute;

            Vector2 expected = new(
                testSubject.ComputedStyle.PositionX,
                testSubject.ComputedStyle.PositionY
            );
            
            Assertions.AssertVector(testSubject.GlobalPosition).IsEqual(expected);
        }
    }
}
