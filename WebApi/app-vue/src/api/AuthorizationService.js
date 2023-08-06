import axios from '@/utils/axios';
import tokenService from "@/api/tokenService";
import router from "@/router";

const authorizationService = {
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
            await tokenService.removeTokens();
            await router.push('/login');
        }
    },
    async revoke() {
        try {
            const tokenApiModel = {
                accessToken: null, 
                refreshToken: await tokenService.getRefreshToken()}; 
            console.log(tokenApiModel)
            await axios.post('/api/Token/revoke', tokenApiModel);
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

export default authorizationService;