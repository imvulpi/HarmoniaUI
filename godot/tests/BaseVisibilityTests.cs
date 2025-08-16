using GdUnit4;
using Godot;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="UINode"/> <see cref="VisibilityType"/>
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class BaseVisibilityTests
    {
        /// <summary>
        /// Sets the window mode to <see cref="DisplayServer.WindowMode.Windowed"/> in order for visuals to get drawn.
        /// </summary>
        [BeforeTest]
        public void BeforeTest()
        {
            DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed);
        }

        /// <summary>
        /// Tests the <see cref="VisibilityType.Visible"/> setting on <see cref="UINode"/>
        /// </summary>
        [TestCase]
        public void VisibileVisibility()
        {
            UINode root = new();
            UINode child1 = new();
            UINode child2 = new();
            root.AddChild(child1);
            root.AddChild(child2);
            ISceneRunner.Load(root);
            root.ParsedStyle.Width = new StyleValue(1, Unit.Percent);
            root.ParsedStyle.Height = new StyleValue(1, Unit.Percent);
            
            child1.ParsedStyle.Width = new StyleValue(500f, Unit.Pixel);
            child1.ParsedStyle.Height = new StyleValue(500f, Unit.Pixel);
            child1.ParsedStyle.Visibility = VisibilityType.Visible;
            
            child2.ParsedStyle.Width = new StyleValue(500f, Unit.Pixel);
            child2.ParsedStyle.Height = new StyleValue(500f, Unit.Pixel);
            child2.ParsedStyle.Visibility = VisibilityType.Visible;


            Assertions.AssertInt((int)child2.ParsedStyle.Visibility).IsEqual((int)VisibilityType.Visible);
            Assertions.AssertInt((int)child1.ParsedStyle.Visibility).IsEqual((int)VisibilityType.Visible);
            Assertions.AssertBool(child2.Visible).IsEqual(true);
            Assertions.AssertBool(child1.Visible).IsEqual(true);
            Assertions.AssertVector(child2.Position).IsEqual(new Vector2(0, 500));
        }

        /// <summary>
        /// Tests the <see cref="VisibilityType.Transparent"/> setting on <see cref="UINode"/>
        /// </summary>
        /// <remarks>
        /// Node with <see cref="VisibilityType.Transparent"/> shouldn't be visible but the layout should be applied.
        /// Which means the node will effectively offset other nodes.
        /// </remarks>
        [TestCase]
        public void TransparentVisibility()
        {
            UINode root = new();
            UINode child1 = new();
            UINode child2 = new();
            root.AddChild(child1);
            root.AddChild(child2);
            ISceneRunner.Load(root);
            root.ParsedStyle.Width = new StyleValue(1, Unit.Percent);
            root.ParsedStyle.Height = new StyleValue(1, Unit.Percent);
            
            child1.ParsedStyle.Width = new StyleValue(500f, Unit.Pixel);
            child1.ParsedStyle.Height = new StyleValue(500f, Unit.Pixel);
            child1.ParsedStyle.Visibility = VisibilityType.Transparent;
            
            child2.ParsedStyle.Width = new StyleValue(500f, Unit.Pixel);
            child2.ParsedStyle.Height = new StyleValue(500f, Unit.Pixel);
            child2.ParsedStyle.Visibility = VisibilityType.Transparent;


            Assertions.AssertInt((int)child2.ParsedStyle.Visibility).IsEqual((int)VisibilityType.Transparent);
            Assertions.AssertInt((int)child1.ParsedStyle.Visibility).IsEqual((int)VisibilityType.Transparent);
            Assertions.AssertBool(child2.Visible).IsEqual(false);
            Assertions.AssertBool(child1.Visible).IsEqual(false);
            Assertions.AssertVector(child2.Position).IsEqual(new Vector2(0, 500));
        }

        /// <summary>
        /// Tests the <see cref="VisibilityType.Hidden"/> setting on <see cref="UINode"/>
        /// </summary>
        [TestCase]
        public void HiddenVisibility()
        {
            UINode root = new();
            UINode child1 = new();
            UINode child2 = new();
            UINode child3 = new();
            root.AddChild(child1);
            root.AddChild(child2);
            root.AddChild(child3);

            ISceneRunner.Load(root);
            root.ParsedStyle.Width = new StyleValue(1, Unit.Percent);
            root.ParsedStyle.Height = new StyleValue(1, Unit.Percent);
            
            child1.ParsedStyle.Width = new StyleValue(500f, Unit.Pixel);
            child1.ParsedStyle.Height = new StyleValue(500f, Unit.Pixel);
            child1.ParsedStyle.Visibility = VisibilityType.Visible;
            
            child2.ParsedStyle.Width = new StyleValue(500f, Unit.Pixel);
            child2.ParsedStyle.Height = new StyleValue(500f, Unit.Pixel);
            child2.ParsedStyle.Visibility = VisibilityType.Hidden;

            child3.ParsedStyle.Width = new StyleValue(500f, Unit.Pixel);
            child3.ParsedStyle.Height = new StyleValue(500f, Unit.Pixel);
            child3.ParsedStyle.Visibility = VisibilityType.Visible;

            Assertions.AssertVector(child3.Position).IsEqual(new Vector2(0, 500));
            Assertions.AssertInt((int)child1.ParsedStyle.Visibility).IsEqual((int)VisibilityType.Visible);
            Assertions.AssertInt((int)child2.ParsedStyle.Visibility).IsEqual((int)VisibilityType.Hidden);
            Assertions.AssertInt((int)child3.ParsedStyle.Visibility).IsEqual((int)VisibilityType.Visible);
            Assertions.AssertBool(child1.Visible).IsEqual(true);
            Assertions.AssertBool(child2.Visible).IsEqual(false);
            Assertions.AssertBool(child3.Visible).IsEqual(true);
        }
    }
}
