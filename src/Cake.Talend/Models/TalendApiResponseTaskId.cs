using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Talend.Models {
    /// <summary>
    /// Returns the ID of a task, given the tasks name.
    /// </summary>
    public class TalendApiResponseTaskId: TalendApiResponse {
#pragma warning disable 1591
        public int taskId { get; set; }
#pragma warning restore 1591
    }
}
