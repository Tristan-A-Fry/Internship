using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ecommapp.Data;
using System.Drawing;
using ecommapp.Models;



/*
Purpose of Repository.cs
    The Repository.cs file contains the implementation of the IRepository<T> interface. This class is a generic repository that provides concrete implementations 
    for the data access methods defined in IRepository<T>. The primary purpose of this class is to handle the CRUD operations for various entities in a centralized, 
    reusable, and consistent manner.


public class Repository<T> : IRepository<T> where T : class
    Declares the Repository class, which implements the IRepository<T> interface.
    The class is generic (<T>) and constrained to reference types (where T : class).


public async Task<T> UpsertAsync(T entity)
{
    _context.Entry(entity).State = entity.GetType().GetProperty("Id").GetValue(entity).Equals(0) ? EntityState.Added : EntityState.Modified;
    await _context.SaveChangesAsync();
    return entity;
}
    Inserts a new entity or updates an existing one.
    entity.GetType().GetProperty("Id").GetValue(entity): Gets the value of the Id property of the entity.
    EntityState.Added: If the Id is 0, it's a new entity, so set the state to Added.
    EntityState.Modified: Otherwise, it's an existing entity, so set the state to Modified.
    SaveChangesAsync(): Saves changes to the database asynchronously.
    Returns the upserted entity.


*/

namespace ecommapp.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
       private readonly AppDbContext _context;
       private readonly DbSet<T> _dbSet;

       public Repository(AppDbContext context) 
       {
        _context = context; 
        _dbSet = context.Set<T>(); 
       }

       public async Task<IEnumerable<T>> GetAllAsync(int skip, int take)
        {

            return await _dbSet.Skip(skip).Take(take).ToListAsync();
        }

       public async Task<T> GetByIdAsync(int id)
       {
        return await _dbSet.FindAsync(id);
       }

       public async Task<T> UpsertAsync(T entity)
       {
            _context.Entry(entity).State = entity.GetType().GetProperty("Id").GetValue(entity).Equals(0) ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
       }
       public async Task<T> InsertAsync(T entity)
       {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
       }


       public async Task DeleteAsync(int id)
       {
        var entity = await _dbSet.FindAsync(id);
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
       }
    }
}