using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cachetwo
{
    [DataContract]
    public class CustomDataContractClass
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember(Name = "Id")]
        public int AlternateId { get; set; }

        [IgnoreDataMember]
        public int Ignored { get; set; }

        public int NotMember { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember]
        public string NameUpper => Name?.ToUpperInvariant();
    }
}
