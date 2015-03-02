using MarvelApp.Portable.Model.DataContext.Base;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.Base
{
    /// <summary>
    /// Un repositorio para trabajar en modo offline con los datos de un backend de Azure Mobile Services 
    /// </summary>
    public class BaseMobileServiceRepository<T> : IRepository<T>
        where T : class, new()
    {
        private IMobileServiceSyncTable<T> Table { get; set; }

        public BaseMobileServiceRepository(IMobileServiceDataContext dataContext)
        {
            Table = dataContext.MobileServiceClient.GetSyncTable<T>();
        }

        public async Task<T> CreateAsync(T value)
        {
            await Table.InsertAsync(value);
            return value;
        }

        public async Task<int> CreateAsync(IEnumerable values)
        {
            int count = 0;
            foreach (T value in values)
            {
                await CreateAsync(value);
                count++;
            }
            return count;
        }

        public Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate).ToListAsync();
        }

        public Task<List<T>> GetAllAsync()
        {
            return FindAsync(value => true);
        }

        public async Task<int> UpdateAsync(T value)
        {
            await Table.UpdateAsync(value);
            return 1;
        }

        public async Task<int> DeleteAsync(T value)
        {
            await Table.DeleteAsync(value);
            return 1;
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

