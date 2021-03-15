import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { AppRoutes } from './AppRoutes';
import { SignInPage } from '../pages/SignInPage';
import { SignUpPage } from '../pages/SignUpPage';

export const PublicRoutes: React.FC = () => {
    return (
        <Switch>
            <Route exact path={AppRoutes.SignUp} component={SignUpPage} />
            <Route exact path={AppRoutes.SignIn} component={SignInPage} />
            <Route path="/" component={SignInPage} />
        </Switch>
    );
};
