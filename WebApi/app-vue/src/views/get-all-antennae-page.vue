<template>
  <dx-data-grid
      :data-source="store"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @row-click="onRowClick"
  >
    <dx-column
        caption="Модель"
        data-field="model"
        data-type="string"
    />
    <dx-column
        caption="Вертикальный размер(диаметр антенны)"
        data-field="verticalSizeDiameter"
        data-type="decimal"
    />
    <dx-paging :page-size="5"/>
    <dx-pager
        :show-page-size-selector="true"
        :allowed-page-sizes="[8, 12, 20]"
    />
    <dx-editing
        :allow-deleting="true"
        :texts="{confirmDeleteMessage: 'Вы уверены, что хотите удалить эту запись?'}"
    />
  </dx-data-grid>
</template>

<script setup>

import {
  DxDataGrid,
  DxColumn,
  DxPager,
  DxPaging,
  DxEditing,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import antennaService from "@/api/antennaService";
import { useRouter } from 'vue-router';

const router = useRouter();
const store = new CustomStore({
  key: 'id',
  load: async (loadOptions) => {
    return await antennaService.getAntennaeForGrid(loadOptions);
  },
  remove: async (oid) => {
    const baseResponse = await antennaService.deleteAntenna(oid);
    return {data: baseResponse.result};
  },
});
async function onRowClick(e) {
  try {
    const antennaId = e.key;
    await router.push({name: 'antennaDetail', params: {mode: "read", id: antennaId}});
  } catch (error) {
    console.log(error)
  }
}
</script>