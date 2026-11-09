namespace RaceDay.Mvc.Services
{
    public class RaceDayApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RaceDayApiClient(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HttpResponseMessage> SendAsync(
            HttpMethod method,
            string endpoint,
            object? body = null)
        {
            var request = new HttpRequestMessage(method, endpoint);

            var session = _httpContextAccessor.HttpContext?.Session;
            var apiCookie = session?.GetString("ApiSessionCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                request.Headers.TryAddWithoutValidation("Cookie", apiCookie);
            }

            if (body != null)
            {
                request.Content = JsonContent.Create(body);
            }

            var response = await _httpClient.SendAsync(request);

            // Save the API session cookie after login.
            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                var sessionCookie = cookies.FirstOrDefault(c =>
                    c.StartsWith(".AspNetCore.Session=", StringComparison.OrdinalIgnoreCase));

                if (sessionCookie != null && session != null)
                {
                    session.SetString(
                        "ApiSessionCookie",
                        sessionCookie.Split(';')[0]);
                }
            }

            return response;
        }

        public async Task<T?> GetAsync<T>(string endpoint)
        {
            var response = await SendAsync(HttpMethod.Get, endpoint);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> PostAsync(
            string endpoint,
            object body)
        {
            return await SendAsync(HttpMethod.Post, endpoint, body);
        }

        public async Task<HttpResponseMessage> PutAsync(
            string endpoint,
            object body)
        {
            return await SendAsync(HttpMethod.Put, endpoint, body);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await SendAsync(HttpMethod.Delete, endpoint);
        }



        public async Task<HttpResponseMessage> UploadFileAsync(string endpoint, IFormFile file)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            var session = _httpContextAccessor.HttpContext?.Session;
            var apiCookie = session?.GetString("ApiSessionCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                request.Headers.TryAddWithoutValidation("Cookie", apiCookie);
            }

            var content = new MultipartFormDataContent();

            var fileContent = new StreamContent(file.OpenReadStream());

            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(fileContent, "file", Path.GetFileName(file.FileName));

            request.Content = content;

            return await _httpClient.SendAsync(request);

        }

    }
}
