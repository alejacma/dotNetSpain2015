using AlejaCMa.MarvelAPI;

namespace MarvelApp.Portable.Configuration
{
    /// <summary>
    /// La configuración necesaria para el acceso a la API de Marvel.
    /// Incluye las claves necesarias para acceder a la API: https://developer.marvel.com/account
    /// </summary>
    public class MarvelAPIConfiguration : IMarvelAPIConfiguration
    {
        public string PublicKey { get { return "<<Your public Marvel API Key>>"; } }

        public string PrivateKey { get { return "<<Your private Marvel API Key"; } }

        public string DatabaseName { get { return "MarvelAPICache.db"; } }
    }
}
