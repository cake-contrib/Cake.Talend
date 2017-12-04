namespace Cake.Talend {
    /// <summary>
    /// The two main repositories.
    /// </summary>
    public enum RepositoryEnum {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Snapshots = 0,
        Releases = 1
    }
 
    public static class RepositoryEnumExtensions {
        public static string AsString(this RepositoryEnum repository) {
            switch(repository) {
                case RepositoryEnum.Snapshots:
                    return "snapshots";
                case RepositoryEnum.Releases:
                    return "releases";
                default:
                    return null;
            }
        }

        public static RepositoryEnum Parse(string repository) {
            if(string.IsNullOrWhiteSpace(repository)) {
                throw new System.ArgumentNullException(nameof(repository));
            }
            switch(repository.ToLower().Trim()) {
                case "snapshots":
                    return RepositoryEnum.Snapshots;
                case "releases":
                    return RepositoryEnum.Releases;
                default:
                    throw new System.ArgumentException($"{repository} is not a valid repository.");
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
