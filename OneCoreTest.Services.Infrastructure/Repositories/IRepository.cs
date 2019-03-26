using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.Services.Infrastructure.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAll();
        Task<T> Get(long id);
        Task<T> Get(string id);
        Task<T> Get(Expression<Func<T, bool>> where);
        Task<IQueryable<T>> GetMany(Expression<Func<T, bool>> where);
        Task<int> Save();
        Task<IQueryable<T>> GetMany(Expression<Func<T, bool>> where, string order, bool descending, int pageSize, int page);
        /// <summary>
        /// Gets the collection of items (resul set) from the database.
        /// </summary>
        /// <param name="where">Filtering expression.</param>
        /// <param name="order">The order by clause, for example: "Column1 ASC, Column2 DESC"</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="page">The page number.</param>
        /// <returns></returns>
        Task<IQueryable<T>> GetMany(Expression<Func<T, bool>> where, string order, int pageSize, int page);
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns></returns>
        Task<int> Insert(T entity);
        /// <summary>
        /// Inserts a new entity with the specified user.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="user">The user identifier.</param>
        /// <returns></returns>
        Task<int> Insert(T entity, string user);
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns></returns>
        Task<int> Update(T entity);
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <param name="user">The user Identifier.</param>
        /// <returns></returns>
        Task<int> Update(T entity, string user);
        Task<int> Delete(T entity);
        /// <summary>
        /// Deletes the specified entity using the specified user.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<int> Delete(T entity, string user);
        Task<int> Count(Expression<Func<T, bool>> where);
        /// <summary>
        /// Begins the insert action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="user">The user that performs the operation.</param>
        /// <returns>The processed entity.</returns>
        Task<T> BeginInsertAsync(T entity, string user);
        /// <summary>
        /// Begins the Update action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="user">The user that performs the operation.</param>
        /// <returns>The processed entity.</returns>
        Task<T> BeginUpdateAsync(T entity, string user);
        /// <summary>
        /// Begins the Delete action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="user">The user that performs the operation.</param>
        /// <returns>The processed entity.</returns>
        Task<T> BeginDeleteAsync(T entity, string user);
        /// <summary>
        /// Begins the insert action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <returns>The processed entity.</returns>
        Task<T> BeginInsertAsync(T entity);
        /// <summary>
        /// Begins the Update action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <returns>The processed entity.</returns>
        Task<T> BeginUpdateAsync(T entity);
        /// <summary>
        /// Begins the Delete action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <returns>The processed entity.</returns>
        Task<T> BeginDeleteAsync(T entity);
    }
}
