using System.Data.Entity;
using System.Threading.Tasks;

namespace MyFriendOrganizer.UI.Data.Repositories
{
    public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
        where TEntity : class // TEntity moet een class zijn
        where TContext : DbContext // TContext moet van type DbContext zijn
    {
        protected readonly TContext Context;

        protected GenericRepository(TContext context) // protected constructor can only be used as base class
        {
            this.Context = context;

        }
        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model); // Get DbSet<TEntity>
        }

        public virtual async Task<TEntity> GetByIdAsync(int id) //virtual can be overridden
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public bool HasChanges()
        {
            return Context.ChangeTracker.HasChanges();
        }

        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model); // Get DbSet<TEntity>
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
