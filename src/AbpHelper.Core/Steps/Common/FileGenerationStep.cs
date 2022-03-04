using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Expressions;
using Elsa.Results;
using Elsa.Scripting.JavaScript;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;

namespace EasyAbp.AbpHelper.Core.Steps.Common
{
    public class FileGenerationStep : Step
    {
        public WorkflowExpression<string> TargetFile
        {
            get => GetState(() => new JavaScriptExpression<string>(FileFinderStep.DefaultFileParameterName));
            set => SetState(value);
        }

        public string Contents
        {
            get => GetState<string>();
            set => SetState(value);
        }

        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var targetFile = await context.EvaluateAsync(TargetFile, cancellationToken);

            LogInput(() => targetFile);
            LogInput(() => targetFile,$"Current Value: {targetFile}");
            LogInput(() => Contents, $"Contents length: {Contents.Length}");

            if (targetFile.IsNullOrEmpty())
            {
                targetFile = @"D:\\Downloads\try2.txt";
            }

            var dir = Path.GetDirectoryName(targetFile);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Logger.LogInformation($"Directory {dir} created.");
            }

            await File.WriteAllTextAsync(targetFile, Contents);
            Logger.LogInformation($"File {targetFile} generated.");

            return Done();
        }
    }
}