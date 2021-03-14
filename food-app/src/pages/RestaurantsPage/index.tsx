import {
    Button,
    Grid,
    IconButton,
    InputAdornment,
    TextField,
    Typography,
} from '@material-ui/core';
import { Search } from '@material-ui/icons';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useHistory } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Restaurant } from '../../api/models/Restaurant';
import { getRestaurantService } from '../../api/services/ServiceProvider';
import { Loading } from '../../components/Loading';
import { RestaurantCard } from '../../components/RestaurantCard';
import {
    AddRestaurantForm,
    AddRestaurantFormModal,
} from '../../components/AddRestaurantFormModal';
import { AddCard } from '../../components/AddCard';
import { AppState } from '../../store';
import { RestaurantsPageStyles } from './styles';
import { AppRoutes } from '../../routes/AppRoutes';
import { useSelector } from 'react-redux';
import { Page404 } from '../../components/Page404';

const limit = 5;

export const RestaurantsPage: React.FC = () => {
    const { owner } = useSelector((state: AppState) => {
        return {
            owner: state.UserStore.user!.role === 'owner',
        };
    });
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            loadMore: t('labels.loadMore'),
            search: t('labels.search'),
            addRestaurant: t('labels.addRestaurant'),
            restaurants: t('labels.restaurants'),
        }),
        [t],
    );
    const {
        page,
        footer,
        searchContainer,
        search,
        title,
    } = RestaurantsPageStyles();
    const [restaurants, setRestaurants] = React.useState<Restaurant[]>([]);
    const [canLoadMore, setCanLoadMore] = React.useState(false);
    const [lastSearch, setLastSearch] = React.useState('');
    const [searchTerm, setSearchTerm] = React.useState('');
    const [openAddRestaurantModal, setOpenAddRestaurantModal] = React.useState(
        false,
    );
    const { push } = useHistory();

    const [
        getRestaurantsApiRequest,
        makeGetRestaurantsApiRequest,
    ] = useApiRequestHook<Array<Restaurant>>();
    const [
        addRestaurantApiRequest,
        makeAddRestaurantApiRequest,
    ] = useApiRequestHook<Restaurant>();

    React.useEffect(() => {
        const restaurantService = getRestaurantService();
        makeGetRestaurantsApiRequest(
            restaurantService.getRestaurants(searchTerm, null, limit),
        );
    }, [makeGetRestaurantsApiRequest, searchTerm]);

    const cardClick = React.useCallback(
        (id: string) => {
            push(AppRoutes.Restaurants + `/${id}`);
        },
        [push],
    );

    React.useEffect(() => {
        if (getRestaurantsApiRequest.state === 'success') {
            const restaurantResponse = getRestaurantsApiRequest.response.data;
            if (restaurantResponse) {
                setRestaurants(
                    lastSearch !== searchTerm
                        ? restaurantResponse
                        : restaurants.concat(restaurantResponse),
                );
                setCanLoadMore(restaurantResponse.length === limit);
                setLastSearch(searchTerm);
            }
        }
        //eslint-disable-next-line
    }, [getRestaurantsApiRequest, setRestaurants]);
    const loadMore = React.useCallback(() => {
        const lastId = restaurants[restaurants.length - 1].id;
        const restaurantService = getRestaurantService();
        makeGetRestaurantsApiRequest(
            restaurantService.getRestaurants(searchTerm, lastId, limit),
        );
    }, [restaurants, makeGetRestaurantsApiRequest, searchTerm]);

    const onSearchChange = React.useCallback(
        (event) => {
            setSearchTerm(event.target.value);
        },
        [setSearchTerm],
    );

    const onSearch = React.useCallback(() => {
        //const lastId = restaurants[restaurants.length - 1].id;
        const restaurantService = getRestaurantService();
        makeGetRestaurantsApiRequest(
            restaurantService.getRestaurants(searchTerm, null, limit),
        );
    }, [searchTerm, makeGetRestaurantsApiRequest]);
    const handleMouseDownSearch = React.useCallback(
        (event: React.MouseEvent<HTMLButtonElement>) => {
            event.preventDefault();
        },
        [],
    );
    const onSearchKeyPress = React.useCallback(
        (e: React.KeyboardEvent<HTMLDivElement>) => {
            if (!e.shiftKey && e.which === 13) {
                e.preventDefault();
                onSearch();
            }
        },
        [onSearch],
    );
    const addRestaurantClick = React.useCallback(() => {
        setOpenAddRestaurantModal(true);
    }, [setOpenAddRestaurantModal]);
    const handleRestaurantAddClose = React.useCallback(() => {
        setOpenAddRestaurantModal(false);
    }, [setOpenAddRestaurantModal]);
    const addRestaurant = React.useCallback(
        (data: AddRestaurantForm) => {
            const restaurantService = getRestaurantService();
            makeAddRestaurantApiRequest(
                restaurantService.createRestaurant(data.name, data.description),
            );
        },
        [makeAddRestaurantApiRequest],
    );
    React.useEffect(() => {
        if (addRestaurantApiRequest.state === 'success') {
            push(
                AppRoutes.Restaurants +
                    `/${addRestaurantApiRequest.response.data?.id}`,
            );
        }
        //eslint-disable-next-line
    }, [addRestaurantApiRequest, push]);
    const addErrorCode =
        addRestaurantApiRequest.state === 'fail'
            ? addRestaurantApiRequest.response.errorCode
            : '';

    return (
        <div className={page}>
            <Typography variant="h3" color="primary" className={title}>
                {copy.restaurants}
            </Typography>
            {getRestaurantsApiRequest.state === 'idle' ||
            getRestaurantsApiRequest.state === 'loading' ||
            addRestaurantApiRequest.state === 'loading' ? (
                <Loading />
            ) : null}
            <div className={searchContainer}>
                <TextField
                    id="search"
                    name="search"
                    label={copy.search}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position="end">
                                <IconButton
                                    color="primary"
                                    aria-label="toggle password visibility"
                                    onClick={onSearch}
                                    onMouseDown={handleMouseDownSearch}
                                    edge="end"
                                >
                                    <Search />
                                </IconButton>
                            </InputAdornment>
                        ),
                    }}
                    fullWidth
                    size="medium"
                    className={search}
                    value={searchTerm}
                    onChange={onSearchChange}
                    onKeyDown={onSearchKeyPress}
                />
            </div>
            {restaurants.length === 0 &&
            getRestaurantsApiRequest.state === 'success' &&
            !owner ? (
                'NO CONENT'
            ) : (
                <Grid container spacing={3}>
                    {owner ? (
                        <Grid item key={'rest-add-button'}>
                            <AddCard
                                onClick={addRestaurantClick}
                                label={copy.addRestaurant}
                            />
                        </Grid>
                    ) : null}
                    {restaurants.map((item) => (
                        <Grid item key={`rest-${item.id}`}>
                            <RestaurantCard
                                onClick={() => cardClick(item.id)}
                                title={item.name}
                                description={item.description}
                            />
                        </Grid>
                    ))}
                </Grid>
            )}
            {restaurants.length === 0 || !canLoadMore ? null : (
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
            <AddRestaurantFormModal
                open={openAddRestaurantModal}
                handleClose={handleRestaurantAddClose}
                onSubmit={addRestaurant}
                isLoading={addRestaurantApiRequest.state === 'loading'}
                errorCode={addErrorCode}
            />
        </div>
    );
};
