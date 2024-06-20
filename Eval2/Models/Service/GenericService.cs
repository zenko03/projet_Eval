using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Eval2.Models.Service
{
    public class GenericService<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericService(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public async Task<T> GetById(int id, string includeProperties = null, string idPropertyName = null)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, idPropertyName);
            var convert = Expression.Convert(property, typeof(int)); // Convertir en int
            var equal = Expression.Equal(convert, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }
        public async Task<IEnumerable<T>> GetAllById(int id, string idPropertyName = null)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, idPropertyName); // Assuming 'Id' is your actual ID property name
            var convert = Expression.Convert(property, typeof(int));
            var equal = Expression.Equal(convert, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await _dbSet.Where(lambda).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllById(string id, string idPropertyName = null)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, idPropertyName); // Assuming 'Id' is your actual ID property name
            var convert = Expression.Convert(property, typeof(string));
            var equal = Expression.Equal(convert, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await _dbSet.Where(lambda).ToListAsync();
        }


        public async Task<T> GetById(string id, string includeProperties = null, string idPropertyName = null)
        {
            IQueryable<T> query = _dbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            var parameter = Expression.Parameter(typeof(T)); // otrany oe mideclarer variable miteny oe ity le entity ho ampiasaina
            var property = Expression.Property(parameter, idPropertyName); //acceder a la proriete
            var convert = Expression.Convert(property, typeof(string)); // Convertir en string
            var equal = Expression.Equal(convert, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await query.FirstOrDefaultAsync(lambda);
        }


        public T Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }



        public TKey? GetLastInsertedId<TKey>(Func<T, TKey> keySelector)
        {
            var lastEntity = _dbSet.OrderByDescending(keySelector).FirstOrDefault();
            if (lastEntity != null)
            {
                return keySelector(lastEntity);
            }
            // Retourne une valeur par défaut si aucune entité n'a été trouvée
            return default(TKey);
        }

        public void CreateWithCsv(List<T> entities)
        {
            _dbSet.AddRange(entities);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetPage(int pageNumber, int pageSize, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            // Charger les propriétés de navigation spécifiées
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }


        public int GetTotalPages(int pageSize)
        {
            int totalItems = _dbSet.Count();
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }

        public async Task<IEnumerable<T>> GetPageById(int id, int pageNumber, int pageSize, string includeProperties = "", string idPropertyName = "")
        {
            IQueryable<T> query = _dbSet;

            // Charger les propriétés de navigation spécifiées
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, idPropertyName);
            var convert = Expression.Convert(property, typeof(int)); // Convertir en int
            var equal = Expression.Equal(convert, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await query
                .Where(lambda)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPageById(string id, int pageNumber, int pageSize, string includeProperties = "", string idPropertyName = "Id")
        {
            IQueryable<T> query = _dbSet;

            // Charger les propriétés de navigation spécifiées
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, idPropertyName);
            var convert = Expression.Convert(property, typeof(string)); // Convertir en int
            var equal = Expression.Equal(convert, Expression.Constant(id));
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return await query
                .Where(lambda)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}
