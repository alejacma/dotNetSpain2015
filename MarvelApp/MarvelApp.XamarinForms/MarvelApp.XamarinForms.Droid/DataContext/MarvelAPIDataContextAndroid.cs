using MarvelApp.Portable.Model.DataContext;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;
using System.IO;
using Xamarin.Forms;

namespace MarvelApp.DataContext
{
    /// <summary>
    /// El contexto de datos de los repositorios que usaremos para trabajar con los datos de la API de Marvel
    /// </summary>
    public class MarvelAPIDataContextAndroid : MarvelAPIDataContext
    {
        protected override ISQLitePlatform SQLitePlatform
        {
            get
            {
                return new SQLitePlatformAndroid();
            }
        }

        protected override SQLiteConnectionString ConnectionString
        {
            get
            {
                return new SQLiteConnectionString(Path.Combine(Forms.Context.ApplicationContext.FilesDir.AbsolutePath, DatabaseName), false);
            }
        }
    }
}
