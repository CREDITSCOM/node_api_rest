using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Service.RestApiNode.Models
{
    public partial class DelegatedStructure
    {
        public DelegatedStructure()
        {
            Donors = new List<DelegatedInfo>();
            Recipients = new List<DelegatedInfo>();
        }
        public Decimal Incoming { get; set; }

        public Decimal Outgoing { get; set; }

        public ICollection<DelegatedInfo> Donors { get; set; }

        public ICollection<DelegatedInfo> Recipients { get; set; }
    }
}
