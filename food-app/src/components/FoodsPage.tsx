import { Button, Grid } from '@material-ui/core';
import React from 'react';
import { useTranslation } from 'react-i18next';

import { makeStyles, Theme } from '@material-ui/core/styles';
import { Food } from '../api/models/Food';
import { useApiRequestHook } from '../api/hooks/useApiRequestHook';
import {
    getFoodService,
    getOrderService,
} from '../api/services/ServiceProvider';
import { AddFoodForm, AddFoodFormModal } from './AddFoodFormModal';
import { Loading } from './Loading';
import { AddCard } from './AddCard';
import { FoodCard } from './FoodCard';
import { EditFoodForm, EditFoodFormModal } from './EditFoodFormModal';
import { ConfirmDialog } from './ConfirmDialog';
import { ConfirmationNumber } from '@material-ui/icons';
import { OrderItem } from '../api/models/OrderItem';
import { Order } from '../api/models/Order';
import { useHistory } from 'react-router-dom';
import { AppRoutes } from '../routes/AppRoutes';
import { Error } from '../components/Error';

export const useStyles = makeStyles((theme: Theme) => ({
    page: {
        width: '100%',
        marginTop: 32,
        borderTopStyle: 'solid',
        borderTopWidth: 0.5,
        borderTopColor: '#EEE',
        paddingTop: 32,
        paddingBottom: 32,
    },
    orderContainer: {
        marginBottom: 32,
    },
}));

