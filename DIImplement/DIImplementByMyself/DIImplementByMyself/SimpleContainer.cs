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
            if (!_registration.TryGetValue(type, out var impltype))
            { 
                throw new Exception($"Type {type.Name} is not registered.");
            }
            var lifetime = _lifetime[type];
            if (lifetime == Lifetime.Singleton && _singleton.TryGetValue(type, out var instance))
            { 
                return instance;
            }
            var ctor = impltype.GetConstructors().FirstOrDefault();
            var parameters = ctor.GetParameters();
            var args = parameters.Select(p => Resolve(p.ParameterType)).ToArray();
            instance =ctor.Invoke(args);
            if (lifetime == Lifetime.Singleton)
            { 
                _singleton[type] = instance;
            }
            return instance;
        }
        public T Resolve<T>()
        { 
            return (T)Resolve(typeof(T));
        }
    }
}
