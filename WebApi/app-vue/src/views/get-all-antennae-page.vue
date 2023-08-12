<template>
  <dx-data-grid
      :data-source="dataSource"
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
    />
  </dx-data-grid>
</template>

<script>

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

export default {
  components: {
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxPager,
    DxEditing,
  },
  data() {
    return {
      dataSource: store,
      events: [],
    };
  },
  methods: {
    async onRowClick(e) {
      try {
        const antennaId = e.key;
        this.$router.push({name: 'antennaDetail', params: {mode: "read", id: antennaId}});
      } catch (error) {
        console.log(error)
      }
    }
  }
};
</script>