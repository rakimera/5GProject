<template>
  <div>
    <h2>Подробнее о пользователе</h2>

    <dx-form
        id="form"
        label-location="top"
        :form-data="dataSource"
        :disabled="isFormDisabled">
    </dx-form>
    
    <button @click="editUser" v-if="isFormDisabled">Редактировать</button>

    <button @click="saveChanges" v-if="!isFormDisabled">Подтвердить</button>

  </div>
</template>

<script>

import userService from "@/api/userService";
import DxForm from "devextreme-vue/form";
import {reactive} from "vue";

export default {
  components: {
    DxForm,

  },
  data() {
    const dataSource = reactive({
      login: "",
      name: "",
      surname: "",
      role: "",
    })
    return {
      dataSource,
      isFormDisabled: true,
    };
  },
  created() {
    this.loadUserDetail();
    const mode = this.$route.params.mode;
    if (mode === "create") {
      this.isFormDisabled = false;
    } else {
      this.loadUserDetail();
    }
  },
  methods: {
    async loadUserDetail() {
      const oid = this.$route.params.id;
      const mode = this.$route.params.mode;
      console.log(oid + " <======= oid")
      console.log(mode + " <======= mode")
      const response = await userService.getUser(oid);
      this.dataSource.login = response.data.result.login;
      this.dataSource.name = response.data.result.name;
      this.dataSource.surname = response.data.result.surname;
      this.dataSource.role = response.data.result.role;
    },

    editUser() {
      this.isFormDisabled = false;
    },

    async saveChanges() {
      const updatedData = {
        id: this.$route.params.id,
        login: this.dataSource.login,
        name: this.dataSource.name,
        surname: this.dataSource.surname,
        role: this.dataSource.role,
      };

      try {
        console.log(updatedData)
        await userService.updateUser(updatedData);
        this.isFormDisabled = true;
      } catch (error) {
        console.error("Ошибка при сохранении изменений:", error);
      }
    },
  },
};
</script>
