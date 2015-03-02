using Microsoft.WindowsAzure.MobileServices;

namespace MarvelApp.Portable.Configuration
{
    /// <summary>
    /// La configuración necesaria para el acceso al backend de Azure Mobile Services
    /// </summary>
    public class MarvelBackendConfiguration
    {
        public string ApplicationUrl { get { return "<<Your Azure Mobile Services App Url>>"; } }

        public string ApplicationKey { get { return "<<Your Azure Mobile Services App Key>>"; } }

        public MobileServiceAuthenticationProvider AuthenticationProvider { get { return MobileServiceAuthenticationProvider.Twitter; } }

        public string AuthenticationProviderName { get { return "Twitter"; } }

        public string DatabaseName { get { return "MarvelBackendCache.db"; } }
    }
}
