<template>
  <dx-data-grid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="id"
  >
    <dx-column data-field="roleName" data-type="string" caption="Роль"/>
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
import roleService from "@/api/roleService";
import notify from "devextreme/ui/notify";

const dataSource = ref(null);

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    const response = await roleService.getAllRoles(loadOptions);
    return response.data;
  },
  async insert(values) {
    if (values.roleName.length <= 2) {
      notify({
        message: 'Имя роли должно содержать более двух символов',
        position: {
          my: 'center top',
          at: 'center top',
        },
      }, 'error', 2000);
      return {error: "Имя роли должно содержать более двух символов."};
    }
    const baseResponse = await roleService.createRole(values);
    await dataSource.value.load();
    return {data: baseResponse};
  },
  async remove(oid) {
    const baseResponse = await roleService.deleteRole(oid);
    return {data: baseResponse};
  },
  async update(oid, values) {
    const updateRoleDto = {
      id: oid,
      roleName: values.roleName
    };

    console.log(updateRoleDto)
    const baseResponse = await roleService.updateRole(updateRoleDto);
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
