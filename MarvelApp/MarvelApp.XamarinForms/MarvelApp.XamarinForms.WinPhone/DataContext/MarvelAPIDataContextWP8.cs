using MarvelApp.Portable.Model.DataContext;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WindowsPhone8;
using System.IO;
using Windows.Storage;

namespace MarvelApp.DataContext
{
    /// <summary>
    /// El contexto de datos de los repositorios que usaremos para trabajar con los datos de la API de Marvel
    /// </summary>
    public class MarvelAPIDataContextWP8 : MarvelAPIDataContext
    {
        protected override ISQLitePlatform SQLitePlatform
        {
            get
            {
                return new SQLitePlatformWP8();
            }
        }

        protected override SQLiteConnectionString ConnectionString
        {
            get
            {
                return new SQLiteConnectionString(Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseName), false);
            }
        }
    }
}
