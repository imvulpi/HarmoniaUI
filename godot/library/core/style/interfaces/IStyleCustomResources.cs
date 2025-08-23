namespace HarmoniaUI.Core.Style.Interfaces
{
    /// <summary>
    /// Defines the contract for custom resources of a style in a harmonia node.
    /// </summary>
    public interface IStyleCustomResources
    {
        /// <summary>
        /// A custom layout resource that will be used to retrieve a layout engine.
        /// It's passed 'as is' to the <see cref="Engines.Layout.ILayoutEngine"/> 
        /// associated with the layout resource type
        /// </summary>
        /// <remarks>
        /// You can create a custom <see cref="LayoutResource"/> and a engine supporting it 
        /// by registering it in <see cref="Engines.Registry.UIEngines"/>.
        /// </remarks>
        public LayoutResource LayoutResource { get; set; }

        /// <summary>
        /// A custom visual resource that will be used to retrieve a visual engine.
        /// It's passed 'as is' to the <see cref="Engines.Visual.IVisualEngine"/>
        /// associated with the visual resource type
        /// </summary>
        /// <remarks>
        /// You can create a custom <see cref="VisualResource"/> and a engine supporting it 
        /// by registering it in <see cref="Engines.Registry.UIEngines"/>.
        /// </remarks>
        public VisualResource VisualResource { get; set; }

        /// <summary>
        /// A custom input resource that will be used to retrieve a input engine.
        /// It's passed 'as is' to the <see cref="Engines.Input.IInputEngine"/>
        /// associated with the input resource type
        /// </summary>
        /// <remarks>
        /// You can create a custom <see cref="InputResource"/> and a engine supporting it 
        /// by registering it in <see cref="Engines.Registry.UIEngines"/>.
        /// </remarks>
        public InputResource InputResource { get; set; }
    }
}
