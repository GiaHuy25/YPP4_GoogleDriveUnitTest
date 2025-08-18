using DIImplementByMyself;

namespace DITest
{
    [TestClass]
    public class SimpleContainerTests
    {
        public interface IService { }
        public class Service : IService { }

        public class Repository { }

        public class ServiceWithDependency : IService
        {
            public Repository Repository { get; }
            public ServiceWithDependency(Repository repository)
            {
                Repository = repository;
            }
        }

        [TestMethod]
        public void Resolve_Transient_ReturnsDifferentInstances()
        {
            var container = new SimpleContainer();
            container.Register<IService, Service>(Lifetime.Transient);

            var s1 = container.Resolve<IService>();
            var s2 = container.Resolve<IService>();

            Assert.AreNotSame(s1, s2);
        }

        [TestMethod]
        public void Resolve_Singleton_ReturnsSameInstance()
        {
            var container = new SimpleContainer();
            container.Register<IService, Service>(Lifetime.Singleton);

            var s1 = container.Resolve<IService>();
            var s2 = container.Resolve<IService>();

            Assert.AreSame(s1, s2);
        }

        [TestMethod]
        public void Resolve_WithDependency_InjectsCorrectly()
        {
            var container = new SimpleContainer();
            container.Register<Repository>(Lifetime.Transient);
            container.Register<IService, ServiceWithDependency>(Lifetime.Transient);

            var service = container.Resolve<IService>() as ServiceWithDependency;

            Assert.IsNotNull(service);
            Assert.IsNotNull(service.Repository);
            Assert.IsInstanceOfType(service.Repository, typeof(Repository));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Resolve_UnregisteredType_ThrowsException()
        {
            var container = new SimpleContainer();
            container.Resolve<IService>();
        }
    }
}