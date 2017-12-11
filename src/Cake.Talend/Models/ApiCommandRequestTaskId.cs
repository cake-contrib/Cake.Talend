namespace Cake.Talend.Models {
    /// <summary>
    /// A command that only takes a taskID
    /// </summary>
    internal class ApiCommandRequestTaskId : ApiCommandRequest {
#pragma warning disable 1591
        public int taskId { get; set; }
#pragma warning restore 1591
    }
}
