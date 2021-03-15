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
                return {
                    success: false,
                    errorCode: 'UNAUTHORIZED',
                    data: null,
                };
            }
            if (err.response.status === 404) {
                return {
                    success: false,
                    errorCode: 'NOT_FOUND',
                    data: null,
                };
            }
            if (err.response.status === 403) {
                return {
                    success: false,
                    errorCode: 'FORBID',
                    data: null,
                };
            }
            if (err.response.status === 400 && !err.response.data) {
                return {
                    success: false,
                    errorCode: 'BAD_REQUEST',
                    data: null,
                };
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
