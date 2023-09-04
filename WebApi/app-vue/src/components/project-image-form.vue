<template>
    <div class="widget-container">
<!--        <dx-form
            id="form"
            :form-data="dataSource"
            method="post"
            enctype="multipart/form-data">
            <dx-item
                data-field='projectId'
                editor-type='dxTextBox'
            >
                <dx-label
                    :visible="false"
                />
            </dx-item>-->
        <dx-file-uploader
                select-button-text="Добавить фото"
                accept="image/*"
                multiple="false"
                upload-mode="useForm"
                @value-changed="onAddImage"
        />
<!--        </dx-form>-->
    </div>
</template>

<script setup>

import {DxFileUploader} from "devextreme-vue/file-uploader";
import projectImageService from "@/api/projectImageService";
import notify from "devextreme/ui/notify";
import {useRoute} from "vue-router";

const route = useRoute();
let id = route.params.id;
/*import {reactive} from "vue";*/
/*import {DxItem, DxLabel} from "devextreme-vue/form";*/

/*let dataSource = reactive({});*/
function onAddImage(e) {
    try {
        const projectImageDto = {projectId: id}
        const response = projectImageService.createProjectImage(projectImageDto, e.value)
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