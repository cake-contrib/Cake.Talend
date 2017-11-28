using RestSharp.Deserializers;
using System.Collections.Generic;

namespace Cake.Talend.Models {
#pragma warning disable 1591
    public class ListServerApiResponse {

        [DeserializeAs(Name = "executionTime")]
        public Executiontime ExecutionTime { get; set; }

        [DeserializeAs(Name = "result")]
        public List<Server> Results { get; set; }

        [DeserializeAs(Name = "returnCode")]
        public int ReturnCode { get; set; }

        [DeserializeAs(Name = "error")]
        public string Error { get; set; }

    }
#pragma warning restore 1591
}
