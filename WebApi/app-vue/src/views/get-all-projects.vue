<template>
    <DxDataGrid
            :data-source="dataSource"
            :show-borders="true"
            :remote-operations="true"
            key-expr="ID"
            @row-click="onRowClick"
    >
        <DxColumn
                data-field="projectNumber"
                caption="Номер проекта"
                data-type="string"
        />
        <DxColumn
                data-field="contrAgent.companyName"
                caption="Контрагент"
                data-type="string"
        />
        <DxColumn
                data-field="executor.name"
                caption="Управляющий специалист"
                data-type="string"
        />
        <DxColumn
                data-field="executor.executiveCompany."
                caption="Управляющая компания"
                data-type="string"
        />
        <DxColumn
                data-field="projectStatus"
                caption="Стадия проекта"
                data-type="string"
        />
        <DxColumn
                data-field="address"
                caption="Адрес"
                data-type="string"
        />
        <DxPaging :page-size="5"/>
        <DxPager
                :show-page-size-selector="true"
                :allowed-page-sizes="[8, 12, 20]"
        />
        <DxEditing
                :allow-deleting="true"
        />
    </DxDataGrid>
    <DxButton
            class="button"
            text="Создать"
            type="success"
            :use-submit-behavior="true"
            :on-click="onCreateProjectClick"
    />
</template>
<script setup>
import {
    DxDataGrid,
    DxColumn,
    DxPager,
    DxPaging,
    DxEditing,
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