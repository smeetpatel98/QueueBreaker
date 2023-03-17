using System;
using System.Threading.Tasks;
using QueueBreaker_API.Data;

namespace QueueBreaker_API.Contracts
{
    public interface IUsersInterface : IRepositoryBase<User>
    {
        Task<User> Authenticate(string Email, string password);
    }
}
