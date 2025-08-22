using GdUnit4;
using Godot;
using HarmoniaUI.Commons;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Nodes;
using System;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="UINode"/> sizes.
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class BaseSizeTests
    {
        /// <summary>
        /// Tests the default size of the <see cref="UINode"/> which should be the viewport/parent size.
        /// </summary>
        /// <param name="viewportHeight">Height of the viewport pixels</param>
        /// <param name="viewportWidth">Width of the viewport pixels</param>
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
        public void DefaultSize(int viewportWidth, int viewportHeight)
        {
            UINode root = new();
            ISceneRunner sceneRunner = ISceneRunner.Load(root);

            root.GetWindow().Size = new Vector2I(viewportWidth, viewportHeight);
            Assertions.AssertVector(root.Size).IsEqual(ViewportHelper.GetViewportSize(root));
        }

        /// <summary>
        /// Checks the size that was set in pixels.
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        [TestCase(500, 500)]
        [TestCase(1152, 648)]
        [TestCase(1920, 1080)]
        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(0.5f, 0.5f)]
        [TestCase(-1, -1)]
        [TestCase(8192, 8192)]
        public void PixelSize(float width, float height)
        {
            UINode root = new();
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.NormalStyle.Width = new StyleValue(width, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(height, Unit.Pixel);

            Vector2 expected = new Vector2(MathF.Max(0, width), MathF.Max(0, height));
            Assertions.AssertVector(root.Size).IsEqual(expected);
        }

        /// <summary>
        /// Checks a child size which is based on percentage from parent
        /// </summary>
        /// <param name="parentWidth">Parent width in pixels</param>
        /// <param name="parentHeight">Parent height in pixels</param>
        /// <param name="childWidthPercent">Child width in percentages</param>
        /// <param name="childHeightPercent">Child height in percentages</param>
        [TestCase(320, 480, 0.5f, 0.5f)]
        [TestCase(360, 640, 0.25f, 0.75f)]
        [TestCase(361, 641, 0.33f, 0.66f)]   // odd
        [TestCase(375, 667, 0.75f, 0.25f)]
        [TestCase(414, 896, 1.0f, 0.5f)]
        [TestCase(768, 1024, 0.5f, 0.25f)]
        [TestCase(800, 1280, 0.5f, 0.75f)]
        [TestCase(801, 1025, 0.33f, 0.5f)]  // odd
        [TestCase(834, 1112, 0.66f, 0.33f)]
        [TestCase(1024, 768, 0.5f, 1.0f)]
        [TestCase(1280, 720, 0.25f, 0.5f)]
        [TestCase(1366, 768, 0.5f, 0.5f)]
        [TestCase(1367, 769, 0.75f, 0.75f)]  // odd
        [TestCase(1440, 900, 0.33f, 0.66f)]
        [TestCase(1441, 901, 0.5f, 0.25f)]  // odd
        [TestCase(1920, 1080, 0.66f, 0.33f)]
        [TestCase(3840, 2160, 0.5f, 0.5f)]
        [TestCase(4096, 2160, 1.0f, 1.0f)]
        public void PercentageSize(int parentWidth, int parentHeight, float childWidthPercent, float childHeightPercent)
        {
            UINode root = new();
            UINode child = new(); // subject
            root.AddChild(child);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GetWindow().Size = new Vector2I(parentWidth, parentHeight);
            root.NormalStyle.Width = new StyleValue(parentWidth, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(parentHeight, Unit.Pixel);
            child.NormalStyle.Width = new StyleValue(childWidthPercent, Unit.Percent);
            child.NormalStyle.Height = new StyleValue(childHeightPercent, Unit.Percent);

            Vector2 expected = new(
                parentWidth * childWidthPercent,
                parentHeight * childHeightPercent
            );

            Assertions.AssertVector(child.Size).IsEqual(expected);
        }

        /// <summary>
        /// Checks the child which has both size based on <see cref="Unit.WidthPercent"/> of the parent.
        /// </summary>
        /// <param name="parentWidth">Parent width in pixels</param>
        /// <param name="parentHeight">Parent height in pixels</param>
        /// <param name="childWidthPercent">Child height and width based on (w%) <see cref="Unit.WidthPercent"/></param>
        [TestCase(320, 480, 0.5f)]
        [TestCase(360, 640, 0.25f)]
        [TestCase(361, 641, 0.33f)]   // odd
        [TestCase(375, 667, 0.75f)]
        [TestCase(414, 896, 1.0f)]
        [TestCase(768, 1024, 0.5f)]
        [TestCase(800, 1280, 0.5f)]
        [TestCase(801, 1025, 0.33f)]  // odd
        [TestCase(834, 1112, 0.66f)]
        [TestCase(1024, 768, 0.5f)]
        [TestCase(1280, 720, 0.25f)]
        [TestCase(1366, 768, 0.5f)]
        [TestCase(1367, 769, 0.75f)]  // odd
        [TestCase(1440, 900, 0.33f)]
        [TestCase(1441, 901, 0.5f)]  // odd
        [TestCase(1920, 1080, 0.66f)]
        [TestCase(3840, 2160, 0.5f)]
        [TestCase(4096, 2160, 1.0f)]
        public void PercentageWidthSize(int parentWidth, int parentHeight, float childWidthPercent)
        {
            UINode root = new();
            UINode child = new(); // subject
            root.AddChild(child);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GetWindow().Size = new Vector2I(parentWidth, parentHeight);
            root.NormalStyle.Width = new StyleValue(parentWidth, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(parentHeight, Unit.Pixel);
            child.NormalStyle.Width = new StyleValue(childWidthPercent, Unit.WidthPercent);
            child.NormalStyle.Height = new StyleValue(childWidthPercent, Unit.WidthPercent);

            Vector2 expected = new(
                parentWidth * childWidthPercent,
                parentWidth * childWidthPercent
            );

            Assertions.AssertVector(child.Size).IsEqual(expected);
        }

        /// <summary>
        /// Checks the child which has both size based on <see cref="Unit.HeightPercent"/> of the parent.
        /// </summary>
        /// <param name="parentWidth">Parent width in pixels</param>
        /// <param name="parentHeight">Parent height in pixels</param>
        /// <param name="childHeightPercent">Child height and width based on (h%) <see cref="Unit.HeightPercent"/></param>
        [TestCase(320, 480, 0.5f)]
        [TestCase(360, 640, 0.25f)]
        [TestCase(361, 641, 0.33f)]   // odd
        [TestCase(375, 667, 0.75f)]
        [TestCase(414, 896, 1.0f)]
        [TestCase(768, 1024, 0.5f)]
        [TestCase(800, 1280, 0.5f)]
        [TestCase(801, 1025, 0.33f)]  // odd
        [TestCase(834, 1112, 0.66f)]
        [TestCase(1024, 768, 0.5f)]
        [TestCase(1280, 720, 0.25f)]
        [TestCase(1366, 768, 0.5f)]
        [TestCase(1367, 769, 0.75f)]  // odd
        [TestCase(1440, 900, 0.33f)]
        [TestCase(1441, 901, 0.5f)]  // odd
        [TestCase(1920, 1080, 0.66f)]
        [TestCase(3840, 2160, 0.5f)]
        [TestCase(4096, 2160, 1.0f)]
        public void PercentageHeightSize(int parentWidth, int parentHeight, float childHeightPercent)
        {
            UINode root = new();
            UINode child = new(); // subject
            root.AddChild(child);

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GetWindow().Size = new Vector2I(parentWidth, parentHeight);
            root.NormalStyle.Width = new StyleValue(parentWidth, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(parentHeight, Unit.Pixel);
            child.NormalStyle.Width = new StyleValue(childHeightPercent, Unit.HeightPercent);
            child.NormalStyle.Height = new StyleValue(childHeightPercent, Unit.HeightPercent);

            Vector2 expected = new(
                parentHeight * childHeightPercent,
                parentHeight * childHeightPercent
            );

            Assertions.AssertVector(child.Size).IsEqual(expected);
        }

        /// <summary>
        /// Checks <see cref="Unit.ViewportHeight"/>, changes the window size 
        /// and check the <see cref="UINode"/> size which is based on <see cref="Unit.ViewportHeight"/>
        /// </summary>
        /// <param name="viewportHeight">Height of the viewport in pixels</param>
        [TestCase(240)]   // very small
        [TestCase(360)]   // small older phones
        [TestCase(480)]   // older mobile landscape
        [TestCase(568)]   // iPhone 5/SE
        [TestCase(667)]   // iPhone 6/7/8
        [TestCase(720)]   // HD
        [TestCase(721)]   // odd, just over HD
        [TestCase(812)]   // iPhone X/XS/11 Pro
        [TestCase(853)]   // 480p widescreen height
        [TestCase(896)]   // modern large phone
        [TestCase(1024)]  // tablet
        [TestCase(1080)]  // Full HD
        [TestCase(1200)]  // UXGA / some monitors
        [TestCase(1440)]  // QHD
        [TestCase(1600)]  // UXGA variants
        [TestCase(2160)]  // 4K UHD
        [TestCase(8192)]  // very big
        [TestCase(997)]   // odd arbitrary
        public void ViewportHeightSize(int viewportHeight)
        {
            UINode root = new();
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.NormalStyle.Width = new StyleValue(0.5f, Unit.ViewportHeight);
            root.NormalStyle.Height = new StyleValue(0.5f, Unit.ViewportHeight);

            var windowSize = root.GetWindow().Size;
            root.GetWindow().Size = new Vector2I(windowSize.X, viewportHeight);

            float viewportY = ViewportHelper.GetViewportSize(root).Y;
            Assertions.AssertVector(root.Size).IsEqual(new Vector2(viewportY / 2, viewportY / 2));
        }

        /// <summary>
        /// Checks <see cref="Unit.ViewportWidth"/>, changes the window size 
        /// and check the <see cref="UINode"/> size which is based on <see cref="Unit.ViewportWidth"/>
        /// </summary>
        /// <param name="viewportWidth">Width of the viewport in pixels</param>
        [TestCase(240)]   // very small
        [TestCase(320)]   // small mobile portrait
        [TestCase(360)]   // typical Android phone portrait
        [TestCase(375)]   // IPhone portrait
        [TestCase(768)]   // tablet portrait
        [TestCase(1024)]  // small laptop/tablet landscape
        [TestCase(1280)]  // common desktop/laptop
        [TestCase(1366)]  // very common laptop
        [TestCase(1440)]  // QHD width
        [TestCase(1920)]  // Full HD
        [TestCase(2560)]  // QHD
        [TestCase(3840)]  // 4K
        [TestCase(997)]   // odd arbitrary value to test rounding
        [TestCase(8192)]  // very big
        public void ViewportWidthSize(int viewportWidth)
        {
            UINode root = new();
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.NormalStyle.Width = new StyleValue(0.5f, Unit.ViewportWidth);
            root.NormalStyle.Height = new StyleValue(0.5f, Unit.ViewportWidth);

            var windowSize = root.GetWindow().Size;
            root.GetWindow().Size = new Vector2I(viewportWidth, windowSize.Y);

            float viewportX = ViewportHelper.GetViewportSize(root).X;
            Assertions.AssertVector(root.Size).IsEqual(new Vector2(viewportX / 2, viewportX / 2));
        }

        /// <summary>
        /// Checks both <see cref="Unit.ViewportWidth"/> and <see cref="Unit.ViewportHeight"/> on a <see cref="UINode"/>
        /// </summary>
        /// <remarks>
        /// Changes viewport size to <paramref name="viewportHeight"/> and <paramref name="viewportWidth"/>,
        /// and checks the root node which size is set in <see cref="Unit.ViewportWidth"/> and <see cref="Unit.ViewportHeight"/>
        /// </remarks>
        /// <param name="viewportWidth"></param>
        /// <param name="viewportHeight"></param>
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
        public void ViewportWidthAndHeightSize(int viewportWidth, int viewportHeight)
        {
            UINode root = new();
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.NormalStyle.Width = new StyleValue(0.5f, Unit.ViewportWidth);
            root.NormalStyle.Height = new StyleValue(0.5f, Unit.ViewportHeight);

            root.GetWindow().Size = new Vector2I(viewportWidth, viewportHeight);

            Vector2 viewportSize = ViewportHelper.GetViewportSize(root);
            Assertions.AssertVector(root.Size).IsEqual(viewportSize / 2);
        }
        
        /// <summary>
        /// Checks whether the min size values enlarge a <see cref="UINode"/> with smaller size of min values.
        /// </summary>
        /// <param name="widthPx">Width of the node in pixels to test</param>
        /// <param name="heightPx">Height of the node in pixels to test</param>
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
        public void MinSizeEnlargesSize(int widthPx, int heightPx)
        {
            UINode root = new();
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.NormalStyle.Width = new StyleValue(widthPx/2, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(heightPx/2, Unit.Pixel);
            root.NormalStyle.MinWidth = new StyleValue(widthPx, Unit.Pixel);
            root.NormalStyle.MinHeight = new StyleValue(heightPx, Unit.Pixel);

            Assertions.AssertVector(root.Size).IsEqual(new Vector2(widthPx, heightPx));
        }

        /// <summary>
        /// Checks whether the max size values restrict a <see cref="UINode"/> to max values.
        /// </summary>
        /// <param name="widthPx">Width of the node in pixels to test</param>
        /// <param name="heightPx">Height of the node in pixels to test</param>
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
        public void MaxSizeRestrictsSize(int widthPx, int heightPx)
        {
            UINode root = new();
            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.NormalStyle.Width = new StyleValue(widthPx, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(heightPx, Unit.Pixel);
            root.NormalStyle.MaxWidth = new StyleValue(widthPx / 2, Unit.Pixel);
            root.NormalStyle.MaxHeight = new StyleValue(heightPx / 2, Unit.Pixel);

            Assertions.AssertVector(root.Size).IsEqual(new Vector2(widthPx/2, heightPx/2));
        }
    }
}