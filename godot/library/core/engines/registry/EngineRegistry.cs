using Godot;
using System;
using System.Collections.Generic;

namespace HarmoniaUI.Core.Engines.Registry
{
    /// <inheritdoc cref="IEngineRegistry{T1, T2}"/>
    public class EngineRegistry<T1, T2>(T1 defaultEngine) : IEngineRegistry<T1, T2> 
        where T1 : class 
        where T2 : Resource
    {
        public T1 Default { get; set; } = defaultEngine;

        /// <summary>
        /// Stores the resource type and the associated with it engine.
        /// </summary>
        private readonly Dictionary<Type, T1> _engines = [];
        
        public T1 GetEngine(T2 resourceType)
        {
            if (resourceType == null) return Default;
            if (_engines.TryGetValue(resourceType.GetType(), out T1 engine))
                return engine;
            
            return Default;
        }

        public void Register<T>(T1 engine) where T : T2
        {
            _engines[typeof(T)] = engine;
        }
    }
}
