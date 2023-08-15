<template>
  <div class="antenna-form">
    <h2 v-text="pageAntennaDescription"></h2>
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
          data-field="model">
        <dx-label :text="'Модель'"/>
        <dx-required-rule message="Модель антенны должны быть заполнена"/>
      </dx-simple-item>
      <dx-simple-item
          data-field="verticalSizeDiameter">
        <dx-label :text="'Вертикальный размер(диаметр антенны)'"/>
        <dx-required-rule message="Вертикальный размер(диаметр антенны) должнен быть заполнен"/>
      </dx-simple-item>
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
  DxRequiredRule,
  DxButtonItem,
  DxButtonOptions,
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import antennaService from "@/api/antennaService";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";

const route = useRoute();
const router = useRouter();
let dataSource = reactive({});
const routeParams = {name: "Журнал антенн"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageAntennaDescription = ref("Подробно об антенне");
const formRef = ref(null);

onBeforeMount(async () => {
  if (mode === "read") {
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
      if (mode === "read") {
        const responseUpdate = await antennaService.updateAntenna(dataSource);
        if (responseUpdate.data.success) {
          notify({
            message: 'Антенна успешно отредактирована',
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
        const response = await antennaService.createAntenna(dataSource);
        if (response.data.success) {
          notify({
            message: 'Антенна успешно создана',
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

.antenna-form {
  max-width: 1000px;
  margin: 50px auto auto;
}
#form h2 {
  margin-left: 40px;
  font-weight: normal;
  font-size: 35px;
}
</style>