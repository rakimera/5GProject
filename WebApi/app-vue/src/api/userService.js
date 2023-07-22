import axios from '../utils/axios';
import storeExtension from '../utils/storeExtension';


const userService = {

    async getUsers() {
        return await axios.get(`/api/users`);
    },
    createUser(user) {
        return new Promise((resolve, reject) => {
            axios.post('/api/users', user)
                .then((response) => {
                    resolve(response.data);
                })
                .catch((error) => {
                    reject(error);
                });
        })
    },

    async updateUser(user) {
        return await axios.put('/api/users', user)
    },

    deleteUser(oid) {
        return new Promise((resolve, reject) => {
            axios.delete(`/api/users/${oid}`)
                .then((response) => {
                    resolve(response.data);
                })
                .catch((error) => {
                    reject(error);
                });
        });
    },

    getAllUsers(loadOptions) {
        let options = storeExtension.getParams(loadOptions)
        return new Promise((resolve, reject) =>  {
            axios.get(`/api/users/Index/${options}`)
                .then((response)  => {
                    resolve(response.data);
                })
                .catch((error) => {
                    reject(error);
                })
        });
    },
};

export default userService;
