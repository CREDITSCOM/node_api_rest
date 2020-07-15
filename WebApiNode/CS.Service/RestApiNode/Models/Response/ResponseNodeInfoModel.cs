using NodeAPIClient.Models;

namespace CS.Service.RestApiNode.Models
{
    public class ResponseNodeInfoModel : AbstractResponseApiModel
    {
        public NodeInfo NodeInfo { get; set; }
    }
}
