using System.Reflection;

namespace GoogleDriveUnittestWithDapper
{
    public enum Lifetime
    {
        Transient,
        Singleton
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

        public T Resolve<T>() => (T)Resolve(typeof(T));

        private object Resolve(Type type)
        {
            return !_registrations.TryGetValue(type, out var entry)
                ? throw new InvalidOperationException($"Type {type.Name} is not registered.")
                : entry.Lifetime switch
                {
                    Lifetime.Singleton when _singletons.TryGetValue(type, out var existing) => existing,
                    Lifetime.Singleton => _singletons[type] = CreateInstance(entry.ImplType, type),
                    Lifetime.Transient => CreateInstance(entry.ImplType, type),
                    _ => throw new InvalidOperationException("Unsupported lifetime")
                };
        }

        private object CreateInstance(Type implType, Type originalType)
        {
            if (_factories.TryGetValue(originalType, out var factory))
                return factory();

            var ctor = _ctorCache.TryGetValue(implType, out var cachedCtor)
                ? cachedCtor
                : _ctorCache[implType] = implType.GetConstructors().FirstOrDefault()
                    ?? throw new InvalidOperationException($"No public constructor found for {implType.Name}");

            var args = ctor.GetParameters()
                           .Select(p => Resolve(p.ParameterType))
                           .ToArray();

            return ctor.Invoke(args);
        }
    }
}
