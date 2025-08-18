using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIImplementByMyself
{
    public enum Lifetime
    {
        Transient,
        Singleton
    }
    public class SimpleContainer
    {
        private readonly Dictionary<Type, Type> _registration = new();
        private readonly Dictionary<Type, object> _singleton = new();
        private readonly Dictionary<Type, Lifetime> _lifetime = new();

        public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient) where TImplementation : TInterface
        { 
            var interfaceType = typeof(TInterface);
            _registration[interfaceType] = typeof(TImplementation);
            _lifetime[interfaceType] = lifetime;
        }
        public void Register<TImplementation>(Lifetime lifetime = Lifetime.Transient)
        { 
            var type = typeof(TImplementation);
            _registration[type] = type;
            _lifetime[type] = lifetime;
        }
        private object Resolve(Type type)
        {
            var implType = _registration.TryGetValue(type, out var registeredType)
                ? registeredType
                : throw new Exception($"Type {type.Name} is not registered.");

            var lifetime = _lifetime[type];

            var instance = (lifetime, _singleton.TryGetValue(type, out var existing))
                switch
            {
                (Lifetime.Singleton, true) => existing,
                _ => CreateInstance(implType, lifetime, type)
            };

            return instance;
        }

        private object CreateInstance(Type implType, Lifetime lifetime, Type originalType)
        {
            var ctor = implType.GetConstructors().FirstOrDefault()
                ?? throw new Exception($"No public constructor found for {implType.Name}");

            var parameters = ctor.GetParameters();
            var args = parameters.Select(p => Resolve(p.ParameterType)).ToArray();

            var instance = ctor.Invoke(args);

            _ = lifetime == Lifetime.Singleton
                ? _singleton[originalType] = instance
                : instance;

            return instance;
        }

        public T Resolve<T>() => (T)Resolve(typeof(T));
    }
}
