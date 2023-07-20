import axios from '../utils/axios';
import storeExtension from '../utils/storeExtension';

const userService = {
    async getUsers() {
        return await axios.get('/api/users');
    },
    async createUser(user) {
        return await axios.post('/api/users', user);
    },
    async updateUser(user) {
        return await axios.put('/api/users', user);
    },
    async deleteUser(oid) {
        return await axios.delete(`/api/users/${oid}`);
    },
    getAllUsers(loadOptions) {
        let options = storeExtension.getParams(loadOptions)
        return new Promise((resolve) =>  {
            axios.get(`/api/users/Index/${options}`).then((response)  => resolve(response.data))
        }) ;
    },
};

export default userService;
