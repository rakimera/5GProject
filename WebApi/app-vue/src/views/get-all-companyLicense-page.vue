<template>
  <dx-data-grid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="id"
  >
    <dx-column data-field="number" data-type="string" caption="Номер лицензии">
      <dx-required-rule message="Пожалуйста, укажите номер лицензии"/>
    </dx-column>
    <dx-column data-field="dateOfIssue" data-type="datetime" caption="Дата присвоения">
      <dx-required-rule message="Пожалуйста, укажите дату получение лицензии"/>
    </dx-column>
    <dx-paging :page-size="5"/>
    <dx-pager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
    <dx-editing
        :allow-updating="true"
        :allow-deleting="true"
        :allow-adding="true"
        mode="row"
        :texts="{confirmDeleteMessage: 'Вы уверены, что хотите удалить эту запись?'}"
    />
  </dx-data-grid>
</template>

<script setup>
import {ref, onMounted} from "vue";
import {
  DxDataGrid,
  DxColumn,
  DxPaging,
  DxPager,
  DxEditing,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import companyLicenseService from "@/api/companyLicenseService";
import {DxRequiredRule} from "devextreme-vue/form";

const dataSource = ref(null);

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    const response = await companyLicenseService.getAllLicenses(loadOptions);
    return response.data;
  },
  async insert(values) {
    const baseResponse = await companyLicenseService.createLicense(values);
    await dataSource.value.load();
    return {data: baseResponse};
  },
  async remove(oid) {
    const baseResponse = await companyLicenseService.deleteLicense(oid);
    return {data: baseResponse};
  },
  async update(oid, values) {
    const updateCompanyLicenseDto = {
      id: oid,
      number: values.number,
      dateOfIssue: values.dateOfIssue
    };

    console.log(updateCompanyLicenseDto)
    const baseResponse = await companyLicenseService.updateLicense(updateCompanyLicenseDto);
    await dataSource.value.load();
    return {data: baseResponse};
  },
});

onMounted(() => {
  dataSource.value = store;
});
</script>

<style>
.dx-datagrid .dx-row:hover {
  background-color: #f2f2f2;
  cursor: pointer;
}
</style>
