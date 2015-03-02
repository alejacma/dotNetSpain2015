using AutoMapper;
using MarvelApp.Portable.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Clients
{
    /// <summary>
    /// Cliente de la API de Marvel. 
    /// Mapea las clases que devuelve la API a las clases que necesitamos en la app
    /// </summary>
    public class MarvelAPIClient
    {
        AlejaCMa.MarvelAPI.MarvelAPIClient client;

        static MarvelAPIClient()
        {
            Mapper.CreateMap<AlejaCMa.MarvelAPI.Character, Entities.MarvelAPI.Character>()
                .ForMember(
                    ce => ce.Thumbnail,
                    m => m.MapFrom(c => c.Thumbnail == null ? null : string.Format("{0}.{1}", c.Thumbnail.Path, c.Thumbnail.Extension)));

            Mapper.CreateMap<AlejaCMa.MarvelAPI.Comic, Entities.MarvelAPI.Comic>()
                .ForMember(
                    ce => ce.Thumbnail,
                    m => m.MapFrom(c => c.Thumbnail == null ? null : string.Format("{0}.{1}", c.Thumbnail.Path, c.Thumbnail.Extension)));

            Mapper.CreateMap<AlejaCMa.MarvelAPI.Creator, Entities.MarvelAPI.Creator>()
                .ForMember(
                    ce => ce.Thumbnail,
                    m => m.MapFrom(c => c.Thumbnail == null ? null : string.Format("{0}.{1}", c.Thumbnail.Path, c.Thumbnail.Extension)));
        }

        public MarvelAPIClient()
        {
            client = new AlejaCMa.MarvelAPI.MarvelAPIClient(new MarvelAPIConfiguration());
        }

        public async Task<Entities.MarvelAPI.Character> GetCharacterAsync(int id)
        {
            var apiResults = await client.Characters.GetOneAsync(id);
            return Mapper.Map<Entities.MarvelAPI.Character>(apiResults.Data.Results.FirstOrDefault());
        }

        public async Task<Entities.MarvelAPI.Comic> GetComicAsync(int id)
        {
            var apiResults = await client.Comics.GetOneAsync(id);
            return Mapper.Map<Entities.MarvelAPI.Comic>(apiResults.Data.Results.FirstOrDefault());
        }

        public async Task<Entities.MarvelAPI.Creator> GetCreatorAsync(int id)
        {
            var apiResults = await client.Creators.GetOneAsync(id);
            return Mapper.Map<Entities.MarvelAPI.Creator>(apiResults.Data.Results.FirstOrDefault());
        }

        public async Task<List<Entities.MarvelAPI.Character>> GetCharactersAsync(string nameStartsWith, int? limit = null, int? offset = null)
        {
            var apiResults = await client.Characters.GetAllAsync(
                limit, offset, new AlejaCMa.MarvelAPI.APIFilter("nameStartsWith", nameStartsWith));
            return apiResults.Data.Results.Select(c => Mapper.Map<Entities.MarvelAPI.Character>(c)).ToList();
        }

        public async Task<List<Entities.MarvelAPI.Comic>> GetCharacterComicsAsync(int id, int? limit = null, int? offset = null)
        {
            var apiResults = await client.Characters.GetComicsAsync(id, limit, offset);
            return apiResults.Data.Results.Select(c => Mapper.Map<Entities.MarvelAPI.Comic>(c)).ToList();
        }

        public async Task<List<Entities.MarvelAPI.Character>> GetComicCharactersAsync(int id, int? limit = null, int? offset = null)
        {
            var apiResults = await client.Comics.GetCharactersAsync(id, limit, offset);
            return apiResults.Data.Results.Select(c => Mapper.Map<Entities.MarvelAPI.Character>(c)).ToList();
        }

        public async Task<List<Entities.MarvelAPI.Creator>> GetComicCreatorsAsync(int id, int? limit = null, int? offset = null)
        {
            var apiResults = await client.Comics.GetCreatorsAsync(id, limit, offset);
            return apiResults.Data.Results.Select(c => Mapper.Map<Entities.MarvelAPI.Creator>(c)).ToList();
        }

        public async Task<List<Entities.MarvelAPI.Comic>> GetCreatorComicsAsync(int id, int? limit = null, int? offset = null)
        {
            var apiResults = await client.Creators.GetComicsAsync(id, limit, offset);
            return apiResults.Data.Results.Select(c => Mapper.Map<Entities.MarvelAPI.Comic>(c)).ToList();
        }
    }
}
