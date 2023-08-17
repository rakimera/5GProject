<template>
    <div class="project-form">
        <h2 v-text="pageDescription"></h2>
        <dx-form
                id="form"
                ref="formRef"
                label-location="top"
                :form-data="dataSource"
                :read-only="isFormDisabled"
                :show-colon-after-label="true"
                :show-validation-summary="true"
        >
            <dx-tabbed-item>
                <dx-tab-panel-options
                    :defer-rendering="false"
                />
                <dx-tab
                    title="Контрагент и адрес установки"
                >
                    <div class="field-container">
                        <DxSelectBox
                            :data-source="contrAgents"
                            :input-attr="{ 'aria-label': 'Контрагент' }"
                            label="Выберите контрагента"
                            display-expr="companyName"
                            value-expr="id"
                            name ='contrAgentId'
                            v-model="dataSource.contrAgentId"
                        />
                        <DxGroupItem
                            caption="Заполните адресс установки"
                        >
                            <DxSelectBox
                                :data-source="towns"
                                :input-attr="{ 'aria-label': 'Город' }"
                                label="Выберите город"
                                display-expr="townName"
                                value-expr="id"
                                name ='townId'
                                v-model="dataSource.townId"
                            />
                            <dx-item
                                data-field='arial'
                                editor-type='dxTextBox'
                                :editor-options="{ stylingMode: 'filled', placeholder: 'Район' }"
                            >
                                <dx-required-rule message="Введите район в котором расположен объект"/>
                                <dx-string-length-rule
                                    :min=2
                                    message="Район не может содержать менее 2 символов"
                                />
                                <dx-pattern-rule
                                    :pattern="namePattern"
                                    message="Поле должно состоять только из букв"
                                />
                                <dx-label :visible="false" />
                            </dx-item>
                            <dx-item
                                data-field='street'
                                editor-type='dxTextBox'
                                :editor-options="{ stylingMode: 'filled', placeholder: 'Улица' }"
                            >
                                <dx-required-rule message="Введите утицу на которой расположен объект"/>
                                <dx-label :visible="false" />
                            </dx-item>
                            <dx-item
                                data-field='house'
                                editor-type='dxTextBox'
                                :editor-options="{ stylingMode: 'filled', placeholder: 'Номер здания' }"
                            >
                                <dx-required-rule message="Введите номер здания на котором расположен объект"/>
                                <dx-label :visible="false" />
                            </dx-item>
                        </DxGroupItem>
                    </div>
                </dx-tab>
            </dx-tabbed-item>>
            <dx-button-item>
                <dx-button-options
                        width="100%"
                        type="default"
                        styling-mode="outlined"
                        template="Редактировать"
                        :on-click="onClickEditProject"
                        :visible="isFormDisabled"
                        :use-submit-behavior="true"
                >
                </dx-button-options>
            </dx-button-item>
            <dx-button-item>
                <dx-button-options
                        width="100%"
                        type="success"
                        styling-mode="outlined"
                        :template="mode === 'create' ? 'Создать' : 'Сохранить изменения'"
                        :on-click="onClickSaveChanges"
                        :visible="!isFormDisabled"
                        :use-submit-behavior="true"
                >
                </dx-button-options>
            </dx-button-item>
        </dx-form>
    </div>
</template>
<script setup>

import {
    DxForm,
    DxLabel,
    DxPatternRule,
    DxRequiredRule,
    DxStringLengthRule,
    DxButtonItem,
    DxButtonOptions,
    DxTabbedItem,
    DxTabPanelOptions,
    DxTab, DxGroupItem, DxItem
} from "devextreme-vue/form";
import {onBeforeMount, reactive, ref} from "vue";
import projectService from "@/api/projectService";
import {useRoute, useRouter} from "vue-router";
import notify from "devextreme/ui/notify";
import DxSelectBox from "devextreme-vue/select-box";
import contrAgentService from "@/api/contrAgentService";
import townService from "@/api/townService";

const route = useRoute();
const router = useRouter();
let dataSource = reactive({});
const routeParams = {name: "projects"};
let isFormDisabled = ref(true);
let oid = route.params.id;
const mode = route.params.mode;
const pageDescription = ref("Подробно о проекте");
const namePattern = ref("^[a-zA-Zа-яА-Я]+$")
const formRef = ref(null);
const contrAgents = ref([]);
const towns = ref([]);

onBeforeMount(async () => {
    if (mode === "read") {
        const response = await projectService.getProject(oid);
        Object.assign(dataSource, response.data.result);
    } else {
        const response = await contrAgentService.getContrAgents();
        contrAgents.value = response.data.result;
        
        const townResponse = await townService.getTowns();
        towns.value = townResponse.data.result;
        
        isFormDisabled.value = false;
        pageDescription.value = "Создание проекта"
    }
})
function onClickEditProject() {
    isFormDisabled.value = false;
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
            if (mode === "read") {
                const responseUpdate = await projectService.updateProject(dataSource);
                if (responseUpdate.data.success) {
                    notify({
                        message: 'Проект успешно отредактирован',
                        position: {
                            my: 'center top',
                            at: 'center top',
                        },
                    }, 'success', 1000);
                    isFormDisabled.value = true;
                } else {
                    notify(responseUpdate.data.messages, 'error', 2000);
                }
            } else {
                const response = await projectService.createProject(dataSource);
                if (response.data.success) {
                    notify({
                        message: 'Проект успешно создан',
                        position: {
                            my: 'center top',
                            at: 'center top',
                        },
                    }, 'success', 1000);
                    await router.push(routeParams);
                } else {
                    notify(response.data.messages, 'error', 2000);
                }
            }
        }

    } catch (error) {
        console.error("Ошибка при сохранении изменений:", error);
    }
}
</script>
<style scoped>

.project-form {
    max-width: 1000px;
    margin: 50px auto auto;
}
#form h2 {
    margin-left: 40px;
    font-weight: normal;
    font-size: 35px;
}
.field-container {
    border: 1px solid #d3d3d3;
    margin: 20px 20px 0 20px;
}
</style>