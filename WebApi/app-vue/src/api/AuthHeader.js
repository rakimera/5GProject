export default async function authHeader() {
    try {
        const user = JSON.parse(localStorage.getItem('userToken'));
        if (user && user.accessToken) {
            return { Authorization: 'Bearer ' + user.accessToken };
        }
    } catch (error) {
        throw error;
    }
}
