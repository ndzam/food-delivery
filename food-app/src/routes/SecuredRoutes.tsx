import React from 'react';
import { Switch, Route, Redirect } from 'react-router-dom';
import { RestaurantsPage } from '../pages/RestaurantsPage';
import { RestaurantDetailsPage } from '../pages/RestaurantDetailsPage';
import { OrderDetailsPage } from '../pages/OrderDetailsPage';
import { OrdersPage } from '../pages/OrdersPage';
import { UsersPage } from '../pages/UsersPage';

export const SecuredRoutes: React.FC<{ owner: boolean }> = ({ owner }) => {
    return (
        <Switch>
            <Redirect from="/signup" to="/" />
            <Redirect from="/signin" to="/" />
            {!owner ? <Redirect from="/users" to="/" /> : null}
            <Route
                exact
                path="/restaurants/:id"
                component={RestaurantDetailsPage}
            />
            <Route exact path="/restaurants" component={RestaurantsPage} />
            <Route exact path="/orders/:id" component={OrderDetailsPage} />
            <Route exact path="/orders" component={OrdersPage} />
            {owner ? <Route exact path="/users" component={UsersPage} /> : null}
            <Route exact path="/" component={RestaurantsPage} />
        </Switch>
    );
};
