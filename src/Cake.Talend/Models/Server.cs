namespace Cake.Talend.Models {
#pragma warning disable 1591
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
#pragma warning restore 1591
}
