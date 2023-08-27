<template>
  <h2 v-text="pageAntennaDescription"></h2>
  <dx-form
      id="antenna-form"
      ref="formRef"
      label-location="top"
      :form-data="dataSource"
      :read-only="isFormDisabled"
      :show-colon-after-label="true"
      :show-validation-summary="true"
  >
    <dx-item
        data-field="model"
        editor-type='dxTextBox'
        :editor-options="{ 
                      stylingMode: 'filled',
                      labelMode: 'floating',
                      label: 'Модель антенны' }"
    >
      <dx-label :visible="false"/>
      <dx-required-rule message="Модель антенны должны быть заполнена"/>
    </dx-item>
    <dx-item
        data-field="verticalSizeDiameter"
        editor-type='dxTextBox'
        :editor-options="{ 
                      stylingMode: 'filled',
                      labelMode: 'floating',
                      label: 'Вертикальный размер(диаметр антенны)' }">
      <dx-label :text="false"/>
      <dx-required-rule message="Вертикальный размер(диаметр антенны) должнен быть заполнен"/>
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
          :on-click="onClickEditAntenna"
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
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";
import antennaService from "@/api/antennaService";


const props = defineProps({
  onSaveAntenna: Function,
})
const router = useRouter();
const route = useRoute();
let dataSource = reactive({});
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = ref(route.params.mode);
const pageAntennaDescription = ref("Подробно об антенне");
const formRef = ref(null);

onBeforeMount(async () => {
  if (mode.value === "read") {
    const response = await antennaService.getAntenna(oid);
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
    pageAntennaDescription.value = "Создание антенны"
  }
})
function onClickEditAntenna() {
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
        const responseUpdate = await antennaService.updateAntenna(dataSource);
        if (responseUpdate.data.success) {
          notify({
            message: 'Антенна успешно отредактирована',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
        } else {
          notify(responseUpdate.data.messages, 'error', 2000);
        }
        isFormDisabled.value = true;
      } else {
        const response = await antennaService.createAntenna(dataSource);
        if (response.data.success) {
          notify({
            message: 'Антенна успешно создана',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
          await router.push({name: 'projectDetail', params: {mode: "read", id: response.data.result}});
          props.onSaveAntenna(response.data.result);
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