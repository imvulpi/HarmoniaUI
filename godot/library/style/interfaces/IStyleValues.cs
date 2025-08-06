namespace HarmoniaUI.library.style.interfaces
{
    /// <summary>
    /// Represents single value style properties, that are computed into a pixel value
    /// </summary>
    /// <typeparam name="T">Type depending on raw/parsed/computed style</typeparam>
    public interface IStyleValues<T>
    {
        /// <summary>
        /// Width of the element (e.g. "100px", "50%", , leave empty for auto).
        /// </summary>
        public T Width { get; set; }

        /// <summary>
        /// Height of the element (e.g. "100px", "50%", , leave empty for auto).
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
    }
}
