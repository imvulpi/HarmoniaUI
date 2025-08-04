using HarmoniaUI.library.style;
using System;
using System.Collections.Generic;

/// TODO? Replace both this and other engine registry with an abstract?
namespace HarmoniaUI.library.core.visual
{
    /// <summary>
    /// Registry for <see cref="IVisualEngine"/>s, allows for registry and resolution of engines based on the <see cref="VisualResource"/> related to the engine
    /// </summary>
    public static class VisualEngineRegistry
    {
        static VisualEngineRegistry()
        {
            // Register engines here to allow editor preview
            BaseVisualEngine baseVisualEngine = new BaseVisualEngine();
            Register<VisualResource>(baseVisualEngine);
            Fallback = baseVisualEngine;
        }

        /// <summary>
        /// Registered engines inside the registry
        /// </summary>
        private static readonly Dictionary<Type, IVisualEngine> _engines = new();

        /// <summary>
        /// Fallback or default visual engine that's used to resolve <see cref="VisualResource"/> or unknown visual resource
        /// </summary>
        public static IVisualEngine Fallback { get; set; }

        /// <summary>
        /// Adds a visual engine into the registry, thats resolved by a type of <typeparamref name="T"/> visual resource
        /// </summary>
        /// <typeparam name="T">Visual resource type to be used to retrieve the engine</typeparam>
        /// <param name="engine">Engine instance to be registered</param>

        public static void Register<T>(IVisualEngine engine) where T : VisualResource
        {
            _engines[typeof(T)] = engine;
        }

        /// <summary>
        /// Gets the engine that's resolved by <paramref name="style"/> type or <see cref="Fallback"/>.
        /// </summary>
        /// <param name="style">Style of which type is used to retrieve the engine</param>
        /// <returns>engine resolved by <paramref name="style"/> type; otherwise <see cref="Fallback"/></returns>

        public static IVisualEngine GetEngine(VisualResource style)
        {
            if (style == null) return Fallback;
            return GetEngineOrFallback(style.GetType());
        }

        /// <summary>
        /// Gets the engine or falls back to default.
        /// </summary>
        /// <param name="type">Type to retrieve</param>
        private static IVisualEngine GetEngineOrFallback(Type type)
        {
            if (_engines.TryGetValue(type, out IVisualEngine engine))
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
