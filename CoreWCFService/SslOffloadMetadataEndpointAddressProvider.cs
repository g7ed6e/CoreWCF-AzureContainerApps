using System.Text;

namespace CoreWCFService;

public class SslOffloadMetadataEndpointAddressProvider : IMetadataEndpointAddressProvider
{
    public Uri GetEndpointAddress(HttpRequest request)
    {
        return new Uri("https://corewcf-service.bravesky-516d42c4.francecentral.azurecontainerapps.io/Service.svc");
        const string delimiter = "://";
        string scheme = request.Headers.TryGetValue("X-Forwarded-Proto", out var value) 
            ? value
            : request.Scheme;
        var host = request.Host.Value ?? string.Empty;
        var pathBase = request.PathBase.Value ?? string.Empty;
        var path = request.Path.Value ?? string.Empty;
        var queryString = request.QueryString.Value ?? string.Empty;

        // PERF: Calculate string length to allocate correct buffer size for StringBuilder.
        var length = scheme.Length + delimiter.Length + host.Length
                     + pathBase.Length + path.Length + queryString.Length;

        return new Uri(new StringBuilder(length)
            .Append(scheme)
            .Append(delimiter)
            .Append(host)
            .Append(pathBase)
            .Append(path)
            .Append(queryString)
            .ToString());
    }
}