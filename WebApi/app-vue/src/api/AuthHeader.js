import authService from "@/api/AuthService";
export default async function authHeader() {
    try {
        const user = JSON.parse(localStorage.getItem('userToken'));
        if (user && user.accessToken) {
            return { Authorization: 'Bearer ' + user.accessToken };
        } else {
            const user = JSON.parse(localStorage.getItem('refreshToken'));
            const tokenApiModel = { accessToken: user.accessToken, refreshToken: user.refreshToken };
            try {
                const response = await authService.refreshingToken(tokenApiModel);
                return { Authorization: 'Bearer ' + response.data.accessToken };
            } catch (error) {
                console.error('Ошибка обновления токена:', error);
                return {};
            }
        }
    } catch (error) {
        throw error;
    }
}
