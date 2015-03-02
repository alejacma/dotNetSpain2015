using SQLite.Net.Attributes;

namespace MarvelApp.Portable.Model.Entities.MarvelAPI
{
    /// <summary>
    /// Uno de los personajes de un cómic
    /// </summary>
    public class Character
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }
    }
}
