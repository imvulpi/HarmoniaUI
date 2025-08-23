using GdUnit4;
using Godot;
using HarmoniaUI.Commons;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Nodes;
using HarmoniaUI.Tests.Utils;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests the behaviour of <see cref="UINode"/> when resizing the viewport/window
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class ViewportResizingTests
    {
        /// <summary>
        /// Checks the <see cref="UINode"/> size and position after the window gets resized to a set size, 
        /// and after changing it back to the original window size.
        /// </summary>
        /// <param name="width">Size to set the window width to</param>
        /// <param name="height">Size to set the window height to</param>
        [TestCase(320, 480)]
        [TestCase(360, 640)]
        [TestCase(768, 1024)]
        [TestCase(800, 1280)]
        [TestCase(1024, 768)]
        [TestCase(1280, 720)]
        [TestCase(1366, 768)]
        [TestCase(1440, 900)]
        [TestCase(1920, 1080)]
        [TestCase(3840, 2160)]
        [TestCase(4096, 2160)]
        public void WindowSizeResize(int width, int height)
        {
            UINode root = new UINode();
            UINode child = new UINode();
            root.AddChild(child);

            ISceneRunner.Load(root);
            root.NormalStyle.Width = new StyleValue(1, Unit.Percent);
            root.NormalStyle.Height = new StyleValue(1, Unit.Percent);
            child.NormalStyle.Width = new StyleValue(0.5f, Unit.Percent);
            child.NormalStyle.Height = new StyleValue(0.5f, Unit.Percent);
            Vector2I originalViewportSize = root.GetWindow().Size;
            root.GetWindow().Size = new(width, height);
            Vector2 viewportSize = ViewportHelper.GetViewportSize(root);

            Assertions.AssertVector(root.Size).IsEqualApprox(viewportSize, TestConstants.Tolerance);
            Assertions.AssertVector(child.Size).IsEqualApprox(viewportSize/2, TestConstants.Tolerance);
            Assertions.AssertVector(root.Position).IsEqualApprox(Vector2.Zero, TestConstants.Tolerance);
            Assertions.AssertVector(child.Position).IsEqualApprox(Vector2.Zero, TestConstants.Tolerance);

            root.GetWindow().Size = originalViewportSize;

            Assertions.AssertVector(root.Size).IsEqualApprox(originalViewportSize, TestConstants.Tolerance);
            Assertions.AssertVector(child.Size).IsEqualApprox(originalViewportSize / 2, TestConstants.Tolerance);
            Assertions.AssertVector(root.Position).IsEqualApprox(Vector2.Zero, TestConstants.Tolerance);
            Assertions.AssertVector(child.Position).IsEqualApprox(Vector2.Zero, TestConstants.Tolerance);
        }
    }
}
