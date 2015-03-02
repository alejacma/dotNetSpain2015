using Microsoft.WindowsAzure.Mobile.Service;

namespace MarvelBackendService.DataObjects
{
    public class FavoriteCharacter : EntityData
    {
        public string UserId { get; set; }

        public int CharacterId { get; set; }
    }
}