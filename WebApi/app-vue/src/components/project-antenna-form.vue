<template>
    <div>
      <div id="data-grid-demo">
        <dx-data-grid
            :data-source="dataSource"
            :show-borders="true"
            :remote-operations="true"
            key-expr="id"
        >
          <dx-editing
              :allow-updating="true"
              :allow-adding="true"
              :allow-deleting="true"
              :texts="{confirmDeleteMessage: 'Вы уверены, что хотите удалить эту запись?'}"
              mode="form"
          />
          <dx-column
              data-field="antennaId"
              caption="Антенна"
              data-type="string"
          >
            <dx-required-rule message="Вы не выбрали антенну"></dx-required-rule>
            <dx-lookup
                :data-source="antennas"
                value-expr="id"
                display-expr="model"
            />
          </dx-column>
          <dx-column data-field="azimuth" data-type="number" caption="Азимут"
                     :editor-options="{stylingMode: 'filled', labelMode: 'floating'}">
            <dx-label :visible="false"/>
              <dx-required-rule message="Вы не запонели азимут"></dx-required-rule>
          </dx-column>
          <dx-column data-field="height" data-type="number" caption="Высота установки"
                     :editor-options="{stylingMode: 'filled', labelMode: 'floating'}">
            <dx-required-rule message="Вы не запонели высоту установки антенны"></dx-required-rule>
          </dx-column>
          <dx-column data-field="latitude" data-type="number" caption="Широта"
                     :editor-options="{stylingMode: 'filled', labelMode: 'floating'}">
            <dx-required-rule message="Вы не запонели широту установки антенны"></dx-required-rule>
          </dx-column>
          <dx-column data-field="longitude" data-type="number" caption="Долгота"
                     :editor-options="{stylingMode: 'filled', labelMode: 'floating'}">
            <dx-required-rule message="Вы не запонели долготу установки антенны"></dx-required-rule>
          </dx-column>
          <dx-column data-field="tilt" data-type="number" caption="Тильт"
                     :editor-options="{stylingMode: 'filled', labelMode: 'floating'}">
            <dx-required-rule message="Вы не запонели тильт антенны"></dx-required-rule>
          </dx-column>
          <dx-column data-field="projectId" data-type="string" :visible="false">
            <dx-form-item
                :editor-options="{
                disabled: true}"
                editor-type="dxTextArea"
                :visible="false"
                :data="projectId"
            />
          </dx-column>
          <dx-paging :page-size="5"/>
          <dx-pager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
        </dx-data-grid>
      </div>
    </div>
</template>
<script setup>
import {
  DxLabel
} from 'devextreme-vue/form';
import {onMounted, ref} from "vue";
import antennaService from "@/api/antennaService";
import projectAntennaService from "@/api/projectAntennaService";
import {useRoute} from "vue-router";
import {
  DxDataGrid,
  DxColumn,
  DxFormItem,
  DxPaging,
  DxEditing,
  DxPager,
  DxLookup
} from 'devextreme-vue/data-grid';
import 'devextreme-vue/text-area';
import {DxRequiredRule} from "devextreme-vue/validator";
import CustomStore from "devextreme/data/custom_store";
import notify from "devextreme/ui/notify";

const route = useRoute();
let projectId = route.params.id;
let dataSource = ref(null);
const antennas = ref([]);

const store = new CustomStore({
  key: "id",
  async load(loadOptions) {
    const response = await projectAntennaService.getProjectAntennaeForGrid(loadOptions, projectId);
    if (response.data.success) {
      notify({
        message: 'Данные сохранены',
        position: {
          my: 'center top',
          at: 'center top',
        },
      }, 'success', 1000);
    } else {
      notify(response.data.messages, 'error', 2000);
    }
    return response.data;
  },
  async insert(values) {
    values.projectId = projectId;
    const baseResponse = await projectAntennaService.createProjectAntenna(values)
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
    const baseResponse = await projectAntennaService.deleteProjectAntenna(id);
    if (baseResponse.data.success) {
      notify({
        message: 'Антенна удалена',
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
    values.id = id;
    const baseResponse = await projectAntennaService.updateProjectAntenna(values);
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
});

onMounted(async () => {
  dataSource.value = store;

  const antennaResponse = await antennaService.getAntennae();
  antennas.value = antennaResponse.data.result;
})

</script>
<style scoped>
#form-container {
    margin: 10px 10px 30px;
}
</style>
