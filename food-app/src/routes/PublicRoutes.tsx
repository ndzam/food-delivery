import React from 'react';
import { Switch, Route, Router } from 'react-router-dom';
import { History } from 'history';
import { AppRoutes } from './AppRoutes';
import { SignInPage } from '../pages/SignInPage';
import { SignUpPage } from '../pages/SignUpPage';

interface PublicRoutesProps {
    history: History<unknown>;
}

export const PublicRoutes: React.FC<PublicRoutesProps> = (props) => {
    const { history } = props;
    return (
        <Router history={history}>
            <Switch>
                <Route exact path={AppRoutes.SignUp} component={SignUpPage} />
                <Route exact path={AppRoutes.SignIn} component={SignInPage} />
                <Route path="/" component={SignInPage} />
            </Switch>
        </Router>
    );
};
