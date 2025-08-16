using GdUnit4;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Types;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="StyleParser"/> parsing from string to <see cref="StyleValue"/>
    /// </summary>
    [TestSuite]
    public class StyleParserTests
    {
        /// <summary>
        /// Tests the parsing from string to a value and unit <see cref="StyleValue"/> performed by <see cref="StyleParser"/>
        /// </summary>
        /// <param name="stringRepresentation">String representation of <see cref="StyleValue"/> which gets parsed</param>
        /// <param name="expectedValue">Expected value after parsing</param>
        /// <param name="expectedUnit">Expected unit after parsing</param>
        [TestCase("-10px", -10, Unit.Pixel)]
        [TestCase("10px", 10, Unit.Pixel)]
        [TestCase("2.5px", 2.5f, Unit.Pixel)]
        [TestCase("-2.5px", -2.5f, Unit.Pixel)]
        [TestCase("-10%", -0.1f, Unit.Percent)]
        [TestCase("10%", 0.1f, Unit.Percent)]
        [TestCase("2.5%", 0.025f, Unit.Percent)]
        [TestCase("-2.5%", -0.025f, Unit.Percent)]
        [TestCase("-10w%", -0.1f, Unit.WidthPercent)]
        [TestCase("10w%", 0.1f, Unit.WidthPercent)]
        [TestCase("2.5w%", 0.025f, Unit.WidthPercent)]
        [TestCase("-2.5w%", -0.025f, Unit.WidthPercent)]
        [TestCase("-10h%", -0.1f, Unit.HeightPercent)]
        [TestCase("10h%", 0.1f, Unit.HeightPercent)]
        [TestCase("2.5h%", 0.025f, Unit.HeightPercent)]
        [TestCase("-2.5h%", -0.025f, Unit.HeightPercent)]
        [TestCase("-10vw", -0.1f, Unit.ViewportWidth)]
        [TestCase("10vw", 0.1f, Unit.ViewportWidth)]
        [TestCase("2.5vw", 0.025f, Unit.ViewportWidth)]
        [TestCase("-2.5vw", -0.025f, Unit.ViewportWidth)]
        [TestCase("-10vh", -0.1f, Unit.ViewportHeight)]
        [TestCase("10vh", 0.1f, Unit.ViewportHeight)]
        [TestCase("2.5vh", 0.025f, Unit.ViewportHeight)]
        [TestCase("-2.5vh", -0.025f, Unit.ViewportHeight)]
        [TestCase("", 0, Unit.Auto)]
        [TestCase("10", 0, Unit.Auto)]
        [TestCase("-", 0, Unit.Auto)]
        [TestCase("bad_entry", 0, Unit.Auto)]
        public void ParseSingleString(string stringRepresentation, float expectedValue, Unit expectedUnit)
        {
            var result = StyleParser.ParseValue(stringRepresentation);
            Assertions.AssertFloat(result.Value).IsEqual(expectedValue);
            Assertions.AssertInt((int)result.Unit).IsEqual((int)expectedUnit);
        }
    }
}