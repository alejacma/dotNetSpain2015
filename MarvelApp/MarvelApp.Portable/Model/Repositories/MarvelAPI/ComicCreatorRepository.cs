using MarvelApp.Portable.Model.Clients;
using MarvelApp.Portable.Model.DataContext.Base;
using MarvelApp.Portable.Model.Entities.MarvelAPI;
using MarvelApp.Portable.Model.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.MarvelAPI
{
    /// <summary>
    /// Un repositorio para la relación entre cómics y sus creadores. Te devuelve todo lo que encuentre en el 
    /// repositorio, y se va online a conseguir la información que le falte
    /// </summary>
    public class ComicCreatorRepository : BaseSQLiteRepository<ComicCreator>
    {
        public delegate void FoundOneComicCallback(Comic comic);
        public delegate void FoundOneCreatorCallback(Creator creator);

        private ComicRepository comicRepository;
        private CreatorRepository creatorRepository;

        public ComicCreatorRepository(ISQLiteDataContext dataContext)
            : base(dataContext)
        {
            comicRepository = new ComicRepository(dataContext);
            creatorRepository = new CreatorRepository(dataContext);
        }

        public async Task<ComicCreator> FindAsync(int comicId, int creatorId, bool createIfNotFound)
        {
            var comicCreator = (await FindAsync(cc => cc.ComicId == comicId && cc.CreatorId == creatorId)).FirstOrDefault();
            if (createIfNotFound && (comicCreator == null))
            {
                comicCreator = await CreateAsync(new ComicCreator()
                {
                    Id = string.Format("{0}:{1}", comicId, creatorId),
                    ComicId = comicId,
                    CreatorId = creatorId
                });
            }
            return comicCreator;
        }

        public async Task FindComicsAsync(int creatorId, FoundOneComicCallback foundOneComic)
        {
            await comicRepository.InitializeAsync();

            var comicsFound = await FindComicsInRepositoryAsync(creatorId, foundOneComic);

            await FindComicsOnlineAsync(creatorId, comicsFound, foundOneComic);
        }

        private async Task<List<Comic>> FindComicsInRepositoryAsync(int creatorId, FoundOneComicCallback foundOneComic)
        {
            var comicsFound = new List<Comic>();

            var creatorComics = await FindAsync(cc => cc.CreatorId == creatorId);
            foreach (var creatorComic in creatorComics)
            {
                var comic = await comicRepository.FindAsync(creatorComic.ComicId, false);
                if (comic != null)
                {
                    comicsFound.Add(comic);
                    foundOneComic(comic);
                }
            }

            return comicsFound;
        }

        private async Task FindComicsOnlineAsync(
            int creatorId, List<Comic> comicsAlreadyFound, FoundOneComicCallback foundOneComic)
        {
            MarvelAPIClient client = new MarvelAPIClient();
            var onlineComics = await client.GetCreatorComicsAsync(creatorId);
            foreach (var onlineComic in onlineComics)
            {
                if (comicsAlreadyFound.Any(c => c.Id == onlineComic.Id))
                {
                    continue;
                }

                Comic comic = null;
                try
                {
                    if (onlineComic.Thumbnail == null)
                    {
                        // Hay cómics que en la lista tienen su thumbnail vacío, pero al pedírselos a la API 
                        // individualmente lo tienen relleno
                        comic = await comicRepository.FindAsync(onlineComic.Id, true);
                    }
                    else
                    {
                        comic = await comicRepository.FindAsync(onlineComic.Id, false);
                        if (comic == null)
                        {
                            comic = await comicRepository.CreateAsync(onlineComic);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("ComicCreatorRepository.FindComicsOnlineAsync error: {0}", ex.Message));
                }

                try
                {
                    var comicCreator = await FindAsync(onlineComic.Id, creatorId, true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("ComicCreatorRepository.FindComicsOnlineAsync error: {0}", ex.Message));
                }

                foundOneComic(comic ?? onlineComic);
            }
        }


        public async Task FindCreatorsAsync(int comicId, FoundOneCreatorCallback foundOneCreator)
        {
            await creatorRepository.InitializeAsync();

            var creatorsFound = await FindCreatorsInRepositoryAsync(comicId, foundOneCreator);

            await FindCreatorsOnlineAsync(comicId, creatorsFound, foundOneCreator);
        }

        private async Task<List<Creator>> FindCreatorsInRepositoryAsync(int comicId, FoundOneCreatorCallback foundOneCreator)
        {
            var creatorsFound = new List<Creator>();

            var comicCreators = await FindAsync(cc => cc.ComicId == comicId);
            foreach (var comicCreator in comicCreators)
            {
                var creator = await creatorRepository.FindAsync(comicCreator.CreatorId, false);
                if (creator != null)
                {
                    creatorsFound.Add(creator);
                    foundOneCreator(creator);
                }
            }

            return creatorsFound;
        }

        private async Task FindCreatorsOnlineAsync(
            int comicId, List<Creator> creatorsAlreadyFound, FoundOneCreatorCallback foundOneCreator)
        {
            MarvelAPIClient client = new MarvelAPIClient();
            var onlineCreators = await client.GetComicCreatorsAsync(comicId);
            foreach (var onlineCreator in onlineCreators)
            {
                if (creatorsAlreadyFound.Any(c => c.Id == onlineCreator.Id))
                {
                    continue;
                }

                Creator creator = null;
                try
                {
                    if (onlineCreator.Thumbnail == null)
                    {
                        // Hay creadores que en la lista tienen su thumbnail vacío, pero al pedírselos a la API 
                        // individualmente lo tienen relleno
                        creator = await creatorRepository.FindAsync(onlineCreator.Id, true);
                    }
                    else
                    {
                        creator = await creatorRepository.FindAsync(onlineCreator.Id, false);
                        if (creator == null)
                        {
                            creator = await creatorRepository.CreateAsync(onlineCreator);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("ComicCreatorRepository.FindCreatorsOnlineAsync error: {0}", ex.Message));
                }

                try
                {
                    var comicCreator = await FindAsync(comicId, onlineCreator.Id, true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("ComicCreatorRepository.FindCreatorsOnlineAsync error: {0}", ex.Message));
                }

                foundOneCreator(creator ?? onlineCreator);
            }
        }
    }
}
