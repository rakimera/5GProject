<template>
    <div class="widget-container">
      <form
            id="form"
            method="post"
            ref="formRef"
            enctype="multipart/form-data">
            <input
                name='projectId'
                :value="id"
                hidden="true"
            >
            <dx-file-uploader
                class="mt-3"
                select-button-text="Добавить фото"
                labelText="Или перенесите файл сюда"
                :showFileList= "false"
                :max-file-size="4000000"
                accept="image/*"
                :multiple="false"
                upload-mode="useForm"
                @value-changed="onAddImage"
                name="uploadedFile"
            />
        <span class="mx-3">
        Размер не более 4 MB:
      </span>
        </form>
      <div class="row">
        <div class="card col-md-8 m-auto mt-5" v-for="image in images" :key="image.id">
          <img :src="'data:image;base64,' + image.image" :alt="image.id" class="card-img mt-3">
          <div class="card-body text-center">
            <dx-button
                :width="120"
                text="Удалить"
                type="danger"
                styling-mode="contained"
                @click="onDelete(image.id)"
            />          
          </div>
        </div>
      </div>
    </div>
</template>

<script setup>

import {DxFileUploader} from "devextreme-vue/file-uploader";
import projectImageService from "@/api/projectImageService";
import notify from "devextreme/ui/notify";
import {useRoute} from "vue-router";
import DxButton from 'devextreme-vue/button';
import {onMounted, ref} from "vue";

const route = useRoute();
const formRef = ref(null);
let images = ref([]);
let id = route.params.id;

onMounted(async ()=> {
  await dataLoad();
})

async function dataLoad(){
  try {
    const response = await projectImageService.getAllByProjectId(id);
    if (response.data.success && response.data.result !== null){
      images.value = response.data.result;
    }
  }catch (error){
    notify({
      message: 'Ошибка при получении фото',
      position: {
        my: 'center top',
        at: 'center top',
      }}, 'error', 2000);
  }
}

async function onDelete(id){
  try {
    const response = await projectImageService.deleteProjectImage(id)
    if (response.data.success){
      notify({
        message: 'Фото удалено',
        position: {
          my: 'center top',
          at: 'center top',
        },
      }, 'success', 1000);
      await dataLoad();}
    else {
      notify({
        message: response.data.messages,
        position: {
          my: 'center top',
          at: 'center top'}
      }, 'error', 2000);
    }
  }catch (error){
    notify({
      message: 'Ошибка при удалении фото',
      position: {
        my: 'center top',
        at: 'center top',
      }}, 'error', 2000);
  }
}
async function onAddImage() {
    try {
        const response = await projectImageService.createProjectImage(formRef.value)
        if (response.data.success){
            notify({
                message: 'Фото добавлено',
                position: {
                    my: 'center top',
                    at: 'center top',
                },
            }, 'success', 1000);
          await dataLoad();} 
        else{
            notify({
                message: response.data.messages,
                position: {
                    my: 'center top',
                    at: 'center top'}
            }, 'error', 2000);
        } 
    } catch (error){
        notify({
            message: 'Ошибка при отправке файла на сервер',
            position: {
                my: 'center top',
                at: 'center top',
            }}, 'error', 2000);
    }
}
</script>

<style scoped>

</style>