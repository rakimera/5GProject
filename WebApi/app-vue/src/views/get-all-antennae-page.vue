<template>
  <DxDataGrid
      :data-source="store"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @row-click="onRowClick"
  >
    <DxColumn
        data-field="model"
        data-type="string"
    />
    <DxColumn
        data-field="vertical diameter"
        data-type="string"
    />
    <DxPaging :page-size="2"/>
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
import AuthenticationService from "@/api/AuthenticationService";

const store = new CustomStore({
  key: 'id',
  load: async (loadOptions) => {
    return await antennaService.getAntennaeForGrid(loadOptions);
  },
  insert: async (antenna) => {
    const baseResponse = await antennaService.createAntenna(antenna);
    return {data: baseResponse.result};
  },
  update: async (id) => {
    try {
      const baseResponse = await antennaService.updateAntenna(id);
      return {data: baseResponse.result};
    } catch (error) {
      console.log(error);
    }
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
        let role = await AuthenticationService.getRole();
        console.log(role)
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