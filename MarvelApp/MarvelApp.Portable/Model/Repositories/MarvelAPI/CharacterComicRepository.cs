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
    /// Un repositorio para la relación entre personajes y sus cómics. Te devuelve todo lo que encuentre en el 
    /// repositorio, y se va online a conseguir la información que le falte
    /// </summary>
    public class CharacterComicRepository : BaseSQLiteRepository<CharacterComic>
    {
        public delegate void FoundOneComicCallback(Comic comic);
        public delegate void FoundOneCharacterCallback(Character character);

        private ComicRepository comicRepository;
        private CharacterRepository characterRepository;

        public CharacterComicRepository(ISQLiteDataContext dataContext)
            : base(dataContext)
        {
            comicRepository = new ComicRepository(dataContext);
            characterRepository = new CharacterRepository(dataContext);
        }

        public async Task<CharacterComic> FindAsync(int characterId, int comicId, bool createIfNotFound)
        {
            var characterComic = (await FindAsync(cc => cc.CharacterId == characterId && cc.ComicId == comicId)).FirstOrDefault();
            if (createIfNotFound && (characterComic == null))
            {
                characterComic = await CreateAsync(new CharacterComic()
                {
                    Id = string.Format("{0}:{1}", characterId, comicId),
                    CharacterId = characterId,
                    ComicId = comicId
                });
            }
            return characterComic;
        }

        public async Task FindComicsAsync(int characterId, FoundOneComicCallback foundOneComic)
        {
            await comicRepository.InitializeAsync();

            var comicsFound = await FindComicsInRepositoryAsync(characterId, foundOneComic);

            await FindComicsOnlineAsync(characterId, comicsFound, foundOneComic);
        }

        private async Task<List<Comic>> FindComicsInRepositoryAsync(int characterId, FoundOneComicCallback foundOneComic)
        {
            var comicsFound = new List<Comic>();

            var characterComics = await FindAsync(cc => cc.CharacterId == characterId);
            foreach (var characterComic in characterComics)
            {
                var comic = await comicRepository.FindAsync(characterComic.ComicId, false);
                if (comic != null)
                {
                    comicsFound.Add(comic);
                    foundOneComic(comic);
                }
            }

            return comicsFound;
        }

        private async Task FindComicsOnlineAsync(
            int characterId, List<Comic> comicsAlreadyFound, FoundOneComicCallback foundOneComic)
        {
            MarvelAPIClient client = new MarvelAPIClient();
            var onlineComics = await client.GetCharacterComicsAsync(characterId);
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
                    Debug.WriteLine(string.Format("CharacterComicRepository.FindComicsOnlineAsync error: {0}", ex.Message));
                }

                try
                {
                    var characterComic = await FindAsync(characterId, onlineComic.Id, true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("CharacterComicRepository.FindComicsOnlineAsync error: {0}", ex.Message));
                }

                foundOneComic(comic ?? onlineComic);
            }
        }

        public async Task FindCharactersAsync(int comicId, FoundOneCharacterCallback foundOneCharacter)
        {
            await characterRepository.InitializeAsync();

            var charactersFound = await FindCharactersInRepositoryAsync(comicId, foundOneCharacter);

            await FindCharactersOnlineAsync(comicId, charactersFound, foundOneCharacter);
        }

        private async Task<List<Character>> FindCharactersInRepositoryAsync(int comicId, FoundOneCharacterCallback foundOneCharacter)
        {
            var charactersFound = new List<Character>();

            var comicCharacters = await FindAsync(cc => cc.ComicId == comicId);
            foreach (var comicCharacter in comicCharacters)
            {
                var character = await characterRepository.FindAsync(comicCharacter.CharacterId, false);
                if (character != null)
                {
                    charactersFound.Add(character);
                    foundOneCharacter(character);
                }
            }

            return charactersFound;
        }

        private async Task FindCharactersOnlineAsync(
            int comicId, List<Character> charactersAlreadyFound, FoundOneCharacterCallback foundOneCharacter)
        {
            MarvelAPIClient client = new MarvelAPIClient();
            var onlineCharacters = await client.GetComicCharactersAsync(comicId);
            foreach (var onlineCharacter in onlineCharacters)
            {
                if (charactersAlreadyFound.Any(c => c.Id == onlineCharacter.Id))
                {
                    continue;
                }

                Character character = null;
                try
                {
                    if (onlineCharacter.Thumbnail == null)
                    {
                        // Hay personajes que en la lista tienen su thumbnail vacío, pero al pedírselos a la API 
                        // individualmente lo tienen relleno
                        character = await characterRepository.FindAsync(onlineCharacter.Id, true);
                    }
                    else
                    {
                        character = await characterRepository.FindAsync(onlineCharacter.Id, false);
                        if (character == null)
                        {
                            character = await characterRepository.CreateAsync(onlineCharacter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("CharacterComicRepository.FindCharactersOnlineAsync error: {0}", ex.Message));
                }

                try
                {
                    var characterComic = await FindAsync(onlineCharacter.Id, comicId, true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("CharacterComicRepository.FindCharactersOnlineAsync error: {0}", ex.Message));
                }

                foundOneCharacter(character ?? onlineCharacter);
            }
        }
    }
}
