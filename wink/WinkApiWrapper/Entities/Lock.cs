using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WinkApiWrapper.Entities
{
    [DataContract]
    public class Lock: BaseDevice
    {
        [DataMember]
        public bool Locked { get; set; }
    }
}
