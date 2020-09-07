using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace GoFish.Web.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection Decorate<TService, TImplementation>(this IServiceCollection services) where TImplementation : TService
        {
            List<ServiceDescriptor> decoratedDescriptors = new List<ServiceDescriptor>();
            foreach (var sd in services)
            {
                if (sd.ServiceType.Equals(typeof(TService)))
                {
                    decoratedDescriptors.Add(sd);
                }
            }

            foreach (var sd in decoratedDescriptors)
            {
                services.Remove(sd);
                services.Add(ServiceDescriptor.Describe(sd.ImplementationType, sd.ImplementationType, sd.Lifetime));
                services.Add(ServiceDescriptor.Describe(typeof(TService), sp =>
                {
                    foreach (var ctor in typeof(TImplementation).GetConstructors().OrderByDescending(c => c.GetParameters().Length))
                    {
                        List<object> parameters = new List<object>();
                        foreach (var param in ctor.GetParameters())
                        {
                            object paraminstance = sp.GetService(param.ParameterType.Equals(typeof(TService)) ? sd.ImplementationType : param.ParameterType);
                            if (paraminstance is null) continue;
                            parameters.Add(paraminstance);
                        }
                        return ctor.Invoke(parameters.ToArray());
                    }

                    return null;
                }, sd.Lifetime));
            }

            return services;
        }
    }
}
