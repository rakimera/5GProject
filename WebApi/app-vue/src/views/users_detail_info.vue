<template>
  <div class="user-form">
    <h2 v-text="pageDescription"></h2>
    <div class="user-form">
      <dx-form
          id="form"
          ref="formRef"
          label-location="top"
          :form-data="formData"
          :read-only="isFormDisabled"
          :show-colon-after-label="true"
          :show-validation-summary="true"
      >
        <dx-simple-item
            data-field="login"
            :editor-options="{ stylingMode: 'filled', placeholder: 'Логин' }"
        >
          <dx-label :text="'Логин'"/>
          <dx-required-rule message="Пожалуйста, введите email"/>
          <dx-email-rule message="Пожалуйста, введите корректный email"/>
        </dx-simple-item>
        <dx-simple-item
            v-if="mode === 'create'"
            data-field="password"
            editor-type="dxTextBox"
            :editor-options="{ stylingMode: 'filled', placeholder: 'Пароль', mode: 'password' }"
        >
          <dx-label :text="'Пароль'"/>
          <dx-required-rule message="Пожалуйста, введите пароль"/>
          <dx-pattern-rule
              :pattern="passwordPattern"
              message="Пароль должен содержать минимум 8 символов, включая строчную букву, заглавную букву, цифру и специальный символ"
          />
        </dx-simple-item>
        <dx-simple-item
            data-field="name"
            :editor-options="{ stylingMode: 'filled', placeholder: 'Имя' }"
        >
          <dx-label :text="'Имя'"/>
          <dx-required-rule message="Пожалуйста, введите имя"/>
          <dx-string-length-rule
              :min=2
              message="Имя не может содержать менее 2 символов"
          />
          <dx-pattern-rule
              :pattern="namePattern"
              message="Нельзя использовать цифры в имени"
          />
        </dx-simple-item>
        <dx-simple-item
            data-field="surname"
            :editor-options="{ stylingMode: 'filled', placeholder: 'Фамилия' }"
        >
          <dx-label :text="'Фамилия'"/>
          <dx-required-rule message="Пожалуйста, введите фамилию"/>
          <dx-string-length-rule
              :min="2"
              message="Фамилия не может содержать менее 2 символов"
          />
          <dx-pattern-rule
              :pattern="namePattern"
              message="Нельзя использовать цифры в фамилии"
          />
        </dx-simple-item>
        <dx-simple-item data-field="roles" :editor-options="{ stylingMode: 'filled' }">
          <dx-label :text="'Роли'"/>
          <dx-tag-box
              v-model="formData.roles"
              :items="roleOptions"
              display-expr="roleName"
              value-expr="roleName"
              :show-clear-button="false"
              :show-drop-down-button="false"
              :apply-value-mode="'useButtons'"
              :read-only="isFormDisabled && !isEditMode"
          />
        </dx-simple-item>

        <dx-button-item>
          <dx-button-options
              width="100%"
              type="default"
              styling-mode="outlined"
              template="Редактировать"
              :on-click="onClickEditUser"
              :visible="isFormDisabled"
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
              :visible="!isFormDisabled"
              :use-submit-behavior="true"
          >
          </dx-button-options>
        </dx-button-item>
      </dx-form>
    </div>
  </div>
</template>
<script setup>
import {
  DxForm,
  DxSimpleItem,
  DxRequiredRule,
  DxEmailRule,
  DxButtonItem,
  DxButtonOptions,
  DxStringLengthRule,
  DxPatternRule, DxLabel
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import userService from "@/api/userService";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";
import {DxTagBox} from "devextreme-vue/tag-box";
import roleService from "@/api/roleService";

const route = useRoute();
const router = useRouter();
const formData = reactive({});
const routeParams = {name: "users_table"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref(mode === "create" ? "Создание пользователя" : "Подробно о пользователе");
const created = ref(false);
const formRef = ref(null);
const namePattern = ref("^[a-zA-Zа-яА-Я]+$")
const passwordPattern = ref(
    "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$"
);
const roleOptions = ref([]);
const isEditMode = ref(false);

onBeforeMount(async () => {
  const response = await roleService.getRoles();
  roleOptions.value = response.data.result;
  if (mode === "read") {
    const response = await userService.getUser(oid);
    Object.assign(formData, response.data.result);
    formData.roles = response.data.result.roles;
    console.log(formData.roles)
    isEditMode.value = false;
  } else {
    isFormDisabled.value = false;
    created.value = true;
    formData.Roles = [];
    isEditMode.value = true;
  }
});

function onClickEditUser() {
  isFormDisabled.value = false;
  isEditMode.value = true;
}

async function onClickSaveChanges() {
  try {
    const formInstance = formRef.value.instance;
    const isFormValid = await formInstance.validate();
    if (isFormValid.isValid === false) {
      notify({
        message: 'Данные не корректны',
        position: {
          my: 'center top',
          at: 'center top',
        },
      }, 'warning', 1000);
    } else {
      if (mode === "read") {
        const responseUpdate = await userService.updateUser(formData);
        if (responseUpdate.data.success) {
          notify({
            message: 'Пользователь успешно отредактирован',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
          isFormDisabled.value = true;
        } else {
          notify(responseUpdate.data.messages, 'error', 2000);
        }
      } else {
        const response = await userService.createUser(formData);
        if (response.data.success) {
          notify({
            message: 'Пользователь успешно создан',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
          await router.push(routeParams);
        } else {
          notify(response.data.messages, 'error', 2000);
        }
      }
    }
  } catch (error) {
    console.error("Ошибка при сохранении изменений:", error);
  }
}
</script>

<style scoped>
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