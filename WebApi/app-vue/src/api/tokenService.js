const tokenService = {
    
    async getAccessToken() {
        try {
            const user = JSON.parse(localStorage.getItem('userToken'));
            if (user) {
                return  user;
            }
        } catch (error) {
            console.error("Ошибка при получении Accsess токена из локального хранилища:", error);
        }
        return {};
    },

    async getRefreshToken() {
        try {
            const user = JSON.parse(localStorage.getItem('refreshToken'));
            if (user) {
                return  user;
            }
        } catch (error) {
            console.error("Ошибка при получении Refresh токена из локального хранилища:", error);
        }
        return {};
    },
    
    async removeTokens (){
        try {
           await localStorage.removeItem('userToken');
           await localStorage.removeItem('refreshToken');
        }
        catch (error){
            console.log(error)
        }
    },

    async updateTokens(response) {
        try {
            console.log(response)
            await localStorage.setItem('userToken', JSON.stringify(response.data.result.accessToken));
            await localStorage.setItem('refreshToken', JSON.stringify(response.data.result.refreshToken));
        }
        catch (error) {
            console.log("Ошибка записи токена" + error)
        }
       
    }
}


 export default tokenService