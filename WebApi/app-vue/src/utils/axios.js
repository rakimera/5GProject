import axios from 'axios';
import authHeader from './authHeader';

const instance = axios.create({
    baseURL: 'https://localhost:7015/',
});
axios.defaults.headers.common = authHeader();

export default instance;