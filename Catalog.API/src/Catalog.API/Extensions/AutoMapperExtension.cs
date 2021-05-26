using AutoMapper;
using Catalog.Domain.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.API.Extensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperExt(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new CatalogProfile());
            });
            
            IMapper mapper = mapperConfig.CreateMapper();
            
            return services.AddSingleton(mapper);
        }
    }
}