import { AxiosPromise } from 'axios';

export async function makeApiRequest<T>(httpPromise: AxiosPromise<unknown>) {
    try {
        const response = await httpPromise;
        return response.data as T;
    } catch (err) {
        if (err.response) {
            return err.response.data as T;
        } else {
            return {
                success: false,
                errorCode: 'ERR_CONNECTION_REFUSED',
                data: null,
            };
        }
    }
}
