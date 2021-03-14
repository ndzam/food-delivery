import { Button, Grid, Typography } from '@material-ui/core';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Order } from '../../api/models/Order';
import { getOrderService } from '../../api/services/ServiceProvider';
import { Loading } from '../../components/Loading';
import { RestaurantCard } from '../../components/RestaurantCard';
import { AppState } from '../../store';
import { OrdersPageStyles } from './styles';
import { AppRoutes } from '../../routes/AppRoutes';
import { useSelector } from 'react-redux';
import { Page404 } from '../../components/Page404';
import { OrderCard } from '../../components/OrderCard';
import { getDate } from '../../utils/dateConverterUtils';

const limit = 5;

export const OrdersPage: React.FC = () => {
    const { owner } = useSelector((state: AppState) => {
        return {
            owner: state.UserStore.user!.role === 'owner',
        };
    });
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            loadMore: t('labels.loadMore'),
            orders: t('labels.orders'),
        }),
        [t],
    );
    const { page, footer, title } = OrdersPageStyles();
    const [orders, setOrders] = React.useState<Order[]>([]);
    const [canLoadMore, setCanLoadMore] = React.useState(false);

    const { push } = useHistory();

    const [getOrdersApiRequest, makeGetOrdersApiRequest] = useApiRequestHook<
        Order[]
    >();
    React.useEffect(() => {
        const orderService = getOrderService();
        makeGetOrdersApiRequest(orderService.getOrders(null, limit));
    }, [makeGetOrdersApiRequest]);

    const cardClick = React.useCallback(
        (id: string) => {
            push(AppRoutes.Orders + `/${id}`);
        },
        [push],
    );

    React.useEffect(() => {
        if (getOrdersApiRequest.state === 'success') {
            const orderResponse = getOrdersApiRequest.response.data;
            if (orderResponse) {
                setOrders(orders.concat(orderResponse));
                setCanLoadMore(orderResponse.length === limit);
            }
        }
        //eslint-disable-next-line
    }, [getOrdersApiRequest, setOrders]);
    const loadMore = React.useCallback(() => {
        const lastId = orders[orders.length - 1].orderId;
        const orderService = getOrderService();
        makeGetOrdersApiRequest(orderService.getOrders(lastId, limit));
    }, [orders, makeGetOrdersApiRequest]);

    return (
        <div className={page}>
            <Typography variant="h3" color="primary" className={title}>
                {copy.orders}
            </Typography>
            {getOrdersApiRequest.state === 'idle' ||
            getOrdersApiRequest.state === 'loading' ? (
                <Loading />
            ) : null}
            {orders.length === 0 && getOrdersApiRequest.state === 'success' ? (
                'NO CONENT'
            ) : (
                <Grid container spacing={3}>
                    {orders.map((item) => (
                        <Grid item key={`rest-${item.orderId}`}>
                            <OrderCard
                                onClick={() => cardClick(item.orderId)}
                                name={item.restaurantName}
                                status={item.status.currentStatus}
                                price={item.totalPrice.toString()}
                                date={getDate(item.date)}
                            />
                        </Grid>
                    ))}
                </Grid>
            )}
            {orders.length === 0 || !canLoadMore ? null : (
                <div className={footer}>
                    <Button
                        onClick={loadMore}
                        variant="outlined"
                        color="secondary"
                    >
                        {copy.loadMore}
                    </Button>
                </div>
            )}
        </div>
    );
};
