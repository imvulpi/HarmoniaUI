using GdUnit4;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Nodes;
using static HarmoniaUI.Tests.Utils.TestConstants;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="UINode"/> <see cref="SizingType"/>.
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class BaseSizingTypeTests
    {
        /// <summary>
        /// Checks whether <see cref="SizingType.Border"/> works as expected
        /// </summary>
        /// <remarks>
        /// <see cref="SizingType.Border"/> should shrink the content size by padding and border sizes.
        /// <para>
        /// Most people <b>want to use this option</b>, due to not needing to recalculate or worry much about overflowing
        /// in changes to padding, or border sizes. It can shrink the content, so use it carefully.
        /// </para>
        /// </remarks>
        /// <param name="width">Node width in pixels</param>
        /// <param name="height">Node height in pixels</param>
        /// <param name="padding">Padding in string (gets parsed)</param>
        /// <param name="border">Border in string (gets parsed)</param>
        [TestCase(320, 480, "0", "0")]                 // no padding, no border
        [TestCase(320, 480, "10px", "1px")]           // simple small border
        [TestCase(360, 640, "20px 10px", "5px 2px")]  // 2-value padding and border
        [TestCase(375, 667, "10% 5%", "5% 2%")]       // percent padding and border
        [TestCase(768, 1024, "5w% 2h%", "3w% 1h%")]   // viewport units
        [TestCase(800, 1280, "0", "0")]               // zero padding/border (baseline)
        [TestCase(801, 1025, "-10px", "0")]           // negative padding
        [TestCase(834, 1112, "0", "-5px")]            // negative border
        [TestCase(1024, 768, "10px 5%", "5% 10px")]   // mixed units for both
        [TestCase(1280, 720, "200px", "50px")]        // large px values
        [TestCase(1366, 768, "100%", "50%")]          // large percentages
        [TestCase(1440, 900, "0.5% 1%", "1% 0.5%")]   // small % values
        [TestCase(1920, 1080, "10px 0", "0 5px")]     // asymmetric padding and border
        [TestCase(3840, 2160, "9999px", "9999px")]    // absurdly large values
        [TestCase(4096, 2160, "1w% 2h%", "2w% 1h%")]  // viewport units with mix
        [TestCase(320, 480, "10px 5%", "5% 10px")]    // mixed px/% both padding and border
        [TestCase(360, 640, "-20px", "-10px")]        // negative padding and border
        [TestCase(375, 667, "50%", "25%")]            // high % values
        [TestCase(768, 1024, "0", "50px")]            // border only
        [TestCase(800, 1280, "50px", "0")]            // padding only
        public void BorderSizing(float width, float height, string padding, string border)
        {
            UINode root = new UINode();
            ISceneRunner.Load(root);

            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            root.ParsedStyle.SizingType = SizingType.Border;
            root.ParsedStyle.BorderWidth = StyleParser.ParseSides(border);
            root.ParsedStyle.Padding = StyleParser.ParseSides(padding);
        
            Assertions.AssertVector(root.Size).IsEqualApprox(new Godot.Vector2(width, height), Tolerance);
        }

        /// <summary>
        /// Checks whether <see cref="SizingType.Padding"/> works as expected
        /// </summary>
        /// <remarks>
        /// <see cref="SizingType.Padding"/> should enlarge the size set in styles by border.
        /// but shrink the content by padding.
        /// <para>
        /// It's an intermediate sizing between <see cref="SizingType.Border"/> and <see cref="SizingType.Content"/>
        /// </para>
        /// </remarks>
        /// <param name="width">Node width in pixels</param>
        /// <param name="height">Node height in pixels</param>
        /// <param name="padding">Padding in string (gets parsed)</param>
        /// <param name="border">Border in string (gets parsed)</param>
        [TestCase(320, 480, "0", "0")]                 // no padding, no border
        [TestCase(320, 480, "10px", "1px")]           // simple small border
        [TestCase(360, 640, "20px 10px", "5px 2px")]  // 2-value padding and border
        [TestCase(375, 667, "10% 5%", "5% 2%")]       // percent padding and border
        [TestCase(768, 1024, "5w% 2h%", "3w% 1h%")]   // viewport units
        [TestCase(800, 1280, "0", "0")]               // zero padding/border (baseline)
        [TestCase(801, 1025, "-10px", "0")]           // negative padding
        [TestCase(834, 1112, "0", "-5px")]            // negative border
        [TestCase(1024, 768, "10px 5%", "5% 10px")]   // mixed units for both
        [TestCase(1280, 720, "200px", "50px")]        // large px values
        [TestCase(1366, 768, "100%", "50%")]          // large percentages
        [TestCase(1440, 900, "0.5% 1%", "1% 0.5%")]   // small % values
        [TestCase(1920, 1080, "10px 0", "0 5px")]     // asymmetric padding and border
        [TestCase(3840, 2160, "9999px", "9999px")]    // absurdly large values
        [TestCase(4096, 2160, "1w% 2h%", "2w% 1h%")]  // viewport units with mix
        [TestCase(320, 480, "10px 5%", "5% 10px")]    // mixed px/% both padding and border
        [TestCase(360, 640, "-20px", "-10px")]        // negative padding and border
        [TestCase(375, 667, "50%", "25%")]            // high % values
        [TestCase(768, 1024, "0", "50px")]            // border only
        [TestCase(800, 1280, "50px", "0")]            // padding only
        public void PaddingSizing(float width, float height, string padding, string border)
        {
            UINode root = new UINode();
            ISceneRunner.Load(root);

            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            root.ParsedStyle.SizingType = SizingType.Padding;
            root.ParsedStyle.BorderWidth = StyleParser.ParseSides(border);
            root.ParsedStyle.Padding = StyleParser.ParseSides(padding);
        
            Assertions.AssertVector(root.Size).IsEqualApprox(new Godot.Vector2(
                width + root.ComputedStyle.BorderWidth.Left + root.ComputedStyle.BorderWidth.Right, 
                height + root.ComputedStyle.BorderWidth.Top + root.ComputedStyle.BorderWidth.Bottom
            ), Tolerance);
        }

        /// <summary>
        /// Checks whether <see cref="SizingType.Content"/> works as expected
        /// </summary>
        /// <remarks>
        /// <see cref="SizingType.Content"/> should enlarge the size set in styles by padding and border
        /// (or really anything other than content).
        /// <para>
        /// The <b>advantage</b> is that content size of nodes always remain the same, but the <b>downside</b> is that
        /// designers need to calculate sizes and spacings carefully to not overflow, size the content size can't shrink
        /// </para>
        /// </remarks>
        /// <param name="width">Node width in pixels</param>
        /// <param name="height">Node height in pixels</param>
        /// <param name="padding">Padding in string (gets parsed)</param>
        /// <param name="border">Border in string (gets parsed)</param>
        [TestCase(320, 480, "0", "0")]                 // no padding, no border
        [TestCase(320, 480, "10px", "1px")]           // simple small border
        [TestCase(360, 640, "20px 10px", "5px 2px")]  // 2-value padding and border
        [TestCase(375, 667, "10% 5%", "5% 2%")]       // percent padding and border
        [TestCase(768, 1024, "5w% 2h%", "3w% 1h%")]   // viewport units
        [TestCase(800, 1280, "0", "0")]               // zero padding/border (baseline)
        [TestCase(801, 1025, "-10px", "0")]           // negative padding
        [TestCase(834, 1112, "0", "-5px")]            // negative border
        [TestCase(1024, 768, "10px 5%", "5% 10px")]   // mixed units for both
        [TestCase(1280, 720, "200px", "50px")]        // large px values
        [TestCase(1366, 768, "100%", "50%")]          // large percentages
        [TestCase(1440, 900, "0.5% 1%", "1% 0.5%")]   // small % values
        [TestCase(1920, 1080, "10px 0", "0 5px")]     // asymmetric padding and border
        [TestCase(3840, 2160, "9999px", "9999px")]    // absurdly large values
        [TestCase(4096, 2160, "1w% 2h%", "2w% 1h%")]  // viewport units with mix
        [TestCase(320, 480, "10px 5%", "5% 10px")]    // mixed px/% both padding and border
        [TestCase(360, 640, "-20px", "-10px")]        // negative padding and border
        [TestCase(375, 667, "50%", "25%")]            // high % values
        [TestCase(768, 1024, "0", "50px")]            // border only
        [TestCase(800, 1280, "50px", "0")]            // padding only
        public void ContentSizing(float width, float height, string padding, string border)
        {
            UINode root = new UINode();
            ISceneRunner.Load(root);

            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            root.ParsedStyle.SizingType = SizingType.Content;
            root.ParsedStyle.BorderWidth = StyleParser.ParseSides(border);
            root.ParsedStyle.Padding = StyleParser.ParseSides(padding);

            Assertions.AssertVector(root.Size).IsEqualApprox(new Godot.Vector2(
                width + root.ComputedStyle.BorderWidth.Left + root.ComputedStyle.BorderWidth.Right
                + root.ComputedStyle.Padding.Left + root.ComputedStyle.Padding.Right,

                height + root.ComputedStyle.BorderWidth.Top + root.ComputedStyle.BorderWidth.Bottom
                + root.ComputedStyle.Padding.Top + root.ComputedStyle.Padding.Bottom
            ), Tolerance);
        }
    }
}
