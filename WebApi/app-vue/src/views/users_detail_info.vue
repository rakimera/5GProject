<template>
  <div>
    <h2 v-text="pageDescription"></h2>
    <dx-form
        id="form"
        label-location="top"
        :form-data="createSource"
        :disabled="isFormDisabled"
        v-if="created"
    ></dx-form>
    <dx-form
        id="form"
        label-location="top"
        :form-data="dataSource"
        :read-only="isFormDisabled"
        v-if="!created"
    ></dx-form>
    <DxButton
        text="Редактировать"
        :on-click="onClickEditUser"
        v-if="isFormDisabled"
    />
    <DxButton
        text="Подтвердить"
        :on-click="onClickSaveChanges"
        v-if="!isFormDisabled"
    />
  </div>
</template>
<script setup>
import DxForm from "devextreme-vue/form";
import DxButton from "devextreme-vue/button";
import {onBeforeMount, reactive, ref} from "vue";
import userService from "@/api/userService";
import {useRoute, useRouter} from "vue-router";

const route = useRoute();
const router = useRouter();
const dataSource = reactive({
  login: "",
  name: "",
  surname: "",
  role: "",
});
const createSource = reactive({
  login: "",
  name: "",
  surname: "",
  role: "",
  password: "",
});
const routeParams = {name: "users_table"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref("Подробно о пользователе");
const created = ref(false);

onBeforeMount(async () => {
  if (mode === "create") {
    isFormDisabled.value = false;
    pageDescription.value = "Создание пользователя";
    created.value = true;
  } else {
    try {
      const response = await userService.getUser(route.params.id);
      const userData = response.data.result;
      Object.assign(dataSource, userData);
    } catch (error) {
      console.error("Ошибка при загрузке данных пользователя:", error);
    }
  }
});

function onClickEditUser() {
  isFormDisabled.value = false;
}

async function onClickSaveChanges() {
  try {
    if (mode === "read") {
      const updatedData = {
        id: oid,
        login: dataSource.login,
        name: dataSource.name,
        surname: dataSource.surname,
        role: dataSource.role,
      };
      await userService.updateUser(updatedData);
      isFormDisabled.value = true;
    } else {
      const createdData = {
        login: createSource.login,
        name: createSource.name,
        surname: createSource.surname,
        role: createSource.role,
        password: createSource.password,
      };
      await userService.createUser(createdData);
      await router.push(routeParams);
    }
  } catch (error) {
    console.error("Ошибка при сохранении изменений:", error);
  }
}
</script>