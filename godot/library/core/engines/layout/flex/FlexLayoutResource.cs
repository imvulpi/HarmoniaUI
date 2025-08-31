using Godot;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.Core.Style.Parsed;
using HarmoniaUI.Core.Style.Types;
using System;

namespace HarmoniaUI.Core.Engines.Layout.Flex
{
    /// <summary>
    /// Resource which enables the flex layout engine and allows customization.
    /// <para>
    /// Allows customization of the positioning and spacing between the children nodes,
    /// enables wrapping and direction in which the flexbox will position children.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Properties set in this resource are passed to the <see cref="FlexLayoutEngine"/>.
    /// </remarks>
    [GlobalClass]
    [Tool]
    public partial class FlexLayoutResource : LayoutResource
    {
        /// <summary>
        /// Describes the wrapping style of children in the flex container
        /// </summary>
        [Export]
        public FlexWrap Wrap { get; set; }

        /// <summary>
        /// Describes the direction in which children will get positioned.
        /// When wrapping the overflowing till occur in the other axis. 
        /// (ex. <see cref="FlexDirection.Row"/> - overflowing on Y, as opposed to X)
        /// </summary>
        [Export]
        public FlexDirection Direction { get; set; }

        /// <summary>
        /// Spacing between lines or nodes depending on the set <see cref="Direction"/> on the X axis.
        /// </summary>
        [Export]
        public FlexJustifyContent JustifyX { get; set; }

        /// <summary>
        /// Spacing between lines or nodes depending on the set <see cref="Direction"/> on the Y axis.
        /// </summary>
        [Export]
        public FlexJustifyContent JustifyY { get; set; }

        /// <summary>
        /// Gap between children and lines, on rows and columns.
        /// </summary>
        /// <remarks>
        /// It's a 2x style value, rows and columns can be set separetely.
        /// <para>
        /// "<c>10px 20px</c>" is Row=10px, Column=20px.
        /// </para>
        /// </remarks>
        [Export]
        public string GapRaw { get; set; }

        /// <summary>
        /// Gap in between rows
        /// </summary>
        /// <remarks>
        /// Depending on <see cref="Direction"/> it might be in between nodes or lines.
        /// </remarks>
        public StyleValue GapRow { get; set; }
        
        /// <summary>
        /// Gap in between columns.
        /// </summary>
        /// <remarks>
        /// Depending on <see cref="Direction"/> it might be in between nodes or lines.
        /// </remarks>
        public StyleValue GapColumn { get; set; }

        /// <summary>
        /// Parses the <see cref="GapRaw"/>.
        /// </summary>
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
