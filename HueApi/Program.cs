using HueApi.Clients;
using HueApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<HueApiClient>();
builder.Services.AddSingleton<HueDeviceService>();
builder.Services.AddSingleton<HueDiscoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/hue/apikey", (HueApiClient hueApiClient) => hueApiClient.GetApiKeyAsync());
app.MapGet("/hue/founddevices",
    async (HueDeviceService hueDeviceService) => await hueDeviceService.HueDeviceServiceAsync());
app.MapPut("/disco/start", (HueDiscoService hueDiscoService) => hueDiscoService.StartAsync());
app.MapPut("/disco/stop", (HueDiscoService hueDiscoService) => hueDiscoService.StopAsync());

app.Run();