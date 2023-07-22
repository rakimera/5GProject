export default async function authHeader() {
    try {
        const user = JSON.parse(localStorage.getItem('userToken'));
        if (user && user.accessToken) {
            return { Authorization: 'Bearer ' + user.accessToken };
        }
    } catch (error) {
        console.error("Ошибка при получении данных из локального хранилища:", error);
    }
    return {};
}
