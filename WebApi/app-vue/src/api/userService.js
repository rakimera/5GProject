import axios from '../utils/axios';
import authHeader from "@/api/AuthHeader";

const userService = {
    getUsers() {
        return axios.get('/api/users', { headers: authHeader()});
    },
    createUser(user) {
        return axios.post('/api/users', user, { headers: authHeader()});
    },
    updateUser(user) {
        return axios.put('/api/users', user, { headers: authHeader()});
    },
    deleteUser(oid) {
        return axios.delete(`/api/users/${oid}`, { headers: authHeader()});
    },
    getUser(oid){
        return axios.get(`/api/users/${oid}`, { headers: authHeader()});
    }
};

export default userService;
