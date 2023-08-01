import axios from 'axios';
import authHeader from '@/api//AuthHeader';
import authService from "@/api/AuthService";

const instance = axios.create({
    baseURL: 'https://localhost:7015/',
});

instance.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalConfig = error.config;

        if (originalConfig.url !== "/api/Auth/login" && error.response) {

            if (error.response.status === 401 && !originalConfig._retry) {
                originalConfig._retry = true
                try {
                    const refreshToken = JSON.parse(localStorage.getItem('refreshToken'));
                    const userToken = JSON.parse(localStorage.getItem('userToken'));
                    const tokenApiModel = {accessToken: userToken, refreshToken: refreshToken};
                    const response = await authService.refreshingToken(tokenApiModel);
                    localStorage.setItem('userToken', JSON.stringify(response.data.result.accessToken));
                    localStorage.setItem('refreshToken', JSON.stringify(response.data.result.refreshToken));
                    return instance(originalConfig);
                } catch (error) {
                    console.error('Ошибка обновления токена:', error);
                    return Promise.reject(error);
                }
            }
            return Promise.reject(error);
        }
    }
);

instance.interceptors.request.use(
    async (config) => {
        const token = await authHeader();
        if (token) {
            config.headers.Authorization = 'Bearer ' + token;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);
export default instance;