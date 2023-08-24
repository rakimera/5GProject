<template>
  <h2 v-text="pageTranslatorSpecDescription"></h2>
  <dx-form
      id="translatorSpec-form"
      ref="formRef"
      label-location="top"
      :form-data="dataSource"
      :read-only="isFormDisabled"
      :show-colon-after-label="true"
      :show-validation-summary="true"
  >
    <dx-item
        data-field="frequency"
        editor-type='dxTextBox'
        :editor-options="{ 
                      stylingMode: 'filled',
                      labelMode: 'floating',
                      label: 'Частота' }">
      <dx-label :text="false"/>
      <dx-required-rule message="Частота передатчика должна быть заполнена"/>
    </dx-item>
    <dx-item
        data-field="power">
      <dx-label :text="'Мощность'"/>
      <dx-required-rule message="Мощность передатчика должна быть заполнена"/>
    </dx-item>
    <dx-item
        data-field="gain"
        editor-type='dxTextBox'
        :editor-options="{ 
                      stylingMode: 'filled',
                      labelMode: 'floating',
                      label: 'Коэффициент усиления антенны' }">
      <dx-label :text="false"/>
      <dx-required-rule message="Коэффициент усиления антенны должен быть заполнен"/>
    </dx-item>
    <dx-item
        data-field="antennaId"
        :visible="false"
        :options="{
              value: antennaIdProp}">
      <antenna-form
          :antenna-id="antennaIdProp">
      </antenna-form>
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
          :on-click="onClickEditTranslatorSpec"
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
import AntennaForm from "@/components/antenna-form.vue";
import {onBeforeMount, reactive, ref, defineProps} from "vue";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";
import translatorSpecsService from "@/api/translatorSpecsService";


const props = defineProps({
  onSaveTranslatorSpec: Function,
  antennaIdProp: String
})
const router = useRouter();
const route = useRoute();
let dataSource = reactive({});
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = ref(route.params.mode);
const pageTranslatorSpecDescription = ref("Подробно о передатчике");
const formRef = ref(null);

onBeforeMount(async () => {
  if (mode.value === "read") {
    const response = await translatorSpecsService.getTranslatorSpec(oid);
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
    pageTranslatorSpecDescription.value = "Создание передатчика"
  }
})
function onClickEditTranslatorSpec() {
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
        const responseUpdate = await translatorSpecsService.updateTranslatorSpec(dataSource);
        if (responseUpdate.data.success) {
          notify({
            message: 'Передатчик успешно отредактирован',
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
        const response = await translatorSpecsService.createTranslatorSpec(dataSource);
        if (response.data.success) {
          notify({
            message: 'Передатчик успешно создан',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
          await router.push({name: 'projectDetail', params: {mode: "read", id: response.data.result}});
          props.antennaIdProp();
          props.onSaveTranslatorSpec();
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
