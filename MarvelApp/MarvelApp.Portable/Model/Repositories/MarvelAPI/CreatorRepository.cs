using MarvelApp.Portable.Model.Clients;
using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelAPI;
using MarvelApp.Portable.Model.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.MarvelAPI
{
    /// <summary>
    /// Un repositorio de creadores. Si no encuentra un creador en el repositorio, lo busca online
    /// </summary>
    public class CreatorRepository : BaseSQLiteRepository<Creator>
    {
        public CreatorRepository(ISQLiteDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<Creator> FindAsync(int id, bool searchOnlineIfNotFound)
        {
            var creator = (await FindAsync(c => c.Id == id)).FirstOrDefault();
            if (searchOnlineIfNotFound && (creator == null))
            {
                creator = await (new MarvelAPIClient()).GetCreatorAsync(id);
                if (creator != null)
                {
                    await CreateAsync(creator);
                }
            }
            return creator;
        }
    }
}
