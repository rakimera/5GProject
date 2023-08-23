<template>
  <h2 v-text="pageDescription"></h2>
  <dx-form
      id="project-form"
      ref="formRef"
      label-location="top"
      :form-data="dataSource"
      :read-only="isFormDisabled"
      :show-colon-after-label="true"
      :show-validation-summary="true"
  >
    <dx-item
        data-field='projectNumber'
        editor-type='dxTextBox'
        :editor-options="{ 
                      stylingMode: 'filled',
                      labelMode: 'floating',
                      label: 'Номер проекта (сайт)' }"
    >
      <dx-required-rule message="Укажите Sitename(номер) проекта"></dx-required-rule>
      <dx-label
          :visible="false"
      />
    </dx-item>
    <dx-item
        data-field='contrAgentId'
        editor-type="dxSelectBox"
        :editor-options="{ 
                        placeholder: 'Выберите контрагента', 
                        items: contrAgents, 
                        displayExpr: 'companyName', 
                        valueExpr: 'id',
                        labelMode: 'floating',
                        label: 'Контрагент'}"
    >
      <dx-required-rule message="Вы не выбрали контрагента"></dx-required-rule>
      <dx-label :visible="false"/>
    </dx-item>
    <dx-item
        data-field='townName'
        editor-type="dxSelectBox"
        :editor-options="{ 
                        placeholder: 'Выберите город', 
                        items: towns, 
                        displayExpr: 'townName', 
                        valueExpr: 'townName',
                        labelMode: 'floating',
                        label: 'Город'}"
    >
      <dx-required-rule message="Вы не выбрали город установки"></dx-required-rule>
      <dx-label
          :visible="false"
      />
    </dx-item>
    <dx-item
        data-field='address'
        editor-type='dxTextBox'
        :editor-options="{ 
                      stylingMode: 'filled', 
                      label: 'Район, улица, дом', 
                      labelMode: 'floating' }"
    >
      <dx-label
          :visible="false"
      />
    </dx-item>
    <dx-button-item>
      <dx-button-options
          width="100%"
          type="success"
          styling-mode="outlined"
          :template="mode === 'create' ? 'Создать и продолжить' : 'Сохранить изменения'"
          :on-click="onClickSaveChanges"
          :visible="!isFormDisabled"
          :use-submit-behavior="true"
      >
      </dx-button-options>
    </dx-button-item>
    <dx-button-item>
      <dx-button-options
          width="100%"
          type="default"
          styling-mode="outlined"
          template="Редактировать"
          :on-click="onClickEditProject"
          :visible="isFormDisabled"
          :use-submit-behavior="true"
      >
      </dx-button-options>
    </dx-button-item>
  </dx-form>
</template>
<script setup>

import {
  DxButtonItem,
  DxButtonOptions,
  DxForm,
  DxItem,
  DxLabel
} from "devextreme-vue/form";
import {
  DxRequiredRule
} from 'devextreme-vue/validator';
import {onBeforeMount, reactive, ref, defineProps} from "vue";
import contrAgentService from "@/api/contrAgentService";
import townService from "@/api/townService";
import projectService from "@/api/projectService";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";

const props = defineProps({
    onSaveProject: Function,
})
const router = useRouter();
const route = useRoute();
let dataSource = reactive({});
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = ref(route.params.mode);
const pageDescription = ref("Подробно о проекте");
const formRef = ref(null);
const contrAgents = ref([]);
const towns = ref([]);

onBeforeMount(async () => {
  const response = await contrAgentService.getContrAgents();
  contrAgents.value = response.data.result;

  const townResponse = await townService.getTowns();
  towns.value = townResponse.data.result;

  if (mode.value === "read") {
    const response = await projectService.getProject(oid);
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
    pageDescription.value = "Создание проекта"
  }
})
function onClickEditProject() {
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
      if (mode.value === "read") {
        const responseUpdate = await projectService.updateProject(dataSource);
        if (responseUpdate.data.success) {
          notify({
            message: 'Проект успешно отредактирован',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
        } else {
          notify(responseUpdate.data.messages, 'error', 2000);
        }
      } else {
        const response = await projectService.createProject(dataSource);
        if (response.data.success) {
          notify({
            message: 'Проект успешно создан',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
            await router.push({name: 'projectDetail', params: {mode: "read", id: response.data.result}});
            props.onSaveProject()
        } else {
          notify({
            message: response.data.messages,
            position: {
              my: 'center top',
              at: 'center top'}
          }, 'error', 2000);
        }
      }
    }

  } catch (error) {
    console.error("Ошибка при сохранении изменений:", error);
    notify({
      message: "Ошибка сервера при сохранении изменений:",
      position: {
        my: 'center top',
        at: 'center top'}
    }, 'error', 2000);
  }
}
</script>

<style scoped>
#form h2 {
  margin-left: 40px;
  font-weight: normal;
  font-size: 35px;
}
</style>