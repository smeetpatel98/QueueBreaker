using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Services
{
  
    public class ItemsRepository : IItemsInterface
    {
        private readonly QUEUEBREAKERContext _db;

        public ItemsRepository(QUEUEBREAKERContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Item entity)
        {
            await _db.Items.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Item entity)
        {
            _db.Items.Remove(entity);
            return await Save();
        }

        public async Task<IList<Item>> FindAll()
        {
            var result = await _db.Items.Include(x => x.Category)
                .ToListAsync();
            return result;
        }

        public async Task<IList<Item>> FindByCanteenId(int id)
        {
            var result = await _db.Items.Include(x => x.Category).Where(x => x.Category.CanteenId == id)
               .ToListAsync();
            return result;
        }
        public async Task<Item> FindById(int id)
        {
            var result = await _db.Items
                .FirstOrDefaultAsync(q => q.Id == id);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Items.AnyAsync(q => q.Id == id);
        }

        public async Task<Item> FindByName(string name)
        {
            return await _db.Items.FirstOrDefaultAsync(q => q.Name == name);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Item entity)
        {
            _db.Items.Update(entity);
            return await Save();
        }

    }
}
