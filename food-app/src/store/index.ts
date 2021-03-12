import { combineReducers, createStore } from 'redux';
import { UserState, UserActions } from './user/types';
import UserStore from './user/reducer';

export type AppState = {
    UserStore: UserState;
};

export type AppActions = UserActions;

const rootReducer = combineReducers<AppState, AppActions>({
    UserStore,
});

export default createStore(rootReducer);
