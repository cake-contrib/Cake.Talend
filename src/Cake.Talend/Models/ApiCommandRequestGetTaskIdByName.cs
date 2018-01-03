namespace Cake.Talend.Models {
    /// <summary>
    /// Returns the ID of a task, given its name.
    /// </summary>
    public class ApiCommandRequestGetTaskIdByName: ApiCommandRequest {
#pragma warning disable 1591
        public string taskName { get; set; }
#pragma warning restore 1591
    }
}
