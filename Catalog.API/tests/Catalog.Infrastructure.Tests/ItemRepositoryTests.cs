using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Catalog.Infrastructure.Repositories;
using Xunit;
using Shouldly;
using Catalog.Domain.Entities;
using System.Linq;

namespace Catalog.Infrastructure.Tests
{
    public class ItemRepositoryTests
    {
        [Fact]
        public async Task shouldGetData()
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "shouldGetData")
                .Options;
            
            await using var context = new TestCatalogContext(options);
            context.Database.EnsureCreated();

            var sut = new ItemRepository(context);
            var result = await sut.GetAsync();

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task shouldReturnNullWithIdNotPresent()
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "shouldReturnNullWithNoIdPresent")
                .Options;

            await using var context = new TestCatalogContext(options);
            context.Database.EnsureCreated();

            var sut = new ItemRepository(context);
            var result = await sut.GetAsync(Guid.NewGuid());

            result.ShouldBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task shouldReturnRecordById(string guid) 
        {
            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "shouldReturnRecordById")
                .Options;

            await using var context = new TestCatalogContext(options);
            context.Database.EnsureCreated();

            var sut = new ItemRepository(context);
            var result = await sut.GetAsync(new Guid(guid));

            result.Id.ShouldBe(new Guid(guid));
        }

        [Fact]
        public async Task shouldAddNewItem()
        {
            var testItem = new Item
            {
                Name = "Test album",
                Description = "Description",
                Labelname = "Label name",
                Price = new Price { Amount = 13, Currency = "EUR" },
                PictureUrl = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };

            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase(databaseName: "shouldAddNewItem")
                .Options;

            await using var context = new TestCatalogContext(options);
            context.Database.EnsureCreated();

            var sut = new ItemRepository(context);

            sut.Add(testItem);
            await sut.UnitOfWork.SaveEntitiesAsync();

            context.Items
                .FirstOrDefault(_ => _.Id == testItem.Id)
                .ShouldNotBeNull();
        }

        [Fact]
        public async Task shouldUpdateItem()
        {
            var testItem = new Item
            {
                Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
                Name = "Test album",
                Description = "Description updated",
                Labelname = "Label name",
                Price = new Price { Amount = 50, Currency = "EUR" },
                PictureUrl = "https://mycdn.com/pictures/32423423",
                ReleaseDate = DateTimeOffset.Now,
                AvailableStock = 6,
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab")
            };

            var options = new DbContextOptionsBuilder<CatalogContext>()
                .UseInMemoryDatabase("shouldUpdateItem")
                .Options;

            await using var context = new TestCatalogContext(options);
            context.Database.EnsureCreated();

            var sut = new ItemRepository(context);
            sut.Update(testItem);

            await sut.UnitOfWork.SaveEntitiesAsync();

            context.Items
                .FirstOrDefault(x => x.Id == testItem.Id)
                ?.Description.ShouldBe("Description updated");
        }
    }
}
