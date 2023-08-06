<template>
  <DxDataGrid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @row-click="onRowClick"
  >
    <DxColumn
        data-field="companyName"
        caption="Название компании"
        data-type="string"
    />
    <DxColumn
        data-field="bin"
        caption="БИН"
        data-type="string"
    />
    <DxColumn
        data-field="directorName"
        caption="Имя директора"
        data-type="string"
    />
    <DxColumn
        data-field="directorSurname"
        caption="Фамилия директора"
        data-type="string"
    />
    <DxColumn
        data-field="directorPatronymic"
        caption="Отчество директора"
        data-type="string"
    />
    <DxColumn
        data-field="amplificationFactor"
        caption="Коэффициент усиления"
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
import contrAgentService from "@/api/contrAgentService";
import { useRouter } from 'vue-router';


const router = useRouter();
const dataSource = new CustomStore({
  key: 'id',

  load: async (loadOptions) => {
    return await contrAgentService.getAllContrAgents(loadOptions);
  },
  remove: async (oid) => {
    const baseResponse = await contrAgentService.deleteContrAgent(oid);
    return {data: baseResponse.result};
  },
});
async function onRowClick(e) {
  try {
    const contrAgentId = e.key;
    await router.push({name: 'contrAgentDetail', params: {mode: "read", id: contrAgentId}});
  } catch (error) {
    console.log(error)
  }
}
</script>

<style>
.dx-datagrid .dx-row:hover {
  background-color: #f2f2f2;
  cursor: pointer;
}
</style>