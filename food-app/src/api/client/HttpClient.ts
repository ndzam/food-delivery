import axios from 'axios';

export const HttpClient = axios.create({
    baseURL: 'https://localhost:5001/api',
    withCredentials: false,
});
