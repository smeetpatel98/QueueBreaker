using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Services
{
  
    public class CategoryRepository : ICategoryInterface
    {
        private readonly QUEUEBREAKERContext _db;

        public CategoryRepository(QUEUEBREAKERContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Category entity)
        {
            await _db.Categories.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Category entity)
        {
            _db.Categories.Remove(entity);
            return await Save();
        }

        public async Task<IList<Category>> FindAll()
        {
            var result = await _db.Categories
                .ToListAsync();
            return result;
        }

        public async Task<IList<Category>> FindByCanteenId(int id)
        {
            var result = await _db.Categories
                .Where(q => q.Id == id).ToListAsync();
            return result;
        }
        public async Task<Category> FindById(int id)
        {
            var result = await _db.Categories
                .FirstOrDefaultAsync(q => q.Id == id);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Categories.AnyAsync(q => q.Id == id);
        }

        public async Task<Category> FindByName(string name)
        {
            return await _db.Categories.FirstOrDefaultAsync(q => q.Name == name);
        }

        public async Task<bool> Save()
        {
            try
            {
                var changes = await _db.SaveChangesAsync();
                return changes > 0;
            }catch (Exception e)
            {
                var ex = e.Message;
                return false;
            }
        }

        public async Task<bool> Update(Category entity)
        {
            _db.Categories.Update(entity);
            return await Save();
        }

    }
}
