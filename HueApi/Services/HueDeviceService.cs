using System.Text.RegularExpressions;
using HueApi.Clients;
using HueApi.Models;
using HueApi.Models.Hue;

namespace HueApi.Services;

public class HueDeviceService
{
    private readonly HueApiClient _hueApiClient;

    public HueDeviceService(HueApiClient hueApiClient)
    {
        _hueApiClient = hueApiClient;
    }

    public async Task<List<HueLight>> HueDeviceServiceAsync()
    {
        var deviceSearchFilter = Environment.GetEnvironmentVariable("HUE_DEVICE_SEARCH_FILTER");
        deviceSearchFilter ??= "(.*)";

        var deviceResponse = await _hueApiClient.GetDevicesAsync();
        var devices = deviceResponse.data.Where(x => Regex.IsMatch(x.metadata.name, deviceSearchFilter)).ToList();
        var lights = new List<HueLight>();
        foreach (var device in devices)
        {
            var lightService =
                device.services.FirstOrDefault(x => x.rtype.Equals("light", StringComparison.CurrentCultureIgnoreCase));
            if (lightService != null)
                lights.Add(new HueLight(lightService.rid, device.metadata.name));
        }

        return lights;
    }
}