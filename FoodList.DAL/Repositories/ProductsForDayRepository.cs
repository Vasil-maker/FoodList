using FoodList.DAL.Interfaces;
using FoodList.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodList.DAL.Repositories
{
    public class ProductsForDayRepository : IBaseRepository<ProductsForDayEntity>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public ProductsForDayRepository(ApplicationDbContext application)
        {
            _applicationDbContext = application;
        }


        public async Task Create(ProductsForDayEntity entity)
        {
            await _applicationDbContext.ProductsForDay.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(ProductsForDayEntity entity)
        {
            _applicationDbContext.ProductsForDay.Remove(entity);
            await _applicationDbContext.SaveChangesAsync();
        }

        public IQueryable<ProductsForDayEntity> GetAll()
        {
            return _applicationDbContext.ProductsForDay;
        }

        public async Task<ProductsForDayEntity> Update(ProductsForDayEntity entity)
        {
            _applicationDbContext.ProductsForDay.Update(entity);
            await _applicationDbContext.SaveChangesAsync();

            return entity;
        }
    }
}
