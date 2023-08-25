<template>
  <div>
    <div class="long-title"><h3>Personal details</h3></div>
    <div id="form-container">
      <dx-form
          id="project-antenna-form"
          ref="formRef"
          :col-count="1"
          :form-data="dataSource"
          label-location="top"
          :read-only="isFormDisabled"
          :show-colon-after-label="true"
          :show-validation-summary="true"
      >
        <dx-group-item
            caption="Phones"
            name="phones-container"
        >
          <dx-group-item
              item-type="group"
              name="phones"
          >
            <dx-simple-item
                v-for="(projectAntennaDto, index) in antennaOptions"
                :key="'projectAntennaDto' + (index + 1)"
                :data-field="'projectAntennaDto[' + index + ']'"
                editor-type="dxSelectBox"
                :editor-options="{ 
                        placeholder: 'Выберите антенну', 
                        items: antennas, 
                        displayExpr: 'model', 
                        valueExpr: 'id',
                        labelMode: 'floating',
                        label: 'Антенна ' + (index + 1)}"
            >
              <dx-label :text="'Антенна ' + (index + 1)"/>
              <dx-simple-item
                  v-for="(translatorSpecsDto, index) in translatorOptions"
                  :key="'antennaTranslatorDto' + (index + 1)"
                  :data-field="'antennaTranslatorDto[' + index + ']'"
                  editor-type="dxSelectBox"
                  :editor-options="{ 
                        placeholder: 'Выберите транслятор', 
                        items: antennas, 
                        displayExpr: 'frequency', 
                        valueExpr: 'id',
                        labelMode: 'floating',
                        label: 'Транслятор ' + (index + 1)}"
              >
                <dx-label :text="'Антенна ' + (index + 1)"/>
              </dx-simple-item>
            </dx-simple-item>
          </dx-group-item>
          <dx-button-item
              :button-options="addAntennaButtonOptions"
              css-class="add-antenna-button"
              horizontal-alignment="left"
          />
        </dx-group-item>
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
    </div>
  </div>
</template>
<script setup>
import {
  DxForm,
  DxSimpleItem,
  DxGroupItem,
  DxButtonItem,
  DxLabel, DxButtonOptions,
} from 'devextreme-vue/form';
import {onBeforeMount, reactive, ref} from "vue";
import antennaService from "@/api/antennaService";
import projectAntennaService from "@/api/projectAntennaService";
import {useRoute} from "vue-router";
import notify from "devextreme/ui/notify";

let isFormDisabled = ref(true);
const formRef = ref(null);
const route = useRoute();
let id = route.params.id;
const mode = ref(route.params.mode);
let dataSource = reactive({});
const antennas = ref([]);
const translators = ref([]);
const antennaOptions = ref(getAntennasOptions(antennas.value));
const translatorOptions = ref(getAntennasOptions(translators.value));
const addAntennaButtonOptions = {
  icon: 'add',
  text: 'добавить антенну',
  onClick: () => {
    antennas.value.push('');
    antennaOptions.value = getAntennasOptions(antennas.value);
  },
};

onBeforeMount(async () => {
  const response = await antennaService.getAntennae();
  antennas.value = response.data.result;
  
  const translatorsResponse = await translatorService.getAntennae();
  translators.value = translatorsResponse.data.result;

  if (mode.value === "read") {
    const response = await projectAntennaService.getAllByProjectId(id);
    console.log(response)
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
  }
})

function onClickEditProject() {
  isFormDisabled.value = false;
}
function getAntennasOptions(antennas) {
  const options = [];
  for (let i = 0; i < antennas.length; i += 1) {
    options.push(generateNewAntennaOptions(i));
  }
  return options;
}
function generateNewAntennaOptions(index) {
  return {
    mask: '+1 (X00) 000-0000',
    maskRules: { X: /[01-9]/ },
    buttons: [{
      name: 'trash',
      location: 'after',
      options: {
        stylingMode: 'text',
        icon: 'trash',
        onClick: () => {
          antennas.value.splice(index, 1);
          antennaOptions.value = getAntennasOptions(antennas.value);
        },
      },
    }],
  };
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
        const responseUpdate = await projectAntennaService.updateProjectAntenna(dataSource);
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
        isFormDisabled.value = true;
      } else {
        const response = await projectAntennaService.createProjectAntenna(dataSource);
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
#form-container {
  margin: 10px 10px 30px;
}

.long-title h3 {
  font-family:
      'Segoe UI Light',
      'Helvetica Neue Light',
      'Segoe UI',
      'Helvetica Neue',
      'Trebuchet MS',
      Verdana;
  font-weight: 200;
  font-size: 28px;
  text-align: center;
  margin-bottom: 20px;
}

.add-antenna-button {
  margin-top: 10px;
}
</style>
