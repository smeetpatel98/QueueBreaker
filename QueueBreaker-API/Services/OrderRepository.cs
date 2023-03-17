using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Services
{
  
    public class OrderRepository : IOrderInterface
    {
        private readonly QUEUEBREAKERContext _db;

        public OrderRepository(QUEUEBREAKERContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Order entity)
        {
            await _db.Orders.AddAsync(entity);
            return await Save();
        }

       

        public async Task<bool> Delete(Order entity)
        {
            _db.Orders.Remove(entity);
            return await Save();
        }

        public async Task<IList<Order>> FindAll()
        {
            var result = await _db.Orders.Include(x => x.User).Include(x => x.OrderItems).ThenInclude(y=>y.Item).ToListAsync();
           
            return result;
        }

        public async Task<IList<Order>> FindByCanteenId(int id)
        {
            var result = await _db.Orders.Include(x => x.User).Include(x => x.OrderItems).ThenInclude(y => y.Item).Where(q => q.CanteenId == id).ToListAsync();
            return result;
        }
        public async Task<Order> FindById(int id)
        {
            var result = await _db.Orders.Include(x => x.User).Include(x => x.OrderItems).ThenInclude(y => y.Item)
                .FirstOrDefaultAsync(q => q.Id == id);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Orders.AnyAsync(q => q.Id == id);
        }

        public  Task<Order> FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Order entity)
        {
            _db.Orders.Update(entity);
            return await Save();
        }

    }
}
