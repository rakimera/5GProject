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
          data-field="companyName"
          :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}">
        <dx-label :visible="false" text='Название компании'/>
        <dx-required-rule message="Название компании должно быть заполнено"/>
      </dx-simple-item>

      <dx-simple-item
          data-field='townName'
          editor-type="dxSelectBox"
          :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating',
                      items: towns,
                      displayExpr: 'townName',
                      valueExpr: 'townName',}"
      >
        <dx-label :visible="false" text='Город'/>
        <dx-required-rule message="Вы не выбрали город"></dx-required-rule>
      </dx-simple-item>
      <dx-simple-item
          data-field='address'
          editor-type='dxTextBox'
          :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}"
      >
        <dx-label :visible="false" text='Адрес'/>
        <dx-required-rule message="Пожалуйста укажите адрес компании"/>
      </dx-simple-item>
      <dx-simple-item
          data-field="licenseNumber"
          :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}">
        <dx-label :visible="false" text='Номер лицензии компании'/>
      </dx-simple-item>

      <dx-simple-item
          data-field="licenseDateOfIssue"
          :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}">
        <dx-label :visible="false" text='Дата выдачи номера лицензии компании'/>
        <dx-date-box
            :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}"
            label="Дата выдачи номера лицензии компании"
            v-model="dataSource.licenseDateOfIssue"
            :input-attr="{ 'aria-label': 'Date' }"
            type="date"
            :read-only="isFormDisabled"
        />
      </dx-simple-item>
      <dx-simple-item
          data-field="bin"
          :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}">
        <dx-label :visible="false" text='БИН'/>
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
              :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}">
          <dx-label :visible="false" text='Имя директора'/>
          <dx-required-rule message="Имя директора должно быть заполенено"/>
          <dx-pattern-rule
                  :pattern="stringPattern"
                  message="Нельзя использовать цифры в имени"
          />
      </dx-simple-item>
      <dx-simple-item
              data-field="directorSurname"
              :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}">
          <dx-label :visible="false" text='Фамилия директора'/>
          <dx-required-rule message="Фамилия директора должна быть заполенена"/>
          <dx-pattern-rule
                  :pattern="stringPattern"
                  message="Нельзя использовать цифры в фамилии"
          />
      </dx-simple-item>
      <dx-simple-item
              data-field="directorPatronymic"
              :editor-options="{
                      stylingMode: 'filled',
                      labelMode: 'floating'}">
          <dx-label :visible="false" text='Отчество директора'/>
          <dx-pattern-rule
                  :pattern="stringPattern"
                  message="Нельзя использовать цифры в отчестве"
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
import townService from "@/api/townService";

const route = useRoute();
const router = useRouter();
let dataSource = reactive({});
dataSource.licenseDateOfIssue = null;
const routeParams = {name: "ExecutiveCompaniesJournal"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref("Подробно о компании");
const binPattern = ref("^[0-9]")
const stringPattern = ref("^[a-zA-Zа-яА-ЯЁё]+$")
const formRef = ref(null);
const towns = ref([]);

onBeforeMount(async () => {
  const townResponse = await townService.getTowns();
  towns.value = townResponse.data.result;
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
    if (dataSource.licenseDateOfIssue !== null) {
      const licenseDate = new Date(dataSource.licenseDateOfIssue);
      const currentDate = new Date();

      if (licenseDate > currentDate) {
        notify({
          message: "Дата лицензии не может быть в будущем",
          position: {
            my: "center top",
            at: "center top",
          },
        }, "warning", 2000);
        return;
      }

      const minDate = new Date(currentDate);
      minDate.setFullYear(minDate.getFullYear() - 20);

      if (licenseDate < minDate) {
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