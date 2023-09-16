<template>
  <div class="card mt-5">
    <div class="card-header">
      Просчет проекта
    </div>
    <div class="card-body">
      <p class="card-text">Перед экспортом убедитесь, что заполнены антенны и их передатчики, иначе расчеты будут не верны.</p>
      <p class="card-text">Так-же если просчеты не соответствуют ожиданиям проверьте заполненныу свойста антенны вне проекта.</p>
      <div>
        <dx-button
          text="Экспорт"
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
import notify from "devextreme/ui/notify";
import projectService from "@/api/projectService";

const route = useRoute();
let id = route.params.id;
async function calculateProject(){
  try {
    const response = await projectService.getProjectFile(id);

    const blob = new Blob([response.data], { type: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document' });
    const blobUrl = window.URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = blobUrl;
    link.download = 'project.docx';
    link.click();

    window.URL.revokeObjectURL(blobUrl);
    
    notify({
      message: 'Проект получен',
      position: {
        my: 'center top',
        at: 'center top',
      },
    }, 'success', 1000);
  }catch (error){
    notify({
      message: "Ошибка при получении файла:" + error,
      position: {
        my: 'center top',
        at: 'center top'}
    }, 'error', 2000);
  }
}
</script>
<style scoped>

</style>