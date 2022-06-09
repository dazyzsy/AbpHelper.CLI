using EasyAbp.AbpHelper.Core.Generator;
using EasyAbp.AbpHelper.Core.Models;
using EasyAbp.AbpHelper.Core.Steps.Abp.ModificationCreatorSteps.Typescript;
using EasyAbp.AbpHelper.Core.Workflow;
using Elsa.Services.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAbp.AbpHelper.Core.Steps.Baan.AntdPro
{
    public class AddRoutesInConfigStep : TypeScriptModificationCreatorStep
    {


        public AddRoutesInConfigStep([NotNull] TextGenerator textGenerator) : base(textGenerator)
        {
        }

        protected override IList<ModificationBuilder<IEnumerable<LineNode>>> CreateModifications(
            WorkflowExecutionContext context)
        {
            var model = context.GetVariable<object>("Model");
            string templateDir = context.GetVariable<string>(VariableNames.TemplateDirectory);
            //string importContents = TextGenerator.GenerateByTemplateName(templateDir, "RoutingModule_ImportList", model);
            string moduleContents = TextGenerator.GenerateByTemplateName(templateDir, "Config_Routes", model);

            int LineExpression(IEnumerable<LineNode> lines) => lines.Last(l => l.IsMath($"component: './Welcome',")).LineNumber + 2;

            return new List<ModificationBuilder<IEnumerable<LineNode>>>
            {
                new InsertionBuilder<IEnumerable<LineNode>>(
                    lines => lines.Last(l => l.LineContent.Contains("component: './Welcome',")).LineNumber,
                    moduleContents,
                    InsertPosition.After
                    //lines => lines.Where(l => l.IsMath("^import")).All(l => !l.LineContent.Contains(importContents))
                ),
                //new ReplacementBuilder<IEnumerable<LineNode>>(
                //    LineExpression,
                //    LineExpression,
                //    moduleContents
                //)
            };
        }
    }
}
