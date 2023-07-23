export default async function authHeader() {
    try {
        const user = JSON.parse(localStorage.getItem('userToken'));
        if (user) {
            console.log(user)
            return { Authorization: 'Bearer ' + user };
        }
    } catch (error) {
        console.error("Ошибка при получении данных из локального хранилища:", error);
    }
    return {};
}
