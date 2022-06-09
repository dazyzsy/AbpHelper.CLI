﻿using EasyAbp.AbpHelper.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAbp.AbpHelper.Core.Commands.BaanGenerate
{
    public class BaanGenerateCommandOption : CommandOptionsBase
    {
        protected virtual string OverwriteVariableName => CommandConsts.OverwriteVariableName;

        [Option("no-overwrite", Description = "Specify not to overwrite existing files or content")]
        public bool NoOverwrite { get; set; }
    }
}