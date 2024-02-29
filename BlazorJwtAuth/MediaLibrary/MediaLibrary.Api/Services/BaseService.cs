using AutoMapper;
using MediaLibrary.Common.Entities;
using MediaLibrary.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace MediaLibrary.Api.Services;

public abstract class BaseService<TEntity, TModel>(DbContext dbContext, IMapperBase mapper)
    where TEntity : BaseEntity
    where TModel : IModel, new()
{
    public async Task<TModel> CreateAsync(TModel model)
    {
        var entity = mapper.Map<TEntity>(model);
        await dbContext.Set<TEntity>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<TModel>(entity);
    }

    public async Task<TModel> GetByIdAsync(int id)
    {
        if (id <= 0) return new TModel();

        var entity = await GetEntityByIdAsync(id);
        return mapper.Map<TModel>(entity);
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var entities = await dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .ToListAsync();
        return entities.Select(mapper.Map<TModel>);
    }

    public async Task<TModel> UpdateAsync(int id, TModel model)
    {
        var entity = await GetEntityByIdAsync(id);
        mapper.Map(model, entity);
        await dbContext.SaveChangesAsync();
        return mapper.Map<TModel>(entity);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetEntityByIdAsync(id);
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    private async Task<TEntity> GetEntityByIdAsync(int id)
    {
        var entity = await dbContext.FindAsync<TEntity>(id);

        if (entity == null) throw new Exception($"Cannot find entity type {typeof(TEntity)} with id {id}");

        return entity;
    }
}
