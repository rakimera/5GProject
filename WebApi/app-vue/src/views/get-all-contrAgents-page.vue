<template>
  <div class="row justify-content-center mt-5">
    <div class="col-11 ">
      <dx-data-grid
          :data-source="dataSource"
          :show-borders="true"
          :remote-operations="false"
          :columnAutoWidth="true"
          :allowColumnResizing="true"
          key-expr="ID"
          @row-click="onRowClick"
      >
        <dx-search-panel
                :visible="true"
                placeholder="Поиск"
                width= 250
        />
        <dx-column
            data-field="companyName"
            caption="Название компании"
            data-type="string"
        />
        <dx-column
            data-field="bin"
            caption="БИН"
            data-type="string"
        />
        <dx-column
            data-field="directorName"
            caption="Имя директора"
            data-type="string"
        />
        <dx-column
            data-field="directorSurname"
            caption="Фамилия директора"
            data-type="string"
        />
        <dx-column
            data-field="directorPatronymic"
            caption="Отчество директора"
            data-type="string"
        />
        <dx-column
            data-field="email"
            caption="Электронная почта компании"
            data-type="string"
        />
        <dx-column
            data-field="phoneNumber"
            caption="Номер телефона компании"
            data-type="string"
        />
        <dx-column
            data-field="address"
            caption="Адрес"
            data-type="string"
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
        <dx-header-filter :visible="true"/>
      </dx-data-grid>
      <dx-button
          class="button mt-3"
          text="Создать"
          type="success"
          :use-submit-behavior="true"
          :on-click="onCreateContrAgentClick"
      />
    </div>
  </div>
</template>

<script setup>
import {
    DxDataGrid,
    DxColumn,
    DxPager,
    DxPaging,
    DxEditing, DxSearchPanel, DxHeaderFilter,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import "whatwg-fetch";
import contrAgentService from "@/api/contrAgentService";
import {useRouter} from 'vue-router';
import {DxButton} from "devextreme-vue/button";


const router = useRouter();
const dataSource = new CustomStore({
  key: 'id',

  load: async (loadOptions) => {
    return await contrAgentService.getAllContrAgents(loadOptions);
  },
  remove: async (oid) => {
    const baseResponse = await contrAgentService.deleteContrAgent(oid);
    return {data: baseResponse.result};
  },
});

async function onRowClick(e) {
  try {
    const contrAgentId = e.key;
    console.log(contrAgentId)
    await router.push({name: 'contrAgentDetail', params: {mode: "read", id: contrAgentId}});
  } catch (error) {
    console.log(error)
  }
}

const onCreateContrAgentClick = async () => {
  try {
    await router.push({name: 'contrAgentDetail', params: {mode: "create", id: null}});
  } catch (error) {
    console.log(error);
  }
};

</script>

<style>
.dx-datagrid .dx-row:hover {
  background-color: #f2f2f2;
  cursor: pointer;
}
</style>