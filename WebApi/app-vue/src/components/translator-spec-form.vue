<template>
  <div>
    <dx-data-grid
        :data-source="dataSource"
        :show-borders="true"
        :remote-operations="false"
        :column-auto-width="true"
        key-expr="id"
    >
      <dx-editing
          :allow-updating="false"
          :allow-deleting="true"
          :texts="{confirmDeleteMessage: 'Вы уверены, что хотите удалить эту запись?'}"
          mode="form"
      >
      </dx-editing>
      <dx-toolbar>
        <dx-item show-text="always" location="before" @click="addClick" widget="dxButton" :options="addButton">
        <dx-item name="saveButton"/>
        </dx-item>
      </dx-toolbar>
      <dx-column
          data-field="frequency"
          caption="Частота"
          data-type="number"
          :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
          alignment="left">
        <dx-label :visible="false"/>
      </dx-column>
      <dx-column
          data-field="antennaId"
          data-type="string"
          :visible="false">
        <dx-form-item
            :editor-options="{
                disabled: true}"
            editor-type="dxTextArea"
            :visible="false"
            :data="antennaId"
        />
      </dx-column>
      <dx-column
          data-field="id"
          :visible="false">
      </dx-column>
      <dx-paging :page-size="5"/>
      <dx-pager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
      <dx-sorting mode="multiple"/>
        <DxMasterDetail
                :enabled="true"
                template="master-detail"
        />
        <template #master-detail="{ data }">
            <radiation-zone-grid :master-detail-data="data"/>
        </template>
    </dx-data-grid>
    <dx-popup :show-title="true" :width="700" title="Добавление передатчика" v-model:visible="popupVisible" :disabled="loading">
      <div class="position-absolute top-50 start-50 button-indicator">
        <dx-load-indicator
            id="large-indicator"
            :height="60"
            :width="60"
            :visible="loading"
        />
      </div>
      <form
          id="form"
          ref="formRef"
          method="post"
          enctype="multipart/form-data"
      >
        <div class="dx-fieldset">
          <div class="dx-field">
            <dx-number-box
                :input-attr="{ 'aria-label': 'Full Name' }"
                name="frequency"
                data-type="number"
                class="dx-field-value"
                caption="Частота"
                label="Частота"
                :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
            >
            </dx-number-box>
          </div>
        </div>
        <div class="fileuploader-container">
          <dx-file-uploader
              select-button-text="Добавить вертикаль"
              :allowed-file-extensions="['.csv', '.xlsx', '.xlsm']"
              labelText="Или перенесите файл сюда"
              ref="verticalFileUploaderRef"
              :showFileList= "true"
              :max-file-size="4000000"
              accept=".csv, .xlsx, .xlsm"
              :multiple="false"
              upload-mode="useForm"
              name="vertical"
          />
        </div>
        <div class="fileuploader-container">
          <dx-file-uploader
              select-button-text="Добавить горизонталь"
              labelText="Или перенесите файл сюда"
              ref="horizontalFileUploaderRef"
              :allowed-file-extensions="['.csv', '.xlsx', '.xlsm']"
              :showFileList= "true"
              :max-file-size="4000000"
              accept=".csv, .xlsx, .xlsm"
              :multiple="false"
              upload-mode="useForm"
              name="horizontal"
          />
        </div>
        <p class="mt-5 text-center">Доступны файлы размером не более 4мб с расширением: .csv, .xlsx, .xlsm</p>
        <input
            name='antennaId'
            :value="antennaId"
            hidden="true"
        >
        <dx-button
            class="button"
            text="Сохранить"
            type="success"
            @click="onButtonClick"
        >
        </dx-button>
        <dx-button
            class="button"
            text="Отмена"
            type="danger"
            @click="onButtonCancelClick"
        />
        <dx-button
            class="button"
            text="Скачать шаблон"
            type="normal"
            @click="downloadFile"
        />
      </form>
    </dx-popup>
  </div>
</template>

<script setup>

