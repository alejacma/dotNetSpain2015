using SQLite.Net.Async;

namespace MarvelApp.Portable.Model.DataContext.Base
{
    /// <summary>
    /// El contexto de datos de los repositorios que por debajo utilicen SQLite de forma asíncrona 
    /// tiene que implementar este interfaz
    /// </summary>
    public interface ISQLiteDataContext
    {
        SQLiteAsyncConnection SQLiteAsyncConnection { get; }
    }
}
