using CoreWCF;
using System;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using System.Web.Services.Description;

namespace CoreWCFService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);
    }

    public class Service : IService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    public class SslOffloadMetadataEndpointAddressProvider : IMetadataEndpointAddressProvider
    {
        public Uri GetEndpointAddress(HttpRequest request)
        {
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
}
