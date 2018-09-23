using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cachetwo
{
    public class CustomJsonAttributeClass
    {
        public int Id { get; set; }

        [JsonProperty("Id")]
        public int AlternateId { get; set; }

        [JsonIgnore]
        public int Ignored { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public string NameUpper => Name?.ToUpperInvariant();
    }
}
