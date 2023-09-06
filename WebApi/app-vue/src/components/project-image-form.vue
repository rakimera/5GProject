<template>
    <div class="widget-container">
      <div class="container">
        <div class="image-container" v-for="image in images.value" :key="image.id">
          <h1>Член</h1>
          <img :src="dataUrl(image.image)">
          <div>
            <dx-button
                :width="120"
                text="Contained"
                type="normal"
                styling-mode="contained"
                @click="onDelete(image.id)"
            />
          </div>
        </div>
      </div>  
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
                select-button-text="Добавить фото"
                labelText="Или перенесите файл сюда"
                accept="image/*"
                multiple="false"
                upload-mode="useForm"
                @value-changed="onAddImage"
                name="uploadedFile"
            />
        </form>
       
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
const images = ref([]);
let id = route.params.id;

onMounted(async ()=> {
  await dataLoad();
})

function dataUrl(data) {
  console.log("Алё")
  let base64String = btoa(new Uint8Array(data).reduce((data, byte) => data + String.fromCharCode(byte), ''));
  
  return ("data:image/jpg;base64," + base64String);
}
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
async function onAddImage(e) {
  console.log(e)
    try {
        console.log(formRef.value)
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