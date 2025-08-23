using Godot;
using HarmoniaUI.Commons;
using HarmoniaUI.Core.Style.Computed;
using HarmoniaUI.Core.Style.Parsed;

namespace HarmoniaUI.Tests.Utils
{
    /// <summary>
    /// Node for testing the colors in a defined rectangle.
    /// </summary>
    /// <remarks>
    /// Tests whether all pixels in a rectangle are set to a specified <see cref="ExpectedColor"/>.
    /// Sizing can break at pixel values thus, <see cref="TolerancePx"/> allows setting a tolerance.
    /// it shrinks the checking rectangle by the value set (in Pixels).
    /// </remarks>
    [Tool]
    [GlobalClass]
    public partial class ColorTest : HarmoniaTest, ITest
    {
        /// <summary>
        /// The value of which position will be relative to when setting percentage (%) values.
        /// </summary>
        /// <remarks>
        /// The % value by default isn't actually from parent size, it has to be manually set.
        /// </remarks>
        [ExportSubgroup("Sizing")]
        [Export] Vector2 RelativePositionPx { get; set; }
        /// <summary>
        /// The value of which size will be relative to when setting percentage (%) values.
        /// </summary>
        /// <remarks>
        /// The % value by default isn't actually from parent size, it has to be manually set.
        /// </remarks>
        [Export] Vector2 RelativeSizePx { get; set; }
        
        /// <summary>
        /// Raw style value width. See also: <seealso cref="Core.Style.Types.StyleValue"/>
        /// </summary>
        [Export] string Width { get; set; }

        /// <summary>
        /// Raw style value height. See also: <seealso cref="Core.Style.Types.StyleValue"/>
        /// </summary>
        [Export] string Height { get; set; }

        /// <summary>
        /// Raw style value X position. See also: <seealso cref="Core.Style.Types.StyleValue"/>
        /// </summary>
        [Export] string PositionX { get; set; }
        
        /// <summary>
        /// Raw style value Y position. See also: <seealso cref="Core.Style.Types.StyleValue"/>
        /// </summary>
        [Export] string PositionY { get; set; }

        /// <summary>
        /// Color that should be present in the rectangle.
        /// </summary>
        /// <remarks>
        /// When using <see cref="TolerancePx"/> - inside the green rectangle visible in the editor.
        /// </remarks>
        [ExportSubgroup("Testing")]
        [Export] public Color ExpectedColor { get; set; }

        /// <summary>
        /// Pixel amount the rectangle should be shrinked by.
        /// </summary>
        /// <remarks>
        /// Tolerance is used to combat pixel value differences.
        /// </remarks>
        [Export] public float TolerancePx { get => _tolerance; set { _tolerance = value; QueueRedraw(); } }
        
        /// <inheritdoc cref="TolerancePx"/>
        private float _tolerance = 1f;

        /// <summary>
        /// Computes bounds and sets up events.
        /// </summary>
        public override void _Ready()
        {
            if (Engine.IsEditorHint()) return;
            ComputeBounds();
            RenderingServer.FramePostDraw += CheckRectInViewportPixels;
            GetViewport().SizeChanged += ViewportSizeChangedHandler;
        }

        /// <summary>
        /// Computes the size and positions and sets their pixel values in the node.
        /// </summary>
        public void ComputeBounds()
        {
            var widthParsed = StyleParser.ParseValue(Width);
            var heightParsed = StyleParser.ParseValue(Height);
            var positionXParsed = StyleParser.ParseValue(PositionX);
            var positionYParsed = StyleParser.ParseValue(PositionY);
            var viewportSize = ViewportHelper.GetViewportSize(this);

            Position = new(
                StyleComputer.GetPixel(positionXParsed, viewportSize, RelativePositionPx, RelativePositionPx.X, 0),
                StyleComputer.GetPixel(positionYParsed, viewportSize, RelativePositionPx, RelativePositionPx.Y, 0)
            );

            Size = new(
                StyleComputer.GetPixel(widthParsed, viewportSize, RelativeSizePx, RelativeSizePx.X, 0),
                StyleComputer.GetPixel(heightParsed, viewportSize, RelativeSizePx, RelativeSizePx.Y, 0)
            );
        }

        /// <summary>
        /// Checks the viewport image colors in a defined rectangle.
        /// <para>
        /// If all pixels of the rectangle match <see cref="ExpectedColor"/> the tests passes,
        /// otherwise the test will fail.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Colors are approximated to avoid floating issues.
        /// </remarks>
        private void CheckRectInViewportPixels()
        {
            if (Tested) return;

            Passed = true;
            Image image = GetViewport().GetTexture().GetImage();
            image = CorrectImage(image);
            Vector2 imageSize = image.GetSize();

            int startX = (int)(Position.X + TolerancePx);
            int startY = (int)(Position.Y + TolerancePx);
            int endX = (int)(Position.X + Size.X - (2 * TolerancePx));
            int endY = (int)(Position.Y + Size.Y - (2 * TolerancePx));

            for (int i = startX; i < endX; i++)
            {
                int x = i;
                for (int o = startY; o < endY; o++)
                {
                    int y = o;
                    if (x < imageSize.X && y < imageSize.Y
                        && !image.GetPixel(x, y).IsEqualApprox(ExpectedColor))
                    {
                        FailTest();
                        return;
                    }
                }
            }

            PassTest();
        }

        /// <summary>
        /// Converts an image in such way that it represents the actual viewports colors.
        /// </summary>
        /// <param name="image">Image to be converted</param>
        /// <returns>Image with correct colors</returns>
        private static Image CorrectImage(Image image)
        {
            if (image.DetectAlpha() != Image.AlphaMode.None)
            {
                image.Convert(Image.Format.Rgba8);
            }
            else
            {
                image.Convert(Image.Format.Rgb8);
            }
            return image;
        }

        /// <summary>
        /// Computes bounds again and restarts the test.
        /// </summary>
        private void ViewportSizeChangedHandler()
        {
            ComputeBounds();
            Restart();
        }

        /// <summary>
        /// Draws the red and green rectangle.
        /// </summary>
        /// <remarks>
        /// Red rectangle is the overall size,
        /// Green rectangle is the rectangle area that will be tested
        /// <para>
        /// Doesn't draw in the editor to avoid issues with tests failing due
        /// to the rectangle borders colors being drawn.
        /// </para>
        /// </remarks>
        public override void _Draw()
        {
            if (!Engine.IsEditorHint()) return;
            DrawRect(new Rect2(0, 0, Size), Colors.Red, false, 1);
            DrawRect(new Rect2(TolerancePx, TolerancePx, Size.X - (2 * TolerancePx), Size.Y - (2 * TolerancePx)), Colors.Green, false, 1);
        }

#if DEBUG

        public override void _Process(double delta)
        {
            if (!Engine.IsEditorHint()) return;
            ComputeBounds();
        }
#endif

    }
}