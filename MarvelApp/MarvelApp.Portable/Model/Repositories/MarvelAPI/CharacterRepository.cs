using MarvelApp.Portable.Model.Clients;
using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelAPI;
using MarvelApp.Portable.Model.Repositories.Base;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.MarvelAPI
{
    /// <summary>
    /// Un repositorio de personajes. Si no encuentra un personaje en el repositorio, lo busca online
    /// </summary>
    public class CharacterRepository : BaseSQLiteRepository<Character>
    {
        public CharacterRepository(ISQLiteDataContext dataContext)
            : base(dataContext)
        {
        }

        public async Task<Character> FindAsync(int id, bool searchOnlineIfNotFound)
        {
            var character = (await FindAsync(c => c.Id == id)).FirstOrDefault();
            if (searchOnlineIfNotFound && (character == null))
            {
                character = await (new MarvelAPIClient()).GetCharacterAsync(id);
                if (character != null)
                {
                    await CreateAsync(character);
                }
            }
            return character;
        }
    }
}
