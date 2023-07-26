import axios from '../utils/axios';
import authHeader from "@/api/AuthHeader";

const header = await authHeader();
const userService = {
    getUsers() {
        console.log(authHeader())
        return axios.get('/api/users', { headers: header});
    },
    createUser(user) {
        return axios.post('/api/users', user, { headers: header});
    },
    updateUser(user) {
        return axios.put('/api/users', user, { headers: header});
    },
    deleteUser(oid) {
        return axios.delete(`/api/users/${oid}`, { headers: header});
    },
    getUser(oid){
        return axios.get(`/api/users/${oid}`, { headers: header});
    }
};

export default userService;
