<template>
  <div class="project-form">
    <h2
        v-text="pageDescription"></h2>
    <dx-form
        id="form"
        label-location="top"
        :form-data="createSource"
        :disabled="isFormDisabled"
        v-if="created">
    </dx-form>
    <dx-form
        id="form"
        label-location="top"
        :form-data="dataSource"
        :read-only="isFormDisabled"
        v-if="!created">
    </dx-form>
    <DxButton
        text="Редактировать"
        :on-click="onClickEditContrAgent"
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
import {DxForm} from "devextreme-vue/form";
import DxButton from 'devextreme-vue/button';
import {onBeforeMount, reactive, ref} from "vue";
import contrAgentService from "@/api/contrAgentService";
import {useRoute, useRouter} from "vue-router";

const route = useRoute();
const router = useRouter();
let dataSource = reactive({
  'Название компании':"",
  'БИН': "",
  'Имя директора':"",
  'Фамилия директора': "",
  'Отчетсво директора':"",
  'Коэффициент усиления': "",
});
const createSource = reactive({
  'Название компании':"",
  'БИН': "",
  'Имя директора':"",
  'Фамилия директора': "",
  'Отчетсво директора':"",
  'Коэффициент усиления': "",
});
const routeParams = {name: "Журнал контрагентов"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref("Подробно о контрагенте");
const created = ref(false);

onBeforeMount(async () => {

})
async function onClickSaveChanges() {
  try {
    if (mode === "read"){
      const updatedData = {
        id: oid,
        bin: dataSource["БИН"],
        companyName: dataSource["Название компании"],
        directorName: dataSource["Имя директора"],
        directorSurname: dataSource["Фамилия директора"],
        directorPatronymic: dataSource["Отчетсво директора"],
        amplificationFactor: dataSource["Коэффициент усиления"],
      };
      await contrAgentService.updateContrAgent(updatedData);
      isFormDisabled.value = true;
    }
    else {
      const createdData = {
        bin: createSource["БИН"],
        companyName: createSource["Название компании"],
        directorName: createSource["Имя директора"],
        directorSurname: createSource["Фамилия директора"],
        directorPatronymic: createSource["Отчетсво директора"],
        amplificationFactor: createSource["Коэффициент усиления"],
      };
      await contrAgentService.createContrAgent(createdData)
      await router.push(routeParams);
    }
  } catch (error) {
    console.error("Ошибка при сохранении изменений:", error);
  }
}
onBeforeMount(async () => {
    if (mode === "create") {
    isFormDisabled.value = false;
    pageDescription.value = "Создание контрагента"
    created.value = true;
  } else {
    const response = await contrAgentService.getContrAgent(oid);
    dataSource["БИН"] = response.data.result.bin;
    dataSource["Название компании"] = response.data.result.companyName;
    dataSource["Имя директора"] = response.data.result.directorName;
    dataSource["Фамилия директора"] = response.data.result.directorSurname;
    dataSource["Отчетсво директора"] = response.data.result.directorPatronymic;
    dataSource["Коэффициент усиления"] = response.data.result.amplificationFactor;
  }
})
function onClickEditContrAgent() {
  isFormDisabled.value = false;
}
</script>
<style>
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