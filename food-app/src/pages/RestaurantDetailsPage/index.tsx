import React from 'react';
import { Delete, Edit } from '@material-ui/icons';
import { useHistory, useParams } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Restaurant } from '../../api/models/Restaurant';
import { getRestaurantService } from '../../api/services/ServiceProvider';
import { RestaurantDetailsPageStyles } from './styles';
import { Loading } from '../../components/Loading';
import { IconButton, Typography } from '@material-ui/core';
import { AppRoutes } from '../../routes/AppRoutes';
import {
    EditRestaurantForm,
    EditRestaurantFormModal,
} from '../../components/EditRestaurantFormModal';
import { FoodsPage } from '../../components/FoodsPage';
import { useSelector } from 'react-redux';
import { AppState } from '../../store';
import { Page404 } from '../../components/Page404';
import { ConfirmDialog } from '../../components/ConfirmDialog';
import { useTranslation } from 'react-i18next';

export const RestaurantDetailsPage: React.FC = () => {
    const {
        page,
        content,
        details,
        options,
        editStyle,
        deleteStyle,
        title,
    } = RestaurantDetailsPageStyles();
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            restaurant: t('labels.restaurant'),
            confirmDelete: t('confirm.delete'),
            delete: t('labels.delete'),
        }),
        [t],
    );
    const { push } = useHistory();
    const { id } = useParams<{ id: string }>();
    const { owner } = useSelector((state: AppState) => {
        return {
            owner: state.UserStore.user!.role === 'owner',
        };
    });
    const [restaurant, setRestaurant] = React.useState<Restaurant | null>(null);
    const [openDeleteConfirm, setOpenDeleteConfirm] = React.useState(false);
    const [
        openEditRestaurantModal,
        setOpenEditRestaurantModal,
    ] = React.useState(false);
    const [
        getRestaurantRequest,
        makeGetRestaurantRequest,
    ] = useApiRequestHook<Restaurant>();

    const [
        deleteRestaurantApiRequest,
        makeDeleteRestaurantApiRequest,
    ] = useApiRequestHook();

    const [
        editRestaurantApiRequest,
        makeEditRestaurantApiRequest,
    ] = useApiRequestHook<Restaurant>();

    React.useEffect(() => {
        const restaurantService = getRestaurantService();
        makeGetRestaurantRequest(restaurantService.getRestaurant(id));
    }, [makeGetRestaurantRequest, id]);

    React.useEffect(() => {
        if (getRestaurantRequest.state === 'success') {
            setRestaurant(getRestaurantRequest.response.data!);
        }
    }, [getRestaurantRequest, setRestaurant]);

    const onEdit = React.useCallback(() => {
        setOpenEditRestaurantModal(true);
    }, [setOpenEditRestaurantModal]);
    const onDeleteClick = React.useCallback(() => {
        setOpenDeleteConfirm(true);
    }, [setOpenDeleteConfirm]);

    const onDeleteCancel = React.useCallback(() => {
        setOpenDeleteConfirm(false);
    }, [setOpenDeleteConfirm]);

    const onDeleteConfirm = React.useCallback(() => {
        const restaurantService = getRestaurantService();
        makeDeleteRestaurantApiRequest(restaurantService.deleteRestaurant(id));
    }, [makeDeleteRestaurantApiRequest, id]);

    React.useEffect(() => {
        if (deleteRestaurantApiRequest.state === 'success') {
            push(AppRoutes.Restaurants);
        }
    }, [deleteRestaurantApiRequest, push]);

    const handleRestaurantEditClose = React.useCallback(() => {
        setOpenEditRestaurantModal(false);
    }, [setOpenEditRestaurantModal]);

    const editRestaurant = React.useCallback(
        (data: EditRestaurantForm) => {
            const restaurantService = getRestaurantService();
            makeEditRestaurantApiRequest(
                restaurantService.editRestaurant(
                    data.id,
                    data.name,
                    data.description,
                ),
            );
        },
        [makeEditRestaurantApiRequest],
    );

    React.useEffect(() => {
        if (editRestaurantApiRequest.state === 'success') {
            setRestaurant(editRestaurantApiRequest.response.data!);
            setOpenEditRestaurantModal(false);
        }
        //eslint-disable-next-line
    }, [editRestaurantApiRequest, setRestaurant, setOpenEditRestaurantModal]);

    if (
        getRestaurantRequest.state === 'idle' ||
        getRestaurantRequest.state === 'loading'
    ) {
        return <Loading />;
    }

    if (getRestaurantRequest.state === 'success' && restaurant === null) {
        return <Loading />;
    }

    const editErrorCode =
        editRestaurantApiRequest.state === 'fail'
            ? editRestaurantApiRequest.response.errorCode
            : '';

    if (
        getRestaurantRequest.state === 'fail' &&
        (getRestaurantRequest.response.errorCode === 'NOT_FOUND' ||
            getRestaurantRequest.response.errorCode === 'FORBID')
    )
        return <Page404 />;
    return (
        <div className={page}>
            <Typography variant="h4" color="primary" className={title}>
                {copy.restaurant}
            </Typography>
            {deleteRestaurantApiRequest.state === 'loading' ||
            editRestaurantApiRequest.state === 'loading' ? (
                <Loading />
            ) : null}
            <div className={content}>
                <div className={details}>
                    <Typography variant="h3" color="secondary">
                        {restaurant!.name}
                    </Typography>
                    <Typography variant="h5">
                        {restaurant!.description}
                    </Typography>
                </div>
                {owner ? (
                    <div className={options}>
                        <IconButton
                            aria-label="log out"
                            component="span"
                            className={editStyle}
                            onClick={onEdit}
                        >
                            <Edit htmlColor="#ffc107" />
                        </IconButton>
                        <IconButton
                            aria-label="log out"
                            component="span"
                            className={deleteStyle}
                            onClick={onDeleteClick}
                        >
                            <Delete htmlColor="#dc3545" />
                        </IconButton>
                    </div>
                ) : null}
            </div>
            <FoodsPage restaurantId={restaurant!.id} owner={owner} />
            <EditRestaurantFormModal
                open={openEditRestaurantModal}
                handleClose={handleRestaurantEditClose}
                onSubmit={editRestaurant}
                isLoading={editRestaurantApiRequest.state === 'loading'}
                data={restaurant!}
                errorCode={editErrorCode}
            />
            {openDeleteConfirm && (
                <ConfirmDialog
                    open
                    onCancel={onDeleteCancel}
                    onConfirm={onDeleteConfirm}
                    text={copy.confirmDelete}
                    confirmLabel={copy.delete}
                />
            )}
        </div>
    );
};
