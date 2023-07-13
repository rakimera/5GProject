import axios from '../utils/axios';

const userService = {
    getUsers() {
        return axios.get('/api/users');
    },
    createUser(user) {
        return axios.post('/api/users', user);
    },
    updateUser(user) {
        return axios.put('/api/users', user);
    },
    deleteUser(oid) {
        return axios.delete(`/api/users/${oid}`);
    },
};

export default userService;
