namespace HarmoniaUI.library.style.interfaces
{
    public interface IStyleSideValues<T>
    {
        /// <summary>
        /// Radius for rounding corners (e.g. "8px", "50%").
        /// </summary>
        public T BorderRadius { get; set; }

        /// <summary>
        /// Width of the border (e.g. "2px").
        /// </summary>
        public T BorderWidth { get; set; }

        /// <summary>
        /// Padding inside the element (space between content and border).
        /// </summary>
        public T Padding { get; set; }

        /// <summary>
        /// Margin outside the element (space between element and siblings).
        /// </summary>
        public T Margin { get; set; }
    }
}
