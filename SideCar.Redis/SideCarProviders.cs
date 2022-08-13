using SideCar.Server;
using SideCar.Server.Strategies;

namespace SideCar.Redis;

public static class ProviderExtensions
{
    public static SideCarProviders AddRedis(this SideCarProviders providers)
    {
        providers.Add<IStrategyExecutor<RetrieveStrategy, string>, RedisStrategyExecutor>();

        return providers;
    }
}
