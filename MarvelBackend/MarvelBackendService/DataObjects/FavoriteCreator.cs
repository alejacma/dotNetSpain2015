using Microsoft.WindowsAzure.Mobile.Service;

namespace MarvelBackendService.DataObjects
{
    public class FavoriteCreator : EntityData
    {
        public string UserId { get; set; }

        public int CreatorId { get; set; }
    }
}