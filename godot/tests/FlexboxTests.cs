using GdUnit4;
using HarmoniaUI.Core.Engines.Layout;
using HarmoniaUI.Core.Engines.Layout.Flex;
using HarmoniaUI.Core.Style.Types;
using HarmoniaUI.Nodes;

namespace HarmoniaUI.Tests
{
    /// <summary>
    /// Tests for the <see cref="BaseLayoutEngine"/> positioning.
    /// </summary>
    [TestSuite]
    [RequireGodotRuntime]
    public class FlexboxTests
    {
        [TestCase]
        public void FlexRowTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.NoWrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            int size = 0;
            foreach (var child in children)
            {
                Assertions.AssertVector(child.GlobalPosition).IsEqual(new(size,0));
                size += 50;
            }
        }

        [TestCase]
        public void FlexColumnTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.NoWrap,
                Direction = FlexDirection.Column,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            int size = 0;
            foreach (var child in children)
            {
                Assertions.AssertVector(child.GlobalPosition).IsEqual(new(0, size));
                size += 50;
            }
        }

        [TestCase]
        public void FlexRowWrappingTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(50, 0));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(0, 50));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(50, 50));
        }

        [TestCase]
        public void FlexColumnWrappingTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Column,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(0, 50));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(50, 0));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(50, 50));
        }

        [TestCase]
        public void FlexRowWrappingReverseTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.WrapReverse,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(50, 0));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(0, 50));
            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(50, 50));
        }

        [TestCase]
        public void FlexColumnWrappingReverseTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.WrapReverse,
                Direction = FlexDirection.Column,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(0, 50));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(50, 0));
            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(50, 50));
        }

        [TestCase]
        public void FlexSpacingCenterTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(400, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.NoWrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.Center,
                JustifyY = FlexJustifyContent.Center
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(100, 25));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(150, 25));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(200, 25));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(250, 25));
        }

        [TestCase]
        public void FlexSpacingEndTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(400, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.NoWrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.End,
                JustifyY = FlexJustifyContent.End
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(200, 50));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(250, 50));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(300, 50));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(350, 50));
        }
        
        [TestCase]
        public void FlexSpacingBetweenTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];     

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(125, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(125, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.SpaceBetween,
                JustifyY = FlexJustifyContent.SpaceBetween
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(75, 0));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(0, 75));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(75, 75));
        }

        [TestCase]
        public void FlexSpacingAroundTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(160, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(160, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.SpaceAround,
                JustifyY = FlexJustifyContent.SpaceAround
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(60, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(60, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(10, 10));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(90, 10));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(10, 90));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(90, 90));
        }

        [TestCase]
        public void FlexSpacingEvenlyTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(150, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(150, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.SpaceEvenly,
                JustifyY = FlexJustifyContent.SpaceEvenly
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(60, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(60, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(10, 10));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(80, 10));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(10, 80));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(80, 80));
        }

        [TestCase]
        public void FlexGapTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new()];

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(125, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(125, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start,
                GapRow = new(25, Unit.Pixel),
                GapColumn = new(25, Unit.Pixel)
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(75, 0));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(0, 75));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(75, 75));
        }

        [TestCase]
        public void FlexGapRowOverflowTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new(), new()];

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Row,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start,
                GapRow = new(50, Unit.Pixel),
                GapColumn = new(0, Unit.Pixel)
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(100, 0));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(0, 50));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(100, 50));
            Assertions.AssertVector(children[4].GlobalPosition).IsEqual(new(0, 100));
        }

        [TestCase]
        public void FlexGapColumnOverflowTest()
        {
            BaseLayoutEngine baseLayoutEngine = new BaseLayoutEngine();
            UINode root = new();
            UINode[] children = [new(), new(), new(), new(), new()];

            ISceneRunner sceneRunner = ISceneRunner.Load(root);
            root.GlobalPosition = Godot.Vector2.Zero;
            root.NormalStyle.Width = new StyleValue(100, Unit.Pixel);
            root.NormalStyle.Height = new StyleValue(200, Unit.Pixel);
            root.NormalStyle.LayoutResource = new FlexLayoutResource()
            {
                Wrap = FlexWrap.Wrap,
                Direction = FlexDirection.Column,
                JustifyX = FlexJustifyContent.Start,
                JustifyY = FlexJustifyContent.Start,
                GapRow = new(0, Unit.Pixel),
                GapColumn = new(25, Unit.Pixel)
            };

            foreach (var child in children)
            {
                root.AddChild(child);
                child.NormalStyle.Width = new StyleValue(50, Unit.Pixel);
                child.NormalStyle.Height = new StyleValue(50, Unit.Pixel);
            }

            Assertions.AssertVector(children[0].GlobalPosition).IsEqual(new(0, 0));
            Assertions.AssertVector(children[1].GlobalPosition).IsEqual(new(0, 75));
            Assertions.AssertVector(children[2].GlobalPosition).IsEqual(new(00, 150));
            Assertions.AssertVector(children[3].GlobalPosition).IsEqual(new(50, 0));
            Assertions.AssertVector(children[4].GlobalPosition).IsEqual(new(50, 75));
        }
    }
}
