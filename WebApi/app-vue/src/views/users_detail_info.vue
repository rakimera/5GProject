<template>
  <div>
    <h2>Подробнее о пользователе</h2>
    <DxDataGrid :data-source="dataSource">
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
    </DxDataGrid>
  </div>
</template>

<script>
import {DxColumn, DxDataGrid} from "devextreme-vue/data-grid";
import userService from "@/api/userService";

export default {
  components: {
    DxColumn,
    DxDataGrid,
  },
  data() {
    return {
      dataSource: null,
    };
  },
  created() {
    this.loadUserDetail();
  },
  methods: {
    async loadUserDetail() {
      const oid = this.$route.params.id;
      console.log(oid + "<=======")
      const response = await userService.getUser(oid);
      console.log(response)
      this.dataSource = [response.data.result];
    },
  },
};
</script>
