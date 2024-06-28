using AzureDevOps.WorkItemCreator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureDevOps.WorkItemCreator
{
    internal sealed class UserStoryGenerator
    {
        private static readonly string[] SourceData =
        [
            
        ];

        public static IEnumerable<UserStory> CreateUserStories()
        {
            var titleTemplate = "";
            var descriptionTemplate = "";
            var acceptanceCriteriaTemplate = "";
            var iterationPath = "";
            var areaPath = "";
            var parentFeature = "";

            foreach (var item in SourceData)
            {
                yield return new UserStory
                {
                    Title = string.Format(titleTemplate, ""),
                    Description = string.Format(descriptionTemplate, ""),
                    AcceptanceCriteria = string.Format(acceptanceCriteriaTemplate, ""),
                    Iteration = iterationPath,
                    Area = areaPath,
                    ParentFeature = parentFeature,
                };
            }
        }
    }
}
