<template>
  <div>
    <div class="long-title"><h3>Personal details</h3></div>
    <div id="form-container">
      <dx-form
          id="project-antenna-form"
          :col-count="1"
          :form-data="dataSource"
          label-location="top"
          :read-only="isFormDisabled"
          :show-colon-after-label="true"
          :show-validation-summary="true"
      >
        <dx-group-item
            caption="Phones"
            name="phones-container"
        >
          <dx-group-item
              item-type="group"
              name="phones"
          >
            <dx-simple-item
                v-for="(antenna, index) in antennaOptions"
                :key="'antenna' + (index + 1)"
                :data-field="'antennas[' + index + ']'"
                editor-type="dxSelectBox"
                :editor-options="{ 
                        placeholder: 'Выберите город', 
                        items: antennas, 
                        displayExpr: 'townName', 
                        valueExpr: 'townName',
                        labelMode: 'floating',
                        label: 'Город'}"
            >
              <dx-label :text="'Антенна ' + (index + 1)"/>
            </dx-simple-item>
          </dx-group-item>
          <dx-button-item
              :button-options="addAntennaButtonOptions"
              css-class="add-antenna-button"
              horizontal-alignment="left"
          />
        </dx-group-item>
      </dx-form>
    </div>
  </div>
</template>
<script setup>
import {
    DxForm,
    DxSimpleItem,
    DxGroupItem,
    DxButtonItem,
    DxLabel,
} from 'devextreme-vue/form';
import {onBeforeMount, reactive, ref} from "vue";
import antennaService from "@/api/antennaService";
import projectAntennaService from "@/api/projectAntennaService";
import {useRoute} from "vue-router";

let isFormDisabled = ref(false);
const route = useRoute();
let id = route.params.id;
const mode = ref(route.params.mode);
let dataSource = reactive({});
const antennas = ref([])
const antennaOptions = ref(getAntennasOptions(antennas.value));
const addAntennaButtonOptions = {
  icon: 'add',
  text: 'добавить антенну',
  onClick: () => {
    antennas.value.push('');
    antennaOptions.value = getAntennasOptions(antennas.value);
  },
};

onBeforeMount(async () => {
  const response = await antennaService.getAntennae();
  antennas.value = response.data.result;

  if (mode.value === "read") {
    const response = await projectAntennaService.getAllByProjectId(id);
    console.log(response)
    Object.assign(dataSource, response.data.result);
  } else {
    isFormDisabled.value = false;
  }
})
function getAntennasOptions(antennas) {
  const options = [];
  for (let i = 0; i < antennas.length; i += 1) {
    options.push(generateNewAntennaOptions(i));
  }
  return options;
}
function generateNewAntennaOptions(index) {
  return {
    mask: '+1 (X00) 000-0000',
    maskRules: { X: /[01-9]/ },
    buttons: [{
      name: 'trash',
      location: 'after',
      options: {
        stylingMode: 'text',
        icon: 'trash',
        onClick: () => {
          antennas.value.splice(index, 1);
          antennaOptions.value = getAntennasOptions(antennas.value);
        },
      },
    }],
  };
}
</script>
<style scoped>
#form-container {
  margin: 10px 10px 30px;
}

.long-title h3 {
  font-family:
      'Segoe UI Light',
      'Helvetica Neue Light',
      'Segoe UI',
      'Helvetica Neue',
      'Trebuchet MS',
      Verdana;
  font-weight: 200;
  font-size: 28px;
  text-align: center;
  margin-bottom: 20px;
}

.add-antenna-button {
  margin-top: 10px;
}
</style>
