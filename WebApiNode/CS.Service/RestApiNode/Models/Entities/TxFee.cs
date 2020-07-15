namespace CS.Service.RestApiNode.Models
{
    public class TxFee
    {
        public string Fee;
        public string Comment;

        public TxFee(string fee, string comment)
        {
            Fee = fee;
            Comment = comment;
        }
    }
}
