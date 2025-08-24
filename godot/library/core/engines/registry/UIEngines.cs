using HarmoniaUI.Core.Engines.Input;
using HarmoniaUI.Core.Engines.Layout;
using HarmoniaUI.Core.Engines.Visual;
using HarmoniaUI.Core.Style.Interfaces;
using HarmoniaUI.library.core.engines.layout.flex;

namespace HarmoniaUI.Core.Engines.Registry
{
    /// <summary>
    /// Contains registries of different engines using <see cref="IEngineRegistry{T1, T2}"/>
    /// <para>
    /// For instant registry add a new entry in the static constructor.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This class is used to store different engine registries,
    /// Engine registries can be used to retrieve or register new engines.
    /// </remarks>
    public static class UIEngines
    {
        static UIEngines()
        {
            // Default engines here \/
            BaseVisualEngine baseVisualEngine = new();
            BaseLayoutEngine baseLayoutEngine = new();
            BaseInputEngine baseInputEngine = new();
            
            // Engine registries here \/
            Layout = new EngineRegistry<ILayoutEngine, LayoutResource>(baseLayoutEngine);
            Visual = new EngineRegistry<IVisualEngine, VisualResource>(baseVisualEngine);
            Input = new EngineRegistry<IInputEngine, InputResource>(baseInputEngine);
            
            // Base engine registrations here \/
            Layout.Register<LayoutResource>(baseLayoutEngine);
            Visual.Register<VisualResource>(baseVisualEngine);
            Input.Register<InputResource>(baseInputEngine);

            // Other registries here \/
            // Put your custom engines here, or other parts of the code if you prefer.
            FlexLayoutEngine flexLayoutEngine = new();
            Layout.Register<FlexLayoutResource>(flexLayoutEngine);
        }

        /// <summary>
        /// Registry for the <see cref="ILayoutEngine"/>
        /// </summary>
        public static IEngineRegistry<ILayoutEngine, LayoutResource> Layout { get; set; }
        
        /// <summary>
        /// Registry for the <see cref="IVisualEngine"/>
        /// </summary>
        public static IEngineRegistry<IVisualEngine, VisualResource> Visual { get; set; }

        /// <summary>
        /// Registry for the <see cref="IInputEngine"/>
        /// </summary>
        public static IEngineRegistry<IInputEngine, InputResource> Input { get; set; }
    }
}
