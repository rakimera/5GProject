<template>
  <div>
    <h2>Создание пользователя</h2>
    <dx-form
        id="form"
        label-location="top"
        :form-data="formData">
    </dx-form>
    <DxButton
        text="Создать"
        :on-click="onClickCreateUser"
    />
  </div>
</template>

<script setup>
import DxForm from "devextreme-vue/form";
import DxButton from 'devextreme-vue/button';
import {ref} from "vue";
import userService from "@/api/userService";
import {useRouter} from "vue-router";

const formData = ref({
  login: "",
  name: "",
  surname: "",
  password: "",
  role: "",
});

const router = useRouter();

async function onClickCreateUser() {
  const newUser = {
    login: formData.value.login,
    name: formData.value.name,
    surname: formData.value.surname,
    password: formData.value.password,
    role: formData.value.role,
  };
  console.log(newUser)
  try {
    const response = await userService.createUser(newUser);
    console.log(response)
    if (response.data.success) {
      console.log("Пользователь успешно создан");
      await router.push({name: "users_table"});
    }
  } catch (error) {
    console.error("Ошибка при создании пользователя", error);
  }
}
</script>
