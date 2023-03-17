using QueueBreaker_UI.WASM.Models;
using System.Threading.Tasks;

namespace QueueBreaker_UI.WASM.Contracts
{
    public interface IUserProfileRepository : IBaseRepository<UserProfileModel>
    {
        Task<bool> UpdateUserPassword(UpdatePasswordModel user);
    }
}
