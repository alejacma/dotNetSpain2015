using SQLite.Net.Attributes;

namespace MarvelApp.Portable.Model.Entities.MarvelAPI
{
    /// <summary>
    /// Un cómic
    /// </summary>
    public class Comic
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Format { get; set; }

        public int PageCount { get; set; }

        public string Thumbnail { get; set; }
    }
}
