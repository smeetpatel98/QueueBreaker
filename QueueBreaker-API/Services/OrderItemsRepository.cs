using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Services
{
  
    public class OrderItemsRepository : IOrderItemsInterface
    {
        private readonly QUEUEBREAKERContext _db;

        public OrderItemsRepository(QUEUEBREAKERContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(OrderItem entity)
        {
            await _db.OrderItems.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(OrderItem entity)
        {
            _db.OrderItems.Remove(entity);
            return await Save();
        }

        public async Task<IList<OrderItem>> FindAll()
        {
            var result = await _db.OrderItems.Include(x => x.Item)
                .ToListAsync();
            return result;
        }

        public Task<IList<OrderItem>> FindByCanteenId(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<OrderItem> FindById(int id)
        {
            var result = await _db.OrderItems
                .FirstOrDefaultAsync(q => q.Id == id);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.OrderItems.AnyAsync(q => q.Id == id);
        }

        public async Task<OrderItem> FindByName(string name)
        {
            return await _db.OrderItems.FirstOrDefaultAsync(q => q.Item.Name == name);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(OrderItem entity)
        {
            _db.OrderItems.Update(entity);
            return await Save();
        }

    }
}
