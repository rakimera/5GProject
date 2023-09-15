<template>
  <div class="card mt-5">
    <div class="card-header">
      Просчет проекта
    </div>
    <div class="card-body">
      <p class="card-text">Перед просчетом убедитесь, что заполнены антенны и их передатчики, иначе расчеты будут не верны.</p>
      <p class="card-text">Так-же если просчеты не соответствуют ожиданиям проверьте заполненныу свойста антенны вне проекта.</p>
      <div>
        <dx-button
          text="Просчитать"
          type="default"
          styling-mode="outlined"
          @click="calculateProject"
        />
      </div>
    </div>
  </div>
</template>
<script setup>
import DxButton from "devextreme-vue/button";
import {useRoute} from "vue-router";
import exportService from "@/api/exportService";
import notify from "devextreme/ui/notify";

const route = useRoute();
let id = route.params.id;
async function calculateProject(){
  try {
    const response = await exportService.getEnergyFlow(id);
    if (response.data.success){
      notify({
        message: 'Проект успешно просчитан',
        position: {
          my: 'center top',
          at: 'center top',
        },
      }, 'success', 1000);
    } else {
      notify({
        message: response.data.messages,
        position: {
          my: 'center top',
          at: 'center top'}
      }, 'error', 2000);
    }
  }catch (error){
    notify({
      message: "Ошибка сервера при реализации просчетов:" + error,
      position: {
        my: 'center top',
        at: 'center top'}
    }, 'error', 2000);
  }
}
</script>
<style scoped>

</style>