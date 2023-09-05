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
                select-button-text="Добавить фото"
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

const route = useRoute();
const formRef = ref(null);
let id = route.params.id;
import {ref} from "vue";

async function onAddImage() {
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
        } else{
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