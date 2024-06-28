namespace AzureDevOps.WorkItemCreator.Models
{
    public sealed class UserStory
    {
        public string Title { get; set; }

        public string State { get; set; } = "New";

        /// <summary>
        /// The Description. Supports HTML.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The Acceptance Criteria. Supports HTML.
        /// </summary>
        public string AcceptanceCriteria { get; set; }

        public string Area { get; set; }

        public string Iteration { get; set; }

        public string ParentFeature { get; set; }
    }
}
