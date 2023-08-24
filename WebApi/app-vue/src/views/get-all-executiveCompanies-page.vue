<template>
  <dx-data-grid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @row-click="onRowClick"
  >
    <dx-column
        data-field="companyName"
        caption="Название компании"
        data-type="string"
    />
    <dx-column
        data-field="address"
        caption="Адрес компании"
        data-type="string"
    />
    <dx-column
        data-field="licenseNumber"
        caption="Номер лицензии"
        data-type="string"
    />
    <dx-column
        data-field="licenseDateOfIssue"
        caption="Дата лицензии"
        data-type="date"
    />
    <dx-column
        data-field="bin"
        caption="БИН компании"
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
  </dx-data-grid>
  <dx-button
      class="button"
      text="Создать"
      type="success"
      :use-submit-behavior="true"
      :on-click="onCreateExecutiveCompanyClick"
  />
</template>

<script setup>
import {
  DxDataGrid,
  DxColumn,
  DxPager,
  DxPaging,
  DxEditing
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import executiveCompanyService from "@/api/executiveCompanyService";
import {useRouter} from 'vue-router';
import {DxButton} from "devextreme-vue/button";


const router = useRouter();
const dataSource = new CustomStore({
  key: 'id',

  load: async (loadOptions) => {
    console.log("loadOptions")
    console.log(loadOptions)
    return await executiveCompanyService.getAllExecutiveCompanies(loadOptions);
  },
  remove: async (oid) => {
    const baseResponse = await executiveCompanyService.deleteExecutiveCompany(oid);
    return {data: baseResponse.result};
  },
});

async function onRowClick(e) {
  try {
    const executiveCompanyId = e.key;
    console.log(executiveCompanyId)
    await router.push({name: 'executiveCompaniesDetail', params: {mode: "read", id: executiveCompanyId}});
  } catch (error) {
    console.log(error)
  }
}

const onCreateExecutiveCompanyClick = async () => {
  try {
    await router.push({name: 'executiveCompaniesDetail', params: {mode: "create", id: null}});
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