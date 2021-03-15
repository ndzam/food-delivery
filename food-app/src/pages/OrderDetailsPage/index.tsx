import React from 'react';
import { useParams } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Order } from '../../api/models/Order';
import { getOrderService } from '../../api/services/ServiceProvider';
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

export const OrderDetailsPage: React.FC = () => {
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
            delivered: t('labels.delivered'),
            confirmCancel: t('confirm.cancel'),
            confirmDelivery: t('confirm.delivery'),
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
    const [
        updateOrderStatusRequest,
        makeUpdateOrderStatusRequest,
    ] = useApiRequestHook<Order>();
    const [
        openCancelConfirmDialog,
        setOpenCancelConfirmDialog,
    ] = React.useState(false);
    const [
        openDeliveryConfirmDialog,
        setOpenDeliveryConfirmDialog,
    ] = React.useState(false);

    React.useEffect(() => {
        const orderService = getOrderService();
        makeGetOrderRequest(orderService.getOrder(id));
    }, [makeGetOrderRequest, id]);

    React.useEffect(() => {
        if (getOrderRequest.state === 'success') {
            setOrder(getOrderRequest.response.data!);
        }
    }, [getOrderRequest, setOrder]);

    const openCancelConfirm = React.useCallback(() => {
        setOpenCancelConfirmDialog(true);
    }, [setOpenCancelConfirmDialog]);

    const closeConfirmDialogs = React.useCallback(() => {
        setOpenCancelConfirmDialog(false);
        setOpenDeliveryConfirmDialog(false);
    }, [setOpenCancelConfirmDialog, setOpenDeliveryConfirmDialog]);

    const cancelOrder = React.useCallback(() => {
        const orderService = getOrderService();
        makeUpdateOrderStatusRequest(
            orderService.orderStatusChange(id, 'canceled'),
        );
    }, [makeUpdateOrderStatusRequest, id]);

    const openDeliveryConfirm = React.useCallback(() => {
        setOpenDeliveryConfirmDialog(true);
    }, [setOpenDeliveryConfirmDialog]);

    const deliverOrder = React.useCallback(() => {
        const orderService = getOrderService();
        makeUpdateOrderStatusRequest(
            orderService.orderStatusChange(id, 'delivered'),
        );
    }, [makeUpdateOrderStatusRequest, id]);

    React.useEffect(() => {
        if (updateOrderStatusRequest.state === 'success') {
            setOrder(updateOrderStatusRequest.response.data!);
            setOpenCancelConfirmDialog(false);
        }
    }, [updateOrderStatusRequest, setOrder, setOpenCancelConfirmDialog]);

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
            <Typography variant="h3" color="primary" className={title}>
                {copy.orderDetails}
            </Typography>
            {updateOrderStatusRequest.state === 'loading' ? <Loading /> : null}
            <div className={content}>
                <div className={details}>
                    <Typography variant="h3" color="secondary">
                        {`${copy.from}: ${order?.restaurantName}`}
                    </Typography>
                    <Typography variant="h5">
                        {`${copy.to}: ${order?.userName}`}
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
                {order?.status.currentStatus === 'canceled' ||
                order?.status.currentStatus === 'delivered' ? null : (
                    <div className={options}>
                        <Button
                            variant="contained"
                            color="secondary"
                            size="large"
                            startIcon={<Block />}
                            fullWidth
                            className={optionItem}
                            onClick={openCancelConfirm}
                        >
                            {copy.cancel}
                        </Button>
                        <Button
                            variant="contained"
                            color="primary"
                            size="large"
                            startIcon={<CheckCircleOutline />}
                            fullWidth
                            className={optionItem}
                            onClick={openDeliveryConfirm}
                        >
                            {copy.delivered}
                        </Button>
                    </div>
                )}
            </div>
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

            <div className={tableHolder}>
                <Typography variant="h4" className={title}>
                    {`${copy.currentStatus}: ${order?.status.currentStatus}`}
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
            {(openCancelConfirmDialog || openDeliveryConfirmDialog) && (
                <ConfirmDialog
                    open
                    onCancel={closeConfirmDialogs}
                    onConfirm={
                        openCancelConfirmDialog ? cancelOrder : deliverOrder
                    }
                    text={
                        openCancelConfirmDialog
                            ? copy.confirmCancel
                            : copy.confirmDelivery
                    }
                />
            )}
        </div>
    );
};
