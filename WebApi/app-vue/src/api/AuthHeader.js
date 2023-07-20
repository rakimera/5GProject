import axios from '../utils/axios';

const authHeader = {
    login(loginModel) {
        return axios
            .post('/api/auth/login', loginModel)
            .then(response => {
                if (response.data.accessToken){
                    localStorage.setItem('user', JSON.stringify(response.data))
                }
                
                return response.data
            });
    },
    logout() {
        localStorage.removeItem('user');
    }
};

export default function authHeader() {
    const user = JSON.parse(localStorage.getItem('user'));

    if (user && user.accessToken) {
        return { Authorization: 'Bearer ' + user.accessToken };
    } else {
        return {};
    }
}
