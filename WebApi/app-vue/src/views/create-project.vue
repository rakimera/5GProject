<template>
  <form class="project-form" @submit.prevent="onSubmit">
    <dx-Form
        :col-count="1"
        enctype="multipart/form-data"
        :form-data="formData"
        :disabled="loading"
    >
    <DxGroupItem
        caption="Заполение нового проекта"
    >
      <DxTabbedItem>
        <DxTabPanelOptions
            :defer-rendering="false"
        />
        <DxTab
            title="Контрагент"
        >
          <div class="fileuploader-container" v-if="counterAgentsLoaded">
            <DxSelectBox
                :data-source="dataSource"
                :input-attr="{ 'aria-label': 'Контрагенты' }"
                label="Выберите контрагента"
                display-expr="companyName"
                value-expr="id"
                v-model="formData.contrAgentId"
            />
          </div>
          <div v-else>Loading...</div>
        </DxTab>
        <DxTab
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
        </DxTab>
        <DxTab
            title="Фото места установки"
        >
          <div class="fileuploader-container">
            <DxFileUploader
                select-button-text="Select photo"
                label-text=""
                accept="image/*"
                upload-mode="useForm"
            />
          </div>
        </DxTab>
      </DxTabbedItem>
    </DxGroupItem>
    </dx-Form>
    <DxButton
        class="button"
        text="Создать"
        type="success"
        :use-submit-behavior="true"
    />
  </form>
</template>
<script>
import { DxFileUploader } from 'devextreme-vue/file-uploader';
import { DxButton } from 'devextreme-vue/button';
import notify from 'devextreme/ui/notify';
import DxSelectBox from 'devextreme-vue/select-box';
import {
  DxForm, DxGroupItem, DxTabbedItem, DxTabPanelOptions, DxTab, DxRequiredRule, DxLabel, DxItem,
} from 'devextreme-vue/form';
import {reactive, ref} from "vue";
import { useRoute, useRouter } from 'vue-router';
import counterAgentService from "@/api/counterAgentService";
import projectService from "@/api/projectService";

export default {
  setup(){
    const formData = reactive({
      contrAgentId:"",
      districtId: "",
      townId: "",
      street: "",
      house: ""
    });
    
    const route = useRoute();
    const router = useRouter();
    const loading = ref(false);
    const counterAgents = ref([]);
    const counterAgentsLoaded = ref(false);

    async function fetchCounterAgents() {
      try {
        const response = await counterAgentService.getContrAgents();
        counterAgents.value = response.data.result;
        counterAgentsLoaded.value = true;
      } catch (error) {
        console.error("Ошибка получения агнетов (не секретных):", error);
      }
    }

    fetchCounterAgents()
    async function onSubmit() {
      loading.value = true;
      const CreateProjectDto = {
        contrAgentId: formData.contrAgentId, 
        districtId: "854",
        townId: "543",
        street: formData.street,
        house: formData.house};
      try {
        await projectService.createProject(CreateProjectDto)
        await router.push(route.query.redirect || '/projects');
        notify('Uncomment the line to enable sending a form to the server.');
      } catch (error){
        loading.value = false;
        notify(error.message, 'error', 2000);
      }
    }

    return {
      formData,
      loading,
      onSubmit,
      counterAgents,
      counterAgentsLoaded,
      dataSource: counterAgents,
    }
  },
    
  components: {
    DxItem, DxLabel, DxRequiredRule,
    DxFileUploader,
    DxButton,
    DxGroupItem,
    DxTabbedItem,
    DxTabPanelOptions,
    DxTab,
    DxForm,
    DxSelectBox,
  },
};

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
.dx-fieldset{
  margin-left: 0px;
}

#form h3 {
  margin-left: 20px;
  font-weight: normal;
  font-size: 22px;
}
</style>
