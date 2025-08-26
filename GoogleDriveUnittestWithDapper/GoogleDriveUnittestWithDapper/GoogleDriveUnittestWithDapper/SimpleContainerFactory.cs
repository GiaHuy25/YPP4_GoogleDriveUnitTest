namespace GoogleDriveUnittestWithDapper
{
    public class SimpleContainerFactory : IServiceProviderFactory<SimpleContainer>
    {
        private readonly SimpleContainer _container;

        public SimpleContainerFactory(SimpleContainer container)
        {
            _container = container;
        }

        public SimpleContainer CreateBuilder(IServiceCollection services)
        {
            var defaultProvider = services.BuildServiceProvider();
            _container.SetFallbackProvider(defaultProvider);

            foreach (var service in services)
            {
                var lifetime = service.Lifetime switch
                {
                    ServiceLifetime.Singleton => Lifetime.Singleton,
                    ServiceLifetime.Scoped => Lifetime.Scoped,
                    ServiceLifetime.Transient => Lifetime.Transient,
                    _ => Lifetime.Transient
                };

                if (service.ImplementationType != null)
                {
                    _container.Register(service.ServiceType, service.ImplementationType, lifetime);
                }
                else if (service.ImplementationFactory != null)
                {
                    _container.RegisterFactory(
                        () => service.ImplementationFactory(defaultProvider)!,
                        lifetime,
                        service.ServiceType
                    );
                }
                else if (service.ImplementationInstance != null)
                {
                    _container.RegisterInstance(service.ServiceType, service.ImplementationInstance, lifetime);
                }
            }

            return _container;
        }

        public IServiceProvider CreateServiceProvider(SimpleContainer containerBuilder) => containerBuilder;
    }
}
