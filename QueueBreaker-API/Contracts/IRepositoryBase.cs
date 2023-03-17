using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueueBreaker_API.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IList<T>> FindAll();
        Task<T> FindById(int id);
        Task<IList<T>> FindByCanteenId(int id);

        Task<T> FindByName(string name);
        Task<bool> isExists(int id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();
    }
}
