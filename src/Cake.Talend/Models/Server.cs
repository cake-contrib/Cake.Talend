namespace Cake.Talend.Models {
    /// <summary>
    /// Describes a server as returned by the Talend API.
    /// </summary>
    public class Server {
#pragma warning disable 1591
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
#pragma warning restore 1591
    }
}
