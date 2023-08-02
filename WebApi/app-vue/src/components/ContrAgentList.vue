<template>
  <div>
    <h1>ContrAgents</h1>
    <ul>
      <li v-for="contrAgent in contrAgents" :key="contrAgent.id">
        {{ contrAgent.companyName }},
        {{ contrAgent.bin }},
        {{ contrAgent.directorName }},
        {{ contrAgent.directorSurname }},
        {{ contrAgent.directorPatronymic }},
        {{ contrAgent.amplificationFactor }},
        {{ contrAgent.id }}
        <button @click="editContrAgent(contrAgent)">Edit</button>
        <button @click="deleteContrAgent(contrAgent)">Delete</button>
      </li>
    </ul>
    <form @submit.prevent="createContrAgent">
      <input type="text" v-model="newContrAgent.companyName" placeholder="Название компании" required>
      <input type="text" v-model="newContrAgent.bin" placeholder="БИН" required>
      <input type="text" v-model="newContrAgent.directorName" placeholder="Имя директора" required>
      <input type="text" v-model="newContrAgent.directorSurname" placeholder="Фамилия директора" required>
      <input type="text" v-model="newContrAgent.directorPatronymic" placeholder="Отчество директора" required>
      <input type="text" v-model="newContrAgent.amplificationFactor" placeholder="Коэффициент усиления" required>
      <button type="submit">Create ContrAgent</button>
    </form>
    <form @submit.prevent="updateContrAgent" v-if="editingContrAgent">
      <input type="text" v-model="editingContrAgent.companyName" placeholder="Название компании" required>
      <input type="text" v-model="editingContrAgent.bin" placeholder="БИН" required>
      <input type="text" v-model="editingContrAgent.directorName" placeholder="Имя директора" required>
      <input type="text" v-model="editingContrAgent.directorSurname" placeholder="Фамилия директора" required>
      <input type="text" v-model="editingContrAgent.directorPatronymic" placeholder="Отчество директора" required>
      <input type="text" v-model="editingContrAgent.amplificationFactor" placeholder="Коэффициент усиления" required>
      <input type="hidden" :value="editingContrAgent.id" required>
      <button type="submit">Update ContrAgent</button>
    </form>
  </div>
</template>

<script>
import contrAgentService from '../api/contrAgentService';

export default {
  data() {
    return {
      contrAgents: [],
      newContrAgent: {
        companyName: '',
        bin: '',
        directorName: '',
        directorSurname: '',
        directorPatronymic: '',
        amplificationFactor: '',
        id: ''
      },
      editingContrAgent: null,
    };
  },
  mounted() {
    this.getContrAgents();
  },
  methods: {
    editContrAgent(contrAgent) {
      this.editingContrAgent = { ...contrAgent };
    },
    async getContrAgents() {
      try {
        const response = await contrAgentService.getContrAgents();
        this.contrAgents = response.data.result;
      } catch (error) {
        console.error(error);
      }
    },
    async createContrAgent() {
      try {
        await contrAgentService.createContrAgent(this.newContrAgent);
        this.newContrAgent = {
          companyName: '',
          bin: '',
          directorName: '',
          directorSurname: '',
          directorPatronymic: '',
          amplificationFactor: '',
        };
        await this.getContrAgents();
      } catch (error) {
        console.error(error);
      }
    },
    async updateContrAgent() {
      try {
        await contrAgentService.updateContrAgent(this.editingContrAgent);
        this.editingContrAgent = null;
        await this.getContrAgents();
      } catch (error) {
        console.error(error);
      }
    },
    async deleteContrAgent(contrAgent) {
      try {
        await contrAgentService.deleteContrAgent(contrAgent.id);
        await this.getContrAgents();
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>