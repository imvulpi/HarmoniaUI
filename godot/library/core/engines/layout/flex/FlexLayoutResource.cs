using Godot;
using HarmoniaUI.Core.Style.Interfaces;

namespace HarmoniaUI.library.core.engines.layout.flex
{
    [GlobalClass]
    [Tool]
    public partial class FlexLayoutResource : LayoutResource
    {
        [Export]
        public FlexWrap Wrap { get; set; }

        [Export]
        public FlexDirection Direction { get; set; }

        [Export]
        public FlexJustifyContent JustifyContent { get; set; }

        [Export]
        public FlexAlignItems AlignItems { get; set; }

        [Export]
        public FlexAlignContent AlignContent { get; set; }

        [Export]
        public string Gap { get; set; }
    }
}
