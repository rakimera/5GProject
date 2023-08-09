<template>
    <h2 v-text="pageDescription"></h2>
  <div class="user-form">
    <dx-form
        id="form"
        label-location="top"
        :form-data="formData"
        :read-only="isFormDisabled"
    >
      <dx-item
          data-field="login"
          editor-type="dxTextBox"
          :editor-options="{ stylingMode: 'filled', placeholder: 'Логин' }"
      >
        <dx-required-rule message="Пожалуйста, введите email"/>
        <dx-email-rule message="Пожалуйста, введите корректный email"/>
        <dx-label :visible="false"/>
      </dx-item>
      <dx-item
          data-field="name"
          editor-type="dxTextBox"
          :editor-options="{ stylingMode: 'filled', placeholder: 'Имя' }"
      >
        <dx-required-rule message="Пожалуйста, введите имя"/>
        <dx-label :visible="false"/>
      </dx-item>
      <dx-item
          data-field="surname"
          editor-type="dxTextBox"
          :editor-options="{ stylingMode: 'filled', placeholder: 'Фамилия' }"
      >
        <dx-required-rule message="Пожалуйста, введите фамилию"/>
        <dx-label :visible="false"/>
      </dx-item>
      <dx-item
          data-field="role"
          editor-type="dxTextBox"
          :editor-options="{ stylingMode: 'filled', placeholder: 'Роль' }"
      >
        <dx-required-rule message="Пожалуйста, введите роль"/>
        <dx-label :visible="false"/>
      </dx-item>
      <dx-item
          v-if="mode === 'create'"
          data-field="password"
          editor-type="dxTextBox"
          :editor-options="{ stylingMode: 'filled', placeholder: 'Пароль', mode: 'password' }"
      >
        <dx-required-rule message="Пожалуйста, введите пароль"/>
        <dx-label :visible="false"/>
      </dx-item>
      <dx-button-item>
        <dx-button-options
            width="100%"
            type="default"
            styling-mode="outlined"
            template="Редактировать"
            :on-click="onClickEditUser"
            v-if="isFormDisabled"
            :use-submit-behavior="true"
        >
        </dx-button-options>
      </dx-button-item>
      <dx-button-item>
        <dx-button-options
            width="100%"
            type="success"
            styling-mode="outlined"
            :template="mode === 'create' ? 'Создать' : 'Сохранить изменения'"
            :on-click="onClickSaveChanges"
            v-if="!isFormDisabled"
            :use-submit-behavior="true"
        >
        </dx-button-options>
      </dx-button-item>
    </dx-form>
  </div>
</template>
<script setup>
import DxForm, {
  DxItem,
  DxLabel,
  DxRequiredRule,
  DxEmailRule, 
  DxButtonItem,
  DxButtonOptions
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import userService from "@/api/userService";
import {useRoute, useRouter} from "vue-router";

const route = useRoute();
const router = useRouter();
const formData = reactive({
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
const pageDescription = ref(mode === "create" ? "Создание пользователя" : "Подробно о пользователе");
const created = ref(false);

onBeforeMount(async () => {
  if (mode === "create") {
    isFormDisabled.value = false;
    created.value = true;
  } else {
    try {
      const response = await userService.getUser(route.params.id);
      const userData = response.data.result;
      Object.assign(formData, userData);
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
        login: formData.login,
        name: formData.name,
        surname: formData.surname,
        role: formData.role,
      };
      await userService.updateUser(updatedData);
      isFormDisabled.value = true;
    } else {
      const createdData = {
        login: formData.login,
        name: formData.name,
        surname: formData.surname,
        role: formData.role,
        password: formData.password,
      };
      await userService.createUser(createdData);
      await router.push(routeParams);
    }
  } catch (error) {
    console.error("Ошибка при сохранении изменений:", error);
  }
}
</script>

<style lang="scss">
@import "../themes/generated/variables.base.scss";

.user-form {
  max-width: 1000px;
  margin: 50px auto auto;
}

#form h2 {
  margin-left: 20px;
  font-weight: normal;
  font-size: 22px;
}
</style>