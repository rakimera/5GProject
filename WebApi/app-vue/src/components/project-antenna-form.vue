<template>
  <div>
    <div id="form-container">
      <dx-form
        id="project-antenna-form"
        ref="formRef"
        :col-count="1"
        :form-data="dataSource"
        label-location="top"
        :read-only="isFormDisabled"
        :show-colon-after-label="true"
        :show-validation-summary="true">
          <dx-group-item
                  :caption="'Антенна ' + (index + 1)"
                  name="phones-container"
                  v-for="(projectAntennaDto, index) in antennaOptions"
                  :key="'projectAntennaDto' + (index + 1)"
          >
              <dx-simple-item
                      :itemid="antennas"
                      :data-field="'projectAntennaDto[' + index + ']'"
                      editor-type="dxSelectBox"
                      :editor-options="{ 
                    placeholder: 'Выберите антенну', 
                    items: antennas, 
                    displayExpr: 'model', 
                    valueExpr: 'id',
                    labelMode: 'floating',
                    onValueChanged: OnSelectAntenna,
                    label: 'Антенна ' + (index + 1)}"
              >
                  <dx-label :text="'Антенна ' + (index + 1)"/>
              </dx-simple-item>
              <dx-group-item
                      item-type="group"
                      name="phones"
              >
                  <antenna-translator-form
                          :selectedAntennaId="selectedAntennaId">
                  </antenna-translator-form>
                  
              </dx-group-item>
          </dx-group-item>
          <dx-button-item
                  :button-options="addAntennaButtonOptions"
                  css-class="add-antenna-button"
                  horizontal-alignment="left"
          />
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
  DxLabel, DxButtonOptions
} from 'devextreme-vue/form';
import {computed, onBeforeMount, reactive, ref} from "vue";
import antennaService from "@/api/antennaService";
import projectAntennaService from "@/api/projectAntennaService";
import {useRoute} from "vue-router";
import notify from "devextreme/ui/notify";
import translatorService from "@/api/translatorService";
import AntennaTranslatorForm from "@/components/antenna-translator-form.vue";

let selectedAntennaId = ref();
let isFormDisabled = ref(true);
const formRef = ref(null);
const route = useRoute();
let id = route.params.id;
const mode = ref(route.params.mode);
let dataSource = reactive([]);
const antennas = ref([]);
const translators = ref([]);
const antennaOptions = ref(getAntennasOptions(dataSource));
const addAntennaButtonOptions = computed(() =>{
    return {
        icon: 'add',
        text: 'добавить антенну',
        disabled: isFormDisabled.value,
        onClick: () => {
            dataSource.push('');
            antennaOptions.value = getAntennasOptions(dataSource);
        },
    };
});

onBeforeMount(async () => {
  const response = await antennaService.getAntennae();
  antennas.value = response.data.result;
  
  const translatorsResponse = await translatorService.getTranslators();
  translators.value = translatorsResponse.data.result;

  if (mode.value === "read") {
    const response = await projectAntennaService.getAllByProjectId(id);
    console.log(response)
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
  }
})

function OnSelectAntenna(e){
    selectedAntennaId.value = e.value
    emit('selectedAntennaChanged', e.value);
}

function onClickEditProject() {
  isFormDisabled.value = false;
}
function getAntennasOptions(dataSource) {
  const options = [];
  for (let i = 0; i < dataSource.length; i += 1) {
    options.push(generateNewAntennaOptions(i));
  }
  return options;
}

function generateNewAntennaOptions(index) {
  return {
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
