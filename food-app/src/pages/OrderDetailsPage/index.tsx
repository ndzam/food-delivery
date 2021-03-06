import React from 'react';
import { useParams } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Order } from '../../api/models/Order';
import {
    getOrderService,
    getRestaurantService,
} from '../../api/services/ServiceProvider';
import { OrderDetailsPageStyles } from './styles';
import { Loading } from '../../components/Loading';
import { Page404 } from '../../components/Page404';
import {
    Button,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Typography,
} from '@material-ui/core';
import { useTranslation } from 'react-i18next';
import { getDate } from '../../utils/dateConverterUtils';
import { Block, CheckCircleOutline } from '@material-ui/icons';
import { ConfirmDialog } from '../../components/ConfirmDialog';
import { useSelector } from 'react-redux';
import { AppState } from '../../store';
import { getNextStatus } from '../../api/models/OrderStatus';

export const OrderDetailsPage: React.FC = () => {
    const { owner } = useSelector((state: AppState) => {
        return {
            owner: state.UserStore.user!.role === 'owner',
        };
    });
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            orderDetails: t('labels.orderDetails'),
            order: t('labels.order'),
            from: t('labels.from'),
            price: t('labels.price'),
            quantity: t('labels.quantity'),
            total: t('labels.total'),
            name: t('labels.name'),
            status: t('labels.status'),
            currentStatus: t('labels.currentStatus'),
            date: t('labels.date'),
            to: t('labels.to'),
            cancel: t('labels.cancel'),
            block: t('labels.block'),
            blocked: t('labels.blocked'),
            delivered: t('labels.delivered'),
            confirmCancel: t('confirm.cancel'),
            confirmBlock: t('confirm.block'),
            confirmDelivery: t('confirm.delivery'),
            deleted: t('labels.deleted'),
        }),
        [t],
    );
    const {
        page,
        content,
        details,
        title,
        tableHolder,
        table,
        options,
        optionItem,
    } = OrderDetailsPageStyles();
    const { id } = useParams<{ id: string }>();
    const [order, setOrder] = React.useState<Order | null>(null);
    const [getOrderRequest, makeGetOrderRequest] = useApiRequestHook<Order>();
    const [blockUserRequest, makeUserBlockRequest] = useApiRequestHook();
    const [
        updateOrderStatuRequest,
        makeUpdateOrderStatuRequest,
    ] = useApiRequestHook<Order>();
    const [openCancelConfirmDialog, setOpenBlockConfirmDialog] = React.useState(
        false,
    );
    const [openConfirmDialog, setOpenConfirmDialog] = React.useState(false);

    React.useEffect(() => {
        const orderService = getOrderService();
        makeGetOrderRequest(orderService.getOrder(id));
    }, [makeGetOrderRequest, id]);

    React.useEffect(() => {
        if (getOrderRequest.state === 'success') {
            setOrder(getOrderRequest.response.data!);
        }
    }, [getOrderRequest, setOrder]);

    const openBlockConfirm = React.useCallback(() => {
        setOpenBlockConfirmDialog(true);
    }, [setOpenBlockConfirmDialog]);

    const closeConfirmDialogs = React.useCallback(() => {
        setOpenBlockConfirmDialog(false);
        setOpenConfirmDialog(false);
    }, [setOpenBlockConfirmDialog, setOpenConfirmDialog]);

    const blockUser = React.useCallback(() => {
        const restaurantService = getRestaurantService();
        makeUserBlockRequest(
            restaurantService.blockUser(order!.restaurantId, order!.userId),
        );
    }, [makeUserBlockRequest, order]);

    const openDeliveryConfirm = React.useCallback(() => {
        setOpenConfirmDialog(true);
    }, [setOpenConfirmDialog]);

    const updateStatus = React.useCallback(() => {
        const orderService = getOrderService();
        makeUpdateOrderStatuRequest(
            orderService.orderStatusChange(
                order?.orderId!,
                getNextStatus(
                    order?.status.currentStatus!,
                    owner ? 'owner' : 'user',
                )!,
            ),
        );
    }, [makeUpdateOrderStatuRequest, order, owner]);

    React.useEffect(() => {
        if (blockUserRequest.state === 'success') {
            if (order) {
                setOrder({ ...order, isUserBlocked: true });
            }
            setOpenBlockConfirmDialog(false);
        }
        //eslint-disable-next-line
    }, [blockUserRequest, setOrder, setOpenBlockConfirmDialog]);

    React.useEffect(() => {
        if (updateOrderStatuRequest.state === 'success') {
            setOrder(updateOrderStatuRequest.response.data!);
            setOpenConfirmDialog(false);
        } else if (updateOrderStatuRequest.state === 'fail') {
            const orderService = getOrderService();
            makeGetOrderRequest(orderService.getOrder(id));
            setOpenConfirmDialog(false);
        }
        //eslint-disable-next-line
    }, [updateOrderStatuRequest, setOrder, setOpenConfirmDialog]);

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

    const nextStatus = getNextStatus(
        order?.status.currentStatus!,
        owner ? 'owner' : 'user',
    );
    return (
        <div className={page}>
            <Typography variant="h3" color="primary" className={title}>
                {copy.orderDetails}
            </Typography>
            {updateOrderStatuRequest.state === 'loading' ||
            blockUserRequest.state === 'loading' ? (
                <Loading />
            ) : null}
            {blockUserRequest.state === 'loading' ? <Loading /> : null}
            <div className={content}>
                <div className={details}>
                    <Typography variant="h3" color="secondary">
                        {`${copy.from}: ${
                            order?.isRestaurantDeleted
                                ? order?.restaurantId +
                                  ' (' +
                                  copy.deleted +
                                  ')'
                                : order?.restaurantName
                        }`}
                    </Typography>
                    <Typography variant="h5">
                        {`${copy.to}: ${order?.userName} ${
                            order?.isUserBlocked ? '(' + copy.blocked + ')' : ''
                        }`}
                    </Typography>
                    <Typography variant="body1">
                        {`${copy.order}: ${order?.orderId}`}
                    </Typography>
                    <Typography variant="body1">
                        {`${copy.price}: $${order?.totalPrice}`}
                    </Typography>
                    <Typography variant="body1">
                        {`${copy.date}: ${getDate(order?.date!).toISOString()}`}
                    </Typography>
                </div>
                {order?.isRestaurantDeleted ? null : (
                    <div className={options}>
                        {owner ? (
                            <Button
                                variant="contained"
                                color="secondary"
                                size="large"
                                startIcon={<Block />}
                                fullWidth
                                className={optionItem}
                                onClick={openBlockConfirm}
                                disabled={order?.isUserBlocked}
                            >
                                {copy.block}
                            </Button>
                        ) : null}
                        {nextStatus === null || order?.isUserBlocked ? null : (
                            <Button
                                variant="contained"
                                color="primary"
                                size="large"
                                startIcon={<CheckCircleOutline />}
                                fullWidth
                                className={optionItem}
                                onClick={openDeliveryConfirm}
                            >
                                {t(`status.${nextStatus}`)}
                            </Button>
                        )}
                    </div>
                )}
            </div>
            {order?.isRestaurantDeleted ? null : (
                <div className={tableHolder}>
                    <TableContainer component={Paper}>
                        <Table className={table} aria-label="details table">
                            <TableHead>
                                <TableRow>
                                    <TableCell>{copy.name}</TableCell>
                                    <TableCell align="right">
                                        {copy.price}
                                    </TableCell>
                                    <TableCell align="right">
                                        {copy.quantity}
                                    </TableCell>
                                    <TableCell align="right">
                                        {copy.total}
                                    </TableCell>
                                </TableRow>
                            </TableHead>
                            <TableBody>
                                {order?.items.map((row) => (
                                    <TableRow key={row.foodId}>
                                        <TableCell component="th" scope="row">
                                            {row.foodName}
                                        </TableCell>
                                        <TableCell align="right">
                                            {`$ ${row.price}`}
                                        </TableCell>
                                        <TableCell align="right">
                                            {row.quantity}
                                        </TableCell>
                                        <TableCell align="right">
                                            {`$ ${row.price! * row.quantity}`}
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </div>
            )}

            <div className={tableHolder}>
                <Typography variant="h4" className={title}>
                    {`${copy.currentStatus}: ${t(
                        `status.${order?.status.currentStatus!}`,
                    )}`}
                </Typography>
                <TableContainer component={Paper}>
                    <Table className={table} aria-label="details table">
                        <TableHead>
                            <TableRow>
                                <TableCell>{copy.status}</TableCell>
                                <TableCell align="right">{copy.date}</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {order?.status.statusHistory.map((row, index) => (
                                <TableRow key={`row-${index}`}>
                                    <TableCell component="th" scope="row">
                                        {row.status}
                                    </TableCell>
                                    <TableCell align="right">
                                        {getDate(row.date!).toISOString()}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
            </div>
            {(openCancelConfirmDialog || openConfirmDialog) && (
                <ConfirmDialog
                    open
                    onCancel={closeConfirmDialogs}
                    onConfirm={
                        openCancelConfirmDialog ? blockUser : updateStatus
                    }
                    text={
                        openCancelConfirmDialog
                            ? copy.confirmBlock
                            : t(
                                  `confirm.status.${getNextStatus(
                                      order?.status.currentStatus!,
                                      owner ? 'owner' : 'user',
                                  )}`,
                              )
                    }
                />
            )}
        </div>
    );
};
