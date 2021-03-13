import { User } from '../../api/models/User';
import { SetUser, SET_USER, LogoutUser, LOGOUT_USER } from './types';

export const setUser = (user: User): SetUser => ({
    type: SET_USER,
    user: user,
});

export const logoutUser = (): LogoutUser => ({
    type: LOGOUT_USER,
});
