<template>
  <dx-data-grid
      :data-source="store"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
  >
    <dx-editing
        :allow-updating="true"
        :allow-adding="true"
        :allow-deleting="true"
        mode="batch"
    />
    <dx-column
        caption="Градус"
        data-field="degree"
        data-type="int"
    />
    <dx-column
        caption="Значение"
        data-field="value"
        data-type="decimal"
    />
    <dx-paging :enabled="false"/>
  </dx-data-grid>
</template>

<script setup>

import {
  DxDataGrid,
  DxColumn,
  DxPaging,
  DxEditing,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import radiationZoneService from "@/api/radiationZoneService";

const store = new CustomStore({
  key: 'id',
  remove: async (oid) => {
    const baseResponse = await radiationZoneService.deleteRadiationItem(oid);
    return {data: baseResponse.result};
  },
  onModified: async (oid) => {
    const baseResponse = await radiationZoneService.editRadiationItem(oid);
    return {data: baseResponse.result};
  },
});
</script>