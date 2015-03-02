using MarvelApp.Portable.Configuration;
using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelBackend;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using System.Threading;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.DataContext
{
    /// <summary>
    /// El contexto de datos de los repositorios que usaremos para trabajar con los datos del backend de
    /// Azure Mobile Services tiene que heredar de esta clase. Trabajará en modo offline
    /// </summary>
    public abstract class MarvelBackendDataContext : IMobileServiceDataContext
    {
        protected MarvelBackendConfiguration Configuration { get; set; }

        public abstract MobileServiceClient MobileServiceClient { get; set; }

        public MarvelBackendDataContext()
        {
            Configuration = new MarvelBackendConfiguration();
            MobileServiceClient = new MobileServiceClient(Configuration.ApplicationUrl, Configuration.ApplicationKey);
        }

        public async Task InitializeAsync()
        {
            if (!MobileServiceClient.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore(Configuration.DatabaseName);
                store.DefineTable<FavoriteCharacter>();
                store.DefineTable<FavoriteComic>();
                store.DefineTable<FavoriteCreator>();
                await MobileServiceClient.SyncContext.InitializeAsync(store, new MarvelBackendSyncHandler());
            }
        }

        public abstract Task<bool> LoginAsync(bool canShowUI);

        public abstract Task RegisterPushAsync();

        public async Task Sync()
        {
            await MobileServiceClient.SyncContext.PushAsync(new CancellationToken());
            await Pull<FavoriteCharacter>("FavoriteCharacter");
            await Pull<FavoriteComic>("FavoriteComic");
            await Pull<FavoriteCreator>("FavoriteCreator");
        }

        private Task Pull<T>(string name)
        {
            var table = MobileServiceClient.GetSyncTable<T>();
            return table.PullAsync(name, table.CreateQuery(), true, new CancellationToken());
        }

        public abstract void Logout();
    }
}
