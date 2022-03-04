using EasyAbp.AbpHelper.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyAbp.AbpHelper.Core.Commands.Generate.Antd
{
    public class TryCommandOption : GenerateCommandOption
    {
        [Option('f', "file-name", Description = "The name of empty file")]
        public string FileName { get; set; } = null!;
    }
}
