using MarvelApp.Portable.Model.DataContext.Base;
using SQLite.Net.Async;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.Base
{
    /// <summary>
    /// Un repositorio para trabajar con SQLite de forma asíncrona 
    /// </summary>
    public class BaseSQLiteRepository<T> : IRepository<T>
        where T : class, new()
    {
        protected SQLiteAsyncConnection Connection { get; set; }

        public BaseSQLiteRepository(ISQLiteDataContext dataContext)
        {
            Connection = dataContext.SQLiteAsyncConnection;
        }

        public Task InitializeAsync()
        {
            return Connection.CreateTableAsync<T>();
        }

        public async Task<T> CreateAsync(T value)
        {
            await Connection.InsertAsync(value);
            return value;
        }

        public Task<int> CreateAsync(IEnumerable values)
        {
            return Connection.InsertAllAsync(values);
        }

        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return Connection.Table<T>().Where(predicate).ToListAsync();
        }

        public Task<List<T>> GetAllAsync()
        {
            return FindAsync(value => true);
        }

        public Task<int> UpdateAsync(T value)
        {
            return Connection.UpdateAsync(value);
        }

        public Task<int> DeleteAsync(T value)
        {
            return Connection.DeleteAsync(value);
        }

        public async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            List<T> values = await FindAsync(predicate);
            foreach (T value in values)
            {
                await DeleteAsync(value);
            }
            return values.Count;
        }
    }
}

