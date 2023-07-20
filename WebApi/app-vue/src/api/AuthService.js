import axios from '../utils/axios';
import response from "devextreme";

const authService = {
    async login(loginModel) {
        return new Promise((resolve, reject) => {
            axios.post('/api/Auth/login', loginModel)
                .then((resolve) => {
                    resolve(response.data.accessToken)
                })
            
            
            
        });
    },
    refreshToken(tokenApiModel) {
        return axios.post('/api/Token/refresh', tokenApiModel);
    },
    logOut() {
        return axios.post('/api/Token/revoke');
    },
};


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
export default authService;