import { Reducer } from 'redux';
import { User } from '../../api/models/User';
import { getUser, saveUser, removeUser } from '../../utils/storageUtil';
import { UserActions, UserState, SET_USER, LOGOUT_USER } from './types';

const user: User | null = getUser();

let accessToken: string | null = user !== null ? user.token : null;

export function getAccessToken() {
    return accessToken;
}

const initialState: UserState = { user: user };

const UserReducer: Reducer<UserState, UserActions> = (
    state = initialState,
    action,
) => {
    switch (action.type) {
        case SET_USER: {
            saveUser(action.user);
            accessToken = action.user.token;
            return { user: action.user };
        }
        case LOGOUT_USER: {
            removeUser();
            accessToken = null;
            return { user: null };
        }
    }
    return state;
};

export default UserReducer;
