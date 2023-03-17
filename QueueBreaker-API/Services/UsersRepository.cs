using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueBreaker_API.Contracts;
using QueueBreaker_API.Data;
using QueueBreaker_API.Helpers;

namespace QueueBreaker_API.Services
{
    public class UsersRepository : IUsersInterface
    {
        private readonly QUEUEBREAKERContext _db;
        public UsersRepository(QUEUEBREAKERContext db
            )
        {
            _db = db;
        }
        public async Task<bool> Create(User entity)
        {
            await _db.Users.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(User entity)
        {
            _db.Users.Remove(entity);
            return await Save();
        }

        public async Task<IList<User>> FindAll()
        {
            var result = await _db.Users
                .ToListAsync();
            return result;
        }

        public async Task<User> FindById(int id)
        {
            var result = await _db.Users
                .FirstOrDefaultAsync(q => q.UserId == id);
            return result;
        }

        public async Task<User> FindByName(string name)
        {
            var result = await _db.Users
                .Include(x => x.Role)
               // .Include(x => x.Canteen)
               .FirstOrDefaultAsync(q => q.Email == name);
            return result;
        }

        public async Task<bool> isExists(int id)
        {
            return await _db.Users.AnyAsync(q => q.UserId == id);
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(User entity)
        {
            _db.Users.Update(entity);
            return await Save();
        }

        public async Task<User> Authenticate(string Email, string password)
        {
           

            var user = await _db.Users.Include(x=>x.Role).FirstOrDefaultAsync(x => x.Email == Email && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;
            return user;
            // authentication successful so generate jwt token
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, user.UserId.ToString()),
            //        new Claim(ClaimTypes.Role, user.RoleId.ToString()),
            //        new Claim("CompanyId",user.CompanyId.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //user.Token = tokenHandler.WriteToken(token);

            //return user.WithoutPassword();
        }

        public Task<IList<User>> FindByCanteenId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
