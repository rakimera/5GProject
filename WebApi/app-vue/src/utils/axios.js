import axios from 'axios';
import authHeader from '@/api//AuthHeader';
import authService from "@/api/AuthService";

const instance = axios.create({
    baseURL: 'http://localhost:5176/',
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
axios.defaults.headers.common = authHeader();

export default instance;