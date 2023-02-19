var builder = WebApplication.CreateBuilder();

builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
builder.Services.AddSingleton<IServiceBehavior, MetadataEndpointAddressServiceBehavior>();
builder.Services.AddSingleton<IMetadataEndpointAddressProvider, SslOffloadMetadataEndpointAddressProvider>();

var app = builder.Build();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<Service>(options =>
    {
        //options.BaseAddresses.Add(new Uri("https://localhost"));   
    });
    //serviceBuilder.AddServiceEndpoint<Service, IService>(new BasicHttpBinding(BasicHttpSecurityMode.Transport), "/Service.svc/https");
    serviceBuilder.AddServiceEndpoint<Service, IService>(new BasicHttpBinding(), "/Service.svc");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpGetEnabled = true;
});

app.MapGet("/", async context =>
{
    foreach (var (key, value) in context.Request.Headers)
    {
        await context.Response.WriteAsync($"{key}:{value}\n");
    }
});

app.Run();
