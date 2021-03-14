import React from 'react';
import { Switch, Route, Router } from 'react-router-dom';
import { History } from 'history';
import { RestaurantsPage } from '../pages/RestaurantsPage';
import { RestaurantDetailsPage } from '../pages/RestaurantDetailsPage';
import { Layout } from '../components/Layout';

interface SecuredRoutesProps {
    history: History<unknown>;
}

export const SecuredRoutes: React.FC<SecuredRoutesProps> = (props) => {
    const { history } = props;
    return (
        <Router history={history}>
            <Layout>
                <Switch>
                    <Route
                        exact
                        path="/restaurants/:id"
                        component={RestaurantDetailsPage}
                    />
                    <Route
                        exact
                        path="/restaurants"
                        component={RestaurantsPage}
                    />
                    <Route exact path="/" component={RestaurantsPage} />
                </Switch>
            </Layout>
        </Router>
    );
};
