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
      <DxSimpleItem
          data-field="Название компании">
        <DxRequiredRule message="Название компании должно быть заполнено"/>
      </DxSimpleItem>
      <DxSimpleItem
          data-field="БИН">
        <DxRequiredRule message="БИН должнен быть заполнен"/>
        <DxStringLengthRule
            :min=12
            :max=12
            message="БИН состоит из 12 чисел"
        />
        <DxPatternRule
            :pattern="binPattern"
            message="Некорректный БИН"
        />
      </DxSimpleItem>
      <DxSimpleItem
          data-field="Имя директора">
        <DxRequiredRule message="Имя должно быть заполнено"/>
        <DxStringLengthRule
            :min=2
            message="Имя не может содержать менее 2 символов"
        />
        <DxPatternRule
            :pattern="namePattern"
            message="Имя должно состоять только из букв"
        />
      </DxSimpleItem>
      <DxSimpleItem
          data-field="Фамилия директора">
        <DxRequiredRule message="Фамилия должна быть заполнено"/>
        <DxStringLengthRule
            :min="2"
            message="Фамилия не может содержать менее 2 символов"
        />
        <DxPatternRule
            :pattern="namePattern"
            message="Фамилия должна состоять только из букв"
        />
      </DxSimpleItem>
      <DxSimpleItem
          data-field="Отчество директора">
        <DxPatternRule
            :pattern="namePattern"
            message="Отчество должно состоять только из букв"
        />
      </DxSimpleItem>
      <DxSimpleItem
          data-field="Коэффициент усиления">
        <DxRequiredRule message="Коэффициент усиления должнен быть заполнен"/>
        <DxPatternRule
            :pattern="ampPattern"
            message="Коэффициент усиление это числовое значение с возможностью разделение через точку"
        />
      </DxSimpleItem>
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
  DxSimpleItem,
  DxPatternRule,
  DxRequiredRule,
  DxStringLengthRule,
  DxButtonItem,
  DxButtonOptions,
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import contrAgentService from "@/api/contrAgentService";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";

const route = useRoute();
const router = useRouter();
let dataSource = reactive({
  'Название компании':"",
  'БИН': "",
  'Имя директора':"",
  'Фамилия директора': "",
  'Отчество директора':"",
  'Коэффициент усиления': "",
});
const routeParams = {name: "Журнал контрагентов"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref("Подробно о контрагенте");
const ampPattern = ref(/^(\d+(.\d+)*)?$/);
const namePattern = ref("^[a-zA-Zа-яА-Я]+$")
const binPattern = ref("^[0-9]")
const formRef = ref(null);

onBeforeMount(async () => {
  if (mode === "read") {
    const response = await contrAgentService.getContrAgent(oid);
    dataSource["БИН"] = response.data.result.bin;
    dataSource["Название компании"] = response.data.result.companyName;
    dataSource["Имя директора"] = response.data.result.directorName;
    dataSource["Фамилия директора"] = response.data.result.directorSurname;
    dataSource["Отчество директора"] = response.data.result.directorPatronymic;
    dataSource["Коэффициент усиления"] = response.data.result.amplificationFactor;
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
        const updatedData = {
          id: oid,
          bin: dataSource["БИН"],
          companyName: dataSource["Название компании"],
          directorName: dataSource["Имя директора"],
          directorSurname: dataSource["Фамилия директора"],
          directorPatronymic: dataSource["Отчество директора"],
          amplificationFactor: dataSource["Коэффициент усиления"],
        };
        const responseUpdate = await contrAgentService.updateContrAgent(updatedData);
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
        const createdData = {
          bin: dataSource["БИН"],
          companyName: dataSource["Название компании"],
          directorName: dataSource["Имя директора"],
          directorSurname: dataSource["Фамилия директора"],
          directorPatronymic: dataSource["Отчество директора"],
          amplificationFactor: dataSource["Коэффициент усиления"],
        };
        const response = await contrAgentService.createContrAgent(createdData);
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