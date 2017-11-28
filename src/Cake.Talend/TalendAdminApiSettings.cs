namespace Cake.Talend {
    /// <summary>
    /// Settings for Talend Admin Center API
    /// </summary>
    public class TalendAdminApiSettings {
        /// <summary>
        /// The location of the Talend Administration Center.
        /// </summary>
        /// <example>http://localhost:8081/tac</example>
        public string TalendAdminAddress { get; set; }

        /// <summary>
        /// The Talend Administration Center username to login as.
        /// </summary>
        /// <example>admin@company.com</example>
        public string TalendAdminUsername { get; set; }

        /// <summary>
        /// The Talend Administration Center password to login with.
        /// </summary>
        /// <example>admin</example>
        public string TalendAdminPassword { get; set; }
    }
}
