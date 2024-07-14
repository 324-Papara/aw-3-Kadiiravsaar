using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Papara.Base.Entity;
using Papara.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Papara.Data.GenericRepository
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
	{
		private readonly PaparaDbContext dbContext;


		public GenericRepository(PaparaDbContext dbContext)
		{
			this.dbContext = dbContext;
		}


		/// <summary>
		/// Ödev 2 
		/// </summary>
		/// <returns></returns>
	
		public IQueryable<TEntity> Query() => dbContext.Set<TEntity>();

		public async Task<TEntity> GetInclude(long id,
		   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
		{

			IQueryable<TEntity> queryable = Query();

			if (include != null)
				queryable = include(queryable);

			var entity = await queryable.FirstOrDefaultAsync(x => x.Id == id);

			if (entity == null)
			{
				throw new Exception("Entity not found");
			}

			return entity;
		}

		public async Task<List<TEntity>> GetAllInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null)
		{
			IQueryable<TEntity> queryable = Query();

			if (include != null)
				queryable = include(queryable);



			return await queryable.ToListAsync();
		}

		public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> filter)
		{
			return await dbContext.Set<TEntity>().Where(filter).ToListAsync();
		}
		public async Task Save()
		{
			await dbContext.SaveChangesAsync();
		}

		public async Task<TEntity?> GetById(long Id)
		{
			return await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
		}

		public async Task Insert(TEntity entity)
		{
			entity.IsActive = true;
			entity.InsertDate = DateTime.UtcNow;
			entity.InsertUser = "System";
			await dbContext.Set<TEntity>().AddAsync(entity);
		}

		public void Update(TEntity entity)
		{
			dbContext.Set<TEntity>().Update(entity);
		}

		public void Delete(TEntity entity)
		{
			dbContext.Set<TEntity>().Remove(entity);
		}

		public async Task Delete(long Id)
		{
			var entity = await dbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == Id);
			if (entity is not null)
				dbContext.Set<TEntity>().Remove(entity);
		}

		public async Task<List<TEntity>> GetAll()
		{
			return await dbContext.Set<TEntity>().ToListAsync();
		}
	}
}
