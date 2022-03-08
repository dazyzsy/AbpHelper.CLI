using System;
{{~ if !Option.SkipLocalization }}using System.ComponentModel;{{ end ~}}
using BoneTotal.Contract.Base.BaseDto;
using BoneTotal.Contract.Base.BaanEnum;

namespace {{ EntityInfo.Namespace }}.Dtos
{
    [Serializable]
    public class {{ DtoInfo.CreateTypeName }} : BoneEntityDto<{{ EntityInfo.PrimaryKey }}>
    {
        {{~ for prop in EntityInfo.Properties ~}}
        {{~ if prop | abp.is_ignore_property; continue; end ~}}
        {{~ if !Option.SkipLocalization && Option.SkipViewModel ~}}
        [DisplayName("{{ EntityInfo.Name + prop.Name}}")]
        {{~ end ~}}
        public {{ prop.Type}} {{ prop.Name }} { get; set; }
        {{~ if !for.last ~}}

        {{~ end ~}}
        {{~ end ~}}
    }
}