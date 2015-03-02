using MarvelApp.Portable.Model.DataContext;
using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Repositories.MarvelAPI;

namespace MarvelApp.Portable.Model
{
    /// <summary>
    /// La factoría de los repositorios que usamos para trabajar con los datos de la API de Marvel
    /// </summary>
    public class MarvelAPI
    {
        private ISQLiteDataContext DataContext { get; set; }

        public MarvelAPI(MarvelAPIDataContext dataContext)
        {
            DataContext = dataContext;
        }

        public CharacterRepository Characters
        {
            get
            {
                var repository = new CharacterRepository(DataContext);
                repository.InitializeAsync().Wait();
                return repository;
            }
        }

        public ComicRepository Comics
        {
            get
            {
                var repository = new ComicRepository(DataContext);
                repository.InitializeAsync().Wait();
                return repository;
            }
        }

        public CreatorRepository Creators
        {
            get
            {
                var repository = new CreatorRepository(DataContext);
                repository.InitializeAsync().Wait();
                return repository;
            }
        }

        public CharacterComicRepository CharactersComics
        {
            get
            {
                var repository = new CharacterComicRepository(DataContext);
                repository.InitializeAsync().Wait();
                return repository;
            }
        }

        public ComicCreatorRepository ComicsCreators
        {
            get
            {
                var repository = new ComicCreatorRepository(DataContext);
                repository.InitializeAsync().Wait();
                return repository;
            }
        }
    }
}
