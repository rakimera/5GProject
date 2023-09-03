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
            :selectedIndex="selectedIndex"
        />
          <dx-tab
              title="Данные антенны"
              tabIndex=0
          >
            <antenna-form
                :on-save-antenna="onSaveAntenna"
                :translatorSpecId="translatorSpecId">
                
            </antenna-form>
          </dx-tab>
          <dx-tab
              title="Передатчики"
              tabIndex=1
              :disabled="isTabDisabled"
          >
            <translator-spec-form
                :on-save-translatorSpec="onSaveTranslatorSpec"
                :antennaId="antennaId">
            </translator-spec-form>
          </dx-tab>
      </dx-tabbed-item>
    </dx-form>
  </div>
</template>
<script setup>

import {
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
// const router = useRouter();
let dataSource = reactive({});
let isFormDisabled = ref(true);
let isTabDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
let antennaId = ref(null);
let translatorSpecId = ref(null);
const pageAntennaDescription = ref("Подробно об антенне");
const formRef = ref(null);
const index = ref(0);
const selectedIndex = ref(0);

onBeforeMount(async () => {
  if (mode === "read") {
    const response = await antennaService.getAntenna(oid);
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
    pageAntennaDescription.value = "Создание антенны"
  }
})

function onSaveAntenna(e) {
  isTabDisabled.value = false;
  isFormDisabled.value = true;
  index.value++
  antennaId.value = e;
  selectedIndex.value = 1;
  // router.push({name: 'translatorSpecForm', params: {mode: "insert", id: null}});
}

function onSaveTranslatorSpec(e) {
  isTabDisabled.value = false;
  isFormDisabled.value = true;
  translatorSpecId.value = e;
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