using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelBackend;
using MarvelApp.Portable.Model.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.MarvelBackend
{
    /// <summary>
    /// Un repositorio para mis cómics favoritos
    /// </summary>
    public class FavoriteComicRepository : BaseMobileServiceRepository<FavoriteComic>
    {
        public FavoriteComicRepository(IMobileServiceDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<FavoriteComic> FindAsync(int comicId)
        {
            return (await FindAsync(fc => fc.ComicId == comicId)).FirstOrDefault();
        }
    }
}
