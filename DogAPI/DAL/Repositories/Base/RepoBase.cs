using DogAPI.DAL.EF;

using Microsoft.EntityFrameworkCore;

namespace DogAPI.DAL.Repositories.Base;
public class RepoBase<TEntity, TKey> : IRepo<TEntity, TKey> 
    where TEntity : class
    where TKey : IEquatable<TKey>
{
    private readonly bool _disposeContext;
    private bool _isDisposed;

    public Context Context { get; }
    public DbSet<TEntity> Table { get; }

    public RepoBase(Context context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Table = Context.Set<TEntity>();
        _disposeContext = false;
    }

    public virtual int Add(TEntity entity, bool persist = true)
    {
        Table.Add(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual async Task<int> AddAsync(TEntity entity, bool persist = true)
    {
        await Table.AddAsync(entity);
        return persist ? await SaveChangesAsync() : 0;
    }

    public virtual int AddRange(IEnumerable<TEntity> entities, bool persist = true)
    {
        Table.AddRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> entities, bool persist = true)
    {
        await Table.AddRangeAsync(entities);
        return persist ? await SaveChangesAsync() : 0;
    }

    public virtual int Delete(TEntity entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual async Task<int> DeleteAsync(TEntity entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? await SaveChangesAsync() : 0;
    }

    public virtual TEntity? Find(TKey key)
    {
        return Table.Find(key);
    }

    public virtual async Task<TEntity?> FindAsync(TKey key)
    {
        return await Table.FindAsync(key);
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        return Table;
    }

    public virtual int SaveChanges()
    {
        try
        {
            return Context.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred updating the database", ex);
        }
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        try
        {
            return await Context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred updating the database", ex);
        }
    }

    public virtual int Update(TEntity entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual async Task<int> UpdateAsync(TEntity entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? await SaveChangesAsync() : 0;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing && _disposeContext)
            Context.Dispose();

        _isDisposed = true;
    }

    ~RepoBase()
    {
        Dispose(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}
