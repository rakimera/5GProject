<template>
  <div class="antenna-form">
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
              title="Данные по антеннам"
              tabIndex=0
          >
            <antenna-form
                :on-save-antenna="onSaveAntenna"
                :translatorSpecId="translatorSpecId">
            </antenna-form>
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


const route = useRoute();
let dataSource = reactive({});
let isFormDisabled = ref(true);
let isTabDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
let antennaId = ref(null);
let translatorSpecId = ref(null);
const formRef = ref(null);
const index = ref(0);

onBeforeMount(async () => {
  if (mode === "read") {
    const response = await antennaService.getAntenna(oid);
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
  }
})

function onSaveAntenna(e) {
  isTabDisabled.value = false;
  isFormDisabled.value = true;
  index.value++
  antennaId.value = e;
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
