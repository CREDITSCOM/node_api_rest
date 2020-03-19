using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public class ResponseNodeInfoModel : AbstractResponseApiModel
    {
        NodeAPIClient.Models.NodeInfo Info { get; set; }
    }
}
