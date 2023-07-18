<template>
  <DxDataGrid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
  >
    <DxColumn
        data-field="name"
        data-type="string"
    />
    <DxPaging :page-size="2"/>
    <DxPager
        :show-page-size-selector="true"
        :allowed-page-sizes="[8, 12, 20]"
    />
  </DxDataGrid>
</template>

<script>

import {
  DxDataGrid, DxColumn, DxPaging, DxPager,
} from 'devextreme-vue/data-grid';
import CustomStore from 'devextreme/data/custom_store';
import 'whatwg-fetch';
import userService from "@/api/userService";

// function isNotEmpty(value) {
//   return value !== undefined && value !== null && value !== '';
// }

const store = new CustomStore({
  key: 'id',
  load: async (loadOptions) => {
    let test = await userService.getAllUsers(loadOptions);
    return test;
  },
});

export default {
  components: {
    DxDataGrid,
    DxColumn,
    DxPaging,
    DxPager,
  },
  data() {
    return {
      dataSource: store,
    };
  },
};
</script>
