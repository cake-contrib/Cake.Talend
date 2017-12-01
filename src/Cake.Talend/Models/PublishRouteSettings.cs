using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Talend.Models {
    /// <summary>
    /// The necessary settings to publish a route.
    /// </summary>
    public class PublishRouteSettings {
        /// <summary>
        /// The name of the project.
        /// </summary>
        /// <example>Test1</example>
        public string ProjectName { get; set; }
        /// <summary>
        /// The name of the Route.
        /// </summary>
        /// <example>Test1</example>
        public string RouteName { get; set; }
        /// <summary>
        /// The directory in Nexus.
        /// </summary>
        /// <example>org.rsc</example>
        public string RouteGroup { get; set; }
        /// <summary>
        /// If true, make sure artifact repository url points to Snapshots repository. If false, make sure artifact repository url points to Release repository.
        /// If unspecified, assume true.
        /// </summary>
        public bool? IsSnapshot { get; set; }
        /// <summary>
        /// The context to use. Local, Test, or Prod usually.
        /// </summary>
        /// <example>Prod</example>
        public string Context { get; set; }
        /// <summary>
        /// Version on Nexus independent of actual Talend Route version.
        /// </summary>
        /// <example>0.5.1</example>
        public string PublishVersion { get; set; }
        /// <summary>
        /// URL of the Nexus Artifact Repository to Publish build to.
        /// </summary>
        /// <example>Example: http://localhost:8081/nexus/content/repositories/snapshots/ </example>
        public string ArtifactRepositoryUrl { get; set; }
        /// <summary>
        /// Username of the Nexus Artifact Repository to Publish build to.
        /// </summary>
        /// <example>admin</example>
        public string ArtifactRepositoryUsername { get; set; }
        /// <summary>
        /// Password of the Nexus Artifact Repository to Publish build to.
        /// </summary>
        /// <example>Talend123</example>
        public string ArtifactRepositoryPassword { get; set; }
    }
}
