<template>
  <DxDataGrid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
      key-expr="ID"
      @row-click="onRowClick"
  >
    <DxColumn
        data-field="companyName"
        caption="Название компании"
        data-type="string"
    />
    <DxColumn
        data-field="bin"
        caption="БИН"
        data-type="string"
    />
    <DxColumn
        data-field="directorName"
        caption="Имя директора"
        data-type="string"
    />
    <DxColumn
        data-field="directorSurname"
        caption="Фамилия директора"
        data-type="string"
    />
    <DxColumn
        data-field="directorPatronymic"
        caption="Отчество директора"
        data-type="string"
    />
    <DxColumn
        data-field="amplificationFactor"
        caption="Коэффициент усиления"
        data-type="string"
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
import counterAgentService from "@/api/counterAgentService";
import { useRouter } from 'vue-router';



export default {
  setup()
  {
    const router = useRouter();
    const store = new CustomStore({
      key: 'id',

        load: async (loadOptions) => {
          return await counterAgentService.getAllContrAgents(loadOptions);
        },
      remove: async (oid) => {
        const baseResponse = await counterAgentService.deleteContrAgent(oid);
        return {data: baseResponse.result};
      },
    });
    async function onRowClick(e) {
        try {
          const contrAgentId = e.key;
          await router.push({name: 'contrAgentDetail', params: {mode: "read", id: contrAgentId}});
        } catch (error) {
          console.log(error)
        }
      }
    return {
      dataSource: store,
      events: [],
      onRowClick
    };
  },
  components: {
      DxDataGrid,
      DxColumn,
      DxPaging,
      DxPager,
      DxEditing,
  },
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