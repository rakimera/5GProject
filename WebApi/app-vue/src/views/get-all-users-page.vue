<template>
  <DxDataGrid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @row-click="onRowClick"
  >
    <DxColumn data-field="login" data-type="string"/>
    <DxColumn data-field="name" data-type="string"/>
    <DxColumn data-field="surname" data-type="string"/>
    <DxColumn data-field="password" data-type="string"/>
    <DxColumn data-field="role" data-type="string"/>
    <DxPaging :page-size="5"/>
    <DxPager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
    <DxEditing :allow-deleting="true"/>
  </DxDataGrid>
  <DxButton
      class="button"
      text="Создать"
      type="success"
      :use-submit-behavior="true"
      :on-click="onCreateUserClick"
  />
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
import userService from "@/api/userService";
import AuthenticationService from "@/api/AuthenticationService";
import {useRouter} from "vue-router";
import {DxButton} from "devextreme-vue/button";

const dataSource = ref(null);
const router = useRouter();

const onRowClick = async (e) => {
  try {
    const userId = e.key;
    const role = await AuthenticationService.getRole();
    console.log(role);
    const routeParams = {name: "userDetail", params: {id: userId}};
    if (role === "Admin") {
      routeParams.params.mode = "edit";
    } else {
      routeParams.params.mode = "read";
    }
    await router.push(routeParams);
  } catch (error) {
    console.log(error);
  }
};

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    return await userService.getAllUsers(loadOptions);
  },
  async insert(user) {
    const baseResponse = await userService.createUser(user);
    return {data: baseResponse.result};
  },
  async update(id) {
    try {
      const baseResponse = await userService.updateUser(id);
      return {data: baseResponse.result};
    } catch (error) {
      console.log(error);
    }
  },
  async remove(oid) {
    const baseResponse = await userService.deleteUser(oid);
    return {data: baseResponse.result};
  },
});

const onCreateUserClick = async () => {
  try {
    let routeParams = {name: "createUser", params: {mode: "create"}};
    await router.push(routeParams);
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