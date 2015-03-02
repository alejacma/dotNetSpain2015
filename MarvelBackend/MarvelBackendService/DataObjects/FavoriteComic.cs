using Microsoft.WindowsAzure.Mobile.Service;

namespace MarvelBackendService.DataObjects
{
    public class FavoriteComic : EntityData
    {
        public string UserId { get; set; }

        public int ComicId { get; set; }
    }
}