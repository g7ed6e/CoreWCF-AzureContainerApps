var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, MetadataEndpointAddressServiceBehavior>();
builder.Services.AddSingleton<IMetadataEndpointAddressProvider, SslOffloadMetadataEndpointAddressProvider>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<Service>();
    serviceBuilder.AddServiceEndpoint<Service, IService>(new BasicHttpBinding(), "/Service.svc");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.MapGet("/", context => context.Response.WriteAsync("Hello world !"));

app.Run();
