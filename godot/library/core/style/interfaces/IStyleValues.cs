namespace HarmoniaUI.Core.Style.Interfaces
{
    /// <summary>
    /// Represents single value style properties.
    /// Depending on the style stage, the type might be different,
    /// <para>
    /// The strongly typed structure is <see cref="Types.StyleValue"/>
    /// </para>
    /// </summary>
    /// <typeparam name="T">Type depending on raw/parsed/computed style</typeparam>
    public interface IStyleValues<T>
    {
        /// <summary>
        /// Width of the element (ex. "100px", "50%", , leave empty for auto).
        /// </summary>
        public T Width { get; set; }

        /// <summary>
        /// Height of the element (ex. "100px", "50%", , leave empty for auto).
        /// </summary>
        public T Height { get; set; }

        /// <summary>
        /// Minimum width of the element.
        /// </summary>
        public T MinWidth { get; set; }

        /// <summary>
        /// Minimum height of the element.
        /// </summary>
        public T MinHeight { get; set; }

        /// <summary>
        /// Maximum width of the element.
        /// </summary>
        public T MaxWidth { get; set; }

        /// <summary>
        /// Maximum height of the element.
        /// </summary>
        public T MaxHeight { get; set; }

        /// <summary>
        /// Horizontal position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        public T PositionX { get; set; }

        /// <summary>
        /// Vertical position offset (e.g. "10px", "50%") when Relative or Absolute.
        /// </summary>
        public T PositionY { get; set; }

        /// <summary>
        /// An offset of the shadow in local pixels (x, y) from 0,0 of the element.
        /// </summary>
        public T ShadowOffsetX { get; set; }

        /// <summary>
        /// An offset of the shadow in local pixels (x, y) from 0,0 of the element.
        /// </summary>
        public T ShadowOffsetY { get; set; }
    }
}
