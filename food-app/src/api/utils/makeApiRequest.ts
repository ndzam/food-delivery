import { AxiosPromise } from 'axios';
import Store from '../../store';
import { logoutUser } from '../../store/user/actions';

export async function makeApiRequest<T>(httpPromise: AxiosPromise<unknown>) {
    try {
        const response = await httpPromise;
        if (response.status === 204) {
            return {
                success: true,
                data: null,
            };
        }
        return response.data as T;
    } catch (err) {
        if (err.response) {
            if (err.response.status === 401) {
                Store.dispatch(logoutUser());
            }
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
