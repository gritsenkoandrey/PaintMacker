using Managers;
using VContainer;
using VContainer.Unity;

namespace APP
{
    public sealed class GameLifeTimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MCamera>();
            builder.RegisterComponentInHierarchy<MConfig>();
            builder.RegisterComponentInHierarchy<MGame>();
            builder.RegisterComponentInHierarchy<MGUI>();
            builder.RegisterComponentInHierarchy<MInput>();
            builder.RegisterComponentInHierarchy<MLight>();
            builder.RegisterComponentInHierarchy<MWorld>();
            builder.RegisterComponentInHierarchy<MPool>();

            builder.RegisterEntryPoint<Manager>();
        }
    }
}