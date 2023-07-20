import axios from '../utils/axios';
import storeExtension from '../utils/storeExtension';


const userService = {
    async getUsers() {
        return new Promise((resolve, reject) =>{
            console.log(resolve)
            console.log(reject)
            axios.get('/api/users')
                .then((response) => {
                    resolve(response.data);
                })
                .catch((error) => {
                    reject(error);
                });
        })
    },
    async createUser(user) {
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
        return new Promise((resolve, reject) => {
            axios.put('/api/users', user)
                .then((response) => {
                    resolve(response.data);
                })
                .catch((error) => {
                    reject(error);
                });
        })
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
