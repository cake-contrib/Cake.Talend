using RestSharp.Deserializers;

namespace Cake.Talend.Models {
#pragma warning disable 1591
    public class Executiontime {
        public int millis { get; set; }
        public int seconds { get; set; }
    }

    /// <summary>
    /// The base class from which all Talend API calls inherit.
    /// </summary>
    public class TalendApiResponse {
        [DeserializeAs(Name = "executionTime")]
        public Executiontime ExecutionTime { get; set; }

        [DeserializeAs(Name = "returnCode")]
        public int ReturnCode { get; set; }

        [DeserializeAs(Name = "error")]
        public string Error { get; set; }

        public class Executiontime {
            public int millis { get; set; }
            public int seconds { get; set; }
        }
    }
#pragma warning restore 1591
}
