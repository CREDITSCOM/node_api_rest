using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CS.Service.RestApiNode.Models
{
    public class RequestNodeInfoModel : AbstractRequestApiModel
    {
        [JsonIgnore]
        public static ushort DefaultPort => 9088;

        public string Ip { get; set; }

        public string Port { get; set; }

    }
}