interface FoodsPageProps {
    restaurantId: string;
    owner?: boolean;
}
export const FoodsPage: React.FC<FoodsPageProps> = (props) => {
    const { restaurantId, owner = false } = props;
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            loadMore: t('labels.loadMore'),
            search: t('labels.search'),
            addFood: t('labels.addFood'),
            order: t('labels.order'),
            confirmDelete: t('confirm.delete'),
            delete: t('labels.delete'),
        }),
        [t],
    );
    const { page, orderContainer } = useStyles();
    const { push } = useHistory();
    const [foods, setFoods] = React.useState<Food[]>([]);
    const [openAddFoodModal, setOpenAddFoodModal] = React.useState(false);
    const [openEditFoodModal, setOpenEditFoodModal] = React.useState(false);
    const [openDeleteConfirm, setOpenDeleteConfirm] = React.useState(false);
    const [totalPrice, setTotalPrice] = React.useState(0);
    const [editTarget, setEditTarget] = React.useState<Food>({
        id: '',
        price: 0,
        name: '',
        description: '',
    });
    const [orderedFoods, setOrderedFoods] = React.useState<{
        [id: string]: number;
    }>({});
    const [deleteTargetId, setDeleteTargetId] = React.useState('');
    const [getFoodsApiRequest, makeGetFoodsApiRequest] = useApiRequestHook<
        Food[]
    >();
    const [
        addOrderApiRequest,
        makeAddOrderApiRequest,
    ] = useApiRequestHook<Order>();
    const [
        addFoodApiRequest,
        makeAddFoodApiRequest,
    ] = useApiRequestHook<Food>();
    const [
        editFoodApiRequest,
        makeEditFoodApiRequest,
    ] = useApiRequestHook<Food>();
    const [
        deleteFoodApiRequest,
        makeDeleteFoodApiRequest,
    ] = useApiRequestHook();

    const [canOrder, setCanOrder] = React.useState(true);

    React.useEffect(() => {
        const foodService = getFoodService();
        makeGetFoodsApiRequest(foodService.getFoods(restaurantId));
    }, [makeGetFoodsApiRequest, restaurantId]);

    const onFoodClick = React.useCallback((id: string) => {
        console.log('ID', id);
    }, []);

    const onFoodEditClick = React.useCallback(
        (id: string) => {
            const food = foods.find((x) => x.id === id);
            if (food) {
                setEditTarget(food);
                setOpenEditFoodModal(true);
            }
        },
        [setEditTarget, setOpenEditFoodModal, foods],
    );

    const onFoodDeleteClick = React.useCallback(
        (id: string) => {
            setDeleteTargetId(id);
            setOpenDeleteConfirm(true);
        },
        [setOpenDeleteConfirm, setDeleteTargetId],
    );

    const deleteFood = React.useCallback(() => {
        const foodService = getFoodService();
        makeDeleteFoodApiRequest(
            foodService.deleteFood(restaurantId, deleteTargetId),
        );
    }, [makeDeleteFoodApiRequest, deleteTargetId, restaurantId]);

    const cancelDelete = React.useCallback(() => {
        setDeleteTargetId('');
        setOpenDeleteConfirm(false);
    }, [setDeleteTargetId, setOpenDeleteConfirm]);

    React.useEffect(() => {
        if (getFoodsApiRequest.state === 'success') {
            const foodResponse = getFoodsApiRequest.response.data;
            if (foodResponse) {
                setFoods(foodResponse);
            }
        }
    }, [getFoodsApiRequest, setFoods]);
    const addFoodClick = React.useCallback(() => {
        setOpenAddFoodModal(true);
    }, [setOpenAddFoodModal]);
    const handleFoodAddClose = React.useCallback(() => {
        setOpenAddFoodModal(false);
    }, [setOpenAddFoodModal]);
    const handleFoodEditClose = React.useCallback(() => {
        setOpenEditFoodModal(false);
    }, [setOpenEditFoodModal]);
    const addFood = React.useCallback(
        (data: AddFoodForm) => {
            const foodService = getFoodService();
            makeAddFoodApiRequest(
                foodService.createFood(
                    restaurantId,
                    data.name,
                    data.description,
                    data.price,
                ),
            );
        },
        [makeAddFoodApiRequest, restaurantId],
    );
    const editFood = React.useCallback(
        (data: EditFoodForm) => {
            const foodService = getFoodService();
            makeEditFoodApiRequest(
                foodService.editFood(
                    restaurantId,
                    data.id,
                    data.name,
                    data.description,
                    data.price,
                ),
            );
        },
        [makeEditFoodApiRequest, restaurantId],
    );
    React.useEffect(() => {
        if (addFoodApiRequest.state === 'success') {
            setOpenAddFoodModal(false);
            if (addFoodApiRequest.response) {
                const newFoods = [...foods, addFoodApiRequest.response.data!];
                setFoods(newFoods);
            }
        }
        //eslint-disable-next-line
    }, [addFoodApiRequest]);
    React.useEffect(() => {
        if (editFoodApiRequest.state === 'success') {
            setOpenEditFoodModal(false);
            if (editFoodApiRequest.response) {
                const newFoods = [...foods];
                const index = newFoods.findIndex(
                    (x) => x.id === editFoodApiRequest.response.data!.id,
                );
                if (index >= 0) {
                    newFoods[index] = editFoodApiRequest.response.data!;
                }
                setFoods(newFoods);
            }
        }
        //eslint-disable-next-line
    }, [editFoodApiRequest]);

    React.useEffect(() => {
        if (deleteFoodApiRequest.state === 'success') {
            const newFoods = foods.filter((x) => x.id !== deleteTargetId);
            setFoods(newFoods);
            setDeleteTargetId('');
            setOpenDeleteConfirm(false);
        }
        //eslint-disable-next-line
    }, [deleteFoodApiRequest]);

    const addFoodToOrder = React.useCallback(
        (id: string) => {
            const food = foods.find((x) => x.id === id);
            if (food) {
                const newOrders = { ...orderedFoods };
                const count = newOrders[id];
                newOrders[id] = count ? count + 1 : 1;
                setOrderedFoods(newOrders);
                setTotalPrice(totalPrice + food.price);
            }
        },
        [orderedFoods, setOrderedFoods, setTotalPrice, totalPrice, foods],
    );

    const removeFoodToOrder = React.useCallback(
        (id: string) => {
            const food = foods.find((x) => x.id === id);
            if (food) {
                const newOrders = { ...orderedFoods };
                const count = newOrders[id];
                if (count > 0) {
                    newOrders[id] = Math.max(0, count ? count - 1 : 0);
                    setOrderedFoods(newOrders);
                    setTotalPrice(Math.max(0, totalPrice - food.price));
                }
            }
        },
        [orderedFoods, setOrderedFoods, setTotalPrice, totalPrice, foods],
    );

    const makeOrder = React.useCallback(() => {
        const items: OrderItem[] = [];
        for (let foodId in orderedFoods) {
            const quantity = orderedFoods[foodId];
            items.push({ foodId, quantity });
        }
        const orderService = getOrderService();
        makeAddOrderApiRequest(orderService.createOrder(restaurantId, items));
    }, [orderedFoods, makeAddOrderApiRequest, restaurantId]);

    React.useEffect(() => {
        if (addOrderApiRequest.state === 'success') {
            push(
                AppRoutes.Orders +
                    `/${addOrderApiRequest.response.data?.orderId}`,
            );
        } else if (addOrderApiRequest.state === 'fail') {
            setCanOrder(false);
        }
        //eslint-disable-next-line
    }, [addOrderApiRequest, push]);

    const editErrorCode =
        editFoodApiRequest.state === 'fail'
            ? editFoodApiRequest.response.errorCode
            : '';
    const addErrorCode =
        addFoodApiRequest.state === 'fail'
            ? addFoodApiRequest.response.errorCode
            : '';

    return (
        <div className={page}>
            {canOrder ? null : <Error errorCode="CAN_NOT_ORDER" />}
            {getFoodsApiRequest.state === 'idle' ||
            getFoodsApiRequest.state === 'loading' ||
            addFoodApiRequest.state === 'loading' ||
            editFoodApiRequest.state === 'loading' ||
            deleteFoodApiRequest.state === 'loading' ||
            addOrderApiRequest.state === 'loading' ? (
                <Loading />
            ) : null}
            {!owner ? (
                <div className={orderContainer}>
                    <Button
                        variant="contained"
                        color="primary"
                        size="large"
                        startIcon={<ConfirmationNumber />}
                        disabled={totalPrice === 0 || !canOrder}
                        onClick={makeOrder}
                    >
                        {totalPrice > 0
                            ? `${copy.order} (${totalPrice}$)`
                            : copy.order}
                    </Button>
                </div>
            ) : null}
            <Grid container spacing={3}>
                {owner ? (
                    <Grid item key={'rest-add-button'}>
                        <AddCard onClick={addFoodClick} label={copy.addFood} />
                    </Grid>
                ) : null}
                {foods.map((item) => (
                    <Grid item key={`rest-${item.id}`}>
                        <FoodCard
                            onClick={() => onFoodClick(item.id)}
                            onEdit={() => onFoodEditClick(item.id)}
                            onDelete={() => onFoodDeleteClick(item.id)}
                            onAdd={() => addFoodToOrder(item.id)}
                            onRemove={() => removeFoodToOrder(item.id)}
                            title={item.name}
                            description={item.description}
                            price={item.price}
                            canEdit={owner}
                            count={
                                orderedFoods[item.id]
                                    ? orderedFoods[item.id]
                                    : 0
                            }
                        />
                    </Grid>
                ))}
            </Grid>
            <AddFoodFormModal
                open={openAddFoodModal}
                handleClose={handleFoodAddClose}
                onSubmit={addFood}
                isLoading={addFoodApiRequest.state === 'loading'}
                errorCode={addErrorCode}
            />
            {openEditFoodModal && (
                <EditFoodFormModal
                    open
                    handleClose={handleFoodEditClose}
                    onSubmit={editFood}
                    isLoading={editFoodApiRequest.state === 'loading'}
                    data={editTarget}
                    errorCode={editErrorCode}
                />
            )}
            {openDeleteConfirm && (
                <ConfirmDialog
                    open
                    onCancel={cancelDelete}
                    onConfirm={deleteFood}
                    text={copy.confirmDelete}
                    confirmLabel={copy.delete}
                />
            )}
        </div>
    );
};
