using FoodList.DAL.Interfaces;
using FoodList.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodList.DAL.Repositories
{
    public class FoodCulRepository : IBaseRepository<FoodCulEntity>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public FoodCulRepository(ApplicationDbContext application)
        {
            _applicationDbContext = application;
        }

        public async Task Create(FoodCulEntity entity)
        {
            await _applicationDbContext.FoodCul.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(FoodCulEntity entity)
        {
            _applicationDbContext.FoodCul.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public IQueryable<FoodCulEntity> GetAll()
        {
            return _applicationDbContext.FoodCul;
        }

        public async Task<FoodCulEntity> Update(FoodCulEntity entity)
        {
            _applicationDbContext.FoodCul.Update(entity);
            await _applicationDbContext.SaveChangesAsync();

            return entity;
        }
    }
}
