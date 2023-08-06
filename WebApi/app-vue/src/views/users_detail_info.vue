<template>
  <div>
    <h2>Подробнее о пользователе</h2>

    <dx-form
        id="form"
        label-location="top"
        :form-data="dataSource"
        :disabled="isFormDisabled">
    </dx-form>
      <DxButton
              
              text="Редактировать"
              type="normal"
              styling-mode="contained"
              :on-click="onClickEditUser"
              v-if="isFormDisabled"
      />
      <DxButton
              
              text="Подтвердить"
              type="normal"
              styling-mode="contained"
              :on-click="onClickSaveChanges"
              v-if="!isFormDisabled"
      />
  </div>
</template>

<script>

import DxForm from "devextreme-vue/form";
import DxButton from 'devextreme-vue/button';
import {onBeforeMount, reactive, ref} from "vue";
import userService from "@/api/userService";
import {useRoute} from "vue-router";

export default {
  setup(){
    const route = useRoute();
    const dataSource = reactive({
        login: "",
        name: "",
        surname: "",
        role: "",
    });
    let isFormDisabled = ref(true);
    const oid = route.params.id;
    const mode = route.params.mode;

    onBeforeMount(async () => {
        console.log(oid + " <======= oid")
        console.log(mode + " <======= mode")
        const response = await userService.getUser(oid);
        dataSource.login = response.data.result.login;
        dataSource.name = response.data.result.name;
        dataSource.surname = response.data.result.surname;
        dataSource.role = response.data.result.role;
    })
    function onClickEditUser() {
        isFormDisabled.value = false;
    }
    async function onClickSaveChanges() {
        const updatedData = {
            id: oid,
            login: dataSource.login,
            name: dataSource.name,
            surname: dataSource.surname,
            role: dataSource.role,
        };
        try {
            await userService.updateUser(updatedData);
            isFormDisabled.value = true;
        } catch (error) {
            console.error("Ошибка при сохранении изменений:", error);
        }
    }
      
    return {
        dataSource,
        isFormDisabled,
        onClickEditUser,
        onClickSaveChanges,
    };
  },
    components: {
        DxForm,
        DxButton,
    },
};
</script>

