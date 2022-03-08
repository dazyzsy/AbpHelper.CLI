{{-
if Option.SkipCustomRepository
    if EntityInfo.CompositeKeyName
        repositoryType = "IRepository<" + EntityInfo.Name + ">"
    else
        repositoryType = "IRepository<" + EntityInfo.Name + ", " + EntityInfo.PrimaryKey + ">"
    end
    repositoryName = "Repository"
else
    repositoryType = "IRepository<" + EntityInfo.Name + ", " + EntityInfo.PrimaryKey + ">"
    repositoryName = "_repository"
end ~}}

using System;
using System.Linq;
using System.Threading.Tasks;
using {{ EntityInfo.Namespace }}.Dtos;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using Abp.EntityFrameworkCore.Repositories;
using BoneTotal.Contract.Base.BaseDto;
using BaanApiNext.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BoneTotal.Contract.{{ EntityInfo.Name }}.ListDto;
using BaanApiNext.Common.StringUtil;

namespace {{ EntityInfo.Namespace }}
{
    public class {{ EntityInfo.Name }}AppService: BaanApiNextAppServiceBase, I{{ EntityInfo.Name }}AppService
{
        private readonly IDbContextProvider<BaanApiNextDbContext> _dbContextProvider;
        private readonly {{ repositoryType }} {{ repositoryName }};
        
        public {{ EntityInfo.Name }}AppService(
            IDbContextProvider<BaanApiNextDbContext> dbContextProvider,
            {{ repositoryType }} repository
        )
        {
            _dbContextProvider = dbContextProvider;
            {{ repositoryName }} = repository;
        }

        public async Task<ListResultDto<{{ EntityInfo.Name }}ListDto>> GetListAsync(Get{{ EntityInfo.NamePluralized }}Input input)
        {
            var boneQueryInput = ObjectMapper.Map<BoneQueryInput>(input);
            var sql = {{ EntityInfo.Name }}Util.GetListSql(boneQueryInput);
            var query = _dbContextProvider.GetDbContext().{{ EntityInfo.Name }}ListDtos.FromSqlRaw(sql).AsNoTracking();

            var dtos = await query.OrderBy(input.Sorting)
                .ToListAsync();

            return new ListResultDto<{{ EntityInfo.Name }}ListDto>
            {
                Items = dtos
            };
        }

        public async Task<PagedResultDto<{{ EntityInfo.Name }}ListDto>> GetPagedListAsync(Get{{ EntityInfo.NamePluralized }}Input input)
        {
            var boneQueryInput = ObjectMapper.Map<BoneQueryInput>(input);
            var sql = {{ EntityInfo.Name }}Util.GetListSql(boneQueryInput);
            var query = _dbContextProvider.GetDbContext().{{ EntityInfo.Name }}ListDtos.FromSqlRaw(sql).AsNoTracking();

            var totalCount = await query.CountAsync();
            var dtos = await query.OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            return new PagedResultDto<{{ EntityInfo.Name }}ListDto>
            {
                TotalCount = totalCount,
                Items = dtos
            };

        }

        public async Task<{{ EntityInfo.Name }}ListDto> GetEntityByIdAsync(string id)
        {
            {{ EntityInfo.Name }} entity = await {{ repositoryName }}.GetAsync(id);
            var dto = ObjectMapper.Map<{{ EntityInfo.Name }}ListDto>(entity);
            return dto;
        }

        public async Task<{{ EntityInfo.Name }}Dto> GetForEdit({{ EntityInfo.PrimaryKey }} id)
        {
            {{ EntityInfo.Name }} entity = await {{ repositoryName }}.GetAsync(id);
            var dto = ObjectMapper.Map<{{ EntityInfo.Name }}Dto>(entity);
            return dto;
        }

        public async Task<{{ EntityInfo.PrimaryKey }}> CreateOrUpdateAsync({{ EntityInfo.Name }}Dto dto)
        {
            var entity = ObjectMapper.Map<{{ EntityInfo.Name }}>(dto);

            if (string.IsNullOrEmpty(entity.Id))
            {
                entity.Id = StringHelper.GenerateId();

                var id = await _repository.InsertAndGetIdAsync(entity);

                return id;
            }

            var data = await _repository.UpdateAsync(entity);
            return data.Id;
        }
      
        public async Task DeleteByIdAsync({{ EntityInfo.PrimaryKey }} id)
        {
            await {{ repositoryName }}.DeleteAsync(id);
        }
       
    }
}
