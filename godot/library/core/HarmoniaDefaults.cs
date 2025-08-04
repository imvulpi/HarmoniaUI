using Godot;

namespace HarmoniaUI.library.core
{
    /// <summary>
    /// Holds default values used in the HarmoniaUI library
    /// </summary>
    public static class HarmoniaDefaults
    {
        static HarmoniaDefaults()
        {
        }

        /// <summary>
        /// Gets a default empty stylebox
        /// </summary>
        public static StyleBoxFlat GetEmptyStylebox()
        {
            return new()
            {
                BgColor = Color.Color8(0, 0, 0, 0),
                AntiAliasing = true,
                AntiAliasingSize = 1.0f,
                CornerDetail = 16
            };
        }
    }
}
