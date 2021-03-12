import React from 'react';
import { Switch, Route, Router } from 'react-router-dom';
import { History } from 'history';
import { Home } from '../pages/Home';

interface SecuredRoutesProps {
    history: History<unknown>;
}

export const SecuredRoutes: React.FC<SecuredRoutesProps> = (props) => {
    const { history } = props;
    return (
        <Router history={history}>
            <Switch>
                <Route path="/" component={Home} />
            </Switch>
        </Router>
    );
};
