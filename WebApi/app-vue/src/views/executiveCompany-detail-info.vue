<template>
  <div class="executiveCompany-form">
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
          data-field="companyName">
        <dx-label :text="'Название компании'"/>
        <dx-required-rule message="Название компании должно быть заполнено"/>
      </dx-simple-item>

      <dx-simple-item
          data-field="address">
        <dx-label :text="'Адрес компании'"/>
        <dx-required-rule message="Адрес должнен быть заполнен"/>
      </dx-simple-item>

      <dx-simple-item
          data-field="licenseNumber">
        <dx-label :text="'Номер лицензии компании'"/>
      </dx-simple-item>

      <dx-simple-item data-field="licenseDateOfIssue">
        <dx-label :text="'Дата выдачи номера лицензии компании'"/>
        <dx-date-box
            v-model="dataSource.licenseDateOfIssue"
            :input-attr="{ 'aria-label': 'Date' }"
            type="date"
            :read-only="isFormDisabled"
        />
      </dx-simple-item>

      <dx-simple-item
          data-field="bin">
        <dx-label :text="'БИН'"/>
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

      <dx-button-item>
        <dx-button-options
            width="100%"
            type="default"
            styling-mode="outlined"
            template="Редактировать"
            :on-click="onClickEditExecutiveCompany"
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
  DxButtonOptions
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import executiveCompanyService from "@/api/executiveCompanyService";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";
import DxDateBox from 'devextreme-vue/date-box';

const route = useRoute();
const router = useRouter();
let dataSource = reactive({});
dataSource.licenseDateOfIssue = null;
const routeParams = {name: "Журнал компании"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref("Подробно о компании");
const binPattern = ref("^[0-9]")
const formRef = ref(null);

onBeforeMount(async () => {
  if (mode === "read") {
    const response = await executiveCompanyService.getExecutiveCompany(oid);
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
    pageDescription.value = "Создание компании"
  }
})

function onClickEditExecutiveCompany() {
  isFormDisabled.value = false;
}

async function onClickSaveChanges() {
  try {
    if (dataSource.licenseDateOfIssue !== null){
      const licenseDate = new Date(dataSource.licenseDateOfIssue);
      const currentDate = new Date();

      const maxDate = new Date(currentDate);
      maxDate.setFullYear(maxDate.getFullYear() + 10);
      const minDate = new Date(currentDate);
      minDate.setFullYear(minDate.getFullYear() - 20);

      if (licenseDate > maxDate || licenseDate < minDate) {
        notify({
          message: "Дата лицензии должна быть в течение последних 20 лет",
          position: {
            my: "center top",
            at: "center top",
          },
        }, "warning", 2000);
        return;
      }
    }

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
        const responseUpdate = await executiveCompanyService.updateExecutiveCompany(dataSource);
        if (responseUpdate.data.success) {
          notify({
            message: 'Компания успешно отредактирована',
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
        const response = await executiveCompanyService.createExecutiveCompany(dataSource);
        if (response.data.success) {
          notify({
            message: 'Компания успешно создана',
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

.executiveCompany-form {
  max-width: 1000px;
  margin: 50px auto auto;
}

#form h2 {
  margin-left: 40px;
  font-weight: normal;
  font-size: 35px;
}
</style>