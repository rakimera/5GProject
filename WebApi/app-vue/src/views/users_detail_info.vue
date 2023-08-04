<template>
  <div>
    <h2>Подробнее о пользователе</h2>

    <dx-form
        id="form"
        label-location="top"
        :form-data="dataSource">

    </dx-form>
  </div>
</template>

<script>

import userService from "@/api/userService";
import DxForm from "devextreme-vue/form";

export default {
  components: {
    DxForm,

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
      this.dataSource = response.data.result;
    },
  },
};
</script>
