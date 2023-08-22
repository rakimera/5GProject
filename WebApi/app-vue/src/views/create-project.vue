<template>
  <form class="project-form" @submit.prevent="onSubmit">
    <dx-form
        :col-count="1"
        enctype="multipart/form-data"
        :form-data="formData"
        :disabled="loading"
    >
    <dx-group-item
        caption="Заполение нового проекта"
    >
      <dx-tabbed-item>
        <dx-tab-panel-options
            :defer-rendering="false"
        />
        <dx-tab
            title="Контрагент"
        >
          <div class="fileuploader-container">
            <dx-select-box
                :data-source="contrAgents"
                :input-attr="{ 'aria-label': 'Контрагенты' }"
                label="Выберите контрагента"
                display-expr="companyName"
                value-expr="id"
                name ='contrAgentId'
                v-model="formData.contrAgentId"
            />
          </div>
        </dx-tab>
        <dx-tab
            title="Адрес"
        >
          <dx-item
              data-field='street'
              editor-type='dxTextBox'
              :editor-options="{ stylingMode: 'filled', placeholder: 'Улица' }"
          >
            <dx-required-rule message="Введите утицу на которой расположен объект"/>
            <dx-label :visible="false" />
          </dx-item>
          <dx-item
              data-field='house'
              editor-type='dxTextBox'
              :editor-options="{ stylingMode: 'filled', placeholder: 'Номер здания' }"
          >
            <dx-required-rule message="Введите номер здания на котором расположен объект"/>
            <dx-label :visible="false" />
          </dx-item>
            <dx-item
                    data-field='townId'
                    editor-type='dxTextBox'
                    :editor-options="{ stylingMode: 'filled', placeholder: 'id города' }"
            >
                <dx-label :visible="false" />
            </dx-item>
            <dx-item
                    data-field='districtId'
                    editor-type='dxTextBox'
                    :editor-options="{ stylingMode: 'filled', placeholder: 'id района' }"
            >
                <dx-label :visible="false" />
            </dx-item>            
        </dx-tab>
        <dx-tab
            title="Фото места установки"
        >
          <div class="fileuploader-container">
            <dx-file-uploader
                select-button-text="Select photo"
                label-text=""
                accept="image/*"
                upload-mode="useForm"
            />
          </div>
        </dx-tab>
      </dx-tabbed-item>
    </dx-group-item>
    </dx-form>
    <DxButton
        class="button"
        text="Создать"
        type="success"
        :use-submit-behavior="true"
    />
  </form>
</template>
<script setup>
import { DxFileUploader } from 'devextreme-vue/file-uploader';
import { DxButton } from 'devextreme-vue/button';
import notify from 'devextreme/ui/notify';
import DxSelectBox from 'devextreme-vue/select-box';
import {
  DxForm, DxGroupItem, DxTabbedItem, DxTabPanelOptions, DxTab, DxRequiredRule, DxLabel, DxItem,
} from 'devextreme-vue/form';
import {reactive, ref, onBeforeMount} from "vue";
import { useRoute, useRouter } from 'vue-router';
import contrAgentService from "@/api/contrAgentService";
import projectService from "@/api/projectService";

const formData = reactive({});
const route = useRoute();
const router = useRouter();
const loading = ref(false);
const contrAgents = ref([]);

onBeforeMount(async () => {
  loading.value = true;
  const response = await contrAgentService.getContrAgents();
  contrAgents.value = response.data.result;
  console.log(contrAgents)
  loading.value = false;
})
async function onSubmit() {
  loading.value = true;
  try {
    await projectService.createProject(formData)
    await router.push(route.query.redirect || '/projects');
    notify('Uncomment the line to enable sending a form to the server.');
  } catch (error){
    loading.value = false;
    notify(error.message, 'error', 2000);
  }
}
</script>
<style>
.project-form {
  max-width: 1000px;
  margin: auto;
  margin-top: 50px;
}
.button {
  margin-top: 50px;
  margin-right: 20px;
  float: right;
}
.fileuploader-container {
  border: 1px solid #d3d3d3;
  margin: 20px 20px 0 20px;
}
#form h3 {
  margin-left: 20px;
  font-weight: normal;
  font-size: 22px;
}
</style>
