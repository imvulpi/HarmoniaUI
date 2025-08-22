using GdUnit4;
using Godot;
using HarmoniaUI.Tests.Utils;
using System.Threading.Tasks;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests the visuals of nodes (ex. colors, borders)
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class VisualTests
    {
        private const string SHADOW_TEST = "res://tests/scenes/ShadowTest.tscn";
        private const string BACKGROUND_TEST = "res://tests/scenes/BackgroundTest.tscn";
        private const string BORDER_TEST = "res://tests/scenes/BorderTest.tscn";
        private const string HOVER_FOCUS_TEST = "res://tests/scenes/HoverAndFocusTest.tscn";

        /// <summary>
        /// Sets the window mode to <see cref="DisplayServer.WindowMode.Windowed"/> in order for visuals to get drawn.
        /// </summary>
        [BeforeTest]
        public void BeforeTest()
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }

        /// <summary>
        /// Tests the shadow, for correct color and sizes under different viewport sizes
        /// </summary>
        /// <param name="viewportWidth">Width of the viewport</param>
        /// <param name="viewportHeight">Height of the viewport</param>
        /// <returns>Shadow test task</returns>
        [TestCase(320, 480)]
        [TestCase(360, 640)]
        [TestCase(361, 641)]   // odd
        [TestCase(375, 667)]
        [TestCase(414, 896)]
        [TestCase(768, 1024)]
        [TestCase(800, 1280)]
        [TestCase(801, 1025)]  // odd
        [TestCase(834, 1112)]
        [TestCase(1024, 768)]
        [TestCase(1280, 720)]
        [TestCase(1366, 768)]
        [TestCase(1367, 769)]  // odd
        [TestCase(1440, 900)]
        [TestCase(1441, 901)]  // odd
        [TestCase(1920, 1080)]
        [TestCase(3840, 2160)]
        [TestCase(4096, 2160)]
        public async Task ShadowTest(int viewportWidth, int viewportHeight)
        {
            using ISceneRunner runner = ISceneRunner.Load(SHADOW_TEST, true, true);
            var tests = runner.Scene() as TestsRepo;
            tests.GetWindow().Size = new Vector2I(viewportWidth, viewportHeight);
            await TestsHelper.ProcessTestRepo(tests);
        }

        /// <summary>
        /// Tests the background, for correct color and sizes under different viewport sizes
        /// </summary>
        /// <param name="viewportWidth">Width of the viewport</param>
        /// <param name="viewportHeight">Height of the viewport</param>
        /// <returns>Background test task</returns>
        [TestCase(320, 480)]
        [TestCase(360, 640)]
        [TestCase(361, 641)]   // odd
        [TestCase(375, 667)]
        [TestCase(414, 896)]
        [TestCase(768, 1024)]
        [TestCase(800, 1280)]
        [TestCase(801, 1025)]  // odd
        [TestCase(834, 1112)]
        [TestCase(1024, 768)]
        [TestCase(1280, 720)]
        [TestCase(1366, 768)]
        [TestCase(1367, 769)]  // odd
        [TestCase(1440, 900)]
        [TestCase(1441, 901)]  // odd
        [TestCase(1920, 1080)]
        [TestCase(3840, 2160)]
        [TestCase(4096, 2160)]
        public async Task BackgroundTest(int viewportWidth, int viewportHeight)
        {
            using ISceneRunner runner = ISceneRunner.Load(BACKGROUND_TEST, true, true);
            var tests = runner.Scene() as TestsRepo;
            tests.GetWindow().Size = new Vector2I(viewportWidth, viewportHeight);

            await TestsHelper.ProcessTestRepo(tests);
        }

        /// <summary>
        /// Tests the border, for correct color, sizes and rounding under different viewport sizes
        /// </summary>
        /// <remarks>
        /// It's hard to test the roundness, there is a big tolerance there. It will detect whenether there is NO roundness.
        /// </remarks>
        /// <param name="viewportWidth">Width of the viewport</param>
        /// <param name="viewportHeight">Height of the viewport</param>
        /// <returns>Border test task</returns>
        [TestCase(320, 480)]
        [TestCase(360, 640)]
        [TestCase(361, 641)]   // odd
        [TestCase(375, 667)]
        [TestCase(414, 896)]
        [TestCase(768, 1024)]
        [TestCase(800, 1280)]
        [TestCase(801, 1025)]  // odd
        [TestCase(834, 1112)]
        [TestCase(1024, 768)]
        [TestCase(1280, 720)]
        [TestCase(1366, 768)]
        [TestCase(1367, 769)]  // odd
        [TestCase(1440, 900)]
        [TestCase(1441, 901)]  // odd
        [TestCase(1920, 1080)]
        [TestCase(3840, 2160)]
        [TestCase(4096, 2160)]
        public async Task RoundedBorderTest(int viewportWidth, int viewportHeight)
        {
            using ISceneRunner runner = ISceneRunner.Load(BORDER_TEST, true, true);
            var tests = runner.Scene() as TestsRepo;
            tests.GetWindow().Size = new Vector2I(viewportWidth, viewportHeight);

            await TestsHelper.ProcessTestRepo(tests);
        }

        /// <summary>
        /// Tests the background color during interactions of hovering and focus.
        /// </summary>
        /// <remarks>
        /// More specific tests could be created, but we should assume that if merging
        /// is correct and other tests pass this is also good.
        /// </remarks>
        /// <param name="viewportWidth">Width of the viewport</param>
        /// <param name="viewportHeight">Height of the viewport</param>
        /// <returns>Test task for hover and focus interactions</returns>
        [TestCase(320, 480)]
        [TestCase(1280, 720)]
        [TestCase(1920, 1080)]
        [TestCase(3840, 2160)]
        public async Task HoverAndFocusStyleTest(int viewportWidth, int viewportHeight)
        {
            using ISceneRunner runner = ISceneRunner.Load(HOVER_FOCUS_TEST, true, true);
            runner.SimulateMouseMove(new Vector2(10,10));
            var tests = runner.Scene() as TestsRepo;
            tests.GetWindow().Size = new Vector2I(viewportWidth, viewportHeight);

            await TestsHelper.ProcessTestRepo(tests);

            runner.SimulateMouseButtonPress(MouseButton.Left);
            if(tests.TestNodes[0] is ColorTest colorTest)
            {
                colorTest.ExpectedColor = new Color(0, 0, 1, 1);
            }

            await TestsHelper.ProcessTestRepo(tests);
        }
    }
}
