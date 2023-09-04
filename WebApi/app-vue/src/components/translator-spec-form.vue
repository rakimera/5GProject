<template>
  <div>
    <dx-data-grid
        :data-source="dataSource"
        :show-borders="true"
        :remote-operations="true"
        :column-auto-width="true"
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
      <dx-toolbar>
        <dx-item name="addRowButton" show-text="always" location="before" widget="dxButton" :options="addButton">
        </dx-item>
      </dx-toolbar>
      <dx-column
          data-field="frequency"
          caption="Частота"
          data-type="number"
          :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
          alignment="left">
        <dx-label :visible="false"/>
        <dx-required-rule message="Частота передатчика не задана"></dx-required-rule>
      </dx-column>
      <dx-column
          data-field="power"
          data-type="number"
          caption="Мощность"
          :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
          alignment="left">
        <dx-label :visible="false"/>
        <dx-required-rule message="Мощность передатчика не задана"></dx-required-rule>
      </dx-column>
      <dx-column
          data-field="gain"
          data-type="number"
          caption="Коэффициент усиления антенны"
          :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
          alignment="left">
        <dx-label :visible="false"/>
        <dx-required-rule message="Коэффициент усиления сигнала передатчика не задан"></dx-required-rule>
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
      <dx-paging :page-size="5"/>
      <dx-pager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
      <dx-sorting mode="multiple"/>
    </dx-data-grid>
<!--    <dx-button-item>-->
<!--      <dx-button-options-->
<!--          width="20%"-->
<!--          type="default"-->
<!--          styling-mode="outlined"-->
<!--          :template="'Загрузить360'"-->
<!--          :on-click="load360"-->
<!--          :use-submit-behavior="true"-->
<!--      >-->
<!--      </dx-button-options>-->
<!--    </dx-button-item>-->
  </div>
</template>

<script setup>

import {
  DxLabel
} from 'devextreme-vue/form';
import {onMounted, ref, defineProps} from "vue";
import {
  DxColumn,
  DxDataGrid,
  DxEditing,
  DxFormItem,
  DxItem,
  DxSorting,
  DxPager,
  DxPaging, DxToolbar
} from "devextreme-vue/data-grid";
import {DxRequiredRule} from "devextreme-vue/validator";
import translatorSpecService from "@/api/translatorSpecsService";
import notify from "devextreme/ui/notify";
import CustomStore from "devextreme/data/custom_store";

const props = defineProps({
  masterDetailData: {
    type: Object,
    default: () => ({}),
  }})
const antennaId = ref();
let dataSource = ref(null);
const translators = ref([]);
const addButton = {
  text: "Добавить передатчики",
  icon: 'login',
  type: 'success',
  stylingMode:"contained"
}

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    const response = await translatorSpecService.getTranslatorSpecsForGrid(antennaId.value, loadOptions);
    console.log(response);
    return response;
  },
  async insert(values) {
    values.antennaId = antennaId.value;
    const baseResponse = await translatorSpecService.createTranslatorSpec(values);
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
  },
  async update(id, values) {
    console.log(id + values);
  }
});
async function onRowUpdating(options) {
  options.newData = Object.assign(options.oldData, options.newData);
  const baseResponse = await translatorSpecService.updateTranslatorSpec(options.newData);
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
}

onMounted(async () => {
  dataSource.value = store;
  console.log(props.masterDetailData);
  antennaId.value = props.masterDetailData.key;
  const response = await translatorSpecService.getAllByAntennaId(antennaId.value);
  translators.value = response.data.result;
})

</script>

<style scoped>
#form h2 {
  margin-left: 40px;
  font-weight: normal;
  font-size: 35px;
}
</style>


