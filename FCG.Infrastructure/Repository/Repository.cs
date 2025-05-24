﻿using FCG.Domain.Interface;
using FCG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id) =>
        await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task AddAsync(T entity) =>
        await _dbSet.AddAsync(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public void Update(T entity) =>
        _dbSet.Update(entity);

    public void Delete(T entity) =>
        _dbSet.Remove(entity);
}
