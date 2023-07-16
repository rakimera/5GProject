import axios from 'axios';

const instance = axios.create({
    baseURL: 'https://localhost:7015/',
});

export default instance;