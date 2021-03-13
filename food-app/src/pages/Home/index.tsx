import {
    AppBar,
    Button,
    Grid,
    IconButton,
    InputAdornment,
    LinearProgress,
    Tab,
    Tabs,
    TextField,
} from '@material-ui/core';
import { Restaurant, ShoppingBasket } from '@material-ui/icons';
import React from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import { useHistory } from 'react-router-dom';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { getRestaurantService } from '../../api/services/ServiceProvider';
import { Loading } from '../../components/Loading';
import { RestaurantCard } from '../../components/RestaurantCard';
import {
    AddRestaurantForm,
    AddRestaurantFormModal,
} from '../../components/AddRestaurantFormModal';
import { AddCard } from '../../components/AddCard';
import { AppState } from '../../store';
import { HomePageStyles } from './styles';
import { AppRoutes } from '../../routes/AppRoutes';
import { RestaurantsPage } from '../RestaurantsPage';

const limit = 5;

function a11yProps(index: any) {
    return {
        id: `scrollable-force-tab-${index}`,
        'aria-controls': `scrollable-force-tabpanel-${index}`,
    };
}

export const Home: React.FC = () => {
    const { user } = useSelector((state: AppState) => {
        return {
            user: state.UserStore.user!,
        };
    });
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            loadMore: t('labels.loadMore'),
            search: t('labels.search'),
            restaurants: t('labels.restaurants'),
            orders: t('labels.orders'),
        }),
        [t],
    );
    const { page, footer, searchContainer, search } = HomePageStyles();
    const [currentTab, setCurrentTab] = React.useState(0);
    const handleTabChange = React.useCallback(
        (_event, value: number) => {
            setCurrentTab(value);
        },
        [setCurrentTab],
    );
    return (
        <div>
            <AppBar position="static" color="default">
                <Tabs
                    value={currentTab}
                    onChange={handleTabChange}
                    variant="scrollable"
                    scrollButtons="on"
                    indicatorColor="primary"
                    textColor="primary"
                >
                    <Tab
                        label={copy.restaurants}
                        icon={<Restaurant />}
                        {...a11yProps(0)}
                    />
                    <Tab
                        label={copy.orders}
                        icon={<ShoppingBasket />}
                        {...a11yProps(1)}
                    />
                </Tabs>
            </AppBar>
        </div>
    );
};
