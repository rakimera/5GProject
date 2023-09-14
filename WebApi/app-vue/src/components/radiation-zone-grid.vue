<template>
    <div>
        <dx-data-grid
                :data-source="dataSource"
                :show-borders="true"
                :remote-operations="false"
                :column-auto-width="true"
                key-expr="id"
        >
            <dx-editing
                    :allow-updating="false"
                    :allow-adding="false"
                    :allow-deleting="false"
            />
            <dx-column
                    data-field="degree"
                    caption="Градус"
                    alignment="left">
            </dx-column>
            <dx-column
                    data-field="value"
                    caption="Значение"
                    alignment="left">
            </dx-column>
            <dx-column
                    data-field="directionType"
                    caption="Тип направленности"
                    alignment="left">
            </dx-column>
            <dx-sorting mode="multiple"/>
        </dx-data-grid>
    </div>
</template>
<script setup>

import {onMounted, ref, defineProps} from "vue";
import {
    DxColumn,
    DxDataGrid,
    DxEditing,
    DxSorting,
} from "devextreme-vue/data-grid";
import CustomStore from "devextreme/data/custom_store";
import radiationZoneService from "@/api/radiationZoneService";

const props = defineProps({
    masterDetailData: {
        type: Object,
        default: () => ({}),
    }})
const translatorSpecsId = ref();
let dataSource = ref(null);

const store = new CustomStore({
    key: "id",
    async load(loadOptions) {
        const response = await radiationZoneService.getRadiationZonesForGrid(loadOptions, translatorSpecsId.value);
        return response;
    }
});

onMounted(async () => {
    translatorSpecsId.value = props.masterDetailData.key;
    console.log(props.masterDetailData)
    dataSource.value = store;
})

</script>
<style scoped>
</style>
