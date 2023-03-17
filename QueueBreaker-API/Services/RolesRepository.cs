using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Services
{
    public class RolesRepository : IRolesInterface
    {
        private readonly QUEUEBREAKERContext _db;

        public RolesRepository(QUEUEBREAKERContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Role entity)
        {
            await _db.Roles.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Role entity)
        {
            _db.Roles.Remove(entity);
            return await Save();
        }

        public async Task<IList<Role>> FindAll()
        {
            var result = await _db.Roles
                .ToListAsync();
            return result;
        }

        public Task<IList<Role>> FindByCanteenId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Role> FindById(int id)
        {
            var result = await _db.Roles
                .FirstOrDefaultAsync(q => q.RoleId == id);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Roles.AnyAsync(q => q.RoleId == id);
        }

        public async Task<Role> FindByName(string name)
        {
            return await _db.Roles.FirstOrDefaultAsync(q => q.Name == name);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Role entity)
        {
            _db.Roles.Update(entity);
            return await Save();
        }


    }
}
