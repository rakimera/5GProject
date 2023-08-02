import axios from '@/utils/axios';
import tokenService from "@/api/tokenService";

const authService = {
    async login(loginModel) {
        const response = await axios.post('/api/Auth/login', loginModel);
        if (response.data.success === false){
            throw new Error("неверный логин или пароль") ;
        }
        
        await tokenService.updateTokens(response);
    },
    async refreshingToken(tokenApiModel) {
        try {
            return await axios.post('/api/Token/refresh', tokenApiModel);
        } catch (error) {
            console.log("Ошибка получения Refresh токена", error)
        }
    },
    async revoke() {
        try {
            const refreshToken = {
                accessToken: null, 
                refreshToken: await tokenService.getRefreshToken()}; 
            console.log(refreshToken)
            await axios.post('/api/Token/revoke', refreshToken);
            await tokenService.removeTokens();
        }
        catch (error) {
            return error;
        }        
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