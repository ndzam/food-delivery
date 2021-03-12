import { Reducer } from 'redux';
import { User } from '../../api/models/User';
import { getUser, saveUser } from '../../utils/storageUtil';
import { UserActions, UserState, SET_USER } from './types';

const user: User | null = getUser();

const initialState: UserState = { user: user };

const UserReducer: Reducer<UserState, UserActions> = (
    state = initialState,
    action,
) => {
    switch (action.type) {
        case SET_USER: {
            saveUser(action.user);
            return { user: action.user };
        }
    }
    return state;
};

export default UserReducer;
