using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelBackend;
using MarvelApp.Portable.Model.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.MarvelBackend
{
    /// <summary>
    /// Un repositorio para mis personajes favoritos
    /// </summary>
    public class FavoriteCharacterRepository : BaseMobileServiceRepository<FavoriteCharacter>
    {
        public FavoriteCharacterRepository(IMobileServiceDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<FavoriteCharacter> FindAsync(int characterId)
        {
            return (await FindAsync(fc => fc.CharacterId == characterId)).FirstOrDefault();
        }
    }
}
