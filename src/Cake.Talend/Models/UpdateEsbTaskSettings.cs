namespace Cake.Talend.Models {
    /// <summary>
    /// The settings to update an ESB task. Only fill in what is needed.
    /// </summary>
    public class UpdateEsbTaskSettings {
        /// <summary>
        /// The directory in Nexus.
        /// </summary>
        /// <example>org.rsc</example>
        public string JobGroup { get; set; }
        /// <summary>
        /// The name of the feature
        /// </summary>
        /// <example>DemoRESTConsumer</example>
        public string FeatureName { get; set; }
        /// <summary>
        /// The version ID to use
        /// </summary>
        /// <example>0.5.1</example>
        public string VersionID { get; set; }
        /// <summary>
        /// The ID of this ESB Task to update. If cannot find, use the Task name to get ID.
        /// </summary>
        /// <example>22</example>
        public int? EsbTaskID { get; set; }
        /// <summary>
        /// The name of this task
        /// </summary>
        /// <example>RestTest</example>
        public string EsbTaskName { get; set; }
        /// <summary>
        /// Snapshots or Releases.
        /// </summary>
        /// <example>snapshots</example>
        public string Repository { get; set; }
        /// <summary>
        /// The name of the runtime context to use.
        /// </summary>
        /// <example>Default</example>
        public string ContextName { get; set; }
        /// <summary>
        /// The description to show in Admin
        /// </summary>
        /// <example>Route 17</example>
        public string Description { get; set; }

    }
}
