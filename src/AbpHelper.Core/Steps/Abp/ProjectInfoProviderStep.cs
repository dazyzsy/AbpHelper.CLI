﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyAbp.AbpHelper.Core.Models;
using EasyAbp.AbpHelper.Core.Workflow;
using Elsa.Results;
using Elsa.Services.Models;

namespace EasyAbp.AbpHelper.Core.Steps.Abp
{
    public class ProjectInfoProviderStep : StepWithOption
    {
        protected override async Task<ActivityExecutionResult> OnExecuteAsync(WorkflowExecutionContext context, CancellationToken cancellationToken)
        {
            var baseDirectory = await context.EvaluateAsync(BaseDirectory, cancellationToken);
            LogInput(() => baseDirectory);

            var baseUiDirectory = await context.EvaluateAsync(BaseUiDirectory, cancellationToken);
            LogInput(() => baseUiDirectory);

            if (Directory.Exists(baseUiDirectory))
            {
                context.SetVariable(VariableNames.UiDir, baseUiDirectory);
            }
           
            var excludeDirectories = await context.EvaluateAsync(ExcludeDirectories, cancellationToken);
            LogInput(() => excludeDirectories, string.Join("; ", excludeDirectories));

            var temp = excludeDirectories.ToList();
            if (temp!=null && temp.Any(ed=>ed.Contains("node_modules")))
            {
                excludeDirectories = temp.Where(ed => !ed.Contains("node_modules")).ToArray();
            }

            TemplateType templateType;
            if (FileExistsInDirectory(baseDirectory, "*.DbMigrator.csproj", excludeDirectories))
            {
                templateType = TemplateType.Application;
            }
            else if (FileExistsInDirectory(baseDirectory, "*.Host.Shared.csproj", excludeDirectories))
            {
                templateType = TemplateType.Module;
            }
            else
            {
                templateType = TemplateType.Test;
                // throw new NotSupportedException($"Unknown ABP project structure. Directory: {baseDirectory}");
            }

            //// Assume the domain project must be existed for an ABP project
            //var domainCsprojFile = SearchFileInDirectory(baseDirectory, "*.Domain.csproj", excludeDirectories);
            //if (domainCsprojFile == null) throw new NotSupportedException($"Cannot find the domain project file. Make sure it is a valid ABP project. Directory: {baseDirectory}");

            //var fileName = Path.GetFileName(domainCsprojFile);
            //var fullName = fileName.RemovePostFix(".Domain.csproj");

            var fileName = "";
            var fullName = "Test";
            if (templateType != TemplateType.Test)
            {
                var domainCsprojFile = SearchFileInDirectory(baseDirectory, "*.Domain.csproj", excludeDirectories);
                if (domainCsprojFile == null) throw new NotSupportedException($"Cannot find the domain project file. Make sure it is a valid ABP project. Directory: {baseDirectory}");

                fileName = Path.GetFileName(domainCsprojFile);
                fullName = fileName.RemovePostFix(".Domain.csproj");
            }
            else
            {
                var domainCsprojFile = SearchFileInDirectory(baseDirectory, "*.Core.csproj", excludeDirectories);
                if (domainCsprojFile == null) throw new NotSupportedException($"Cannot find the domain project file. Make sure it is a valid ABP project. Directory: {baseDirectory}");

                fileName = Path.GetFileName(domainCsprojFile);
                fullName = fileName.RemovePostFix(".Core.csproj");
            }


            UiFramework uiFramework;
            if (FileExistsInDirectory(baseDirectory, "*.cshtml", excludeDirectories))
            {
                uiFramework = UiFramework.RazorPages;
            }
            else if (FileExistsInDirectory(baseDirectory, "app.module.ts", excludeDirectories))
            {
                uiFramework = UiFramework.Angular;
            }
            else
            {
                uiFramework = UiFramework.None;

            }

            if (templateType == TemplateType.Test)
            {
                uiFramework = UiFramework.Antd;
            }

            string aspNetCoreDir = Path.Combine(baseDirectory, "aspnet-core");
            if (Directory.Exists(aspNetCoreDir))
            {
                context.SetVariable(VariableNames.AspNetCoreDir, aspNetCoreDir);
            }
            else
            {
                context.SetVariable(VariableNames.AspNetCoreDir, baseDirectory);
            }

            if (templateType != TemplateType.Test)
            {
                EnsureSlnFileExists(context, fullName);
            }

            var tiered = false;
            if (templateType == TemplateType.Application)
            {
                tiered = FileExistsInDirectory(baseDirectory, "*.IdentityServer.csproj", excludeDirectories);
            }

            var projectInfo = new ProjectInfo(baseDirectory, baseUiDirectory, fullName, templateType, uiFramework, tiered);

            context.SetLastResult(projectInfo);
            context.SetVariable("ProjectInfo", projectInfo);
            LogOutput(() => projectInfo);

            return Done();
        }

        private void EnsureSlnFileExists(WorkflowExecutionContext context, string projectName)
        {
            string aspNetCoreDir = context.GetVariable<string>(VariableNames.AspNetCoreDir);
            string slnFile = Path.Combine(aspNetCoreDir, $"{projectName}.sln");
            if (!File.Exists(slnFile))
            {
                throw new FileNotFoundException($"The solution file '{projectName}.sln' is not found in '{aspNetCoreDir}'. Make sure you specific the right folder.");
            }
        }
    }
}
