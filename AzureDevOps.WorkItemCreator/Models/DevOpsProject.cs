namespace AzureDevOps.WorkItemCreator.Models
{
    public class DevOpsProject
    {
        public string OrganizationName { get; set; } = default!;

        public string ProjectId { get; set; } = default!;

        public string AccessToken { get; set; } = default!;
    }
}
