import { User } from '../../api/models/User';

export const SET_USER = 'SET_USER';

export interface UserState {
    user: User | null;
}

export interface SetUser {
    type: typeof SET_USER;
    user: User;
}

export type UserActions = SetUser;
