namespace CS.Service.RestApiNode.Models
{
    public class RequestTransactionsApiModel : RequestKeyApiModel
    {
        public long Offset { get; set; }

        public long Limit { get; set; }
    }
}
