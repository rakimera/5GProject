import axios from 'axios';

const instance = axios.create({
    baseURL: 'http://localhost:5176/',
});

export default instance;