using Models.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Models.DAO.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal EntitiesDbContext _db;
        internal DbSet<TEntity> _table = null;

        public GenericRepository(EntitiesDbContext db)
        {
            _db = db;
            _table = _db.Set<TEntity>();
        }

        public IEnumerable<TEntity> Select(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _table;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                return orderBy(query).ToList();

            return query.ToList();
        }

        public TEntity SelectById(object id)
        {
            return _table.Find(id);
        }

        public void Insert(TEntity entity)
        {
            _table.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
           Delete(SelectById(id));
        }

        public void Delete(TEntity entity)
        {
            if (_db.Entry(entity).State == EntityState.Detached)
                _table.Attach(entity);
            _table.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> SelectAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _table;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
                return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<TEntity> SelectByIdAsync(object id)
        {
            return await _table.FindAsync(id);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
