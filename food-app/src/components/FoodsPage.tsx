import { Grid } from '@material-ui/core';
import React from 'react';
import { useTranslation } from 'react-i18next';

import { makeStyles, Theme } from '@material-ui/core/styles';
import { Food } from '../api/models/Food';
import { useApiRequestHook } from '../api/hooks/useApiRequestHook';
import { getFoodService } from '../api/services/ServiceProvider';
import { AddFoodForm, AddFoodFormModal } from './AddFoodFormModal';
import { Loading } from './Loading';
import { AddCard } from './AddCard';
import { FoodCard } from './FoodCard';
import { EditFoodForm, EditFoodFormModal } from './EditFoodFormModal';
import { DeleteConfirmDialog } from './DeleteConfirmDialog';

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
        }),
        [t],
    );
    const { page } = useStyles();
    const [foods, setFoods] = React.useState<Food[]>([]);
    const [openAddFoodModal, setOpenAddFoodModal] = React.useState(false);
    const [openEditFoodModal, setOpenEditFoodModal] = React.useState(false);
    const [openDeleteConfirm, setOpenDeleteConfirm] = React.useState(false);
    const [editTarget, setEditTarget] = React.useState<Food>({
        id: '',
        price: 0,
        name: '',
        description: '',
    });
    const [deleteTargetId, setDeleteTargetId] = React.useState('');
    const [getFoodsApiRequest, makeGetFoodsApiRequest] = useApiRequestHook<
        Food[]
    >();
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
            {getFoodsApiRequest.state === 'idle' ||
            getFoodsApiRequest.state === 'loading' ||
            addFoodApiRequest.state === 'loading' ||
            editFoodApiRequest.state === 'loading' ||
            deleteFoodApiRequest.state === 'loading' ? (
                <Loading />
            ) : null}
            {foods.length === 0 &&
            getFoodsApiRequest.state === 'success' &&
            !owner ? (
                'NO CONENT'
            ) : (
                <Grid container spacing={3}>
                    {owner ? (
                        <Grid item key={'rest-add-button'}>
                            <AddCard
                                onClick={addFoodClick}
                                label={copy.addFood}
                            />
                        </Grid>
                    ) : null}
                    {foods.map((item) => (
                        <Grid item key={`rest-${item.id}`}>
                            <FoodCard
                                onClick={() => onFoodClick(item.id)}
                                onEdit={() => onFoodEditClick(item.id)}
                                onDelete={() => onFoodDeleteClick(item.id)}
                                title={item.name}
                                description={item.description}
                                price={item.price}
                            />
                        </Grid>
                    ))}
                </Grid>
            )}
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
                <DeleteConfirmDialog
                    open
                    onCancel={cancelDelete}
                    onConfirm={deleteFood}
                />
            )}
        </div>
    );
};
