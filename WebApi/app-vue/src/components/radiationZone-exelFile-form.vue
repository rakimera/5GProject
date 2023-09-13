<!--<template>-->
<!--  <div class="container">-->
<!--    <div class="large-12 medium-12 small-12 cell">-->
<!--      <label>File-->
<!--        <input type="file" id="file" ref="file" v-on:change="handleFileUpload()"/>-->
<!--      </label>-->
<!--      <button v-on:click="submitFile()">Submit</button>-->
<!--    </div>-->
<!--  </div>-->
<!--</template>-->
<!--<script>-->
<!--import axios from "@/utils/axios";-->

<!--export default {-->
<!--  data(){-->
<!--    return {-->
<!--      file: ''-->
<!--    }-->
<!--  },-->
<!--  methods: {-->
<!--    submitFile(){-->
<!--      let formData = new FormData();-->
<!--      formData.append('file', this.file);-->
<!--      axios.post( 'api/radiationZone-files',-->
<!--          formData,-->
<!--          {-->
<!--            headers: {-->
<!--              'Content-Type': 'multipart/form-data'-->
<!--            }-->
<!--          }-->
<!--      ).then(function(){-->
<!--        console.log('SUCCESS!!');-->
<!--      })-->
<!--          .catch(function(){-->
<!--            console.log('FAILURE!!');-->
<!--          });-->
<!--    },-->
<!--    handleFileUpload(){-->
<!--      this.file = this.$refs.file.files[0];-->
<!--    }-->
<!--  }-->
<!--}-->
<!--</script>-->


<template>
  <div class="radiationZoneExelFile" id="radiationZoneExelFile">
    <form
        id="form"
        method="post"
        ref="formRef"
        enctype="multipart/form-data">
      <input
          name='translatorSpecId'
          :value="id"
          hidden="true"
      >
      <dx-file-uploader
          class="mt-3"
          select-button-text="Добавить exel файл"
          labelText="Или перенесите файл сюда"
          :showFileList= "false"
          :max-file-size="4000000"
          accept="file/*"
          :multiple="false"
          upload-mode="useForm"
          @value-changed="onExelFile"
          name="uploadedFile"
      />
      <span class="mx-3">
        Размер не более 4 MB:
      </span>
    </form>
    <div class="row">
      <div class="card col-md-8 m-auto mt-5" v-for="exelFile in exelFile" :key="exelFile.id">
        <div class="card-body text-center">
          <dx-button
              :width="120"
              text="Удалить"
              type="danger"
              styling-mode="contained"
              @click="onDelete(exelFile.id)"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>

import {DxFileUploader} from "devextreme-vue/file-uploader";
import radiationZoneExelFileService from "@/api/radiationZoneExelFileService";
import notify from "devextreme/ui/notify";
import {useRoute} from "vue-router";
import DxButton from 'devextreme-vue/button';
import {onMounted, ref} from "vue";

const route = useRoute();
const formRef = ref(null);
let files = ref([]);
let id = route.params.id;

onMounted(async ()=> {
  await dataLoad();
})

async function dataLoad(){
  try {
    const response = await radiationZoneExelFileService.getAllByTranslatorSpecId(id);
    if (response.data.success && response.data.result !== null){
      files.value = response.data.result;
    }
  }catch (error){
    notify({
      message: 'Ошибка при получении файлов',
      position: {
        my: 'center top',
        at: 'center top',
      }}, 'error', 2000);
  }
}

async function onDelete(id){
  try {
    const response = await radiationZoneExelFileService.deleteExelFile(id)
    if (response.data.success){
      notify({
        message: 'Exel файл удалён',
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
      message: 'Ошибка при удалении exel файла',
      position: {
        my: 'center top',
        at: 'center top',
      }}, 'error', 2000);
  }
}
async function onExelFile() {
  try {
    const response = await radiationZoneExelFileService.createExelFile(formRef.value)
    if (response.data.success){
      notify({
        message: 'Exel файл добавлен',
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