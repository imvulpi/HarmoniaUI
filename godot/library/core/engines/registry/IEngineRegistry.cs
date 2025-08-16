namespace HarmoniaUI.Core.Engines.Registry
{
    /// <summary>
    /// Engine registry template, it stores the Engine class and its associated Resource type.
    /// The resource type is used to retrieve and register the engine class.
    /// <para>
    /// Each registry has a default engine the registry can fallback on.
    /// </para>
    /// </summary>
    /// <typeparam name="T1">The engine class</typeparam>
    /// <typeparam name="T2">The resource associated with the engine</typeparam>
    /// <param name="defaultEngine">Default fallback engine</param>
    public interface IEngineRegistry<T1, T2>
    {
        /// <summary>
        /// Engine that the registry fallsback on whenether 
        /// the resource type is null or has no registered engine.
        /// </summary>
        public T1 Default { get; set; }

        /// <summary>
        /// Registers an engine with a <typeparamref name="T"/> resource type. 
        /// It associates the resource type <typeparamref name="T"/> with the instance of an <paramref name="engine"/>
        /// </summary>
        /// <typeparam name="T">Resource type to associate <paramref name="engine"/> with</typeparam>
        /// <param name="engine">Instance of an engine to register</param>
        void Register<T>(T1 engine) where T : T2;

        /// <summary>
        /// Retrieves the engine that's associated with the <paramref name="resourceType"/>
        /// </summary>
        /// <param name="resourceType">Resource type that's associated with the engine (was used to register the engine)</param>
        /// <returns>The engine registered with <paramref name="resourceType"/>; otherwise <see cref="Default"/></returns>
        T1 GetEngine(T2 resourceType);
    }
}
