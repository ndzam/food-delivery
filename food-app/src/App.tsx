import React from 'react';
import { useSelector } from 'react-redux';
import { SecuredRoutes } from './routes/SecuredRoutes';
import { PublicRoutes } from './routes/PublicRoutes';
import { createBrowserHistory } from 'history';
import { AppState } from './store';

const history = createBrowserHistory();

export const App: React.FC = () => {
    const { user } = useSelector((state: AppState) => {
        return {
            user: state.UserStore.user,
        };
    });
    if (user) {
        return <SecuredRoutes history={history} />;
    }
    return <PublicRoutes history={history} />;
};
