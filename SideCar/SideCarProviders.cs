using Microsoft.Extensions.DependencyInjection;

namespace SideCar;

public class SideCarProviders
{
    private readonly IServiceCollection _serviceProvider;

    public SideCarProviders(IServiceCollection serviceProvider) 
        => _serviceProvider = serviceProvider;

    public void Add<T, T1>() where T1 : class, T where T : class 
        => _serviceProvider.AddTransient<T, T1>();
}