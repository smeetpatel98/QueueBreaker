using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Services
{
  
    public class CanteenRepository : ICanteenInterface
    {
        private readonly QUEUEBREAKERContext _db;

        public CanteenRepository(QUEUEBREAKERContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Canteen entity)
        {
            await _db.Canteens.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Canteen entity)
        {
            _db.Canteens.Remove(entity);
            return await Save();
        }

        public async Task<IList<Canteen>> FindAll()
        {
            var result = await _db.Canteens
                .ToListAsync();
            return result;
        }

        public Task<IList<Canteen>> FindByCanteenId(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<Canteen> FindById(int id)
        {
            var result = await _db.Canteens
                .FirstOrDefaultAsync(q => q.Id == id);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Canteens.AnyAsync(q => q.Id == id);
        }

        public async Task<Canteen> FindByName(string name)
        {
            return await _db.Canteens.FirstOrDefaultAsync(q => q.Name == name);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Canteen entity)
        {
            _db.Canteens.Update(entity);
            return await Save();
        }

    }
}
