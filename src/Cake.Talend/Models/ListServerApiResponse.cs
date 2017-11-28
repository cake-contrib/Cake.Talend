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

        public class Executiontime {

            public int millis { get; set; }
            public int seconds { get; set; }
        }

        public class Server {

            public bool active { get; set; }
            public int fileTransferPort { get; set; }
            public string host { get; set; }
            public int id { get; set; }
            public string label { get; set; }
            public int monitoringPort { get; set; }
            public int port { get; set; }
            public int timeOutUnknownState { get; set; }
            public bool useSSL { get; set; }
            public int adminConsolePort { get; set; }
            public string instance { get; set; }
            public bool isRuntimeServer { get; set; }
            public int mgmtRegPort { get; set; }
            public int mgmtServerPort { get; set; }
        }
    }
#pragma warning restore 1591
}
