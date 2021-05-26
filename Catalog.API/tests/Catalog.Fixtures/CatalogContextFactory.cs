using System;
using AutoMapper;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Fixtures
{
    public class CatalogContextFactory
    {
        public readonly TestCatalogContext ContextInstance;
        public readonly IMapper _mapper;

        public CatalogContextFactory(IMapper mapper)
        {
            _mapper = mapper;
            var contextOptions = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            
            EnsureCreation(contextOptions);
            ContextInstance = new TestCatalogContext(contextOptions);
        }

        private void EnsureCreation(DbContextOptions<CatalogContext> contextOptions)
        {
            using var context = new TestCatalogContext(contextOptions);
            context.Database.EnsureCreated();
        }
    }
}