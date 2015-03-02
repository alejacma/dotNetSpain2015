using MarvelApp.Portable.Configuration;
using MarvelApp.Portable.Model.DataContext.Base;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;

namespace MarvelApp.Portable.Model.DataContext
{
    /// <summary>
    /// El contexto de datos de los repositorios que usaremos para trabajar con los datos de la API de Marvel
    /// tiene que heredar de esta clase
    /// </summary>
    public abstract class MarvelAPIDataContext : ISQLiteDataContext
    {
        protected string DatabaseName { get { return (new MarvelAPIConfiguration()).DatabaseName; } }

        protected abstract ISQLitePlatform SQLitePlatform { get; }

        protected abstract SQLiteConnectionString ConnectionString { get; }

        public SQLiteAsyncConnection SQLiteAsyncConnection { get; private set; }

        public MarvelAPIDataContext()
        {
            SQLiteAsyncConnection = new SQLiteAsyncConnection(
                () => new SQLiteConnectionWithLock(SQLitePlatform, ConnectionString));
        }
    }
}
