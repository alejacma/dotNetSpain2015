using Microsoft.WindowsAzure.MobileServices;

namespace MarvelApp.Portable.Model.DataContext.Base
{
    /// <summary>
    /// El contexto de datos de los repositorios que utilicen por debajo Azure Mobile Services tiene que 
    /// implementar este interfaz
    /// </summary>
    public interface IMobileServiceDataContext
    {
        MobileServiceClient MobileServiceClient { get; }
    }
}