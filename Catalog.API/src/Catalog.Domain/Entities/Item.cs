using System;

namespace Catalog.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Labelname { get; set; }
        public Price Price { get; set; }
        public string PictureUrl { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Format { get; set; }
        public int AvailableStock { get; set; }

        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
        public Guid ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}