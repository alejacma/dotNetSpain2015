using SQLite.Net.Attributes;

namespace MarvelApp.Portable.Model.Entities.MarvelAPI
{
    /// <summary>
    /// La relación entre cómics y sus creadores
    /// </summary>
    public class ComicCreator
    {
        [PrimaryKey]
        public string Id { get; set; }

        public int ComicId { get; set; }

        public int CreatorId { get; set; }
    }
}
