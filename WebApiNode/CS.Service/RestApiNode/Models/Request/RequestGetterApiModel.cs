namespace CS.Service.RestApiNode.Models
{
    public class RequestGetterApiModel : RequestTransactionApiModel
    {
        public int InnerId { get; set; }

        public long BlockId { get; set; }

        public long Offset { get; set; }

        public long Limit { get; set; }
    }
}