import { DxNumberBox } from 'devextreme-vue/number-box';
import { DxButton } from 'devextreme-vue/button';
import {
  DxLabel,
} from 'devextreme-vue/form';
import {onMounted, ref, defineProps} from "vue";
import { DxItem } from "devextreme-vue/form";
import {
    DxColumn,
    DxDataGrid,
    DxEditing,
    DxFormItem,
    DxSorting,
    DxPager,
    DxPaging, DxToolbar, DxMasterDetail
} from "devextreme-vue/data-grid";
import { DxPopup } from 'devextreme-vue/popup';
import translatorSpecService from "@/api/translatorSpecsService";
import notify from "devextreme/ui/notify";
import CustomStore from "devextreme/data/custom_store";
import {DxFileUploader} from "devextreme-vue/file-uploader";
import RadiationZoneGrid from "@/components/radiation-zone-grid.vue";
import DxLoadIndicator from "devextreme-vue/load-indicator";
import radiationZoneService from "@/api/radiationZoneService";

const props = defineProps({
  masterDetailData: {
    type: Object,
    default: () => ({}),
  }})
const antennaId = ref();
let dataSource = ref(null);
const addButton = {
  text: "Добавить передатчики",
  icon: 'login',
  type: 'success',
  stylingMode:"contained"
};
const popupVisible = ref(false);
const formRef = ref(null);
const verticalFileUploaderRef = ref(null);
const horizontalFileUploaderRef = ref(null);
const loading = ref(false);

async function downloadFile(){
  try {
    const response = await radiationZoneService.getRadiationZonesTemp();
   
    
      const blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const blobUrl = window.URL.createObjectURL(blob);

      const link = document.createElement('a');
      link.href = blobUrl;
      link.download = 'template.xlsx';
      link.click();

      window.URL.revokeObjectURL(blobUrl);
   
  } catch (e){
    notify("Ошибка при получении шаблона" + e, 'error', 2000);
    }
}

async function onButtonClick(){
  loading.value = true;
   await store.insert(formRef.value)
}

function onButtonCancelClick(){
  const formElement = formRef.value;
  if (formElement) {
    formElement.reset();
  }
  verticalFileUploaderRef.value.instance.reset();
  horizontalFileUploaderRef.value.instance.reset();
  
  popupVisible.value = false;
}
function addClick (){
  popupVisible.value= true;
}

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    const response = await translatorSpecService.getTranslatorSpecsForGrid(antennaId.value, loadOptions);
    console.log(response);
    return response;
  },
  async insert(values) {
    try {
        const baseResponse = await translatorSpecService.createTranslatorSpec(values);
        if (baseResponse.data.success) {
          notify({
            message: 'Данные сохранены',
            position: {
              my: 'center top',
              at: 'center top',
            },
          }, 'success', 1000);
        } else {
          notify(baseResponse.data.messages, 'error', 2000);
        }
        onButtonCancelClick()
        dataSource.value = await updateData();
        return {data: baseResponse};
    } catch (e){
      notify("Ошибка при отправке на сервер проверьте заполненные данные" + e, 'error', 2000);
    }
    finally {
      loading.value = false;
    }
  },
  async remove(id) {
    const baseResponse = await translatorSpecService.deleteTranslatorSpec(id);
    if (baseResponse.data.success) {
      notify({
        message: 'Передатчик удален',
        position: {
          my: 'center top',
          at: 'center top',
        },
      }, 'success', 1000);
    } else {
      notify(baseResponse.data.messages, 'error', 2000);
    }
    return {data: baseResponse};
  }
});

onMounted(async () => {
  dataSource.value = store;
  antennaId.value = props.masterDetailData.key;
});

async function updateData(){
    const response = await translatorSpecService.getAllByAntennaId(antennaId.value);
    return response.data.result;
}

</script>

<style scoped>
#form {
  max-width: 600px;
  margin: auto;
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
.dx-field-value-static, .dx-field-value:not(.dx-switch):not(.dx-checkbox):not(.dx-button) {
  width: 100%;
}
#button .button-indicator {
  height: 32px;
  width: 32px;
  display: inline-block;
  vertical-align: middle;
  margin-right: 5px;
}
</style>


