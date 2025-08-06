using Godot;
using System;

namespace HarmoniaUI.library.core.types
{
    /// <summary>
    /// Tuple of a float value and <see cref="Unit"/> representing the unit of the value
    /// </summary>
    /// <remarks>
    /// ! Value when unit is a Percentage (%) is an actual percent 50% = 0.5
    /// </remarks>
    public struct StyleValue
    {
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

        public readonly float GetPixel(Vector2 viewportSize, Vector2 parentSize, float parentSide, float autoValue)
        {
            return Unit switch
            {
                Unit.Auto => autoValue,
                Unit.Pixel => Value,
                Unit.Percent => MathF.Ceiling(parentSide * Value),
                Unit.ViewportWidth => MathF.Ceiling(viewportSize.X * Value),
                Unit.ViewportHeight => MathF.Ceiling(viewportSize.Y * Value),
                Unit.WidthPercent => MathF.Ceiling(parentSize.X * Value),
                Unit.HeightPercent => MathF.Ceiling(parentSize.Y * Value),
                _ => Value,
            };
        }
    }
}
