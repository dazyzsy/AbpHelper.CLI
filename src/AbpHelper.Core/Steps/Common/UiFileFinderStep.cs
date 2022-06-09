using Elsa.Expressions;
using Elsa.Results;
using Elsa.Scripting.JavaScript;
using Elsa.Services.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyAbp.AbpHelper.Core.Steps.Common
{
    public class UiFileFinderStep : StepWithOption
    {
        public const string DefaultUiFileParameterName = "FileFinderResult";

        public WorkflowExpression<string> SearchUiFileName
        {
            get => GetState<WorkflowExpression<string>>();
            set => SetState(value);
        }

        public WorkflowExpression<string> ResultVariableName
        {
            get => GetState(() => new LiteralExpression(DefaultUiFileParameterName));
            set => SetState(value);
        }

        public WorkflowExpression<bool> ErrorIfNotFound
        {
            get => GetState(() => new JavaScriptExpression<bool>("true"));
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var resultVariableName = await context.EvaluateAsync(ResultVariableName, cancellationToken);
            var baseUiDirectory = await context.EvaluateAsync(BaseUiDirectory, cancellationToken);
            LogInput(() => baseUiDirectory);
            var excludeDirectories = await context.EvaluateAsync(ExcludeDirectories, cancellationToken);
            if (excludeDirectories.Length==0)
            {
                //excludeDirectories = new string[] { Path.Combine(baseUiDirectory, "node_modules")};//.AddIfNotContains("node_modules");
                excludeDirectories = new string[] { "node_modules" };//.AddIfNotContains("node_modules");
            }
            LogInput(() => excludeDirectories, string.Join("; ", excludeDirectories));
            var searchUiFileName = await context.EvaluateAsync(SearchUiFileName, cancellationToken);
            LogInput(() => searchUiFileName);
            var errorIfNotFound = await context.EvaluateAsync(ErrorIfNotFound, cancellationToken);

            var files = SearchFilesInDirectory(baseUiDirectory, searchUiFileName, excludeDirectories);

            var filePathName = files.SingleOrDefault();

            context.SetLastResult(filePathName);
            context.SetVariable(resultVariableName, filePathName);
            if (filePathName == null)
            {
                if (errorIfNotFound) throw new FileNotFoundException(searchUiFileName);
                LogOutput(() => filePathName, $"File: '{filePathName}' not found, stored 'null' in parameter: '{ResultVariableName}'");
            }
            else
            {
                LogOutput(() => filePathName, $"Found file: '{filePathName}', stored in parameter: '{ResultVariableName}'");
            }

            return Done();
        }
    }
}
