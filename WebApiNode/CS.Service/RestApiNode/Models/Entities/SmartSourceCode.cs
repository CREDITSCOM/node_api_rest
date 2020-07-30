namespace CS.Service.RestApiNode.Models
{
    public class SmartSourceCode : AbstractResponseApiModel
    {
        public string gZipped { get; set; }
        //public ICollection<byte> gZipped { get; set; }

        public string SourceString { get; set; }
    }
}
