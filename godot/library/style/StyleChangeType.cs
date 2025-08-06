using System;

namespace HarmoniaUI.library.style
{
    [Flags]
    public enum StyleChangeType
    {
        None = 0,
        Redraw = 1,
        Relayout = 2,
        Other = 4,
    }
}