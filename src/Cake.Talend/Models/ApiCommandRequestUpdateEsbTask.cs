using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Talend.Models {
    /// <summary>
    /// Returns the ID of a task, given its name.
    /// </summary>
    internal class ApiCommandRequestUpdateEsbTask : ApiCommandRequest {
#pragma warning disable 1591
        public string description { get; set; }
        public string featureName { get; set; }
        public string featureType { get; set; }
        public string featureUrl { get; set; }
        public string featureVersion { get; set; }
        public string repository { get; set; }
        public string runtimeContext { get; set; }
        public string runtimePropertyId { get; set; }
        public string runtimeServerName { get; set; }
        public int taskId { get; set; }
        public string taskName { get; set; }
#pragma warning restore 1591
    }
}
