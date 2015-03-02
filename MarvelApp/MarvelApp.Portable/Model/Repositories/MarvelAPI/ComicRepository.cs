using MarvelApp.Portable.Model.Clients;
using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelAPI;
using MarvelApp.Portable.Model.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.MarvelAPI
{
    /// <summary>
    /// Un repositorio de comics. Si no encuentra un cómic en el repositorio, lo busca online
    /// </summary>
    public class ComicRepository : BaseSQLiteRepository<Comic>
    {
        public ComicRepository(ISQLiteDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<Comic> FindAsync(int id, bool searchOnlineIfNotFound)
        {
            var comic = (await FindAsync(c => c.Id == id)).FirstOrDefault();
            if (searchOnlineIfNotFound && (comic == null))
            {
                comic = await (new MarvelAPIClient()).GetComicAsync(id);
                if (comic != null)
                {
                    await CreateAsync(comic);
                }
            }
            return comic;
        }
    }
}
