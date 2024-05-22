using Newtonsoft.Json;

namespace ItTakesAVillage.Services
{
    public class HttpService
    {
        private readonly HttpClient _httpClient;
        private const string _baseAddress = "https://localhost:7233/api/";

        public HttpService()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_baseAddress)
            };
        }

        public async Task<T?> HttpGetRequest<T>(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            var content = await response.Content.ReadAsStringAsync();
            //return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return JsonConvert.DeserializeObject<T>(content);
        }
        public async Task<bool> HttpPostRequest<T>(string requestUri, T entity)
        {
            //var jsonString = JsonSerializer.Serialize<T>(entity);
            var jsonString = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> HttpPutRequest<T>(string requestUri, T entity)
        {
            var jsonString = JsonConvert.SerializeObject(entity);
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, content);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
        public async Task<bool> HttpDeleteRequest<T>(string requestUri)
        {
            var response = await _httpClient.DeleteAsync(requestUri);
            if (!response.IsSuccessStatusCode)
                return false;
            return true;
        }
    }
}
