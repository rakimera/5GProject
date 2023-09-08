<template>
  <div>
    <h2 class="content-block">Profile</h2>
    <div class="content-block dx-card responsive-paddings">
      <dx-form
          id="form"
          label-location="top"
          :form-data="currentUser"
          :colCountByScreen="colCountByScreen"
          :read-only="true"
          :items="formItems"
      >
      </dx-form>
    </div>
  </div>
</template>

<script setup>
import {ref, onMounted} from "vue";
import DxForm from "devextreme-vue/form";
import userService from "@/api/userService";

const currentUser = ref(null);
const fetchCurrentUser = async () => {
  try {
    const response = await userService.getCurrentUser();
    console.log(response);

    if (response.data && response.data.result) {
      currentUser.value = response.data.result;
    } else {
      console.error("Ответ не содержит информацию о текущем пользователе:", response);
    }
  } catch (error) {
    console.error("Ошибка при получении информации о текущем пользователе:", error);
  }
};

const colCountByScreen = {
  xs: 1,
  sm: 2,
  md: 3,
  lg: 3
}

const formItems = ref([
  {
    dataField: "name",
    label: {text: "Имя"},
  },
  {
    dataField: "surname",
    label: {text: "Фамилия"},
  },
  {
    dataField: "login",
    label: {text: "Логин"},
  },
  {
    dataField: "roles",
    label: {text: "Роли"},
  },
  {
    dataField: "executiveCompanyName",
    label: {text: "Компания"},
  },
  {
    dataField: "createdBy",
    label: {text: "Создатель аккаунта"},
  },
  {
    dataField: "created",
    label: {text: "Дата создания аккаунта"},
    editorType: "dxDateBox",
    editorOptions: {
      displayFormat: "dd.MM.yyyy",
    },
  },
  {
      dataField: "phoneNumber",
      label: {text: "Номер телефона"},
      editorOptions: {
          mask:'+7 (000) 000-0000',
      },
  },
]);

onMounted(() => {
  fetchCurrentUser();
});
</script>

<style lang="scss">
.form-avatar {
  float: left;
  height: 120px;
  width: 120px;
  margin-right: 20px;
  border: 1px solid rgba(0, 0, 0, 0.1);
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  background-color: #fff;
  overflow: hidden;

  img {
    height: 120px;
    display: block;
    margin: 0 auto;
  }
}
</style>
