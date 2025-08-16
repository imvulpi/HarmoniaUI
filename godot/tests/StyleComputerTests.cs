using GdUnit4;
using Godot;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Types;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="StyleComputer"/> computations from <see cref="StyleValue"/> to pixels
    /// </summary>
    [TestSuite]
    public class StyleComputerTests
    {
        private const float ViewportX = 1000;
        private const float ViewportY = 600;
        private const float ParentX = 1000;
        private const float ParentY = 600;

        /// <summary>
        /// Tests the computations of <see cref="StyleValue"/> to pixels performed by <see cref="StyleComputer"/>
        /// </summary>
        /// <param name="value">Value with <paramref name="unit"/></param>
        /// <param name="unit">Unit of the <paramref name="value"/></param>
        /// <param name="parentSide">Side of the parent relating to value and unit</param>
        /// <param name="auto">Automatic value, returned when <see cref="Unit.Auto"/></param>
        /// <param name="expected">Expected result of the computations</param>
        [TestCase(-10, Unit.Pixel, 0, 0, -10)]
        [TestCase(10, Unit.Pixel,0,0,10)]
        [TestCase(2.5f, Unit.Pixel,0,0,2.5f)]
        [TestCase(-2.5f, Unit.Pixel, 0, 0, -2.5f),]
        [TestCase(-0.1f, Unit.Percent, ParentX, 0, ParentX * -0.1f)]
        [TestCase(0.1f, Unit.Percent, ParentY, 0, ParentY * 0.1f)]
        [TestCase(0.025f, Unit.Percent, ParentX, 0, ParentX * 0.025f)]
        [TestCase(-0.025f, Unit.Percent, ParentY, 0, ParentY * -0.025f)]
        [TestCase(-0.1f, Unit.WidthPercent, ParentX, 0, ParentX*-0.1f)]
        [TestCase(0.1f, Unit.WidthPercent, ParentX, 0, ParentX*0.1f)]
        [TestCase(0.025f, Unit.WidthPercent, ParentX, 0, ParentX*0.025f)]
        [TestCase(-0.025f, Unit.WidthPercent, ParentX, 0, ParentX*-0.025f)]
        [TestCase(-0.1f, Unit.HeightPercent, ParentY, 0, ParentY*-0.1f)]
        [TestCase(0.1f, Unit.HeightPercent, ParentY, 0, ParentY*0.1f)]
        [TestCase(0.025f, Unit.HeightPercent, ParentY, 0, ParentY*0.025f)]
        [TestCase(-0.025f, Unit.HeightPercent, ParentY, 0, ParentY*-0.025f)]
        [TestCase(-0.1f, Unit.ViewportWidth, ViewportX, 0, ViewportX*-0.1f)]
        [TestCase(0.1f, Unit.ViewportWidth, ViewportX, 0, ViewportX*0.1f)]
        [TestCase(0.025f, Unit.ViewportWidth, ViewportX, 0, ViewportX*0.025f)]
        [TestCase(-0.025f, Unit.ViewportWidth, ViewportX, 0, ViewportX*-0.025f)]
        [TestCase(-0.1f, Unit.ViewportHeight, ViewportY, 0, ViewportY*-0.1f)]
        [TestCase(0.1f, Unit.ViewportHeight, ViewportY, 0, ViewportY*0.1f)]
        [TestCase(0.025f, Unit.ViewportHeight, ViewportY, 0, ViewportY*0.025f)]
        [TestCase(-0.025f, Unit.ViewportHeight, ViewportY, 0, ViewportY*-0.025f)]
        [TestCase(0, Unit.Auto, 0,0,0)]
        [TestCase(0, Unit.Auto, 0, 100, 100)]
        [TestCase(0, Unit.Auto, 0, ParentX, ParentX)]
        [TestCase(0, Unit.Auto, 0, ParentY, ParentY)]
        [RequireGodotRuntime]
        public void ComputeStyleValuePixel(float value, Unit unit, float parentSide, float auto, float expected)
        {
            Vector2 viewportSize = new Vector2(ViewportX, ViewportY);
            Vector2 parentSize = new Vector2(ParentX, ParentY);

            float pixels = StyleComputer.GetPixel(new StyleValue(value, unit), viewportSize, parentSize, parentSide, auto);
            Assertions.AssertFloat(pixels).IsEqual(expected);
        }
    }
}
