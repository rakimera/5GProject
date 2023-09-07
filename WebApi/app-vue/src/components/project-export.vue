<template>
  <div class="card mt-5">
    <div class="card-header">
      Просчет проекта
    </div>
    <div class="card-body">
      <p class="card-text">Перед просчетом убедитесь, что заполнены антенны и их передатчики, иначе расчеты будут не верны.</p>
      <p class="card-text">Так-же если просчеты не соответствуют ожиданиям проверьте заполненныу свойста антенны вне проекта.</p>
      <div>
        <dx-button
          text="Просчитать"
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

async function calculateProject(){
  console.log("кнопочкээээ")
}
</script>
<style scoped>

</style>