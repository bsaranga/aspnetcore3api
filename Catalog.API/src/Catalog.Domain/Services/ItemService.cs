using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Catalog.Domain.Entities;
using Catalog.Domain.Repositories;
using Catalog.Domain.Requests.Item;
using Catalog.Domain.Responses.Item;

namespace Catalog.Domain.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<ItemResponse> AddItemAsync(AddItemRequest request)
        {
            var item = _mapper.Map<Item>(request);
            var result = _itemRepository.Add(item);

            await _itemRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<ItemResponse>(result);
        }

        public Task<ItemResponse> DeleteItemAsync(DeleteItemRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ItemResponse> EditItemAsync(EditItemRequest request)
        {
            var existingRecord = await _itemRepository.GetAsync(request.Id);
            if(existingRecord == null) throw new ArgumentException($"Entity with {request.Id} is not present");

            var entity = _mapper.Map<Item>(request);
            var result = _itemRepository.Update(entity);

            await _itemRepository.UnitOfWork.SaveChangesAsync();
            return _mapper.Map<ItemResponse>(result);
        }

        public async Task<ItemResponse> GetItemAsync(GetItemRequest request)
        {
            if (request?.Id == null) throw new ArgumentNullException();
            var entity = await _itemRepository.GetAsync(request.Id);
            return _mapper.Map<ItemResponse>(entity);
        }

        public async Task<IEnumerable<ItemResponse>> GetItemsAsync()
        {
            var entities = await _itemRepository.GetAsync();
            return entities.Select(entity => _mapper.Map<ItemResponse>(entity));
        }
    }
}