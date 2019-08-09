using Microsoft.Extensions.DependencyInjection;
using WxTeamsSharp.Api;
using WxTeamsSharp.Client;
using WxTeamsSharp.Interfaces.Api;
using WxTeamsSharp.Interfaces.Client;

namespace WxTeamsSharp.Extensions
{
    /// <summary>
    /// Extension for IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add WxTeamsSharp to MS DI Container
        /// </summary>
        /// <param name="services">IServiceCollection DI Container</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection AddWxTeamsSharp(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddScoped<IWxBaseClient, BaseClient>();
            services.AddScoped<IWxTeamsApi, WxTeamsApi>();
            services.AddScoped<IWxTeamsClient, TeamsClient>();
            services.AddScoped<IJsonDeserializer, CustomJsonDeserializer>();
            services.AddScoped<IPolicyProvider, PollyPolicy>();

            return services;
        }
    }
}
