using EasyAbp.AbpHelper.Core.Commands.Generate.Crud;
using EasyAbp.AbpHelper.Core.Models;
using EasyAbp.AbpHelper.Core.Steps.Abp;
using EasyAbp.AbpHelper.Core.Steps.Baan;
using EasyAbp.AbpHelper.Core.Steps.Common;
using EasyAbp.AbpHelper.Core.Workflow;
using EasyAbp.AbpHelper.Core.Workflow.Generate;
using EasyAbp.AbpHelper.Core.Workflow.Generate.Crud;
using Elsa;
using Elsa.Activities;
using Elsa.Activities.ControlFlow.Activities;
using Elsa.Expressions;
using Elsa.Scripting.JavaScript;
using Elsa.Services;
using System;
using System.Linq;

namespace EasyAbp.AbpHelper.Core.Commands.BaanGenerate.Crud
{
    public class BaanCrudCommand : CommandWithOption<BaanCrudCommandOption>
    {
        private const string DbMigrations = "DbMigrations";
        private const string TestGeneration = "TestGeneration";

        public BaanCrudCommand(IServiceProvider serviceProvider)
            : base(serviceProvider, "crud", "Generate a set of CRUD related files according to the specified entity 【Baan Projection】")
        {
        }

        protected override IActivityBuilder ConfigureBuild(BaanCrudCommandOption option,
          IActivityBuilder activityBuilder)
        {
            var entityFileName = option.Entity + ".cs";

            return base.ConfigureBuild(option, activityBuilder)
                .AddOverwriteWorkflow()
                .Then<SetVariable>(
                    step =>
                    {
                        step.VariableName = VariableNames.TemplateDirectory;
                        step.ValueExpression = new LiteralExpression<string>("/Templates/Baan/Crud");
                    })
                .Then<FileFinderStep>(
                    step =>
                    {
                        step.SearchFileName = new LiteralExpression(entityFileName);
                    })
                .Then<EntityParserStep>()
                .Then<BuildBaanDtoInfoStep>()
                .Then<SetModelVariableStep>()
                .Then<IfElse>(
                    step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(BaanCrudCommandOption.SkipEntityConstructors)}"),
                    ifElse =>
                    {
                        ifElse.When(OutcomeNames.False)
                            .AddEntityConstructorsGenerationWorkflow()
                            .Then("EntityUsing")
                            ;
                        ifElse.When(OutcomeNames.True)
                            .Then("EntityUsing")
                            ;
                    })
                .AddEntityUsingGenerationWorkflow("EntityUsing")
                //.AddEfCoreConfigurationWorkflow()
                //.Then<IfElse>(
                //    step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(BaanCrudCommandOption.SkipCustomRepository)}"),
                //    ifElse =>
                //    {
                //        ifElse
                //            .When(OutcomeNames.False)
                //            //.AddCustomRepositoryGeneration()
                //            .Then("ServiceGeneration")
                //            ;
                //        ifElse
                //            .When(OutcomeNames.True)
                //            .AddCustomRepositoryGeneration()
                //            .Then("ServiceGeneration")
                //            ;
                //    }
                //)
                .AddServiceGenerationWorkflow("ServiceGeneration")
                .Then<IfElse>(
                    step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(BaanCrudCommandOption.SkipLocalization)}"),
                    ifElse =>
                    {
                        ifElse.When(OutcomeNames.False)
                            .Then(ActivityNames.Ui)
                            ;
                        ifElse.When(OutcomeNames.True)
                            .AddLocalizationGenerationWorkflow()
                            .Then(ActivityNames.Ui)
                            ;
                    })
                .Then<IfElse>(
                    step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(BaanCrudCommandOption.SkipUi)}"),
                    ifElse =>
                    {
                        ifElse
                            .When(OutcomeNames.False)
                            .Then<Switch>(
                                @switch =>
                                {
                                    @switch.Expression = new JavaScriptExpression<string>("(ProjectInfo.UiFramework)");
                                    @switch.Cases = Enum.GetValues(typeof(UiFramework)).Cast<int>().Select(u => u.ToString()).ToArray();
                                },
                                @switch =>
                                {
                                    @switch.When(UiFramework.None.ToString("D"))
                                        .Then(TestGeneration);

                                    @switch.When(UiFramework.Antd.ToString("D"))
                                        .Then(TestGeneration);

                                    @switch.When(UiFramework.RazorPages.ToString("D"))
                                        .AddUiRazorPagesGenerationWorkflow()
                                        .Then(TestGeneration);

                                    @switch.When(UiFramework.Angular.ToString("D"))
                                        // TODO
                                        //.AddUiAngularGenerationWorkflow()
                                        .Then(TestGeneration);
                                }
                            )
                            ;
                        ifElse
                            .When(OutcomeNames.True)
                            .Then(TestGeneration)
                            ;
                    }
                ).WithName(ActivityNames.Ui)
                .Then<IfElse>(
                    step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(BaanCrudCommandOption.SkipTest)}"),
                    ifElse =>
                    {
                        ifElse
                            .When(OutcomeNames.False)
                            .AddTestGenerationWorkflow()
                            //.Then(DbMigrations)
                            ;
                        //ifElse
                        //    .When(OutcomeNames.True)
                        //    .Then(DbMigrations)
                        //    ;
                    }
                ).WithName(TestGeneration)
                ;

        }

        //protected override IActivityBuilder ConfigureBuild(BaanCrudCommandOption option, IActivityBuilder activityBuilder)
        //{
        //    var entityFileName = option.Entity + ".cs";

        //    return base.ConfigureBuild(option, activityBuilder)
        //        .AddOverwriteWorkflow()
        //        .Then<SetVariable>(
        //            step =>
        //            {
        //                step.VariableName = VariableNames.TemplateDirectory;
        //                step.ValueExpression = new LiteralExpression<string>("/Templates/Crud");
        //            })
        //        .Then<FileFinderStep>(
        //            step => { step.SearchFileName = new LiteralExpression(entityFileName); })
        //        .Then<EntityParserStep>()
        //        .Then<BuildDtoInfoStep>()
        //        .Then<SetModelVariableStep>()
        //        .Then<IfElse>(
        //            step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(CrudCommandOption.SkipEntityConstructors)}"),
        //            ifElse =>
        //            {
        //                ifElse.When(OutcomeNames.False)
        //                    .AddEntityConstructorsGenerationWorkflow()
        //                    .Then("EntityUsing")
        //                    ;
        //                ifElse.When(OutcomeNames.True)
        //                    .Then("EntityUsing")
        //                    ;
        //            })
        //        .AddEntityUsingGenerationWorkflow("EntityUsing")
        //        .AddEfCoreConfigurationWorkflow()
        //        .Then<IfElse>(
        //            step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(CrudCommandOption.SkipCustomRepository)}"),
        //            ifElse =>
        //            {
        //                ifElse
        //                    .When(OutcomeNames.False)
        //                    .AddCustomRepositoryGeneration()
        //                    .Then("ServiceGeneration")
        //                    ;
        //                ifElse
        //                    .When(OutcomeNames.True)
        //                    .Then("ServiceGeneration")
        //                    ;
        //            }
        //        )
        //        .AddServiceGenerationWorkflow("ServiceGeneration")
        //        .Then<IfElse>(
        //            step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(CrudCommandOption.SkipLocalization)}"),
        //            ifElse =>
        //            {
        //                ifElse.When(OutcomeNames.False)
        //                    .AddLocalizationGenerationWorkflow()
        //                    .Then(ActivityNames.Ui)
        //                    ;
        //                ifElse.When(OutcomeNames.True)
        //                    .Then(ActivityNames.Ui)
        //                    ;
        //            })
        //        .Then<IfElse>(
        //            step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(CrudCommandOption.SkipUi)}"),
        //            ifElse =>
        //            {
        //                ifElse
        //                    .When(OutcomeNames.False)
        //                    .Then<Switch>(
        //                        @switch =>
        //                        {
        //                            @switch.Expression = new JavaScriptExpression<string>("(ProjectInfo.UiFramework)");
        //                            @switch.Cases = Enum.GetValues(typeof(UiFramework)).Cast<int>().Select(u => u.ToString()).ToArray();
        //                        },
        //                        @switch =>
        //                        {
        //                            @switch.When(UiFramework.None.ToString("D"))
        //                                .Then(TestGeneration);

        //                            @switch.When(UiFramework.RazorPages.ToString("D"))
        //                                .AddUiRazorPagesGenerationWorkflow()
        //                                .Then(TestGeneration);

        //                            @switch.When(UiFramework.Angular.ToString("D"))
        //                                // TODO
        //                                //.AddUiAngularGenerationWorkflow()
        //                                .Then(TestGeneration);
        //                        }
        //                    )
        //                    ;
        //                ifElse
        //                    .When(OutcomeNames.True)
        //                    .Then(TestGeneration)
        //                    ;
        //            }
        //        ).WithName(ActivityNames.Ui)
        //        .Then<IfElse>(
        //            step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(CrudCommandOption.SkipTest)}"),
        //            ifElse =>
        //            {
        //                ifElse
        //                    .When(OutcomeNames.False)
        //                    .AddTestGenerationWorkflow()
        //                    .Then(DbMigrations)
        //                    ;
        //                ifElse
        //                    .When(OutcomeNames.True)
        //                    .Then(DbMigrations)
        //                    ;
        //            }
        //        ).WithName(TestGeneration)
        //        .Then<IfElse>(
        //            step => step.ConditionExpression = new JavaScriptExpression<bool>($"{OptionVariableName}.{nameof(CrudCommandOption.SkipDbMigrations)}"),
        //            ifElse =>
        //            {
        //                ifElse
        //                    .When(OutcomeNames.False)
        //                    .AddMigrationAndUpdateDatabaseWorkflow()
        //                    ;
        //            }
        //        ).WithName(DbMigrations);
        //}
    }
}
