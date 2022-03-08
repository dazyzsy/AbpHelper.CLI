import React, { useRef, useState } from 'react';
import { Button, message, } from 'antd';
import { GridContent, PageContainer } from '@ant-design/pro-layout';
import ProTable, { ActionType, ProColumns } from '@ant-design/pro-table';
import moment from 'moment';
import { l } from '@/utils/local';
import { SorterResult } from 'antd/lib/table/interface';
import {
    {{ EntityInfo.Name }}ListDto,
} from './data';
import { getPaged{{ EntityInfo.Name }}s } from './service';

const {{ EntityInfo.Name }}ListPage: React.FC<any> = () => {
    const financeCompanyNum = parseInt(localStorage.getItem('selectFinCompany') || '0');
    const logisticCompanyNum = parseInt(localStorage.getItem('selectLogCompany') || '0');
    const languageCode = localStorage.getItem('umi_locale') === 'zh-CN' ? 'o' : '2';
    const erpCode = localStorage.getItem('clientErpCode') ?? '';
    const [sorting, setSorting] = useState<string | undefined>(undefined);
    const [selectItem, setSelectItem] = useState({} as any);
    const [submitting, setSubmitting] = useState<boolean>(false);

    const actionRef = useRef<ActionType>();

    const columns: ProColumns<{{ EntityInfo.Name}}ListDto>[] = [
            {{- for prop in EntityInfo.Properties -}}
            {{- if for.first; "\r\n"; else; "\r\n"; end ~}}
        {
            title: l('{{ prop.Name }}'),
            dataIndex: '{{ prop.Name | abp.camel_case }}',
            width: 150,
            {{~ if prop | abp.is_time }}            hideInSearch:true,
            renderText: (text) => {
                return moment(text).format('YYYY-MM-DD HH:mm:ss');
            },{{ end ~}}

        },
            {{- if for.last; "\r\n"; else; ; end -}}
            {{- end ~}}
    ]

    return (
        <PageContainer>
            <GridContent>
                <ProTable<{{ EntityInfo.Name}}ListDto>
                    actionRef={actionRef || undefined}
                    // rowKey={(e) => `${e.logisticCompanyNum}${e.financeCompanyNum}${e.orderNo}`}
                    columns={columns}
                    size='small'
                    search={%{{{
                    labelWidth: 'auto'
                    }}}%}
                    scroll={%{{{ x: 1300 }}}%}
                    toolBarRender={() => [
                    <Button type="default" loading={submitting}
                         //   onClick={() => handleConfirmTransferOrder(selectItem)}

                        >
                            {`${l('Confirm')} ${l('PutAway')}`}
                        </Button>
                    ]}
                    rowSelection={%{{{
                        hideSelectAll: true,
                        type: 'radio',
                        onChange: (_selectedRowKeys, selectedRows) => {
                                setSelectItem(selectedRows ? selectedRows[0] : {});
                            }
                     }}}%}
                    params={%{{{
                        sorting
                    }}}%}
                    onChange={(_, _filter, _sorter) => {
                    const sorterResult = _sorter as SorterResult<{{ EntityInfo.Name}}ListDto>;
                        if (sorterResult.order) {
                            setSorting(`${sorterResult.field} ${sorterResult.order === 'ascend' ? `ASC` : `DESC`}`);
                        } else {
                            setSorting(undefined)
                        }
                    }}
                    request={async (params: any) => {

                        try {
                        const res = await getPaged{{ EntityInfo.Name}}s({
                                ...params,
                                languageCode,
                                financeCompanyNum,
                                logisticCompanyNum,
                                skipCount: (params?.pageSize || 20) * ((params?.current || 1) - 1),
                                maxResultCount: params?.pageSize || 20
                            });
                            return {
                                data: res.result.items,
                                success: res.success,
                                total: res.result.totalCount
                            };

                        } catch (err) {
                            return {
                                data: [],
                                success: false
                            };
                        }
                    }}
                />
            </GridContent>
        </PageContainer>
    )
}

export default {{ EntityInfo.Name }}ListPage;