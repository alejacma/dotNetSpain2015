using MarvelApp.Portable.Model.DataContext;
using MarvelApp.Portable.Model.Repositories.MarvelBackend;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model
{
    /// <summary>
    /// La factoría de los repositorios que usamos para trabajar con los datos del backend de
    /// Azure Mobile Services
    /// </summary>
    public class MarvelBackend
    {
        private MarvelBackendDataContext DataContext { get; set; }

        public MarvelBackend(MarvelBackendDataContext dataContext)
        {
            DataContext = dataContext;
        }

        public Task InitializeAsync()
        {
            return DataContext.InitializeAsync();
        }

        public Task<bool> LoginAsync(bool canShowUI)
        {
            return DataContext.LoginAsync(canShowUI);
        }

        public Task RegisterPushAsync()
        {
            return DataContext.RegisterPushAsync();
        }

        public Task SyncAllAsync()
        {
            return DataContext.Sync();
        }

        public void Logout()
        {
            DataContext.Logout();
        }

        public FavoriteCharacterRepository FavoriteCharacters { get { return new FavoriteCharacterRepository(DataContext); } }

        public FavoriteComicRepository FavoriteComics { get { return new FavoriteComicRepository(DataContext); } }

        public FavoriteCreatorRepository FavoriteCreators { get { return new FavoriteCreatorRepository(DataContext); } }
    }
}
