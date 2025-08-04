using HarmoniaUI.library.style;
using System;
using System.Collections.Generic;

/// TODO? Replace both this and other engine registry with an abstract?
namespace HarmoniaUI.library.core.layout
{
    /// <summary>
    /// Registry for <see cref="ILayoutEngine"/>s, allows for registry and resolution of engines based on the <see cref="LayoutResource"/> related to the engine
    /// </summary>
    public static class LayoutEngineRegistry
    {
        static LayoutEngineRegistry()
        {
            // Register engines here to allow editor preview
            BaseLayoutEngine baseLayoutEngine = new();
            Register<LayoutResource>(baseLayoutEngine);
            Fallback = baseLayoutEngine;
        }

        /// <summary>
        /// Registered engines inside the registry
        /// </summary>
        private static readonly Dictionary<Type, ILayoutEngine> _engines = [];

        /// <summary>
        /// Fallback or default layout engine that's used to resolve <see cref="LayoutResource"/> or unknown layout resource
        /// </summary>
        public static ILayoutEngine Fallback { get; set; }

        /// <summary>
        /// Adds a layout engine into the registry, thats resolved by a type of <typeparamref name="T"/> layout resource
        /// </summary>
        /// <typeparam name="T">Layout resource type to be used to retrieve the engine</typeparam>
        /// <param name="engine">Engine instance to be registered</param>
        public static void Register<T>(ILayoutEngine engine) where T : LayoutResource
        {
            _engines[typeof(T)] = engine;
        }

        /// <summary>
        /// Gets the engine that's resolved by <paramref name="style"/> type or <see cref="Fallback"/>.
        /// </summary>
        /// <param name="style">Style of which type is used to retrieve the engine</param>
        /// <returns>engine resolved by <paramref name="style"/> type; otherwise <see cref="Fallback"/></returns>
        public static ILayoutEngine GetEngine(LayoutResource style)
        {
            if (style == null) return Fallback;
            return GetEngineOrFallback(style.GetType());
        }

        /// <summary>
        /// Gets the engine or falls back to default.
        /// </summary>
        /// <param name="type">Type to retrieve</param>
        private static ILayoutEngine GetEngineOrFallback(Type type)
        {
            if (_engines.TryGetValue(type, out ILayoutEngine engine))
            {
                return engine;
            }
            else
            {
                return Fallback;
            }
        }
    }
}
