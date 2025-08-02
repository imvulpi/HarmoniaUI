using Godot;

namespace HarmoniaUI.Library.style
{
    /// <summary>
    /// Tuple of a float value and <see cref="Unit"/> representing the unit of the value
    /// </summary>
    /// <remarks>
    /// ! Value when unit is a Percentage (%) is an actual percent 50% = 0.5
    /// </remarks>
    public struct StyleValue(float value, Unit unit)
    {

        /// <summary>
        /// Value of which unit is defined in <see cref="Unit"/>
        /// </summary>
        public float Value { get; set; } = value;

        /// <summary>
        /// Unit of <see cref="Value"/> 
        /// </summary>
        public Unit Unit { get; set; } = unit;

        /// <summary>
        /// Default Value set to <see cref="Unit.Auto"/> and value of 0
        /// </summary>
        public static readonly StyleValue Default = new()
        {
            Unit = Unit.Auto,
            Value = 0,
        };
    }
}