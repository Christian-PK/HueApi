using HueApi.Clients;
using HueApi.Models;

namespace HueApi.Services;

public class HueDiscoService
{
    private readonly HueApiClient _hueApiClient;
    private readonly HueDeviceService _hueDeviceService;
    private CancellationTokenSource _cancellationTokenSource;

    public HueDiscoService(HueApiClient hueApiClient, HueDeviceService hueDeviceService)
    {
        _hueApiClient = hueApiClient;
        _hueDeviceService = hueDeviceService;
    }

    public HueDiscoService()
    {
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task StartAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var lights = await _hueDeviceService.HueDeviceServiceAsync();
        DiscoLoopAsync(lights, _cancellationTokenSource.Token);
    }

    private async Task DiscoLoopAsync(List<HueLight> hueLights, CancellationToken cancellationToken)
    {
        var lights = hueLights.Select(x => new LightState() { Id = x.Id, On = false }).ToList();
        foreach (var l in lights)
            await _hueApiClient.SwitchLight(l.Id, false);

        var light = GetRandomLight(lights);
        var light2 = GetRandomLight(lights);
        var delay = TimeSpan.FromMilliseconds(150);
        
        while (cancellationToken.IsCancellationRequested == false)
        {
            light = GetRandomLight(lights);
            await _hueApiClient.SwitchLight(light2.Id, false);
            await _hueApiClient.SwitchLight(light.Id, true);
            await Task.Delay(delay);

            light2 = GetRandomLight(lights);
            await _hueApiClient.SwitchLight(light2.Id, true);
            await _hueApiClient.SwitchLight(light.Id, false);
            await Task.Delay(delay);
        }
        
        foreach (var l in lights)
            await _hueApiClient.SwitchLight(l.Id, true);
    }

    private LightState GetRandomLight(List<LightState> lights)
    {
        return lights[Random.Shared.Next(lights.Count)];
    }

    public async Task StopAsync()
    {
        await _cancellationTokenSource.CancelAsync();
    }

    private class LightState
    {
        public string Id { get; set; }
        
        public bool On { get; set; }
    }
}