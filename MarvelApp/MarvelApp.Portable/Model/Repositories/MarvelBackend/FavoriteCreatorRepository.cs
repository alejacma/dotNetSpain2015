using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelBackend;
using MarvelApp.Portable.Model.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.MarvelBackend
{
    /// <summary>
    /// Un repositorio para mis creadores favoritos
    /// </summary>
    public class FavoriteCreatorRepository : BaseMobileServiceRepository<FavoriteCreator>
    {
        public FavoriteCreatorRepository(IMobileServiceDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<FavoriteCreator> FindAsync(int creatorId)
        {
            return (await FindAsync(fc => fc.CreatorId == creatorId)).FirstOrDefault();
        }
    }
}
