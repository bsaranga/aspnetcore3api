using System;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Entities;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Services;
using Catalog.Fixtures;
using Catalog.Infrastructure.Repositories;
using Shouldly;
using Xunit;

namespace Catalog.Domain.Tests
{
    public class ItemServiceTests : IClassFixture<CatalogContextFactory>
    {
        private readonly ItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemServiceTests(CatalogContextFactory catalogContextFactory, IMapper mapper)
        {
            _itemRepository = new ItemRepository(catalogContextFactory.ContextInstance);
            _mapper = mapper;
        }

        [Fact]
        public async Task getItemsShouldReturnRightData()
        {
            IItemService sut = new ItemService(_itemRepository, _mapper);

            var result = await sut.GetItemsAsync();
            result.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("b5b05534-9263-448c-a69e-0bbd8b3eb90e")]
        public async Task getItemShouldReturnRightData(string guid)
        {
            IItemService sut = new ItemService(_itemRepository, _mapper);

            var result = await sut.GetItemAsync(new GetItemRequest { Id = new Guid(guid) });
            result.Id.ShouldBe(new Guid(guid));
        }

        [Fact]
        public void getItemShouldThrowExceptionWithNullId()
        {
            IItemService sut = new ItemService(_itemRepository, _mapper);
            sut.GetItemAsync(null).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task addItemShouldAddRightEntity()
        {
            var testItem = new AddItemRequest 
            {
                Name = "Test album",
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
                Price = new Price { Amount = 13, Currency = "EUR" },
                AvailableStock = 10,
                Description = "Greatest album of all time",
                Format = "LP",
                Labelname = "Oceanic",
                PictureUrl = "https://mycnd.spk.com/kjfhsd98739843",
                ReleaseDate = new DateTimeOffset(DateTime.Now)
            };

            IItemService sut = new ItemService(_itemRepository, _mapper);
            var result = await sut.AddItemAsync(testItem);

            result.Name.ShouldBe(testItem.Name);
            result.Description.ShouldBe(testItem.Description);
            result.GenreId.ShouldBe(testItem.GenreId);
            result.ArtistId.ShouldBe(testItem.ArtistId);
            result.Price.Amount.ShouldBe(testItem.Price.Amount);
            result.Price.Currency.ShouldBe(testItem.Price.Currency);
            result.Format.ShouldBe(testItem.Format);
            result.AvailableStock.ShouldBe(testItem.AvailableStock);
        }

        [Fact]
        public async Task editItemShouldEditTheRightEntity()
        {
            var testItem = new EditItemRequest
            {
                Id = new Guid("b5b05534-9263-448c-a69e-0bbd8b3eb90e"),
                Name = "Test album",
                GenreId = new Guid("c04f05c0-f6ad-44d1-a400-3375bfb5dfd6"),
                ArtistId = new Guid("f08a333d-30db-4dd1-b8ba-3b0473c7cdab"),
                Price = new Price { Amount = 13, Currency = "EUR" },
                AvailableStock = 12,
                Description = "SP",
                Format = "SP",
                Labelname = "Vector",
                PictureUrl = "",
                ReleaseDate = new DateTimeOffset(DateTime.Now)
            };

            IItemService sut = new ItemService(_itemRepository, _mapper);
            var result = await sut.EditItemAsync(testItem);

            result.Name.ShouldBe(testItem.Name);
            result.Description.ShouldBe(testItem.Description);
            result.GenreId.ShouldBe(testItem.GenreId);
            result.ArtistId.ShouldBe(testItem.ArtistId);
            result.Price.Amount.ShouldBe(testItem.Price.Amount);
            result.Price.Currency.ShouldBe(testItem.Price.Currency);
            result.Format.ShouldBe(testItem.Format);
            result.AvailableStock.ShouldBe(testItem.AvailableStock);
        }
    }
}
