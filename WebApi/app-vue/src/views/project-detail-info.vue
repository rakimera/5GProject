<template>
    <div class="project-form">
        <h2 v-text="pageDescription"></h2>
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
                    :selected-index="index"
                />
                <dx-tab
                    title="Контрагент и адрес установки"
                    tabIndex=0
                >
                  <project-form
                      :on-save-project="onSaveProject">
                  </project-form>
                </dx-tab>
                <dx-tab
                    title="Антенны-передатчики"
                    tabIndex=1
                    :disabled="isTabDisabled"
                >
                  <project-antenna-form
                  >
                  </project-antenna-form>
                </dx-tab>
                <dx-tab
                    tabIndex=2
                    title="Фото мест установки"
                    :disabled="isTabDisabled"
                >
                    <dx-item
                        data-field='house'
                        editor-type='dxTextBox'
                        :editor-options="{ stylingMode: 'filled', placeholder: 'Номер здания' }"
                    >
                        <dx-label
                            :text="'Номер здания'"
                        />
                    </dx-item>
                </dx-tab>
            </dx-tabbed-item>
        </dx-form>
    </div>
</template>
<script setup>

import {
    DxForm,
    DxLabel,
    DxTabbedItem,
    DxTabPanelOptions,
    DxTab, DxItem,
} from "devextreme-vue/form";

import {onBeforeMount, reactive, ref} from "vue";
import projectService from "@/api/projectService";
import {useRoute, /*useRouter*/} from "vue-router";
import contrAgentService from "@/api/contrAgentService";
import townService from "@/api/townService";
import ProjectForm from "@/components/project-form.vue";
import ProjectAntennaForm from "@/components/project-antenna-form.vue";

const route = useRoute();
/*const router = useRouter();*/
let dataSource = reactive({});
//const routeParams = {name: "projects"};
let isFormDisabled = ref(true);
let isTabDisabled = ref(true);
let oid = route.params.id;
const mode = ref(route.params.mode);
const pageDescription = ref("Подробно о проекте");
const formRef = ref(null);
const contrAgents = ref([]);
const towns = ref([]);
const index = ref(0);

onBeforeMount(async () => {
  const response = await contrAgentService.getContrAgents();
  contrAgents.value = response.data.result;

  const townResponse = await townService.getTowns();
  towns.value = townResponse.data.result;
  
  if (mode.value === "read") {
      const response = await projectService.getProject(oid);
      Object.assign(dataSource, response.data.result);
      isTabDisabled.value = false;
  } else {
      isFormDisabled.value = false;
      pageDescription.value = "Создание проекта"
  }
})

function onSaveProject() {
  isTabDisabled.value = false;
  isFormDisabled.value = true;
  index.value++
}
</script>
<style scoped>

.project-form {
    max-width: 1000px;
    margin: 50px auto auto;
}
#form h2 {
    margin-left: 40px;
    font-weight: normal;
    font-size: 35px;
}
.field-container {
    border: 1px solid #d3d3d3;
    margin: 20px 20px 0 0px;
    
}
</style>