using EasyAbp.AbpHelper.Core.Commands.BaanGenerate.Crud;
using EasyAbp.AbpHelper.Core.Models;
using EasyAbp.AbpHelper.Core.Steps.Abp;
using Elsa.Results;
using Elsa.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAbp.AbpHelper.Core.Steps.Baan
{
    public class BuildBaanDtoInfoStep : Step
    {
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            var entityInfo = context.GetVariable<EntityInfo>("EntityInfo");
            var option = context.GetVariable<object>("Option") as BaanCrudCommandOption;

            try
            {
                string[] actionNames = { "List", string.Empty, string.Empty };

                if (option != null && option.SeparateDto)
                {
                    actionNames[1] = "Create";
                    actionNames[2] = "Update";
                }
                else
                {
                    actionNames[1] = "";
                    // actionNames[1] = "CreateUpdate";
                    actionNames[2] = actionNames[1];
                }

                string[] typeNames = new string[actionNames.Length];

                var useEntityPrefix = option != null && option.EntityPrefixDto;
                var dtoSubfix = option?.DtoSuffix ?? "Dto";

                for (int i = 0; i < typeNames.Length; i++)
                {
                    typeNames[i] = !useEntityPrefix
                        ? $"{entityInfo.Name}{actionNames[i]}{dtoSubfix}"
                        : $"{actionNames[i]}{entityInfo.Name}{dtoSubfix}";
                }

                DtoInfo dtoInfo = new DtoInfo(typeNames[0], typeNames[1], typeNames[2]);

                context.SetLastResult(dtoInfo);
                context.SetVariable("DtoInfo", dtoInfo);
                LogOutput(() => dtoInfo);

                return Done();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Building DTO info failed.");
                if (e is ParseException pe)
                    foreach (var error in pe.Errors)
                        Logger.LogError(error);
                throw;
            }
        }
    }
}
