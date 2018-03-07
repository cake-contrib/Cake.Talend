namespace Cake.Talend {
    /// <summary>
    /// Lists all commands available to Talend Admin API
    /// </summary>
    public static class TalendAdminApiCommands {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string LIST_SERVERS = "listServer";
        public const string LIST_TASKS = "getTasksRelatedToJobs";
        public const string LIST_ESB_TASKS = "listEsbTasks";
        public const string GET_TASK_ID_BY_NAME = "getTaskIdByName";
        public const string GET_ESB_TASK_ID_BY_NAME = "getEsbTaskIdByName";
        public const string UPDATE_ESB_TASK = "updateEsbTask";
        public const string START_ESB_TASK = "startEsbTask";
        public const string STOP_ESB_TASK = "stopEsbTask";
        public const string REQUEST_DEPLOY_ESB_TASK = "requestDeployEsbTask";
        public const string REQUEST_UNDEPLOY_ESB_TASK = "requestUndeployEsbTask";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
