namespace GoogleDriveUnittestWithDapper
{
    public enum Lifetime { Transient, Singleton }

    public class SimpleContainer
    {
        private readonly Dictionary<Type, Type> _registrations = new();
        private readonly Dictionary<Type, object> _singletons = new();
        private readonly Dictionary<Type, Lifetime> _lifetimes = new();
        private readonly Dictionary<Type, Func<object>> _factories = new();

        public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
            where TImplementation : TInterface =>
            Register(typeof(TInterface), typeof(TImplementation), lifetime);

        public void Register<TImplementation>(Lifetime lifetime = Lifetime.Transient) =>
            Register(typeof(TImplementation), typeof(TImplementation), lifetime);

        public void Register<T>(Lifetime lifetime, Func<T> factory) where T : class
        {
            _registrations[typeof(T)] = typeof(T);
            _lifetimes[typeof(T)] = lifetime;
            _factories[typeof(T)] = () => factory();
        }

        private void Register(Type service, Type implementation, Lifetime lifetime)
        {
            _registrations[service] = implementation;
            _lifetimes[service] = lifetime;
        }

        public T Resolve<T>() => (T)Resolve(typeof(T));

        private object Resolve(Type type) =>
            _registrations.ContainsKey(type) switch
            {
                false => throw new Exception(type.Name + " is not registered."),
                true => (_factories.ContainsKey(type),
                         _lifetimes[type],
                         _singletons.ContainsKey(type)) switch
                {
                    (true, _, _) => _factories[type](),
                    (false, Lifetime.Singleton, true) => _singletons[type],
                    _ => CreateInstance(_registrations[type], _lifetimes[type], type)
                }
            };

        private object CreateInstance(Type implType, Lifetime lifetime, Type originalType)
        {
            var ctors = implType.GetConstructors();
            var ctor = ctors.Length > 0 ? ctors[0] : throw new Exception("No public constructor found for " + implType.Name);

            var parameters = ctor.GetParameters();
            var args = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                args[i] = Resolve(parameters[i].ParameterType);

            var instance = ctor.Invoke(args);

            return (lifetime) switch
            {
                Lifetime.Singleton => _singletons[originalType] = instance,
                _ => instance
            };
        }
    }
}
