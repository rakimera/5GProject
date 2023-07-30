import axios from 'axios';
import authHeader from '@/api//AuthHeader';
import authService from "@/api/AuthService";

const instance = axios.create({
    baseURL: 'https://localhost:7015/',
});

axios.interceptors.response.use(
    (response) => response,
    async (error) => {
        if (error.response.status === 401) {
            const user = JSON.parse(localStorage.getItem('refreshToken'));
            const tokenApiModel = { accessToken: user.accessToken, refreshToken: user.refreshToken };
            try {
                const response = await authService.refreshingToken(tokenApiModel);
                localStorage.setItem('userToken', JSON.stringify(response.data.accessToken));
                localStorage.setItem('refreshToken', JSON.stringify(response.data.refreshToken));
                return axios(error.config);
            } catch (error) {
                console.error('Ошибка обновления токена:', error);
                return Promise.reject(error);
            }
        }
        return Promise.reject(error);
    }
);

instance.interceptors.request.use(
    async (config) => {
        const token = await authHeader();
        if (token) {
            config.headers.Authorization = 'Bearer ' + token;
            console.log(token)
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);
export default instance;