using Abp.Extensions;
using Abp.Runtime.Validation;
using BoneTotal.Contract.Base.BaseDto;

namespace {{ EntityInfo.Namespace }}.Dtos
{
    public class Get{{ EntityInfo.NamePluralized }}Input: BonePagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public void Normalize()
        {
           // if (Sorting.IsNullOrEmpty()) Sorting = "Id DESC";
        }
    }
}