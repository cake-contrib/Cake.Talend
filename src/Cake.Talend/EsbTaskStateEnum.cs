using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Talend {
    /// <summary>
    /// The list of states for ESB Tasks.
    /// </summary>
    public enum EsbTaskStateEnum {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Started = 0,
        Stopped = 1,
        Deployed = 2,
        Undeployed = 3,
        Created = 4,
        Updated = 5
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class EsbTaskStateEnumExtensions {
        public static string AsString(this EsbTaskStateEnum esbTaskState) {
            switch (esbTaskState) {
                case EsbTaskStateEnum.Started:
                    return "STARTED";
                case EsbTaskStateEnum.Stopped:
                    return "STOPPED";
                case EsbTaskStateEnum.Deployed:
                    return "DEPLOYED";
                case EsbTaskStateEnum.Undeployed:
                    return "UNDEPLOYED";
                case EsbTaskStateEnum.Created:
                    return "CREATED";
                case EsbTaskStateEnum.Updated:
                    return "UPDATED";
                default:
                    return null;
            }
        }

        public static EsbTaskStateEnum Parse(string esbTaskState) {
            if (string.IsNullOrWhiteSpace(esbTaskState)) {
                throw new System.ArgumentNullException(nameof(esbTaskState));
            }
            switch (esbTaskState.ToLower().Trim()) {
                case "started":
                    return EsbTaskStateEnum.Started;
                case "stopped":
                    return EsbTaskStateEnum.Stopped;
                case "deployed":
                    return EsbTaskStateEnum.Deployed;
                case "undeployed":
                    return EsbTaskStateEnum.Undeployed;
                case "created":
                    return EsbTaskStateEnum.Created;
                case "updated":
                    return EsbTaskStateEnum.Updated;
                default:
                    throw new System.ArgumentException($"{esbTaskState} is not a valid state.");
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
