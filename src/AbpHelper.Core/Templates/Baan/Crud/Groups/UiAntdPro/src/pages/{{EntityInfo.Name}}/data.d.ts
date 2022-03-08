import { BonePagedAndSortedResultRequestDto, BooleanEnum, BoneEntityDto, BoneCuDto } from '@/models/bone';

export interface {{ EntityInfo.Name }}ListDto extends BoneEntityDto<{{ EntityInfo.PrimaryKey }}> {
    {{~ for prop in EntityInfo.Properties ~}}
    {{ prop.Name | abp.camel_case }}: {{ prop.Type | abp.ts_type }};
    {{~ end ~}}
}

export interface Get{{ EntityInfo.NamePluralized }}Input extends BonePagedAndSortedResultRequestDto {
    {{~ for prop in EntityInfo.Properties ~}}
    {{ prop.Name | abp.camel_case }}?: {{ prop.Type | abp.ts_type }};
    {{~ end ~}}
}

export interface {{ EntityInfo.Name }}Dto extends BoneCuDto<{{ EntityInfo.PrimaryKey }}> {
    {{~ for prop in EntityInfo.Properties ~}}
    {{ prop.Name | abp.camel_case }}: {{ prop.Type | abp.ts_type }};
    {{~ end ~}}
}