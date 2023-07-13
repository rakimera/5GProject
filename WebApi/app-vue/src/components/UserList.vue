<template>
  <div>
    <h1>Users</h1>
    <ul>
      <li v-for="user in users" :key="user.id">
        {{ user.login }},
        {{ user.name }},
        {{ user.surname }},
        {{ user.password }},
      </li>
    </ul>
    <form @submit.prevent="createUser">
      <input type="email" v-model="newUser.login" placeholder="Email" required>
      <input type="text" v-model="newUser.name" placeholder="Name" required>
      <input type="text" v-model="newUser.surname" placeholder="Surname" required>
      <input type="text" v-model="newUser.password" placeholder="Password" required>
      <button type="submit">Create User</button>
    </form>
  </div>
</template>

<script>
import userService from '../api/userService';

export default {
  data() {
    return {
      users: [],
      newUser: {
        login: '',
        name: '',
        surname: '',
        password: '',
      },
    };
  },
  mounted() {
    this.getUsers();
  },
  methods: {
    async getUsers() {
      try {
        const response = await userService.getUsers();
        console.log(response.data)
        this.users = response.data.result;
      } catch (error) {
        console.error(error);
      }
    },
    async createUser() {
      try {
        const response = await userService.createUser(this.newUser);
        console.log(response.data);
        // Обработка успешного создания пользователя
        this.newUser = {
          login: '',
          name: '',
          surname: '',
          password: '',
          role: '',
        };
      } catch (error) {
        console.error(error);
        // Обработка ошибки при создании пользователя
      }
    },
  },
};
</script>
