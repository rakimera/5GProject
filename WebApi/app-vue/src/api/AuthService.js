import axios from '../utils/axios';
import response from "devextreme";

const authService = {
    async login(loginModel) {
        return new Promise((resolve, reject) => {
            axios.post('/api/Auth/login', loginModel)
                .then((resolve) => {
                    localStorage.setItem('userToken', JSON.stringify(response.data.accessToken))
                    localStorage.setItem('userToken', JSON.stringify(response.data.refreshToken))
                    console.log(response.data.accessToken)
                    console.log(response.data.refreshToken)
                    console.log(resolve)
                })
                .catch((error) => {
                    reject(error);
                });
        });
    },
    refreshToken(tokenApiModel) {
        return axios.post('/api/Token/refresh', tokenApiModel);
    },
    logOut() {
        return axios.post('/api/Token/revoke');
    },
};

export default authService