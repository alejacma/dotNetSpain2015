using SQLite.Net.Attributes;

namespace MarvelApp.Portable.Model.Entities.MarvelAPI
{
    /// <summary>
    /// La relación entre personajes y sus cómics
    /// </summary>
    public class CharacterComic
    {
        [PrimaryKey]
        public string Id { get; set; }

        public int CharacterId { get; set; }

        public int ComicId { get; set; }
    }
}
