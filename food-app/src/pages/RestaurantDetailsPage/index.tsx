import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Delete, Edit, Error as ErrorIcon } from '@material-ui/icons';
import { useTranslation } from 'react-i18next';
import { useHistory, useParams } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Restaurant } from '../../api/models/Restaurant';
import { getRestaurantService } from '../../api/services/ServiceProvider';
import { RestaurantDetailsPageStyles } from './styles';
import { Loading } from '../../components/Loading';
import { IconButton, Typography } from '@material-ui/core';
import { AppRoutes } from '../../routes/AppRoutes';

export const RestaurantDetailsPage: React.FC = () => {
    const {
        page,
        content,
        details,
        options,
        editStyle,
        deleteStyle,
    } = RestaurantDetailsPageStyles();
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            name: t('labels.name'),
            description: t('labels.description'),
        }),
        [t],
    );
    const { push } = useHistory();
    const { id } = useParams<{ id: string }>();

    const [
        getRestaurantRequest,
        makeGetRestaurantRequest,
    ] = useApiRequestHook<Restaurant>();

    const [
        deleteRestaurantRequest,
        makeDeleteRestaurantRequest,
    ] = useApiRequestHook();

    React.useEffect(() => {
        const restaurantService = getRestaurantService();
        makeGetRestaurantRequest(restaurantService.getRestaurant(id));
    }, [makeGetRestaurantRequest, id]);

    const onEdit = React.useCallback(() => {
        console.log('edit');
    }, []);
    const onDelete = React.useCallback(() => {
        const restaurantService = getRestaurantService();
        makeDeleteRestaurantRequest(restaurantService.deleteRestaurant(id));
    }, [makeDeleteRestaurantRequest, id]);

    React.useEffect(() => {
        console.log(deleteRestaurantRequest);
        if (deleteRestaurantRequest.state === 'success') {
            push(AppRoutes.Restaurants);
        }
    }, [deleteRestaurantRequest, push]);

    if (
        getRestaurantRequest.state === 'idle' ||
        getRestaurantRequest.state === 'loading'
    )
        return <Loading />;

    return (
        <div className={page}>
            {deleteRestaurantRequest.state === 'loading' ? <Loading /> : null}
            <div className={content}>
                <div className={details}>
                    <Typography variant="h3" color="secondary" noWrap>
                        {getRestaurantRequest.response.data?.name}
                    </Typography>
                    <Typography variant="h5">
                        {getRestaurantRequest.response.data?.description}
                    </Typography>
                </div>
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
                        onClick={onDelete}
                    >
                        <Delete htmlColor="#dc3545" />
                    </IconButton>
                </div>
            </div>
        </div>
    );
};
