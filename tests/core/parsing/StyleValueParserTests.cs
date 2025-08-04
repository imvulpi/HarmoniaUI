using GdUnit4;
using HarmoniaUI.Library.style;
using HarmoniaUI.Library.style.parsing;

namespace HarmoniaUI.Tests.core.parsing
{
    [TestSuite]
    public class StyleValueParserTests
    {
        [TestCase]
        public void Parsing_SingleValue_StyleValue()
        {
            StyleValue styleValue = StyleValueParser.Parse("10px");
            
            Assertions.AssertObject(styleValue.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(styleValue.Value).Equals(10.0f);
        }

        [TestCase("100px", Unit.Pixel, 100.0f)]
        [TestCase("-30px", Unit.Pixel, -30.0f)]
        [TestCase("0px", Unit.Pixel, 0.0f)]
        [TestCase("auto", Unit.Auto, 0.0f)]
        [TestCase("10%", Unit.Percent, 0.1f)]
        [TestCase("-10%", Unit.Percent, -0.1f)]
        [TestCase("0%", Unit.Percent, 0.0f)]
        [TestCase("50vw", Unit.ViewportWidth, 0.5f)]
        [TestCase("-50vw", Unit.ViewportWidth, -0.5f)]
        [TestCase("0vw", Unit.ViewportWidth, 0.0f)]
        [TestCase("50vh", Unit.ViewportHeight, 0.5f)]
        [TestCase("-50vh", Unit.ViewportHeight, -0.5f)]
        [TestCase("0vh", Unit.ViewportHeight, 0.0f)]
        [TestCase("", Unit.Auto, 0.0f)]
        public void Parsing_SingleValues_CorrectValue(string stringValue, Unit expectedUnit, float expectedValue)
        {
            StyleValue styleValue = StyleValueParser.Parse(stringValue);

            Assertions.AssertObject(styleValue.Unit).IsEqual(expectedUnit);
            Assertions.AssertFloat(styleValue.Value).IsEqual(expectedValue);
        }

        [TestCase]
        public void Parsing_4ValueRect_Rect()
        {
            StyleSides rectSides = StyleValueParser.ParseRect("25px 50% 75vw 100vh");

            Assertions.AssertObject(rectSides.Top.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(rectSides.Top.Value).IsEqual(25);
            Assertions.AssertObject(rectSides.Right.Unit).IsEqual(Unit.Percent);
            Assertions.AssertFloat(rectSides.Right.Value).IsEqual(0.5f);
            Assertions.AssertObject(rectSides.Bottom.Unit).IsEqual(Unit.ViewportWidth);
            Assertions.AssertFloat(rectSides.Bottom.Value).IsEqual(0.75);
            Assertions.AssertObject(rectSides.Left.Unit).IsEqual(Unit.ViewportHeight);
            Assertions.AssertFloat(rectSides.Left.Value).IsEqual(1.0f);
        }

        [TestCase]
        public void Parsing_3ValueRect_Rect()
        {
            StyleSides rectSides = StyleValueParser.ParseRect("25px 50% 75vw");

            Assertions.AssertInt((int)rectSides.Top.Unit).IsEqual((int)Unit.Pixel);
            Assertions.AssertFloat(rectSides.Top.Value).IsEqual(25);
            Assertions.AssertInt((int)rectSides.Right.Unit).IsEqual((int)Unit.Percent);
            Assertions.AssertFloat(rectSides.Right.Value).IsEqual(0.5f);
            Assertions.AssertInt((int)rectSides.Bottom.Unit).IsEqual((int)Unit.ViewportWidth);
            Assertions.AssertFloat(rectSides.Bottom.Value).IsEqual(0.75);
            Assertions.AssertInt((int)rectSides.Left.Unit).IsEqual((int)Unit.Percent);
            Assertions.AssertFloat(rectSides.Left.Value).IsEqual(0.5f);
        }

        [TestCase]
        public void Parsing_2ValueRect_Rect()
        {
            StyleSides rectSides = StyleValueParser.ParseRect("25px 50%");

            Assertions.AssertObject(rectSides.Top.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(rectSides.Top.Value).IsEqual(25);
            Assertions.AssertObject(rectSides.Right.Unit).IsEqual(Unit.Percent);
            Assertions.AssertFloat(rectSides.Right.Value).IsEqual(0.5f);
            Assertions.AssertObject(rectSides.Bottom.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(rectSides.Bottom.Value).IsEqual(25);
            Assertions.AssertObject(rectSides.Left.Unit).IsEqual(Unit.Percent);
            Assertions.AssertFloat(rectSides.Left.Value).IsEqual(0.5f);
        }

        [TestCase]
        public void Parsing_1ValueRect_Rect()
        {
            StyleSides rectSides = StyleValueParser.ParseRect("25px");

            Assertions.AssertObject(rectSides.Top.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(rectSides.Top.Value).IsEqual(25);
            Assertions.AssertObject(rectSides.Right.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(rectSides.Right.Value).IsEqual(25);
            Assertions.AssertObject(rectSides.Bottom.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(rectSides.Bottom.Value).IsEqual(25);
            Assertions.AssertObject(rectSides.Left.Unit).IsEqual(Unit.Pixel);
            Assertions.AssertFloat(rectSides.Left.Value).IsEqual(25);
        }
    }
}
