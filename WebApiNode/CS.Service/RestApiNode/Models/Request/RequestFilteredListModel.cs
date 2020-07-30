using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class RequestFilteredListModel : AbstractRequestApiModel
    {
        public string Flagg { get; set; }

        public IEnumerable<SingleQueryModel> Queries{ get; set; }
    }
}
