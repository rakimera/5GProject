<template>
  <DxDataGrid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @row-click="onRowClick"
  >
    <DxColumn
        caption="Модель"
        data-field="model"
        data-type="string"
    />
    <DxColumn
        data-field="Вертикальный размер(диаметр антенны)"
        data-type="decimal"
    />
    <DxPaging :page-size="5"/>
    <DxPager
        :show-page-size-selector="true"
        :allowed-page-sizes="[8, 12, 20]"
    />
    <DxEditing
        :allow-deleting="true"
    />
  </DxDataGrid>
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
    var test = await antennaService.getAntennaeForGrid(loadOptions);
    console.log(test);
    return test;
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

<style>
#events {
  background-color: rgba(191, 191, 191, 0.15);
  padding: 20px;
  margin-top: 20px;
}

#events > div {
  padding-bottom: 5px;
}

#events > div::after {
  content: "";
  display: table;
  clear: both;
}

#events #clear {
  float: right;
}

#events .caption {
  float: left;
  font-weight: bold;
  font-size: 115%;
  line-height: 115%;
  padding-top: 7px;
}

#events ul {
  list-style: none;
  max-height: 100px;
  overflow: auto;
  margin: 0;
}

#events ul li {
  padding: 7px 0;
  border-bottom: 1px solid #ddd;
}

#events ul li:last-child {
  border-bottom: none;
}

.dx-datagrid .dx-row:hover {
  background-color: #f2f2f2;
  cursor: pointer;
}
</style>