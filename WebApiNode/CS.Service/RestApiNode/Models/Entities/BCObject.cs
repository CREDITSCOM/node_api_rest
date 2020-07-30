using System.Collections.Generic;

namespace CS.Service.RestApiNode.Models
{
    public class BCObject
    {
        public string Name { get; set; }
        
        public ICollection<byte> ByteCode { get; set; }
    }
}
