using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Services
{
  
    public class CartRepository : ICartInterface
    {
        private readonly QUEUEBREAKERContext _db;

        public CartRepository(QUEUEBREAKERContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Cart entity)
        {
            await _db.Carts.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Cart entity)
        {
            _db.Carts.Remove(entity);
            return await Save();
        }

        public async Task<IList<Cart>> FindAll()
        {
            var result = await _db.Carts.Include(x=>x.Item)
                .ToListAsync();
            return result;
        }

        public async Task<IList<Cart>> FindByCanteenId(int id)
        {
            var result = await _db.Carts.Include(x => x.Item).Where(q => q.CanteenId == id).ToListAsync();
            return result;
        }
        public async Task<Cart> FindById(int id)
        {
            var result = await _db.Carts.Include(x => x.Item)
                .FirstOrDefaultAsync(q => q.Id == id);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Carts.AnyAsync(q => q.Id == id);
        }

        public async Task<Cart> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Cart entity)
        {
            _db.Carts.Update(entity);
            return await Save();
        }
        //public async Task<bool> orderplace()
        //{
        //    return await _db.Carts.AnyAsync();
        //}
    }
}
