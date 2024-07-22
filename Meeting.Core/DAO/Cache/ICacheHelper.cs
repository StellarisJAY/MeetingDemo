namespace Meeting.Core.DAO.Cache
{
    public interface ICacheHelper
    {
        Task<T?> GetAsync<T>(string key, Func<Task<T?>> missFunc);
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value);

        Task DelAsync(string key);
    }
}
