import React from 'react';
import { useSelector } from 'react-redux';
import { SecuredRoutes } from './routes/SecuredRoutes';
import { PublicRoutes } from './routes/PublicRoutes';
import { createBrowserHistory } from 'history';
import { AppState } from './store';
import { Router } from 'react-router-dom';
import { Layout } from './components/Layout';

const history = createBrowserHistory();

export const App: React.FC = () => {
    const { user } = useSelector((state: AppState) => {
        return {
            user: state.UserStore.user,
        };
    });
    let content = user ? <SecuredRoutes /> : <PublicRoutes />;

    return (
        <Router history={history}>
            <Layout>{content}</Layout>
        </Router>
    );
};
