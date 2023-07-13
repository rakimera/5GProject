<template>
  <div>
    <h1>Users</h1>
    <ul>
      <li v-for="user in users" :key="user.id">
        {{ user.name }}
      </li>
    </ul>
    <form @submit.prevent="createUser">
      <input type="text" v-model="newUser.name" placeholder="Name" required>
      <input type="email" v-model="newUser.email" placeholder="Email" required>
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
        name: '',
        email: '',
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
        this.users = response.data;
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
          name: '',
          email: '',
        };
      } catch (error) {
        console.error(error);
        // Обработка ошибки при создании пользователя
      }
    },
  },
};
</script>
