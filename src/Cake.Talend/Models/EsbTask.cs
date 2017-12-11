namespace Cake.Talend.Models {
    /// <summary>
    /// Describes an ESB Task as returned by the Talend API.
    /// </summary>
    public class EsbTask {
#pragma warning disable 1591
        public string applicationFeatureURL { get; set; }
        public string applicationName { get; set; }
        public string applicationType { get; set; }
        public string applicationVersion { get; set; }
        public string contextName { get; set; }
        public int id { get; set; }
        public string jobServerLabelHost { get; set; }
        public string label { get; set; }
        public string pid { get; set; }
        public string repositoryName { get; set; }
#pragma warning restore 1591
    }
}
