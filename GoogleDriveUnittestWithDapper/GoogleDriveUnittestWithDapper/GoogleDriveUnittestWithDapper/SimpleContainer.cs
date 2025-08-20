using System.Reflection;

namespace GoogleDriveUnittestWithDapper
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
        private readonly Dictionary<Type, Func<object>> _factories = new();
        private readonly Dictionary<Type, object> _singletons = new();
        private readonly Dictionary<Type, ConstructorInfo> _ctorCache = new();

        public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TImplementation : TInterface =>
            _registrations[typeof(TInterface)] = (typeof(TImplementation), lifetime);

        public void Register<TImplementation>(Lifetime lifetime = Lifetime.Transient) =>
            _registrations[typeof(TImplementation)] = (typeof(TImplementation), lifetime);

        public void RegisterFactory<T>(Func<T> factory, Lifetime lifetime = Lifetime.Transient)
        {
            var type = typeof(T);
            _registrations[type] = (type, lifetime);
            _factories[type] = () => factory()!;
        }

        public T Resolve<T>() => (T)Resolve(typeof(T), null);

        public IScope CreateScope() => new Scope(this);
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
