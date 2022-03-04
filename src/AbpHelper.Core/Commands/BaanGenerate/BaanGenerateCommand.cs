using EasyAbp.AbpHelper.Core.Commands.BaanGenerate.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAbp.AbpHelper.Core.Commands.BaanGenerate
{

    public class BaanGenerateCommand : CommandBase
    {
        public BaanGenerateCommand(IServiceProvider serviceProvider) : base(serviceProvider, "baangenerate", "Generate files for Baan projects. See 'abphelper baangenerate --help' for details")
        {
            AddAlias("bgen");

            AddCommand<BaanCrudCommand>();
            //AddCommand<ServiceCommand>();
            //AddCommand<MethodsCommand>();
            //AddCommand<LocalizationCommand>();
            //AddCommand<ControllerCommand>();
            //AddCommand<TryCommand>();
        }
    }
}
