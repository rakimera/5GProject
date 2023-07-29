<template>
  <DxDataGrid
      :data-source="dataSource"
      :show-borders="true"
      :remote-operations="true"
  >
    <DxColumn
        data-field="login"
        data-type="string"
    />
    <DxColumn
        data-field="name"
        data-type="string"
    />
    <DxColumn
        data-field="surname"
        data-type="string"
    />
    <DxColumn
        data-field="password"
        data-type="string"
    />
    <DxColumn
        data-field="role"
        data-type="string"
    />
    <DxColumn
        data-field="action"
        data-type="button"
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


const store = new CustomStore({
  key: 'id',
  load: async (loadOptions) => {
    return await userService.getAllUsers(loadOptions);
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
