using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EasyAbp.AbpHelper.Core.Steps.Abp;
using EasyAbp.AbpHelper.Core.Steps.Abp.ModificationCreatorSteps.CSharp;
using EasyAbp.AbpHelper.Core.Steps.Abp.ParseStep;
using EasyAbp.AbpHelper.Core.Steps.Common;
using EasyAbp.AbpHelper.Core.Workflow;
using EasyAbp.AbpHelper.Core.Workflow.Generate;
using Elsa;
using Elsa.Activities;
using Elsa.Activities.ControlFlow.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;

namespace EasyAbp.AbpHelper.Core.Commands.Generate.Antd
{
    public class TryCommand : CommandWithOption<TryCommandOption>
    {
        public TryCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, "try", "Try generate a simple empty file")
        {
        }

        protected override IActivityBuilder ConfigureBuild(TryCommandOption option,
           IActivityBuilder activityBuilder)
        {
            return base.ConfigureBuild(option, activityBuilder)
                .AddOverwriteWorkflow()
                 .Then<SetVariable>(
                    step =>
                    {
                        step.VariableName = "TargetFile";
                        step.ValueExpression = new LiteralExpression<string>(@"D:\\Downloads\try.txt");
                    })
                .Then<FileGenerationStep>(
                    step =>
                    {
                        step.TargetFile = new JavaScriptExpression<string>("TargetFile");
                        step.Contents = "Hello World!";
                    }
                );

        }

    }
}
