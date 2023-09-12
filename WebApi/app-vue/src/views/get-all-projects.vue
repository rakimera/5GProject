<template>
    <div class="row justify-content-center mt-5">
    <div class="col-11 ">
        <dx-data-grid
            :data-source="dataSource"
            :show-borders="true"
            :remote-operations="false"
            :columnAutoWidth="true"    
            :allowColumnResizing="true"
            key-expr="ID"
            @row-click="onRowClick"
        >
            <dx-search-panel
                :visible="true"
                placeholder="Поиск"
                width= 250
            />
            <dx-column
                data-field="projectNumber"
                caption="Номер проекта"
                data-type="string"
            />
            <dx-column
                data-field="contrAgent.companyName"
                caption="Контрагент"
                data-type="string"
            />
            <dx-column
                data-field="executor.name"
                caption="Управляющий специалист"
                data-type="string"
            />
            <dx-column
                data-field="executiveCompany.companyName"
                caption="Управляющая компания"
                data-type="string"
            />
            <dx-column
                data-field="projectStatus.status"
                caption="Стадия проекта"
                data-type="string"
            >
            </dx-column>
            <dx-column
                data-field="districtName"
                caption="Область"
                data-type="string"
            />
            <dx-column
                data-field="townName"
                caption="Город"
                data-type="string"
            />
            <dx-column
                data-field="address"
                caption="Адрес"
                data-type="string"
            />
            <dx-paging :page-size="5"/>
            <dx-pager
                :show-page-size-selector="true"
                :allowed-page-sizes="[8, 12, 20]"
            />
            <dx-editing
                :allow-deleting="true"
                :texts="{confirmDeleteMessage: 'Вы уверены, что хотите удалить эту запись?'}"
            />
            <dx-header-filter :visible="true"/>
        </dx-data-grid>
        <dx-button
            class="button mt-3"
            text="Создать"
            type="success"
            :use-submit-behavior="true"
            :on-click="onCreateProjectClick"
        />
    </div>
    </div>
</template>
<script setup>
import {
    DxDataGrid,
    DxColumn,
    DxPager,
    DxPaging,
    DxEditing,
    DxHeaderFilter, DxSearchPanel
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import projectService from "@/api/projectService";
import { useRouter } from 'vue-router';
import {DxButton} from "devextreme-vue/button";


const router = useRouter();
const dataSource = new CustomStore({
    key: 'id',

    load: async (loadOptions) => {
        const input = await projectService.getProjectsForGrid(loadOptions);
        console.log(input)
        return input
    },
    remove: async (oid) => {
        const baseResponse = await projectService.deleteProject(oid);
        return {data: baseResponse.result};
    },
});
async function onRowClick(e) {
    try {
        const projectId = e.key;
        await router.push({name: 'projectDetail', params: {mode: "read", id: projectId}});
    } catch (error) {
        console.log(error)
    }
}

const onCreateProjectClick = async () => {
    try {
        await router.push({name: 'projectDetail', params: {mode: "create", id: null}});
    } catch (error) {
        console.log(error);
    }
};

</script>
<style>
.dx-datagrid .dx-row:hover {
    background-color: #f2f2f2;
    cursor: pointer;
}
</style>