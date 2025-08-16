using Godot;

namespace HarmoniaUI.Tests.Utils
{
    /// <summary>
    /// Defines useful and repeated constants in tests
    /// </summary>
    public static class TestConstants
    {
        /// <summary>
        /// A common tolerance level that allows a good handling of float values.
        /// </summary>
        public static readonly Vector2 Tolerance = new(0.01f, 0.01f);
    }
}
