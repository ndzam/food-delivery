import React from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';
import { RestaurantsPage } from '../pages/RestaurantsPage';
import { RestaurantDetailsPage } from '../pages/RestaurantDetailsPage';
import { OrderDetailsPage } from '../pages/OrderDetailsPage';
import { OrdersPage } from '../pages/OrdersPage';

export const SecuredRoutes: React.FC = () => {
    return (
        <Switch>
            <Redirect from="/signup" to="/" />
            <Redirect from="/signin" to="/" />
            <Route
                exact
                path="/restaurants/:id"
                component={RestaurantDetailsPage}
            />
            <Route exact path="/restaurants" component={RestaurantsPage} />
            <Route exact path="/orders/:id" component={OrderDetailsPage} />
            <Route exact path="/orders" component={OrdersPage} />
            <Route exact path="/" component={RestaurantsPage} />
        </Switch>
    );
};
