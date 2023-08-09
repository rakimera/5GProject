<template>
  <div class="project-form">
    <h2
        v-text="pageDescription"></h2>
    <form
        @submit="onClickSaveChanges"
    >
    <dx-form
        id="form"
        label-location="top"
        :read-only="isFormDisabled"
        :form-data="dataSource">
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
            message="Нельзя использовать цифры в имени"
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
            message="Нельзя использовать цифры в фамилии"
        />
      </DxSimpleItem>
      <DxSimpleItem
          data-field="Отчество директора">
        <DxRequiredRule message="Отчество должно быть заполнено"/>
        <DxStringLengthRule
            :min=4
            message="Отчество не может содержать менее 4 символов"
        />
        <DxPatternRule
            :pattern="namePattern"
            message="Нельзя использовать цифры в отчестве"
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
      <DxButtonItem
          :button-options="buttonOption"
          horizontal-alignment="left"
          v-if="!isFormDisabled"
      />
    </dx-form>
    </form>
    <DxButton
        text="Редактировать"
        type="success"
        horizontal-alignment="left"
        :on-click="onClickEditContrAgent"
        v-if="isFormDisabled"
    />
      <DxValidationSummary id="summary"/>

  </div>
</template>
<script setup>

import {DxForm, DxSimpleItem,DxPatternRule, DxRequiredRule,DxStringLengthRule,DxButtonItem} from "devextreme-vue/form";
import DxButton from 'devextreme-vue/button';
import DxValidationSummary from 'devextreme-vue/validation-summary';
import {onBeforeMount, onBeforeUpdate, reactive, ref} from "vue";
import contrAgentService from "@/api/contrAgentService";
import {useRoute, useRouter} from "vue-router";

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
const created = ref(false);
const ampPattern = ref(/^(\d+(.\d+)*)?$/);
const namePattern = ref("^[a-zA-Zа-яА-Я]+$")
const binPattern = ref("^[0-9]")
const buttonOption = ref({
  text: 'Подтвердить',
  type: 'success',
  useSubmitBehavior: true,
});
let hasChanges = ref(false);

// Хук beforeRouteUpdate для обработки обновлений параметров маршрута
const beforeRouteUpdate = (to, from, next) => {
  if (hasChanges.value) {
    const confirmMessage = "Вы внесли изменения. Вы уверены, что хотите покинуть страницу?";
    if (confirm(confirmMessage)) {
      hasChanges.value = false; // Сбрасываем флаг изменений перед переходом
      next(); // Продолжаем обновление компонента
    } else {
      next(false); // Останавливаем обновление
    }
  } else {
    next(); // Просто продолжаем обновление компонента
  }
};

// Регистрируем хук beforeRouteUpdate
onBeforeUpdate(beforeRouteUpdate);


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
    created.value = true;
  }
})
function onClickEditContrAgent() {
  isFormDisabled.value = false;
}
async function onClickSaveChanges() {
  try {
    if (mode === "read"){
      const updatedData = {
        id: oid,
        bin: dataSource["БИН"],
        companyName: dataSource["Название компании"],
        directorName: dataSource["Имя директора"],
        directorSurname: dataSource["Фамилия директора"],
        directorPatronymic: dataSource["Отчество директора"],
        amplificationFactor: dataSource["Коэффициент усиления"],
      };
      await contrAgentService.updateContrAgent(updatedData);
      isFormDisabled.value = true;
    }
    else {
      const createdData = {
        bin: dataSource["БИН"],
        companyName: dataSource["Название компании"],
        directorName: dataSource["Имя директора"],
        directorSurname: dataSource["Фамилия директора"],
        directorPatronymic: dataSource["Отчество директора"],
        amplificationFactor: dataSource["Коэффициент усиления"],
      };
      hasChanges = true;
      await contrAgentService.createContrAgent(createdData)
      await router.push(routeParams);
    }
  } catch (error) {
    console.error("Ошибка при сохранении изменений:", error);
  }
}
</script>
<style scoped>
#summary {
  padding-left: 10px;
  margin-top: 20px;
  margin-bottom: 10px;
}
.project-form {
  max-width: 1000px;
  margin: auto;
  margin-top: 50px;
}
#form h2 {
  margin-left: 20px;
  font-weight: normal;
  font-size: 22px;
}
</style>