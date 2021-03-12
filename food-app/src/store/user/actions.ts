import { User } from '../../api/models/User';
import { SetUser, SET_USER } from './types';

export const setUser = (user: User): SetUser => ({
    type: SET_USER,
    user: user,
});
