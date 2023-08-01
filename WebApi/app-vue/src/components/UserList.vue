<template>
  <div>
    <h1>Users</h1>
    <ul>
      <li v-for="user in users" :key="user.id">
        {{ user.login }},
        {{ user.name }},
        {{ user.surname }},
        {{ user.password }},
        {{ user.role }},
        {{ user.id }},
        <button @click="editUser(user)">Edit</button>
        <button @click="deleteUser(user)">Delete</button>
      </li>
    </ul>
    <form @submit.prevent="createUser">
      <input type="email" v-model="newUser.login" placeholder="Email" required>
      <input type="text" v-model="newUser.name" placeholder="Name" required>
      <input type="text" v-model="newUser.surname" placeholder="Surname" required>
      <input type="text" v-model="newUser.password" placeholder="Password" required>
      <input type="text" v-model="newUser.role" placeholder="Role" required>
      <button type="submit">Create User</button>
    </form>
    <form @submit.prevent="updateUser" v-if="editingUser">
      <input type="email" v-model="editingUser.login" placeholder="Email" required>
      <input type="text" v-model="editingUser.name" placeholder="Name" required>
      <input type="text" v-model="editingUser.surname" placeholder="Surname" required>
      <input type="text" v-model="editingUser.password" placeholder="Password" required>
      <input type="text" v-model="editingUser.role" placeholder="Role" required>
      <input type="hidden" :value="editingUser.oid" required>
      <button type="submit">Update User</button>
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
        role: '',
        id: ''
      },
      editingUser: null,
    };
  },
  mounted() {
    this.getUsers();
  },
  methods: {
    editUser(user) {
      this.editingUser = { ...user };
    },
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
        this.newUser = {
          login: '',
          name: '',
          surname: '',
          password: '',
          role: '',
        };
        await this.getUsers();
      } catch (error) {
        console.error(error);
      }
    },
    async updateUser() {
      try {
        const response = await userService.updateUser(this.editingUser);
        console.log(response);
        this.editingUser = null;
        await this.getUsers();
      } catch (error) {
        console.error(error);
      }
    },
    async deleteUser(user) {
      try {
        console.log(user.id)
        const response = await userService.deleteUser(user.id);
        console.log(response);
        await this.getUsers();
      } catch (error) {
        console.error(error);
      }
    },
  },
};
</script>
