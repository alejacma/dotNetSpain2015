using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MarvelApp.Portable.Model.Repositories.Base
{
    /// <summary>
    /// Cualquier repositorio tiene que implementar este interfaz
    /// </summary>
    public interface IRepository<T>
        where T : class, new()
    {
        Task<T> CreateAsync(T value);

        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task<int> UpdateAsync(T value);

        Task<int> DeleteAsync(T value);
    }
}