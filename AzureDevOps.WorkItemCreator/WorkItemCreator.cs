using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using AzureDevOps.WorkItemCreator.Models;
using Microsoft.Extensions.Options;

namespace AzureDevOps.WorkItemCreator
{
    internal sealed class WorkItemCreator
    {
        private static readonly string UserStoryType = "Microsoft.VSTS.WorkItemTypes.UserStory";

        private readonly DevOpsProject _projectInfo;

        /// <summary>
        /// Constructor. Manually set values to match your organization. 
        /// </summary>
        public WorkItemCreator(IOptions<DevOpsProject> project)
        {
            _projectInfo = project.Value ?? throw new ArgumentNullException(nameof(project));
        }

        public async Task<WorkItem[]> CreateUserStory(IEnumerable<UserStory> userStories)
        {
            var tasks = userStories.Select(CreateUserStory).ToArray();

            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Create a bug using the .NET client library
        /// </summary>
        /// <returns>Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem</returns>    
        public async Task<WorkItem> CreateUserStory(UserStory us)
        {
            var orgUrl = new Uri($"https://dev.azure.com/{_projectInfo.OrganizationName}");

            VssBasicCredential credentials = new VssBasicCredential("", _projectInfo.AccessToken);
            var patchDocument = CreatePatchDocument(us);

            VssConnection connection = new VssConnection(orgUrl, credentials);
            WorkItemTrackingHttpClient workItemTrackingHttpClient = connection.GetClient<WorkItemTrackingHttpClient>();

            try
            {
                WorkItem result = await workItemTrackingHttpClient.CreateWorkItemAsync(patchDocument, _projectInfo.ProjectId, UserStoryType);

                Console.WriteLine("User Story Successfully Created: US #{0}", result.Id);

                return result;
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Error creating US: {0}", ex.InnerException.Message);
                return null;
            }
        }

        /// <remarks>
        /// Find paths for fields at
        /// https://dev.azure.com/{_projectInfo.OrganizationName}/{_projectInfo.ProjectId}/_apis/wit/workItemTypes/{UserStoryType}
        /// </remarks>
        private static JsonPatchDocument CreatePatchDocument(UserStory userStory)
        {
            return
            [
                //add fields and their values to your patch document
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Title",
                    Value = userStory.Title
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Description",
                    Value = userStory.Description
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/Microsoft.VSTS.Common.AcceptanceCriteria",
                    Value = userStory.AcceptanceCriteria
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.AreaPath",
                    Value = userStory.Area
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.IterationPath",
                    Value = userStory.Iteration
                },
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.Parent",
                    Value = userStory.ParentFeature
                },
            ];
        }
    }
}
