using CS.Service.RestApiNode.Models;
using Microsoft.Extensions.Configuration;

namespace CS.Service.RestApiNode
{
    public class ParseRequestService
    {
        public IConfiguration Configuration { get; }

        public ParseRequestService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GetNetworkIp(AbstractRequestApiModel request, string defaultValue = "127.0.0.1")
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(request.NetworkAlias))
            {
                string config_path = $"ApiNode:servers:{request.NetworkAlias}:s{ServerNum}:";
                result = Configuration[config_path + "Ip"];
            }
            else
            {
                result = request.NetworkIp;
            }
            if (string.IsNullOrWhiteSpace(result))
            {
                return defaultValue;
            }
            return result;
        }

        public int GetRequestTimeout(AbstractRequestApiModel request)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(request.NetworkAlias))
            {
                string config_path = $"ApiNode:servers:{request.NetworkAlias}:s{ServerNum}:";
                result = Configuration[config_path + "TimeOut"];
            }
            int tout;
            if (!int.TryParse(result, out tout))
            {
                tout = DefaultTimeout;
            }
            return tout;
        }

        public ushort GetPublicPort(AbstractRequestApiModel request)
        {
            return GetPort(request, "Port", DefaultPublicPort);
        }

        public ushort GetExecutorPort(AbstractRequestApiModel request)
        {
            return GetPort(request, "ExecutorPort", DefaultExecutorPort);
        }

        public ushort GetDiagnosticPort(AbstractRequestApiModel request)
        {
            return GetPort(request, "DiagnosticPort", DefaultDiagnosticPort);
        }

        ushort GetPort(AbstractRequestApiModel request, string paramName, ushort defaultValue)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(request.NetworkAlias))
            {
                string config_path = $"ApiNode:servers:{request.NetworkAlias}:s{ServerNum}:";
                result = Configuration[config_path + paramName];
            }
            else
            {
                result = request.NetworkPort;
            }
            ushort port;
            if (! ushort.TryParse(result, out port))
            {
                port = defaultValue;
            }
            return port;
        }
        
        int ServerNum => 1;
        
        ushort DefaultPublicPort => 9090;
        
        ushort DefaultExecutorPort => 9070;
        
        ushort DefaultDiagnosticPort => 9088;
        
        int DefaultTimeout => 60000;
    }
}
