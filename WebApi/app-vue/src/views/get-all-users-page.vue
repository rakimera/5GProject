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
        <dx-column data-field="login" data-type="string" caption="Логин"/>
        <dx-column data-field="name" data-type="string" caption="Имя"/>
        <dx-column data-field="surname" data-type="string" caption="Фамилия"/>
        <dx-column data-field="patronymic" data-type="string" caption="Отчетсво"/>
        <dx-paging :page-size="5"/>
        <dx-pager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
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
          :on-click="onCreateUserClick"
        />
    </div>
  </div>
</template>

<script setup>
import {ref, onMounted} from "vue";
import {
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxPager,
    DxEditing, DxSearchPanel, DxHeaderFilter,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import userService from "@/api/userService";
import AuthenticationService from "@/api/AuthenticationService";
import {useRouter} from "vue-router";
import {DxButton} from "devextreme-vue/button";

const dataSource = ref(null);
const router = useRouter();

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    const response = await userService.getAllUsers(loadOptions);
    return response;
  },
  async remove(oid) {
    const baseResponse = await userService.deleteUser(oid);
    return {data: baseResponse.result};
  },
});

const onRowClick = async (e) => {
  try {
    const userId = e.key;
    const role = await AuthenticationService.getRole();
    console.log(role);
    const routeParams = {name: "userDetail", params: {mode: "read", id: userId}};
    await router.push(routeParams);
  } catch (error) {
    console.log(error);
  }
};


const onCreateUserClick = async () => {
  try {
    await router.push({name: 'userDetail', params: {mode: "create", id: null}});
  } catch (error) {
    console.log(error);
  }
};

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