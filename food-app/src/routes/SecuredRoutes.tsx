import React from 'react';
import { Switch, Route, Router, Redirect } from 'react-router-dom';
import { History } from 'history';
import { RestaurantsPage } from '../pages/RestaurantsPage';
import { RestaurantDetailsPage } from '../pages/RestaurantDetailsPage';
import { OrderDetailsPage } from '../pages/OrderDetailsPage';
import { Layout } from '../components/Layout';
import { OrdersPage } from '../pages/OrdersPage';

interface SecuredRoutesProps {
    history: History<unknown>;
}

export const SecuredRoutes: React.FC<SecuredRoutesProps> = (props) => {
    const { history } = props;
    return (
        <Router history={history}>
            <Layout>
                <Switch>
                    <Redirect from="/signup" to="/" />
                    <Redirect from="/signin" to="/" />
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
                    <Route
                        exact
                        path="/orders/:id"
                        component={OrderDetailsPage}
                    />
                    <Route exact path="/orders" component={OrdersPage} />
                    <Route exact path="/" component={RestaurantsPage} />
                </Switch>
            </Layout>
        </Router>
    );
};
