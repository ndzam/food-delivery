import { User } from '../../api/models/User';

export const SET_USER = 'SET_USER';
export const LOGOUT_USER = 'LOGOUT_USER';

export interface UserState {
    user: User | null;
}

export interface SetUser {
    type: typeof SET_USER;
    user: User;
}

export interface LogoutUser {
    type: typeof LOGOUT_USER;
}

export type UserActions = SetUser | LogoutUser;
