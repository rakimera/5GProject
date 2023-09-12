<template>
  <div class="contrAgent-form">
    <h2 v-text="pageDescription"></h2>
    <dx-form
        id="form"
        ref="formRef"
        label-location="top"
        :form-data="dataSource"
        :read-only="isFormDisabled"
        :show-colon-after-label="true"
        :show-validation-summary="true"
    >
      <dx-simple-item
          data-field="companyName"
          :editor-options="{ stylingMode: 'filled', labelMode: 'floating'}">
        <dx-label :visible="false" text="Название компании"/>

        <dx-required-rule message="Название компании должно быть заполнено"/>
      </dx-simple-item>
      <dx-simple-item
          data-field="bin"
          :editor-options="{ stylingMode: 'filled', labelMode: 'floating'}">
        <dx-label :visible="false" text="БИН"/>
        <dx-required-rule message="БИН должнен быть заполнен"/>
        <dx-string-length-rule
            :min=12
            :max=12
            message="БИН состоит из 12 чисел"
        />
        <dx-pattern-rule
            :pattern="binPattern"
            message="Некорректный БИН"
        />
      </dx-simple-item>
      <dx-simple-item
          data-field="directorName"
          :editor-options="{ stylingMode: 'filled', labelMode: 'floating'}">
        <dx-label :visible="false" text="Имя директора"/>
        <dx-required-rule message="Имя должно быть заполнено"/>
        <dx-string-length-rule
            :min=2
            message="Имя не может содержать менее 2 символов"
        />
        <dx-pattern-rule
            :pattern="namePattern"
            message="Имя должно состоять только из букв"
        />
      </dx-simple-item>
      <dx-simple-item
          data-field="directorSurname"
          :editor-options="{ stylingMode: 'filled', labelMode: 'floating'}">
        <dx-label :visible="false" text="Фамилия директора"/>
        <dx-required-rule message="Фамилия должна быть заполнено"/>
        <dx-string-length-rule
            :min="2"
            message="Фамилия не может содержать менее 2 символов"
        />
        <dx-pattern-rule
            :pattern="namePattern"
            message="Фамилия должна состоять только из букв"
        />
      </dx-simple-item>
      <dx-simple-item
          data-field="directorPatronymic"
          :editor-options="{ stylingMode: 'filled', labelMode: 'floating'}">
        <dx-label :visible="false" text="Отчество директора"/>

        <dx-pattern-rule
            :pattern="namePattern"
            message="Отчество должно состоять только из букв"
        />
      </dx-simple-item>
      <dx-simple-item
          data-field="email"
          :editor-options="{ stylingMode: 'filled', labelMode: 'floating'}">
        <dx-label :visible="false" text="Электронная почта"/>
        <dx-email-rule message="Пожалуйста, введите корректный email"/>
      </dx-simple-item>
      <dx-simple-item
          data-field="phoneNumber"
          :editor-options="{ 
            stylingMode: 'filled', 
            labelMode: 'floating',
            maskRules: phoneRules, 
            mask:'+7 (000) 000-0000'}"
      >
        <dx-label :visible="false" text='Номер телефона'/>
        <dx-required-rule message="Пожалуйста, введите номер телефона"/>
        <dx-pattern-rule
            :pattern="phonePattern"
            message="Введи корректный формат телефона"
        />
      </dx-simple-item>
      <dx-simple-item
          data-field="address"
          :editor-options="{ stylingMode: 'filled', labelMode: 'floating'}">
        <dx-label :visible="false" text="Адрес компании"/>
        <dx-required-rule message="Адрес должнен быть заполнен"/>
      </dx-simple-item>
      <dx-button-item>
        <dx-button-options
            width="100%"
            type="default"
            styling-mode="outlined"
            template="Редактировать"
            :on-click="onClickEditContrAgent"
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
</template>
<script setup>

import {
  DxForm,
  DxLabel,
  DxSimpleItem,
  DxPatternRule,
  DxRequiredRule,
  DxStringLengthRule,
  DxButtonItem,
  DxButtonOptions, DxEmailRule,
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import contrAgentService from "@/api/contrAgentService";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";

const route = useRoute();
const router = useRouter();
let dataSource = reactive({});
const routeParams = {name: "Журнал контрагентов"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref("Подробно о контрагенте");
const namePattern = ref("^[a-zA-Zа-яА-Я]+$")
const binPattern = ref("^[0-9]")
const phoneRules = ref({
  X: /[02-9]/,
});
const phonePattern = ref(/^[02-9]\d{9}$/);
const formRef = ref(null);

onBeforeMount(async () => {
  if (mode === "read") {
    const response = await contrAgentService.getContrAgent(oid);
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
    pageDescription.value = "Создание контрагента"
  }
})
function onClickEditContrAgent() {
  isFormDisabled.value = false;
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
    }
    else {
      if (mode === "read") {
        const responseUpdate = await contrAgentService.updateContrAgent(dataSource);
        if (responseUpdate.data.success) {
          notify({
            message: 'Контрагент успешно отредактирован',
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
        const response = await contrAgentService.createContrAgent(dataSource);
        if (response.data.success) {
          notify({
            message: 'Контрагент успешно создан',
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

.contrAgent-form {
  max-width: 1000px;
  margin: 50px auto auto;
}
#form h2 {
  margin-left: 40px;
  font-weight: normal;
  font-size: 35px;
}
</style>