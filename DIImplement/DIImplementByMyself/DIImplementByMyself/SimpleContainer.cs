using System.Reflection;

namespace DIImplementByMyself
{
    public enum Lifetime
    {
        Transient,
        Singleton,
        Scoped
    }

    public interface IScope : IDisposable
    {
        T Resolve<T>();
    }

    public class SimpleContainer
    {
        private readonly Dictionary<Type, (Type ImplType, Lifetime Lifetime)> _registrations = new();
        private readonly Dictionary<Type, object> _singletons = new();
        private readonly Dictionary<Type, ConstructorInfo> _ctorCache = new();

        /***
         * Registers a service with a specific lifetime.
         * @param TInterface The interface type to register.
         * @param TImplementation The implementation type to register.
         * @param lifetime The lifetime of the service (default is Transient).
         */
        public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TImplementation : TInterface =>
            _registrations[typeof(TInterface)] = (typeof(TImplementation), lifetime);

        /***
         * Registers a service implementation with a specific lifetime.
         * @param TImplementation The implementation type to register.
         * @param lifetime The lifetime of the service (default is Transient).
         */
        public void Register<TImplementation>(Lifetime lifetime = Lifetime.Transient) =>
            _registrations[typeof(TImplementation)] = (typeof(TImplementation), lifetime);

        public T Resolve<T>() => (T)Resolve(typeof(T), null);

        public IScope CreateScope() => new Scope(this);

        /***
         * Resolves a service type, creating an instance based on its lifetime.
         * @param type The type to resolve.
         * @param scope The current scope, if any.
         * @return An instance of the requested type.
         */
        private object Resolve(Type type, Scope scope)
        {
            if (!_registrations.TryGetValue(type, out var entry))
                throw new InvalidOperationException($"Type {type.Name} is not registered.");

            return entry.Lifetime switch
            {
                Lifetime.Singleton when _singletons.TryGetValue(type, out var existing) => existing,
                Lifetime.Singleton => _singletons[type] = CreateInstance(entry.ImplType, scope),
                Lifetime.Scoped => scope?.GetOrCreateScopedInstance(type, () => CreateInstance(entry.ImplType, scope))
                    ?? throw new InvalidOperationException("Scoped service resolution requires an active scope"),
                Lifetime.Transient => CreateInstance(entry.ImplType, scope),
                _ => throw new InvalidOperationException("Unsupported lifetime")
            };
        }

        /***
         * Creates an instance of the implementation type using its constructor.
         * @param implType The implementation type to create.
         * @param scope The current scope, if any.
         * @return An instance of the implementation type.
         */
        private object CreateInstance(Type implType, Scope scope)
        {
            var ctor = _ctorCache.TryGetValue(implType, out var cachedCtor)
                ? cachedCtor
                : _ctorCache[implType] = implType.GetConstructors().FirstOrDefault()
                    ?? throw new InvalidOperationException($"No public constructor found for {implType.Name}");

            var args = ctor.GetParameters()
                           .Select(p => Resolve(p.ParameterType, scope))
                           .ToArray();

            return ctor.Invoke(args);
        }

        private class Scope : IScope
        {
            private readonly SimpleContainer _container;
            private readonly Dictionary<Type, object> _scopedInstances = new();
            private bool _disposed;

            public Scope(SimpleContainer container)
            {
                _container = container;
            }

            public T Resolve<T>() => (T)_container.Resolve(typeof(T), this);

            /***
             * Gets or creates a scoped instance of the specified type.
             * @param type The type to get or create.
             * @param factory A factory function to create the instance if it doesn't exist.
             * @return An instance of the specified type.
             */
            public object GetOrCreateScopedInstance(Type type, Func<object> factory)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(Scope));

                if (!_scopedInstances.TryGetValue(type, out var instance))
                    _scopedInstances[type] = instance = factory();

                return instance;
            }

            public void Dispose()
            {
                _disposed = true;
                _scopedInstances.Clear();
            }
        }
    }
}