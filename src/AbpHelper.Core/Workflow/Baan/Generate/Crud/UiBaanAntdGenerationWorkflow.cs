using System.Collections.Generic;
using EasyAbp.AbpHelper.Core.Steps.Abp;
using EasyAbp.AbpHelper.Core.Steps.Abp.ModificationCreatorSteps.CSharp;
using EasyAbp.AbpHelper.Core.Steps.Baan.AntdPro;
using EasyAbp.AbpHelper.Core.Steps.Common;
using Elsa;
using Elsa.Activities;
using Elsa.Activities.ControlFlow.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;

namespace EasyAbp.AbpHelper.Core.Workflow.Baan.Generate.Crud
{
    public static class UiBaanAntdGenerationWorkflow
    {
        public static IActivityBuilder AddUiBaanAntdGenerationWorkflow(this IOutcomeBuilder builder)
        {
            return builder
                    //.Then<IfElse>(
                    //    step => step.ConditionExpression = new JavaScriptExpression<bool>("ProjectInfo.TemplateType == 2"),
                    //    ifElse =>
                    //    {
                    //        // For module, put generated Razor files under the "ProjectName" folder */
                    //        ifElse
                    //            .When(OutcomeNames.True)
                    //            .Then<SetVariable>(
                    //                step =>
                    //                {
                    //                    step.VariableName = "Bag.PagesFolder";
                    //                    step.ValueExpression = new JavaScriptExpression<string>("ProjectInfo.Name");
                    //                }
                    //            )
                    //            .Then<SetModelVariableStep>()
                    //            .Then(ActivityNames.UiAntdPro)
                    //            ;
                    //        ifElse
                    //            .When(OutcomeNames.False)
                    //            .Then(ActivityNames.UiAntdPro)
                    //            ;
                    //    }
                    //)
                    //.Then<IfElse>(
                    //    step => step.ConditionExpression = new JavaScriptExpression<bool>("ProjectInfo.TemplateType == 1"),
                    //    ifElse =>
                    //    {
                    //        // For module, put generated Razor files under the "ProjectName" folder */
                    //        ifElse
                    //            .When(OutcomeNames.True)
                    //            .Then<SetVariable>(
                    //                step =>
                    //                {
                    //                    step.VariableName = "Bag.PagesFolder";
                    //                    step.ValueExpression = new JavaScriptExpression<string>("ProjectInfo.Name");
                    //                }
                    //            )
                    //            .Then<SetModelVariableStep>()
                    //            .Then(ActivityNames.UiRazor)
                    //            ;
                    //        ifElse
                    //            .When(OutcomeNames.False)
                    //            .Then(ActivityNames.UiRazor)
                    //            ;
                    //    }
                    //)
                    /* Generate razor pages ui files*/
                    .Then<GroupGenerationStep>(
                        step =>
                        {
                            step.GroupName = "UiAntdPro";
                            step.TargetDirectory = new JavaScriptExpression<string>(VariableNames.UiDir);
                        }
                    ).WithName(ActivityNames.UiAntdPro)
                    // .Then<GroupGenerationStep>(
                    //    step =>
                    //    {
                    //        step.GroupName = "UiAntdPro";
                    //        step.TargetDirectory = new JavaScriptExpression<string>(VariableNames.UiDir);
                    //    }
                    //).WithName(ActivityNames.UiAntdPro)
                    /* Add menu name */
                    //.Then<MultiFileFinderStep>(
                    //    step =>
                    //    {
                    //        step.SearchFileName = new JavaScriptExpression<string>("`${ProjectInfo.Name}Menus.cs`");
                    //    }
                    //)
                    //.Then<ForEach>(
                    //    x => { x.CollectionExpression = new JavaScriptExpression<IList<object>>(MultiFileFinderStep.DefaultFileParameterName); },
                    //    branch =>
                    //        branch.When(OutcomeNames.Iterate)
                    //            .Then<MenuNameStep>(step => step.SourceFile = new JavaScriptExpression<string>("CurrentValue"))
                    //            .Then<FileModifierStep>(step => step.TargetFile = new JavaScriptExpression<string>("CurrentValue"))
                    //            .Then(branch)
                    //)
                    ///* Add menu */
                    //.Then<MultiFileFinderStep>(
                    //    step =>
                    //    {
                    //        step.SearchFileName = new JavaScriptExpression<string>("`${ProjectInfo.Name}MenuContributor.cs`");
                    //    }
                    //)
                    //.Then<ForEach>(
                    //    x => { x.CollectionExpression = new JavaScriptExpression<IList<object>>(MultiFileFinderStep.DefaultFileParameterName); },
                    //    branch =>
                    //        branch.When(OutcomeNames.Iterate)
                    //            .Then<MenuContributorStep>(step => step.SourceFile = new JavaScriptExpression<string>("CurrentValue"))
                    //            .Then<FileModifierStep>(step => step.TargetFile = new JavaScriptExpression<string>("CurrentValue"))
                    //            .Then(branch)
                    //)
                    /* Add mapping */
                    //.Then<FileFinderStep>(
                    //    step =>
                    //    {
                    //        step.BaseDirectory = new JavaScriptExpression<string>(@"`${AspNetCoreDir}/src`");
                    //        step.SearchFileName = new JavaScriptExpression<string>("`${ProjectInfo.Name}WebAutoMapperProfile.cs`");
                    //    })
                    //.Then<WebAutoMapperProfileStep>()
                    .Then<FileModifierStep>()
                    /* Modify config.ts */
                    .Then<UiFileFinderStep>(
                        step => {
                            step.SearchUiFileName = new LiteralExpression("config.ts");
                        }
                    )
                    .Then<AddRoutesInConfigStep>()
                    .Then<FileModifierStep>(
                        step => step.NewLine = new JavaScriptExpression<string>(@"'\n'")
                    )
                ;
        }
    }
}
