using System;
using Catalog.Domain.Entities;

namespace Catalog.Domain.Requests.Item
{
    public class AddItemRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Labelname { get; set; }
        public Price Price { get; set; }
        public string PictureUrl { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Format { get; set; }
        public int AvailableStock { get; set; }
        public Guid GenreId { get; set; }
        public Guid ArtistId { get; set; }
    }
}