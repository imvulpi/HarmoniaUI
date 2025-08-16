using GdUnit4;
using Godot;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Nodes;
using static HarmoniaUI.Tests.Utils.TestConstants;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="UINode"/> spacing like margins, padding <b>and border width</b>
    /// </summary>
    [RequireGodotRuntime]
    [TestSuite]
    public class BaseSpacingTests
    {
        /// <summary>
        /// Checks the sizing and offsets of a child with the parent having set padding.
        /// </summary>
        /// <param name="width">Root node width in pixels</param>
        /// <param name="height">Root node height in pixels</param>
        /// <param name="padding">Root node padding</param>
        /// <param name="childWidthPct">Child width in percentage of the root node</param>
        /// <param name="childHeightPct">Child height in percentage of the root node</param>
        [TestCase(320, 480, "1px", 0.5f, 0.5f)]
        [TestCase(360, 640, "50px 20px", 0.75f, 0.2f)]
        [TestCase(768, 1024, "10% 5%", 0, 0)]
        [TestCase(800, 1280, "10w% 7w%", 1.2f, 1.2f)]
        [TestCase(801, 1025, "0", 0.5f, 0.5f)]                           // no padding, odd size
        [TestCase(834, 1112, "5px 10px 15px 20px", 0.3f, 0.7f)]           // 4-value px
        [TestCase(1024, 768, "5% 2%", 0.4f, 0.4f)]                        // symmetric %
        [TestCase(1280, 720, "15w% 10h%", 0.6f, 0.25f)]                   // mixed w%/h%
        [TestCase(1366, 768, "1px 2px 3px 4px", 0.8f, 0.8f)]              // small px padding
        [TestCase(1367, 769, "0.5% 1%", 0.5f, 0.5f)]                      // tiny %
        [TestCase(1440, 900, "20% 10%", 1.0f, 0.5f)]                       // large %
        [TestCase(1920, 1080, "100px", 0.25f, 0.25f)]                     // large px
        [TestCase(3840, 2160, "5w%", 0.9f, 0.9f)]                         // ultra-wide viewport unit
        [TestCase(4096, 2160, "2h%", 0.1f, 0.6f)]                         // tall viewport unit

        // Extreme edge cases
        [TestCase(320, 480, "500px", 0.5f, 0.5f)]                         // padding > parent size (px)
        [TestCase(360, 640, "150% 150%", 0.5f, 0.5f)]                     // padding > 100% of parent
        [TestCase(768, 1024, "-10px -5px", 0.5f, 0.5f)]                   // negative px padding
        [TestCase(800, 1280, "-5% -10%", 0.5f, 0.5f)]                     // negative %
        [TestCase(1280, 720, "200w% 200h%", 0.5f, 0.5f)]                  // insane viewport %
        [TestCase(1440, 900, "9999px", 0.5f, 0.5f)]                       // absurdly large px
        [TestCase(1920, 1080, "50px -50px", 0.5f, 0.5f)]                  // mix positive/negative
        [TestCase(3840, 2160, "100% -100%", 0.5f, 0.5f)]                  // mix full % and negative
        [TestCase(4096, 2160, "-100w% 100h%", 0.5f, 0.5f)]                // mix neg w% and pos h%
        [TestCase(801, 1025, "400% 0", 2f, 2f)]
        public void Padding(int width, int height, string padding, float childWidthPct, float childHeightPct)
        {
            UINode root = new();
            UINode child = new();
            root.AddChild(child);
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.Padding = StyleParser.ParseSides(padding);
            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            child.ParsedStyle.Width = new StyleValue(childWidthPct, Unit.Percent);
            child.ParsedStyle.Height = new StyleValue(childHeightPct, Unit.Percent);
            child.ApplyStyle();

            Assertions.AssertVector(child.Position)
                .IsEqualApprox(new Vector2(root.ComputedStyle.Padding.Left, root.ComputedStyle.Padding.Top),
                                Tolerance);

            Assertions.AssertVector(child.Size)
                .IsEqualApprox(new Vector2(root.ContentWidth * childWidthPct, root.ContentHeight * childHeightPct),
                                Tolerance);
        }

        /// <summary>
        /// Checks the sizing and offsets of a child with the parent having set border width.
        /// </summary>
        /// <param name="width">Root node width in pixels</param>
        /// <param name="height">Root node height in pixels</param>
        /// <param name="border">Root node border</param>
        /// <param name="childWidthPct">Child width in percentage of the root node</param>
        /// <param name="childHeightPct">Child height in percentage of the root node</param>
        [TestCase(320, 480, "1px", 0.5f, 0.5f)]
        [TestCase(360, 640, "50px 20px", 0.75f, 0.2f)]
        [TestCase(768, 1024, "10% 5%", 0, 0)]
        [TestCase(800, 1280, "10w% 7w%", 1.2f, 1.2f)]
        [TestCase(801, 1025, "0", 0.5f, 0.5f)]                           // no padding, odd size
        [TestCase(834, 1112, "5px 10px 15px 20px", 0.3f, 0.7f)]           // 4-value px
        [TestCase(1024, 768, "5% 2%", 0.4f, 0.4f)]                        // symmetric %
        [TestCase(1280, 720, "15w% 10h%", 0.6f, 0.25f)]                   // mixed w%/h%
        [TestCase(1366, 768, "1px 2px 3px 4px", 0.8f, 0.8f)]              // small px padding
        [TestCase(1367, 769, "0.5% 1%", 0.5f, 0.5f)]                      // tiny %
        [TestCase(1440, 900, "20% 10%", 1.0f, 0.5f)]                       // large %
        [TestCase(1920, 1080, "100px", 0.25f, 0.25f)]                     // large px
        [TestCase(3840, 2160, "5w%", 0.9f, 0.9f)]                         // ultra-wide viewport unit
        [TestCase(4096, 2160, "2h%", 0.1f, 0.6f)]                         // tall viewport unit

        // Extreme edge cases
        [TestCase(320, 480, "500px", 0.5f, 0.5f)]                         // padding > parent size (px)
        [TestCase(360, 640, "150% 150%", 0.5f, 0.5f)]                     // padding > 100% of parent
        [TestCase(768, 1024, "-10px -5px", 0.5f, 0.5f)]                   // negative px padding
        [TestCase(800, 1280, "-5% -10%", 0.5f, 0.5f)]                     // negative %
        [TestCase(1280, 720, "200w% 200h%", 0.5f, 0.5f)]                  // insane viewport %
        [TestCase(1440, 900, "9999px", 0.5f, 0.5f)]                       // absurdly large px
        [TestCase(1920, 1080, "50px -50px", 0.5f, 0.5f)]                  // mix positive/negative
        [TestCase(3840, 2160, "100% -100%", 0.5f, 0.5f)]                  // mix full % and negative
        [TestCase(4096, 2160, "-100w% 100h%", 0.5f, 0.5f)]                // mix neg w% and pos h%
        [TestCase(801, 1025, "400% 0", 2f, 2f)]
        public void Border(int width, int height, string border, float childWidthPct, float childHeightPct)
        {
            UINode root = new();
            UINode child = new();
            root.AddChild(child);
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.BorderWidth = StyleParser.ParseSides(border);
            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            child.ParsedStyle.Width = new StyleValue(childWidthPct, Unit.Percent);
            child.ParsedStyle.Height = new StyleValue(childHeightPct, Unit.Percent);
            child.ApplyStyle();

            Assertions.AssertVector(child.Position)
                .IsEqualApprox(new Vector2(root.ComputedStyle.BorderWidth.Left, root.ComputedStyle.BorderWidth.Top),
                                Tolerance);

            Assertions.AssertVector(child.Size)
                .IsEqualApprox(new Vector2(root.ContentWidth * childWidthPct, root.ContentHeight * childHeightPct),
                                Tolerance);
        }

        /// <summary>
        /// Checks the sizing and offsets of a child with the parent having set margin.
        /// </summary>
        /// <param name="width">Root node width in pixels</param>
        /// <param name="height">Root node height in pixels</param>
        /// <param name="margin">Root node margin</param>
        /// <param name="childWidthPct">Child width in percentage of the root node</param>
        /// <param name="childHeightPct">Child height in percentage of the root node</param>
        [TestCase(320, 480, "1px", 0.5f, 0.5f)]
        [TestCase(360, 640, "50px 20px", 0.75f, 0.2f)]
        [TestCase(768, 1024, "10% 5%", 0, 0)]
        [TestCase(800, 1280, "10w% 7w%", 1.2f, 1.2f)]
        [TestCase(801, 1025, "0", 0.5f, 0.5f)]                           // no padding, odd size
        [TestCase(834, 1112, "5px 10px 15px 20px", 0.3f, 0.7f)]           // 4-value px
        [TestCase(1024, 768, "5% 2%", 0.4f, 0.4f)]                        // symmetric %
        [TestCase(1280, 720, "15w% 10h%", 0.6f, 0.25f)]                   // mixed w%/h%
        [TestCase(1366, 768, "1px 2px 3px 4px", 0.8f, 0.8f)]              // small px padding
        [TestCase(1367, 769, "0.5% 1%", 0.5f, 0.5f)]                      // tiny %
        [TestCase(1440, 900, "20% 10%", 1.0f, 0.5f)]                       // large %
        [TestCase(1920, 1080, "100px", 0.25f, 0.25f)]                     // large px
        [TestCase(3840, 2160, "5w%", 0.9f, 0.9f)]                         // ultra-wide viewport unit
        [TestCase(4096, 2160, "2h%", 0.1f, 0.6f)]                         // tall viewport unit

        // Extreme edge cases
        [TestCase(320, 480, "500px", 0.5f, 0.5f)]                         // padding > parent size (px)
        [TestCase(360, 640, "150% 150%", 0.5f, 0.5f)]                     // padding > 100% of parent
        [TestCase(768, 1024, "-10px -5px", 0.5f, 0.5f)]                   // negative px padding
        [TestCase(800, 1280, "-5% -10%", 0.5f, 0.5f)]                     // negative %
        [TestCase(1280, 720, "200w% 200h%", 0.5f, 0.5f)]                  // insane viewport %
        [TestCase(1440, 900, "9999px", 0.5f, 0.5f)]                       // absurdly large px
        [TestCase(1920, 1080, "50px -50px", 0.5f, 0.5f)]                  // mix positive/negative
        [TestCase(3840, 2160, "100% -100%", 0.5f, 0.5f)]                  // mix full % and negative
        [TestCase(4096, 2160, "-100w% 100h%", 0.5f, 0.5f)]                // mix neg w% and pos h%
        [TestCase(801, 1025, "400% 0", 2f, 2f)]
        public void Margin(int width, int height, string margin, float childWidthPct, float childHeightPct)
        {
            UINode root = new();
            UINode child = new();
            root.AddChild(child);
            ISceneRunner sceneRunner = ISceneRunner.Load(root);

            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            child.ParsedStyle.Margin = StyleParser.ParseSides(margin);
            child.ParsedStyle.Width = new StyleValue(childWidthPct, Unit.Percent);
            child.ParsedStyle.Height = new StyleValue(childHeightPct, Unit.Percent);

            Assertions.AssertVector(child.Position)
                .IsEqualApprox(new Vector2(child.ComputedStyle.Margin.Left, child.ComputedStyle.Margin.Top), Tolerance);

            Assertions.AssertVector(child.Size)
                .IsEqualApprox(new Vector2(root.ContentWidth * childWidthPct, root.ContentHeight * childHeightPct), Tolerance);
        }

        /// <summary>
        /// Checks the sizing and offsets of a child with the parent having set border width and padding.
        /// </summary>
        /// <param name="width">Root node width in pixels</param>
        /// <param name="height">Root node height in pixels</param>
        /// <param name="border">Root node border</param>
        /// <param name="padding">Root node padding</param>
        /// <param name="childWidthPct">Child width in percentage of the root node</param>
        /// <param name="childHeightPct">Child height in percentage of the root node</param>
        [TestCase(320, 480, "10px", "5px", 0.5f, 0.5f)]                           // simple px
        [TestCase(360, 640, "20px 10px", "5px 15px", 0.75f, 0.25f)]                // 2-value px
        [TestCase(375, 667, "10% 5%", "5% 2%", 0.6f, 0.6f)]                        // % units
        [TestCase(768, 1024, "5w% 2h%", "2w% 1h%", 0.4f, 0.4f)]                     // viewport %
        [TestCase(800, 1280, "5px 10% 15px 20%", "2% 4px 1% 8px", 0.3f, 0.7f)]      // mixed px/%
        [TestCase(801, 1025, "0", "0", 0.5f, 0.5f)]                                // no margin/padding, odd size
        [TestCase(834, 1112, "-10px", "0", 0.5f, 0.5f)]                            // negative padding only
        [TestCase(1024, 768, "0", "-10px", 0.5f, 0.5f)]                            // negative margin only
        [TestCase(1280, 720, "-5% -5%", "-2% -2%", 0.5f, 0.5f)]                    // both negative %
        [TestCase(1366, 768, "200px", "50px", 0.5f, 0.5f)]                         // large px
        [TestCase(1367, 769, "100%", "50%", 0.5f, 0.5f)]                           // large %
        [TestCase(1440, 900, "9999px", "9999px", 0.5f, 0.5f)]                      // absurd px
        [TestCase(1920, 1080, "400% 0", "0 400%", 0.5f, 0.5f)]                     // absurd %
        [TestCase(3840, 2160, "-50% -50%", "-50% -50%", 0.5f, 0.5f)]               // negative extreme
        [TestCase(4096, 2160, "0.5% 1%", "1% 0.5%", 0.8f, 0.8f)]                   // small %
        [TestCase(320, 480, "1w% 1h%", "2w% 2h%", 0.9f, 0.4f)]                     // viewport w%/h% mix
        [TestCase(360, 640, "10px", "-20px", 0.5f, 0.8f)]                          // pos padding, neg margin
        [TestCase(375, 667, "-20px", "10px", 0.5f, 0.8f)]                          // neg padding, pos margin
        [TestCase(768, 1024, "0", "200%", 0.5f, 0.5f)]                             // margin > parent
        [TestCase(800, 1280, "200%", "0", 0.5f, 0.5f)]                             // padding > parent
        public void BorderAndPadding(int width, int height, string border, string padding, float childWidthPct, float childHeightPct)
        {
            UINode root = new();
            UINode child = new();
            root.AddChild(child);
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.BorderWidth = StyleParser.ParseSides(border);
            root.ParsedStyle.Padding = StyleParser.ParseSides(padding);
            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            child.ParsedStyle.Width = new StyleValue(childWidthPct, Unit.Percent);
            child.ParsedStyle.Height = new StyleValue(childHeightPct, Unit.Percent);
            child.ApplyStyle();

            Assertions.AssertVector(child.Position)
                .IsEqualApprox(new Vector2(
                    root.ComputedStyle.BorderWidth.Left + root.ComputedStyle.Padding.Left,
                    root.ComputedStyle.BorderWidth.Top + root.ComputedStyle.Padding.Top),
                                Tolerance);

            Assertions.AssertVector(child.Size)
                .IsEqualApprox(new Vector2(root.ContentWidth * childWidthPct, root.ContentHeight * childHeightPct),
                                Tolerance);
        }

        /// <summary>
        /// Checks the sizing and offsets of a child with margin which parent has padding.
        /// </summary>
        /// <param name="width">Root node width in pixels</param>
        /// <param name="height">Root node height in pixels</param>
        /// <param name="margin">Root node margin</param>
        /// <param name="padding">Root node padding</param>
        /// <param name="childWidthPct">Child width in percentage of the root node</param>
        /// <param name="childHeightPct">Child height in percentage of the root node</param>
        [TestCase(320, 480, "10px", "5px", 0.5f, 0.5f)]                           // simple px
        [TestCase(360, 640, "20px 10px", "5px 15px", 0.75f, 0.25f)]                // 2-value px
        [TestCase(375, 667, "10% 5%", "5% 2%", 0.6f, 0.6f)]                        // % units
        [TestCase(768, 1024, "5w% 2h%", "2w% 1h%", 0.4f, 0.4f)]                     // viewport %
        [TestCase(800, 1280, "5px 10% 15px 20%", "2% 4px 1% 8px", 0.3f, 0.7f)]      // mixed px/%
        [TestCase(801, 1025, "0", "0", 0.5f, 0.5f)]                                // no margin/padding, odd size
        [TestCase(834, 1112, "-10px", "0", 0.5f, 0.5f)]                            // negative padding only
        [TestCase(1024, 768, "0", "-10px", 0.5f, 0.5f)]                            // negative margin only
        [TestCase(1280, 720, "-5% -5%", "-2% -2%", 0.5f, 0.5f)]                    // both negative %
        [TestCase(1366, 768, "200px", "50px", 0.5f, 0.5f)]                         // large px
        [TestCase(1367, 769, "100%", "50%", 0.5f, 0.5f)]                           // large %
        [TestCase(1440, 900, "9999px", "9999px", 0.5f, 0.5f)]                      // absurd px
        [TestCase(1920, 1080, "400% 0", "0 400%", 0.5f, 0.5f)]                     // absurd %
        [TestCase(3840, 2160, "-50% -50%", "-50% -50%", 0.5f, 0.5f)]               // negative extreme
        [TestCase(4096, 2160, "0.5% 1%", "1% 0.5%", 0.8f, 0.8f)]                   // small %
        [TestCase(320, 480, "1w% 1h%", "2w% 2h%", 0.9f, 0.4f)]                     // viewport w%/h% mix
        [TestCase(360, 640, "10px", "-20px", 0.5f, 0.8f)]                          // pos padding, neg margin
        [TestCase(375, 667, "-20px", "10px", 0.5f, 0.8f)]                          // neg padding, pos margin
        [TestCase(768, 1024, "0", "200%", 0.5f, 0.5f)]                             // margin > parent
        [TestCase(800, 1280, "200%", "0", 0.5f, 0.5f)]                             // padding > parent
        public void MarginAndPadding(int width, int height, string padding, string margin, float childWidthPct, float childHeightPct)
        {
            UINode root = new();
            UINode child = new();
            root.AddChild(child);
            ISceneRunner sceneRunner = ISceneRunner.Load(root);

            root.ParsedStyle.Width = new StyleValue(width, Unit.Pixel);
            root.ParsedStyle.Height = new StyleValue(height, Unit.Pixel);
            root.ParsedStyle.Padding = StyleParser.ParseSides(padding);
            child.ParsedStyle.Margin = StyleParser.ParseSides(margin);
            child.ParsedStyle.Width = new StyleValue(childWidthPct, Unit.Percent);
            child.ParsedStyle.Height = new StyleValue(childHeightPct, Unit.Percent);

            Assertions.AssertVector(child.Position)
                .IsEqualApprox(new Vector2(child.ComputedStyle.Margin.Left + root.ComputedStyle.Padding.Left, 
                                     child.ComputedStyle.Margin.Top + root.ComputedStyle.Padding.Top), Tolerance);

            Assertions.AssertVector(child.Size)
                .IsEqualApprox(new Vector2(root.ContentWidth * childWidthPct, root.ContentHeight * childHeightPct), Tolerance);
        }

        /// <summary>
        /// Checks position offsets of child nodes caused by the margin in multiple nodes.
        /// </summary>
        /// <param name="margin1">Margin of the first child</param>
        /// <param name="margin2">Margin of the second child</param>
        [TestCase("10px", "10px")]                  // equal px margins
        [TestCase("5px", "15px")]                   // different px margins
        [TestCase("0", "0")]                         // no margin at all
        [TestCase("10%", "10%")]                     // equal % margins
        [TestCase("5%", "15%")]                      // different % margins
        [TestCase("5px 10px", "10px 5px")]           // 2-value px margins
        [TestCase("5% 10%", "10% 5%")]               // 2-value % margins
        [TestCase("5w% 2h%", "3w% 1h%")]             // viewport width/height %
        [TestCase("-10px", "-10px")]                 // equal negative px margins (overlap)
        [TestCase("-5px", "10px")]                   // negative vs positive
        [TestCase("10px", "-5px")]                   // positive vs negative
        [TestCase("-5% -5%", "-5% -5%")]             // equal negative %
        [TestCase("9999px", "0")]                    // huge margin on one node
        [TestCase("0", "9999px")]                    // huge margin on the other node
        [TestCase("50%", "50%")]                     // massive percentage margins
        [TestCase("0.5% 1%", "1% 0.5%")]              // small % margins
        [TestCase("10px 5%", "5% 10px")]             // mixed px and %
        [TestCase("1w% 1h%", "2w% 2h%")]              // viewport w%/h% mix
        [TestCase("20px 10px", "20px 10px")]         // equal complex px
        [TestCase("15% 5px", "5px 15%")]             // mixed and swapped
        public void MarginMultipleNodes(string margin1, string margin2)
        {
            UINode root = new();
            UINode child1 = new();
            UINode child2 = new();

            root.AddChild(child1);
            root.AddChild(child2);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            child1.ParsedStyle.Margin = StyleParser.ParseSides(margin1);
            child1.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child1.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);

            child2.ParsedStyle.Margin = StyleParser.ParseSides(margin2);
            child2.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child2.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);

            Assertions.AssertVector(child1.Position)
                .IsEqualApprox(new Vector2(child1.ComputedStyle.Margin.Left, child1.ComputedStyle.Margin.Top), Tolerance);

            Assertions.AssertVector(child2.Position)
                .IsEqualApprox(new Vector2(child2.ComputedStyle.Margin.Left, 
                child1.Size.Y + child2.ComputedStyle.Margin.Top 
                + child1.ComputedStyle.Margin.Top
                + child1.ComputedStyle.Margin.Bottom), Tolerance);
        }

        /// <summary>
        /// Checks position offsets and sizing caused by the padding in multiple nodes.
        /// </summary>
        /// <param name="padding1">Padding of the first child</param>
        /// <param name="padding2">Padding of the second child</param>
        [TestCase("10px", "10px")]                  // equal px margins
        [TestCase("5px", "15px")]                   // different px margins
        [TestCase("0", "0")]                         // no margin at all
        [TestCase("10%", "10%")]                     // equal % margins
        [TestCase("5%", "15%")]                      // different % margins
        [TestCase("5px 10px", "10px 5px")]           // 2-value px margins
        [TestCase("5% 10%", "10% 5%")]               // 2-value % margins
        [TestCase("5w% 2h%", "3w% 1h%")]             // viewport width/height %
        [TestCase("-10px", "-10px")]                 // equal negative px margins (overlap)
        [TestCase("-5px", "10px")]                   // negative vs positive
        [TestCase("10px", "-5px")]                   // positive vs negative
        [TestCase("-5% -5%", "-5% -5%")]             // equal negative %
        [TestCase("9999px", "0")]                    // huge margin on one node
        [TestCase("0", "9999px")]                    // huge margin on the other node
        [TestCase("50%", "50%")]                     // massive percentage margins
        [TestCase("0.5% 1%", "1% 0.5%")]              // small % margins
        [TestCase("10px 5%", "5% 10px")]             // mixed px and %
        [TestCase("1w% 1h%", "2w% 2h%")]              // viewport w%/h% mix
        [TestCase("20px 10px", "20px 10px")]         // equal complex px
        [TestCase("15% 5px", "5px 15%")]             // mixed and swapped
        public void PaddingMultipleNodes(string padding1, string padding2)
        {
            UINode root = new();
            UINode child1 = new();
            UINode child2 = new();

            root.AddChild(child1);
            child1.AddChild(child2);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.Padding = StyleParser.ParseSides(padding1);

            child1.ParsedStyle.Padding = StyleParser.ParseSides(padding2);
            child1.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child1.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);

            child2.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child2.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);

            Assertions.AssertVector(child2.GlobalPosition)
                .IsEqualApprox(new Vector2(
                root.ComputedStyle.Padding.Left
                + child1.ComputedStyle.Padding.Left, 
                root.ComputedStyle.Padding.Top 
                + child1.ComputedStyle.Padding.Top), Tolerance);
        }

        /// <summary>
        /// Checks position offsets and sizing caused by the border in multiple nodes.
        /// </summary>
        /// <param name="border1">Border of the first child</param>
        /// <param name="border2">Border of the second child</param>
        [TestCase("10px", "10px")]                  // equal px margins
        [TestCase("5px", "15px")]                   // different px margins
        [TestCase("0", "0")]                         // no margin at all
        [TestCase("10%", "10%")]                     // equal % margins
        [TestCase("5%", "15%")]                      // different % margins
        [TestCase("5px 10px", "10px 5px")]           // 2-value px margins
        [TestCase("5% 10%", "10% 5%")]               // 2-value % margins
        [TestCase("5w% 2h%", "3w% 1h%")]             // viewport width/height %
        [TestCase("-10px", "-10px")]                 // equal negative px margins (overlap)
        [TestCase("-5px", "10px")]                   // negative vs positive
        [TestCase("10px", "-5px")]                   // positive vs negative
        [TestCase("-5% -5%", "-5% -5%")]             // equal negative %
        [TestCase("9999px", "0")]                    // huge margin on one node
        [TestCase("0", "9999px")]                    // huge margin on the other node
        [TestCase("50%", "50%")]                     // massive percentage margins
        [TestCase("0.5% 1%", "1% 0.5%")]              // small % margins
        [TestCase("10px 5%", "5% 10px")]             // mixed px and %
        [TestCase("1w% 1h%", "2w% 2h%")]              // viewport w%/h% mix
        [TestCase("20px 10px", "20px 10px")]         // equal complex px
        [TestCase("15% 5px", "5px 15%")]             // mixed and swapped
        public void BorderMultipleNodes(string border1, string border2)
        {
            UINode root = new();
            UINode child1 = new();
            UINode child2 = new();

            root.AddChild(child1);
            child1.AddChild(child2);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.ParsedStyle.BorderWidth = StyleParser.ParseSides(border1);

            child1.ParsedStyle.BorderWidth = StyleParser.ParseSides(border2);
            child1.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child1.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);

            child2.ParsedStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child2.ParsedStyle.Height = new StyleValue(0.5f, Unit.Percent);

            Assertions.AssertVector(child2.GlobalPosition)
                .IsEqualApprox(new Vector2(
                root.ComputedStyle.BorderWidth.Left
                + child1.ComputedStyle.BorderWidth.Left, 
                root.ComputedStyle.BorderWidth.Top 
                + child1.ComputedStyle.BorderWidth.Top), Tolerance);
        }
    }
}
