import axios from '@/utils/axios';
import authHeader from "@/api/AuthHeader";
async function updateTokens(response) {
    localStorage.setItem('userToken', JSON.stringify(response.data.result.accessToken));
    localStorage.setItem('refreshToken', JSON.stringify(response.data.result.refreshToken));
    console.log(response.data.result.accessToken);
    console.log(response.data.result.refreshToken);
    
    return response.data;
}
const header = await authHeader();
const authService = {
    async login(loginModel) {
        try {
            const response = await axios.post('/api/Auth/login', loginModel);
            return updateTokens(response);
        } catch (error) {
            console.log("Ошибка получения токена", error)
        }
    },
    async refreshingToken(tokenApiModel) {
        try {
            const response = await axios.post('/api/Token/refresh', tokenApiModel);
            return response;
        } catch (error) {
            console.log("Ошибка получения Refresh токена", error)
        }
    },
    revoke() {
        localStorage.removeItem('userToken');
        localStorage.removeItem('refreshToken');
        return axios.post('/api/Token/revoke', {},{ headers: header});
    },
    loggedIn() {
        return !!localStorage.getItem('userToken');
    },
    async resetPassword(email) {
        try {
            console.log("Будущий сброс пароля по почте", email)
        } catch (errors){
            console.log(errors)
        }
        return "Тут будет сброс пароля, но не в этот раз";
    },
    async createAccount(email, password) {
        try {
            console.log("Будущий сброс пароля по почте", email, password)
        } catch (errors){
            console.log(errors)
        }
        return "Тут будет создание аккаунту, но это не точно";
    },
    async changePassword(password, recoveryCode) {
        try {
            console.log("Будущая смена пароля после получения кода на почту", password, recoveryCode)
        } catch (errors){
            console.log(errors)
        }
        return "Тут будет восстановление пароля, наверное";
    },
};

export default authService;