<template>
  <div>
    <h1>Antennae</h1>
    <ul>
      <li v-for="antenna in antennae" :key="antenna.id">
        {{ antenna.model }},
        {{ antenna.verticalSizeDiameter }},
        {{ antenna.id }}
        <button @click="editAntenna(antenna)">Edit</button>
        <button @click="deleteAntenna(antenna)">Delete</button>
      </li>
    </ul>
    <form @submit.prevent="createAntenna">
      <input type="text" v-model="newAntenna.model" placeholder="Модель антенны" required>
      <input type="text" v-model="newAntenna.verticalSizeDiameter" placeholder="Вертикальный диаметр" required>
      <button type="submit">Create antenna</button>
    </form>
    <form @submit.prevent="updateAntenna" v-if="editingAntenna">
      <input type="text" v-model="editingAntenna.model" placeholder="Модель антенны" required>
      <input type="text" v-model="editingAntenna.verticalSizeDiameter" placeholder="Вертикальный диаметр" required>
      <input type="hidden" :value="editingAntenna.id" required>
      <button type="submit">Update antenna</button>
    </form>
  </div>
</template>

<script>
import antennaService from "../api/antennaService";

export default {
  data() {
    return {
      antennae: [],
      newAntenna: {
        model: '',
        verticalSizeDiameter: '',
        id: ''
      },
      editingAntenna: null,
    };
  },
  mounted() {
    this.getAntennae();
  },
  methods: {
    editAntenna(antenna) {
      this.editingAntenna = { ...antenna };
    },
    async getAntennae() {
      try {
        const response = await antennaService.getAntennae();
        this.antennae = response.data.result;
      } catch (error) {
        console.error(error);
      }
    },
    async createAntenna() {
      try {
        await antennaService.createAntenna(this.newAntenna);
        this.newAntenna = {
          model: '',
          verticalSizeDiameter: '',
        };
        await this.getAntennae();
      } catch (error) {
        console.error(error);
      }
    },
    async updateAntenna() {
      try {
        await antennaService.updateAntenna(this.editingAntenna);
        this.editingAntenna = null;
        await this.getAntennae();
      } catch (error) {
        console.error(error);
      }
    },
    async deleteAntenna(antenna) {
      try {
        await antennaService.deleteAntenna(antenna.id);
        await this.getAntennae();
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>