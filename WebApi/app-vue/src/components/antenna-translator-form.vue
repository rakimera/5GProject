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
      <dx-column
          data-field="translatorSpecsId"
          caption="Частота"
          data-type="string"
          alignment="left">
        <dx-required-rule message="Вы не выбрали частоту"></dx-required-rule>
        <dx-lookup
            :data-source="translators"
            value-expr="id"
            display-expr="frequency"
        />
      </dx-column>
      <dx-column
          data-field="power"
          data-type="number"
          caption="Мощность"
          :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
          alignment="left">
        <dx-label :visible="false"/>
        <dx-required-rule message="Вы не заполнили мощность"></dx-required-rule>
      </dx-column>
      <dx-column
          data-field="transmitLossFactor"
          data-type="number"
          caption="КФ потери сигнала"
          :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
          alignment="left">
        <dx-label :visible="false"/>
        <dx-required-rule message="Вы не заполнили коэффициент потери сигнала"></dx-required-rule>
      </dx-column>
      <dx-column
          data-field="translatorTypeId"
          caption="Тип передатчика"
          data-type="string"
          alignment="left">
        <dx-lookup
            :data-source="translatorTypes"
            value-expr="id"
            display-expr="frequency"
        />
      </dx-column>
      <dx-column
          data-field="gain"
          data-type="number"
          caption="КФ усиления"
          :editor-options="{stylingMode: 'filled', labelMode: 'floating'}"
          alignment="left">
        <dx-required-rule message="Вы не запонели коэффициент усиления сигнала"></dx-required-rule>
      </dx-column>
      <dx-column
          data-field="projectAntennaId"
          data-type="string"
          :visible="false">
        <dx-form-item
            :editor-options="{
                disabled: true}"
            editor-type="dxTextArea"
            :visible="false"
            :data="projectAntennaId"
        />
      </dx-column>
      <dx-paging :page-size="5"/>
      <dx-pager :show-page-size-selector="true" :allowed-page-sizes="[8, 12, 20]"/>
    </dx-data-grid>
  </div>
</template>
<script setup>
import {
    DxLabel
} from 'devextreme-vue/form';
import {onMounted, reactive, ref, defineProps} from "vue";
import {useRoute} from "vue-router";
import {
  DxColumn,
  DxDataGrid,
  DxEditing,
  DxFormItem,
  DxLookup,
  DxPager,
  DxPaging
} from "devextreme-vue/data-grid";
import {DxRequiredRule} from "devextreme-vue/validator";
import translatorService from "@/api/translatorService";

const props = defineProps({
  masterDetailData: {
    type: Object,
    default: () => ({}),
  }})
const antennaId = ref();
const projectAntennaId = ref();
const translatorTypes = ref();
const route = useRoute();
const mode = ref(route.params.mode);
let dataSource = reactive([]);
const translators = ref([]);


onMounted(async () => {
  antennaId.value = props.masterDetailData.row.antennaId;
  projectAntennaId.value = props.masterDetailData.key
  const response = await translatorService.getAllByAntennaId(antennaId.value)
  const translatorTypes = await translatorService.
  translators.value = response.data.result;
})

</script>
<style scoped>
</style>
