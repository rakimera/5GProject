<template>
  <div>
    <h2>Подробнее о контрагенте</h2>
    <dx-form
        id="form"
        label-location="top"
        :form-data="dataSource"
        :read-only="isFormDisabled"
    >
    </dx-form>
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
import {DxForm} from "devextreme-vue/form";
import DxButton from 'devextreme-vue/button';
import {onBeforeMount, reactive, ref} from "vue";
import contrAgentService from "@/api/contrAgentService";
import {useRoute} from "vue-router";

const route = useRoute();
let dataSource = reactive({
  'Название компании':"",
  'БИН': "",
  'Имя директора':"",
  'Фамилия директора': "",
  'Отчетсво директора':"",
  'Коэффициент усиления': "",
});
let isFormDisabled = ref(true);
const oid = route.params.id;

onBeforeMount(async () => {
  const response = await contrAgentService.getContrAgent(oid);
  dataSource["БИН"] = response.data.result.bin;
  dataSource["Название компании"] = response.data.result.companyName;
  dataSource["Имя директора"] = response.data.result.directorName;
  dataSource["Фамилия директора"] = response.data.result.directorSurname;
  dataSource["Отчетсво директора"] = response.data.result.directorPatronymic;
  dataSource["Коэффициент усиления"] = response.data.result.amplificationFactor;
})
function onClickEditUser() {
  isFormDisabled.value = false;
}
async function onClickSaveChanges() {
  const updatedData = {
    id: oid,
    bin: dataSource["БИН"],
    companyName: dataSource["Название компании"],
    directorName: dataSource["Имя директора"],
    directorSurname: dataSource["Фамилия директора"],
    directorPatronymic: dataSource["Отчетсво директора"],
    amplificationFactor: dataSource["Коэффициент усиления"],
  };
  try {
    await contrAgentService.updateContrAgent(updatedData);
    isFormDisabled.value = true;
  } catch (error) {
    console.error("Ошибка при сохранении изменений:", error);
  }
}
</script>