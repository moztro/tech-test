using OneCoreTest.Services.Infrastructure.Auditory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using OneCoreTest.Services.Infrastructure.Ordering;

namespace OneCoreTest.Services.Infrastructure.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;
        private DbSet<T> entities;

        public Repository(DbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public async Task<int> Count(Expression<Func<T, bool>> where)
        {
            return await entities.Where(where).CountAsync();
        }

        public async Task<IQueryable<T>> GetAll()
        {
            return await Task.FromResult(entities);
        }

        public async Task<T> Get(long id)
        {
            return await entities.FindAsync(id);
        }

        public async Task<T> Get(string id)
        {
            return await entities.FindAsync(id);
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns></returns>
        public virtual async Task<int> Insert(T entity)
        {
            await this.BeginInsertAsync(entity);
            return await Save();
        }
        /// <summary>
        /// Isnerts a new entity with the specified user.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <param name="user">The user identifier.</param>
        /// <returns></returns>
        public virtual async Task<int> Insert(T entity, string user)
        {
            this.SetCreationAndUpdate(ref entity, user, true);
            return await this.Insert(entity);
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns></returns>
        public virtual async Task<int> Update(T entity)
        {
            await this.BeginUpdateAsync(entity);
            return await this.Save();
        }
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        /// <param name="user">The user Identifier.</param>
        /// <returns></returns>
        public virtual async Task<int> Update(T entity, string user)
        {
            this.SetCreationAndUpdate(ref entity, user, false);
            return await this.Update(entity);
        }

        public virtual async Task<int> Delete(T entity)
        {
            await this.BeginDeleteAsync(entity);
            return await this.Save();
        }
        public virtual async Task<int> Delete(T entity, string user)
        {
            await this.BeginDeleteAsync(entity, user);
            return await this.Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>> @where)
        {
            return await entities.Where(where).FirstOrDefaultAsync();
        }
        public async Task<IQueryable<T>> GetMany(Expression<Func<T, bool>> @where)
        {
            return await Task.FromResult(entities.Where(where));
        }
        public async Task<IQueryable<T>> GetMany(Expression<Func<T, bool>> @where, string order, int take, int skip)
        {
            const string desc = "DESC";

            var result = this.entities.Where(where);

            await Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(order))
                {
                    // Adding some syntax detection.
                    var columns = order.Split(',').Select(e =>
                    {
                        var orderStatement = e.Trim().Split(' ');
                        return new
                        {
                            Column = orderStatement[0].Trim(),
                            IsAsc = orderStatement.Length == 2 ? (orderStatement[1]?.ToUpper()?.Trim() == desc ? false : true) : true
                        };
                    })
                    .Where(e => !string.IsNullOrWhiteSpace(e.Column));

                    // Flag to indicate if is the firs iteration in the loop.
                    var isFirst = true;

                    foreach (var item in columns)
                    {
                        if (isFirst)
                        {
                            if (item.IsAsc)
                                result = result.OrderBy(item.Column);
                            else
                                result = result.OrderByDescending(item.Column);
                            isFirst = false;
                        }
                        else
                        {
                            if (item.IsAsc)
                                result = result.ThenBy(item.Column);
                            else
                                result = result.ThenByDescending(item.Column);
                        }
                    }
                }
            });

            if (take >= 0)
            {
                result = result.Skip((skip - 1) * take).Take(take);
            }

            return result;
        }

        public async Task<IQueryable<T>> GetMany(Expression<Func<T, bool>> @where, string order, bool descending, int take, int skip)
        {
            if (take <= 0)
            {
                if (descending)
                {
                    return await Task.FromResult(entities.OrderByDescending(order).Where(where));
                }
                else
                {
                    return await Task.FromResult(entities.OrderBy(order).Where(where));
                }
            }
            else
            {
                if (descending)
                {
                    return await Task.FromResult(entities.Where(where).OrderByDescending(order).Skip((skip - 1) * take).Take(take));
                }
                else
                {
                    return await Task.FromResult(entities.Where(where).OrderBy(order).Skip((skip - 1) * take).Take(take));
                }
            }
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }
        /// <summary>
        /// Sets the dates and who is performing the insert/update actions.
        /// </summary>
        /// <param name="entity">The entity to Insert/Update.</param>
        /// <param name="user">The user/employee id who is performing the acttion.</param>
        /// <param name="isNew">Whether the entity is inserted (true) or updated (false).</param>
        private void SetCreationAndUpdate(ref T entity, string user, bool isNew)
        {
            if (entity is IUpdatable)
            {
                ((IUpdatable)entity).UpdatedBy = user;
                ((IUpdatable)entity).UpdatedDate = DateTime.Now;
            }

            if (entity is ICreatable && isNew)
            {
                ((ICreatable)entity).CreatedBy = user;
                ((ICreatable)entity).CreatedDate = DateTime.Now;
            }
        }

        /// <summary>
        /// Begins the insert action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="user">The user that performs the operation.</param>
        /// <returns>The processed entity.</returns>
        public Task<T> BeginInsertAsync(T entity, string user)
        {
            this.SetCreationAndUpdate(ref entity, user, true);
            return this.BeginInsertAsync(entity);
        }
        /// <summary>
        /// Begins the Update action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="user">The user that performs the operation.</param>
        /// <returns>The processed entity.</returns>
        public Task<T> BeginUpdateAsync(T entity, string user)
        {
            this.SetCreationAndUpdate(ref entity, user, false);
            return this.BeginUpdateAsync(entity);
        }
        /// <summary>
        /// Begins the Delete action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <param name="user">The user that performs the operation.</param>
        /// <returns>The processed entity.</returns>
        public virtual Task<T> BeginDeleteAsync(T entity, string user)
        {
            this.SetCreationAndUpdate(ref entity, user, false);
            return this.BeginDeleteAsync(entity);
        }
        /// <summary>
        /// Begins the insert action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <returns>The processed entity.</returns>
        public Task<T> BeginInsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            entities.Add(entity);
            return Task.FromResult(entity);
        }
        /// <summary>
        /// Begins the Update action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <returns>The processed entity.</returns>
        public Task<T> BeginUpdateAsync(T entity)
        {
            try
            {
                this.entities.Attach(entity);
                this.context.Entry(entity).State = EntityState.Modified;
            }
            catch
            {
                context.Entry(entity).State = EntityState.Detached;
                throw;
            }
            return Task.FromResult(entity);
        }
        /// <summary>
        /// Begins the Delete action without invoke the save method.
        /// </summary>
        /// <param name="entity">The Entity.</param>
        /// <returns>The processed entity.</returns>
        public Task<T> BeginDeleteAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            entities.Remove(entity);
            return Task.FromResult(entity);
        }
    }
}
