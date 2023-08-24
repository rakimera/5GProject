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
      <dx-tabbed-item>
        <dx-tab-panel-options
            :defer-rendering="false"
        />
        <dx-tab
            title="Данные антенны"
        >
          <antenna-form
              :on-save-antenna="onSaveAntenna">
          </antenna-form>
        </dx-tab>
        
        <dx-tab
            title="Создание передатчика"
        >
          <translator-spec-form
              :on-save-antenna="onSaveTranslatorSpec">
            <dx-item
                data-field="antennaId"
                :visible="false"
                >
            </dx-item>
          </translator-spec-form>
        </dx-tab>
      </dx-tabbed-item>
    </dx-form>
  </div>
</template>
<script setup>

import {
  DxItem,
  DxForm,
  DxTabbedItem,
  DxTabPanelOptions,
  DxTab
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import antennaService from "@/api/antennaService";
import {useRoute} from "vue-router";
import AntennaForm from "@/components/antenna-form.vue";
import TranslatorSpecForm from "@/components/translatorSpec-form";

const route = useRoute();
let dataSource = reactive({});
let isFormDisabled = ref(true);
let isTabDisabled = ref(true);
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

function onSaveAntenna() {
  isTabDisabled.value = false;
  isFormDisabled.value = true;
}

function onSaveTranslatorSpec() {
  isTabDisabled.value = false;
  isFormDisabled.value = true;
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