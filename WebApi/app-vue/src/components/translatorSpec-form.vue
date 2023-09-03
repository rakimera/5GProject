<template>
  <div>
    <div id="data-grid-demo">
      <dx-data-grid
          :data-source="dataSource"
          :show-borders="true"
          :remote-operations="true"
          key-expr="id"
          @row-updating="onRowUpdating"
      >
        <dx-editing
            :allow-updating="true"
            :allow-adding="true"
            :allow-deleting="true"
            :texts="{confirmDeleteMessage: 'Вы уверены, что хотите удалить эту запись?'}"
            mode="form"
        />
        <dx-column
            data-field="frequency"
            caption="Частота"
            data-type="number"
            :editor-options="{
                stylingMode: 'filled', 
                labelMode: 'floating'}"
        >
          <dx-label :visible="false"/>
          <dx-required-rule message="Частота не задана"></dx-required-rule>
        </dx-column>
        <dx-column 
            data-field="power" 
            data-type="number" 
            caption="Мощность"
            :editor-options="{
                stylingMode: 'filled', 
                labelMode: 'floating'}">
          <dx-label :visible="false"/>
          <dx-required-rule message="Мощность не задана"></dx-required-rule>
        </dx-column>
        <dx-column
            data-field="gain"
            data-type="number"
            caption="Коэффициент усиления антенны"
            :editor-options="{
                stylingMode: 'filled', 
                labelMode: 'floating'}">
          <dx-label :visible="false"/>
          <dx-required-rule message="Коэффициент усиления антенны не задан"></dx-required-rule>
        </dx-column>
        <dx-column data-field="antennaId" data-type="string" :visible="false">
          <dx-form-item
              :editor-options="{
                disabled: true}"
              editor-type="dxTextArea"
              :visible="false"
              :data="antennaId"
          />
        </dx-column>
        <dx-paging :page-size="5"/>
        <dx-pager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
      </dx-data-grid>
    </div>
  </div>
  
  <dx-button-item>
    <dx-button-options
        width="20%"
        type="default"
        styling-mode="outlined"
        :template="'Загрузить360'"
        :on-click="load360"
        :use-submit-behavior="true"
    >
    </dx-button-options>
  </dx-button-item>
</template>

<script setup>

import {
  DxButtonItem,
  DxButtonOptions,
  DxLabel
} from "devextreme-vue/form";
import {
  DxDataGrid,
  DxColumn,
  DxFormItem,
  DxPaging,
  DxEditing,
  DxPager
} from 'devextreme-vue/data-grid';
import 'devextreme-vue/text-area';
import {
  DxRequiredRule
} from 'devextreme-vue/validator';
import {ref, defineProps, onMounted} from "vue";
import {useRoute} from "vue-router";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";
import translatorSpecsService from "@/api/translatorSpecsService";
const props = defineProps({
  onSaveTranslatorSpec: Function,
  antennaId: String
})
const route = useRoute();
let oid = route.params.id;
let dataSource = ref(null);
const translatorSpecs = ref([]);

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    return await translatorSpecsService.getTranslatorSpecsForGrid(loadOptions, oid);
  },
  async insert(values) {
    values.antennaId = props.antennaId;
    const baseResponse = await translatorSpecsService.createTranslatorSpec(values);
    await dataSource.value.load();
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
    return {data: baseResponse};
  },
  // async mounted() {
  //   dataSource.invoke('Add', { fieldName: oid }, 'POST');
  // },
  async remove(id) {
    const baseResponse = await translatorSpecsService.deleteTranslatorSpec(id);
    if (baseResponse.data.success) {
      notify({
        message: 'Передатчик удалён',
        position: {
          my: 'center top',
          at: 'center top',
        },
      }, 'success', 1000);
    } else {
      notify(baseResponse.data.messages, 'error', 2000);
    }
    return {data: baseResponse};
  },
  async update(id, values) {
    console.log(id + values)
  }
});

async function onRowUpdating(options) {
  options.newData = Object.assign(options.oldData, options.newData);
  const baseResponse = await translatorSpecsService.updateTranslatorSpec(options.newData);
  await dataSource.value.load();
  if (baseResponse.data.success) {
    notify({
      message: 'Данные изменены',
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

onMounted(async () => {
  dataSource.value = store;

  const response = await translatorSpecsService.getTranslatorSpecs();
  translatorSpecs.value = response.data.result;
})

</script>

<style scoped>
#form h2 {
  margin-left: 40px;
  font-weight: normal;
  font-size: 35px;
}
</style>

