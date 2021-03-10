import React from 'react';
import { Switch, useHistory, Route } from 'react-router-dom';
import { AppRoutes } from './constants/AppRoutes';
import { SignInPage } from './pages/SignInPage';
import { SignUpPage } from './pages/SignUpPage';

export const App: React.FC = () => {
    return (
        <Switch>
            <Route exact path="/" component={SignInPage} />
            <Route exact path={AppRoutes.SignIn} component={SignInPage} />
            <Route exact path={AppRoutes.SignUp} component={SignUpPage} />
        </Switch>
    );
};
