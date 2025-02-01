using HueApi.Models.Hue;

namespace HueApi.Clients;

public class HueApiClient
{
    private readonly HttpClient _httpClient;
    public HueApiClient(ILogger<HueApiClient> logger)
    {
        var apiKey = Environment.GetEnvironmentVariable("HUE_API_KEY");
        var baseUrl = Environment.GetEnvironmentVariable("HUE_BASE_URL");
        _httpClient = CreateHttpClientWithoutCertificationValidation();
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(baseUrl))
        {
            logger.LogError("Hue API Key or baseUrl environment variable is missing");
            return;
        }
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Add("hue-application-key", apiKey);
    }

    private HttpClient CreateHttpClientWithoutCertificationValidation()
    {
        var handler = new HttpClientHandler();
        handler.ClientCertificateOptions = ClientCertificateOption.Manual;
        handler.ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) => { return true; };
        return new HttpClient(handler);
    }

    public async Task SwitchLight(string id, bool on)
    {
        var state = new HueLightStateCommand(on);
        var url = $"/clip/v2/resource/light/" + id;
        var response = await _httpClient.PutAsJsonAsync(url, state);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<HueGetDeviceResponse> GetDevicesAsync()
    {
        var url = "/clip/v2/resource/device";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<HueGetDeviceResponse>())!;
    }
    
    public async Task<string> GetApiKeyAsync()
    {
        // New client is used because we do not want to use any access tokens
        var httpClient = CreateHttpClientWithoutCertificationValidation();
        httpClient.BaseAddress = _httpClient.BaseAddress;
        var url = "/api";

        var body = "{\"devicetype\":\"app_name#instance_name\", \"generateclientkey\":true}";
        var response = await httpClient.PostAsync(url, new StringContent(body));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}