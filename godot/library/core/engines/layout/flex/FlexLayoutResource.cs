using Godot;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Types;
using System;

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
        public FlexJustifyContent JustifyX { get; set; }

        [Export]
        public FlexJustifyContent JustifyY { get; set; }

        [Export]
        public string GapRaw { get; set; }
        
        public StyleValue GapRow { get; set; }
        public StyleValue GapColumn { get; set; }

        public override LayoutResource Parse()
        {
            if (GapRaw == null || GapRaw == "")
            {
                GapRow = new();
                GapColumn = new();
            }

            string[] values = GapRaw.Split(' ');
            switch(values.Length)
            {
                case 2:
                    GapRow = StyleParser.ParseValue(values[0]);
                    GapColumn = StyleParser.ParseValue(values[1]);
                    break;
                case 1:
                    GapRow = StyleParser.ParseValue(values[0]);
                    GapColumn = GapRow;
                    break;
                case 0:
                    GapRow = new();
                    GapColumn = new();
                    break;
                default:
                    throw new ArgumentException("Invalid number of values for a gap");
            };
#if DEBUG

#else
            GapRaw = null;
#endif
            return this;
        }
    }
}
