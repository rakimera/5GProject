import axios from '../utils/axios';
import userService from "@/api/userService";

async function updateTokens(response) {
    localStorage.setItem('userToken', JSON.stringify(response.data.accessToken));
    localStorage.setItem('refreshToken', JSON.stringify(response.data.refreshToken));
    console.log(response.data.accessToken);
    console.log(response.data.refreshToken);
    return response.data;
}

const authService = {
    async login(loginModel) {
        try {
            const response = await axios.post('/api/Auth/login', loginModel);
            return updateTokens(response);
        } catch (error) {
            throw error;
        }
    },
    async refreshingToken(tokenApiModel) {
        try {
            const response = await axios.post('/api/Token/refresh', tokenApiModel);
            return updateTokens(response);
        } catch (error) {
            throw error;
        }
    },
    revoke() {
        localStorage.removeItem('userToken');
        localStorage.removeItem('refreshToken');
        return axios.post('/api/Token/revoke');
    },
    loggedIn() {
        // Логика проверки, аутентифицирован ли пользователь.
        return !!localStorage.getItem('userToken');
    },
};

export default authService;