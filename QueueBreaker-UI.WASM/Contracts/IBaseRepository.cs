using QueueBreaker_UI.WASM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueueBreaker_UI.WASM.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(string url, int id);
        Task<IList<T>> Get(string url);
        Task<bool> Create(string url, T obj);
       
        Task<bool> Update(string url, T obj, int id);
        Task<bool> Delete(string url, int id);
        Task<int> GetCenteenId();
        Task<int> GetUserId();
        Task SetCenteenId(int CenteenId);
        //Task<UserProfileModel> GetProfile(string url);

        //Task<bool> DeleteTrackingHistoryList(string url, int id);
        //Task<IList<UploadedFileModel>> GetFileList(string url, int id);
        //Task<bool> CreateForFile(string url, T obj);
    }
}
