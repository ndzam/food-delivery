import React from 'react';
import { Delete, Edit } from '@material-ui/icons';
import { useHistory, useParams } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Order } from '../../api/models/Order';
import { getOrderService } from '../../api/services/ServiceProvider';
import { OrderDetailsPageStyles } from './styles';
import { Loading } from '../../components/Loading';
import { IconButton, Typography } from '@material-ui/core';
import { AppRoutes } from '../../routes/AppRoutes';
import { FoodsPage } from '../../components/FoodsPage';
import { useSelector } from 'react-redux';
import { AppState } from '../../store';
import { Page404 } from '../../components/Page404';
import { DeleteConfirmDialog } from '../../components/DeleteConfirmDialog';

export const OrderDetailsPage: React.FC = () => {
    const {
        page,
        content,
        details,
        options,
        editStyle,
        deleteStyle,
    } = OrderDetailsPageStyles();
    const { push } = useHistory();
    const { id } = useParams<{ id: string }>();
    const { owner } = useSelector((state: AppState) => {
        return {
            owner: state.UserStore.user!.role === 'owner',
        };
    });
    const [order, setOrder] = React.useState<Order | null>(null);
    const [getOrderRequest, makeGetOrderRequest] = useApiRequestHook<Order>();

    React.useEffect(() => {
        const orderService = getOrderService();
        makeGetOrderRequest(orderService.getOrder(id));
    }, [makeGetOrderRequest, id]);

    React.useEffect(() => {
        if (getOrderRequest.state === 'success') {
            setOrder(getOrderRequest.response.data!);
        }
    }, [getOrderRequest, setOrder]);

    if (
        getOrderRequest.state === 'idle' ||
        getOrderRequest.state === 'loading'
    ) {
        return <Loading />;
    }

    if (getOrderRequest.state === 'success' && order === null) {
        return <Loading />;
    }

    if (
        getOrderRequest.state === 'fail' &&
        getOrderRequest.response.errorCode === 'NOT_FOUND'
    )
        return <Page404 />;

    console.log('ORDER', order);
    return (
        <div className={page}>
            <div className={content}>
                <div className={details}>
                    <Typography variant="h3" color="secondary">
                        {order!.restaurantId}
                    </Typography>
                    <Typography variant="h5">{order!.date}</Typography>
                </div>
            </div>
        </div>
    );
};
