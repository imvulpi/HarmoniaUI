using Godot;
using System;

namespace HarmoniaUI.Core.Style.Types
{
    /// <summary>
    /// Represents the value and <see cref="Unit"/> of the value
    /// </summary>
    /// <remarks>
    /// Values are set in accordance to mathematics, % will be written in decimals.
    /// ex. 100% = 1, 50% = 0.5 ...
    /// </remarks>
    public struct StyleValue
    {
        /// <summary>
        /// Creates the style value based on the value and unit
        /// </summary>
        /// <param name="value">Value of the style value</param>
        /// <param name="unit">Unit of the style value</param>
        public StyleValue(float value, Unit unit)
        {
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Value of which unit is defined in <see cref="Unit"/>
        /// </summary>
        public float Value { get; set; }

        /// <summary>
        /// Unit of <see cref="Value"/> 
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// Default Value set to <see cref="Unit.Auto"/> and value of 0
        /// </summary>
        public static readonly StyleValue Default = new()
        {
            Unit = Unit.Auto,
            Value = 0,
        };

        /// <summary>
        /// Displays value and unit in a pretty style.
        /// </summary>
        /// <returns>Pretty value and unit string</returns>
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}
