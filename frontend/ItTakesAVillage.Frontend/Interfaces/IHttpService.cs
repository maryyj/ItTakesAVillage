namespace ItTakesAVillage.Frontend.Interfaces
{
    public interface IHttpService
    {
        Task<T?> HttpGetRequest<T>(string requestUri);
        Task<bool> HttpPostRequest<T>(string requestUri, T entity);
        Task<bool> HttpPutRequest<T>(string requestUri, T entity);
        Task<bool> HttpDeleteRequest<T>(string requestUri);
    }
}
