import { AjaxResponse, PagedResultDto } from '@/models/abp';
import request from '@/utils/request';
import { CancelToken } from 'umi-request';
import {
    Get{{ EntityInfo.NamePluralized }}Input,
    {{ EntityInfo.Name }}ListDto,
    {{ EntityInfo.Name }}Dto,
} from './data';

export async function getPaged{{ EntityInfo.NamePluralized }}(
    params: Get{{ EntityInfo.NamePluralized }}Input,
    cancelToken?: CancelToken,
): Promise<AjaxResponse<PagedResultDto<{{ EntityInfo.Name }}ListDto>>> {
    return request('/baan/api/services/app/{{ EntityInfo.Name }}/GetPagedListAsync', {
        params,
        cancelToken,
    });
}

export async function getList(
    params: Get{{ EntityInfo.NamePluralized }}Input,
    cancelToken?: CancelToken,
): Promise<AjaxResponse<{{ EntityInfo.Name }}ListDto[]>> {
    return request('/baan/api/services/app/{{ EntityInfo.Name }}/GetListAsync', {
        params,
        cancelToken,
    });
}

export async function getForEdit(id: {{ EntityInfo.PrimaryKey | abp.ts_type }}): Promise<AjaxResponse<{{ EntityInfo.Name }}Dto>> {
    return request('/baan/api/services/app/{{ EntityInfo.Name }}/GetForEdit', {
        params: { id },
    });
}

export async function createOrUpdate(data: {{ EntityInfo.Name }}Dto): Promise<AjaxResponse<{{ EntityInfo.PrimaryKey | abp.ts_type }}>> {
    return request(`/baan/api/services/app/{{ EntityInfo.Name }}/CreateOrUpdateAsync`, {
        method: 'post',
        data,
    });
}


export async function getEntityById(
    id: string,
    cancelToken?: CancelToken,
): Promise<AjaxResponse<{{ EntityInfo.Name }}ListDto>> {
    return request('/baan/api/services/app/{{ EntityInfo.Name }}/GetEntityByIdAsync', {
        params: { id },
        cancelToken,
    });
}

export async function deleteById(id: string) {
    return request('/baan/api/services/app/{{ EntityInfo.Name }}/DeleteByIdAsync', {
        method: 'DELETE',
        params: { id },
    });
}
