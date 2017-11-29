using RestSharp.Deserializers;

namespace Cake.Talend.Models {
#pragma warning disable 1591
    public class Executiontime {
        public int millis { get; set; }
        public int seconds { get; set; }
    }


    public class TalendApiResponse<T> {
        [DeserializeAs(Name = "executionTime")]
        public Executiontime ExecutionTime { get; set; }

        [DeserializeAs(Name = "result")]
        public T Results { get; set; }

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
