using System;
using {{ EntityInfo.Namespace }}.Dtos;
using BoneTotal.Contract.Base.BaseDto;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using BoneTotal.Contract.{{ EntityInfo.Name }}.ListDto;

namespace {{ EntityInfo.Namespace }}
{
    public interface I{{ EntityInfo.Name }}AppService : IApplicationService
    {
        Task<PagedResultDto<{{ EntityInfo.Name }}ListDto>> GetPagedListAsync(Get{{ EntityInfo.NamePluralized }}Input input);

        Task<ListResultDto<{{ EntityInfo.Name }}ListDto>> GetListAsync(Get{{ EntityInfo.NamePluralized }}Input input);

        Task<{{ EntityInfo.Name }}ListDto> GetEntityByIdAsync({{ EntityInfo.PrimaryKey }} id);

        Task<{{ EntityInfo.Name }}Dto> GetForEdit({{ EntityInfo.PrimaryKey }} id);

        Task<string> CreateOrUpdateAsync({{ EntityInfo.Name }}Dto dto);

        Task DeleteByIdAsync({{ EntityInfo.PrimaryKey }} id);

    }
}