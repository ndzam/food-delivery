import axios from 'axios';
import { getAccessToken } from '../../store/user/reducer';

export const HttpClient = axios.create({
    baseURL: 'https://localhost:5001/api',
    withCredentials: false,
});

HttpClient.interceptors.request.use(
    function (config) {
        const token = getAccessToken();
        if (token != null) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    function (err) {
        return Promise.reject(err);
    },
);
