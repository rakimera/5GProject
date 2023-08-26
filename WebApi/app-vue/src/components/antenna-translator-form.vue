<template>
    <div>
        <div id="form-container">
            <dx-form
                id="antenna-translator-form"
                ref="formRef"
                :col-count="1"
                :form-data="dataSource"
                label-location="top"
                :read-only="isFormDisabled"
                :show-colon-after-label="true"
                :show-validation-summary="true">
                    <dx-group-item
                            item-type="group"
                            name="phones"
                            v-for="(antennaTranslators, i) in translatorOptions"
                            :key="'antennaTranslators' + (i + 1)"
                    >
                        <dx-simple-item
                            :data-field="'antennaTranslators[' + i + ']'"
                            editor-type="dxSelectBox"
                            :editor-options="{ 
                                placeholder: 'Выберите частоту', 
                                items: translators, 
                                displayExpr: 'translatorSpecs.frequency', 
                                valueExpr: 'id',
                                labelMode: 'floating',
                                label: 'Частота антенны' + (i + 1)}"
                        >
                            <dx-label :text="'Частота антенны ' + (i + 1)"/>
                        </dx-simple-item>
                    </dx-group-item>
                <dx-button-item
                        :button-options="addATranslatorButtonOptions"
                        css-class="add-antenna-button"
                        horizontal-alignment="left"
                />
                <dx-button-item>
                    <dx-button-options
                            width="100%"
                            type="success"
                            styling-mode="outlined"
                            :template="mode === 'create' ? 'Создать и продолжить' : 'Сохранить изменения'"
                            :on-click="onClickSaveChanges"
                            :visible="!isFormDisabled"
                            :use-submit-behavior="true"
                    >
                    </dx-button-options>
                </dx-button-item>
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
    DxLabel, DxButtonOptions,
} from 'devextreme-vue/form';
import {computed, onBeforeMount, reactive, ref, defineProps, watch} from "vue";
import projectAntennaService from "@/api/projectAntennaService";
import {useRoute} from "vue-router";
import notify from "devextreme/ui/notify";
import antennaTranslatorService from "@/api/antennaTranslatorService";

const props = defineProps({
    selectedAntennaId: String})

let isFormDisabled = ref(true);
const formRef = ref(null);
const route = useRoute();
/*let id = route.params.id;*/
const mode = ref(route.params.mode);
let dataSource = reactive([]);
const translators = ref([]);
const translatorOptions = ref(getTranslatorOptions(dataSource));
const currentSelectedAntennaId = ref(props.selectedAntennaId);

const addATranslatorButtonOptions = computed(() =>{
    return {
        icon: 'add',
        text: 'добавить частоту',
        disabled: isFormDisabled.value,
        onClick: () => {
            dataSource.push('');
            translatorOptions.value = getTranslatorOptions(dataSource);
        },
    };
});

onBeforeMount(async () => {

    const response = await antennaTranslatorService.getAllByProjectAntennaId(currentSelectedAntennaId.value);
    translators.value = response.data.result;
    console.log(response)
    Object.assign(dataSource, response.data.result);
    isFormDisabled.value = false;
})
function getTranslatorOptions(dataSource) {
    const options = [];
    for (let i = 0; i < dataSource.length; i += 1) {
        options.push(generateNewTranslatorOptions(i));
    }
    return options;
}

function generateNewTranslatorOptions(index) {
    return {
        buttons: [{
            name: 'trash',
            location: 'after',
            options: {
                stylingMode: 'text',
                icon: 'trash',
                onClick: () => {
                    translators.value.splice(index, 1);
                    translatorOptions.value = getTranslatorOptions(translators.value);
                },
            },
        }],
    };
}

async function onClickSaveChanges() {
    try {
        const formInstance = formRef.value.instance;
        const isFormValid = await formInstance.validate();
        if (isFormValid.isValid === false) {
            notify({
                message: 'Данные не корректны',
                position: {
                    my: 'center top',
                    at: 'center top',
                },
            }, 'warning', 1000);
        }
        else {
            if (mode.value === "read") {
                const responseUpdate = await projectAntennaService.updateProjectAntenna(dataSource);
                if (responseUpdate.data.success) {
                    notify({
                        message: 'Проект успешно отредактирован',
                        position: {
                            my: 'center top',
                            at: 'center top',
                        },
                    }, 'success', 1000);
                } else {
                    notify(responseUpdate.data.messages, 'error', 2000);
                }
                isFormDisabled.value = true;
            } else {
                const response = await projectAntennaService.createProjectAntenna(dataSource);
                if (response.data.success) {
                    notify({
                        message: 'Проект успешно создан',
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
            }
        }

    } catch (error) {
        console.error("Ошибка при сохранении изменений:", error);
        notify({
            message: "Ошибка сервера при сохранении изменений:",
            position: {
                my: 'center top',
                at: 'center top'}
        }, 'error', 2000);
    }
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
