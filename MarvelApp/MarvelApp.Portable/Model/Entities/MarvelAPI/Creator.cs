using SQLite.Net.Attributes;

namespace MarvelApp.Portable.Model.Entities.MarvelAPI
{
    /// <summary>
    /// Uno de los creadores de un cómic
    /// </summary>
    public class Creator
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Thumbnail { get; set; }
    }
}
