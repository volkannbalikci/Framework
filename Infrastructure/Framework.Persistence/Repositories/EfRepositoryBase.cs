﻿using Framework.Domain.Entities;
using Framework.Persistence.Constants;
using Framework.Persistence.Dynamic;
using Framework.Persistence.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Persistence.Repositories;

public class EfRepositoryBase<TEntity, TEntityId, TContext> : IAsyncRepository<TEntity, TEntityId>, IRepository<TEntity, TEntityId>
    where TEntity : EntityBase<TEntityId>
    where TContext : DbContext
{
    protected readonly TContext Context;

    public EfRepositoryBase(TContext context)
    {
        Context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        await Context.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities)
    {
        foreach (TEntity entity in entities)
            entity.CreatedDate = DateTime.UtcNow;
        await Context.AddRangeAsync(entities);
        await Context.SaveChangesAsync();
        return entities;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.AnyAsync(cancellationToken);
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, bool permanent = true)
    {
        await SetEntityAsDeletedAsync(entity, permanent);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = true)
    {
        await SetEntityAsDeletedAsync(entities, permanent);
        await Context.SaveChangesAsync();
        return entities;
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListByPaginateAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListByPaginateDynamicAsync(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }

    public IQueryable<TEntity> Query() => Context.Set<TEntity>();

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        Context.Update(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public async Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities)
    {
        foreach (TEntity entity in entities)
            entity.UpdatedDate = DateTime.UtcNow;
        Context.UpdateRange(entities);
        await Context.SaveChangesAsync();
        return entities;
    }


    protected async Task SetEntityAsDeletedAsync(TEntity entity, bool permanent)
    {
        if (!permanent)
        {
            CheckHasEntityHaveOneToOneRelation(entity);
            await setEntityAsSoftDeletedAsync(entity);
        }
        else
        {
            Context.Remove(entity);
        }
    }

    protected void CheckHasEntityHaveOneToOneRelation(TEntity entity)
    {
        bool hasEntityHaveOneToOneRelation =
            Context
                .Entry(entity)
                .Metadata.GetForeignKeys()
                .All(
                    x =>
                        x.DependentToPrincipal?.IsCollection == true
                        || x.PrincipalToDependent?.IsCollection == true
                        || x.DependentToPrincipal?.ForeignKey.DeclaringEntityType.ClrType == entity.GetType()
                ) == false;
        if (hasEntityHaveOneToOneRelation)
            throw new InvalidOperationException(
                RepositoryMessages.OneToOneRelationAttentionWarning
            );
    }

    private async Task setEntityAsSoftDeletedAsync(IEntityTimestamps entity)
    {
        if (entity.DeletedDate.HasValue)
            return;
        entity.DeletedDate = DateTime.UtcNow;

        var navigations = Context
            .Entry(entity)
            .Metadata.GetNavigations()
            .Where(x => x is { IsOnDependent: false, ForeignKey.DeleteBehavior: DeleteBehavior.ClientCascade or DeleteBehavior.Cascade })
            .ToList();
        foreach (INavigation? navigation in navigations)
        {
            if (navigation.TargetEntityType.IsOwned())
                continue;
            if (navigation.PropertyInfo == null)
                continue;

            object? navValue = navigation.PropertyInfo.GetValue(entity);
            if (navigation.IsCollection)
            {
                if (navValue == null)
                {
                    IQueryable query = Context.Entry(entity).Collection(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType()).ToListAsync();
                    if (navValue == null)
                        continue;
                }

                foreach (IEntityTimestamps navValueItem in (IEnumerable)navValue)
                    await setEntityAsSoftDeletedAsync(navValueItem);
            }
            else
            {
                if (navValue == null)
                {
                    IQueryable query = Context.Entry(entity).Reference(navigation.PropertyInfo.Name).Query();
                    navValue = await GetRelationLoaderQuery(query, navigationPropertyType: navigation.PropertyInfo.GetType())
                        .FirstOrDefaultAsync();
                    if (navValue == null)
                        continue;
                }

                await setEntityAsSoftDeletedAsync((IEntityTimestamps)navValue);
            }
        }

        Context.Update(entity);
    }

    protected IQueryable<object> GetRelationLoaderQuery(IQueryable query, Type navigationPropertyType)
    {
        Type queryProviderType = query.Provider.GetType();
        MethodInfo createQueryMethod =
            queryProviderType
                .GetMethods()
                .First(m => m is { Name: nameof(query.Provider.CreateQuery), IsGenericMethod: true })
                ?.MakeGenericMethod(navigationPropertyType)
            ?? throw new InvalidOperationException(RepositoryMessages.IQueryProviderNotFoundThis);
        var queryProviderQuery =
            (IQueryable<object>)createQueryMethod.Invoke(query.Provider, parameters: new object[] { query.Expression })!;
        return queryProviderQuery.Where(x => !((IEntityTimestamps)x).DeletedDate.HasValue);
    }

    protected async Task SetEntityAsDeletedAsync(IEnumerable<TEntity> entities, bool permanent)
    {
        foreach (TEntity entity in entities)
            await SetEntityAsDeletedAsync(entity, permanent);
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TEntity> query = Query();
        if (enableTracking!)
            query = query.AsNoTracking();
        if (include != null)
            query = include(query);
        if (withDeleted!)
            query = query.IgnoreQueryFilters();
        if (predicate != null)
            query = query.Where(predicate);
        return query.FirstOrDefault();
    }

    public Paginate<TEntity> GetListByPaginate(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TEntity> query = Query();
        if (enableTracking!)
            query = query.AsNoTracking();
        if (include != null)
            query = include(query);
        if (withDeleted!)
            query = query.IgnoreQueryFilters();
        if (predicate != null)
            query = query.Where(predicate);
        if (orderBy != null)
            query = orderBy(query);
        return query.ToPaginate(index, size);
    }

    public Paginate<TEntity> GetListByPaginateDynamic(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TEntity> query = Query().ToDynamic(dynamic);
        if (enableTracking!)
            query = query.AsNoTracking();
        if (include != null)
            query = include(query);
        if (withDeleted!)
            query = query.IgnoreQueryFilters();
        if (predicate != null)
            query = query.Where(predicate);
        return query.ToPaginate(index, size);
    }

    public bool Any(Expression<Func<TEntity, bool>>? predicate = null, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TEntity> query = Query();
        if (enableTracking!)
            query = query.AsNoTracking();
        if (withDeleted!)
            query = query.IgnoreQueryFilters();
        if (predicate != null)
            query = query.Where(predicate);
        return query.Any();

    }

    public TEntity Add(TEntity entity)
    {
        entity.CreatedDate = DateTime.UtcNow;
        Context.Add(entity);
        Context.SaveChanges();
        return entity;
    }

    public ICollection<TEntity> AddRange(ICollection<TEntity> entities)
    {
        foreach (TEntity entity in entities)
            entity.CreatedDate = DateTime.UtcNow;
        Context.AddRange(entities);
        Context.SaveChanges();
        return entities;
    }

    public TEntity Update(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        Context.Update(entity);
        Context.SaveChanges();
        return entity;
    }

    public ICollection<TEntity> UpdateRange(ICollection<TEntity> entities)
    {
        foreach (TEntity entity in entities)
            entity.UpdatedDate = DateTime.UtcNow;
        Context.UpdateRange(entities);
        Context.SaveChanges();
        return entities;
    }

    public TEntity Delete(TEntity entity, bool permanent = true)
    {
        throw new NotImplementedException();
    }

    public ICollection<TEntity> DeleteRange(ICollection<TEntity> entity, bool permanent = false)
    {
        throw new NotImplementedException();
    }

    public IQueryable<TEntity> GetListByQueryable(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orederBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TEntity> query = Query();
        if (enableTracking!)
            query = query.AsNoTracking();
        if (include != null)
            query = include(query);
        if (withDeleted!)
            query = query.AsNoTracking();
        if (predicate != null)
            query = query.Where(predicate);
        return query;
    }

    public IQueryable<TEntity> GetListByQueryableDynamic(DynamicQuery dynamic, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool withDeleted = false, bool enableTracking = true)
    {
        IQueryable<TEntity> query = Query().ToDynamic(dynamic);
        if (enableTracking!)
            query = query.AsNoTracking();
        if (include != null)
            query = include(query);
        if (withDeleted!)
            query = query.IgnoreQueryFilters();
        if (predicate != null)
            query = query.Where(predicate);
        return query;
    }
}
