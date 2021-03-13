import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { AddCircle, Error as ErrorIcon, ExitToApp } from '@material-ui/icons';
import { useTranslation } from 'react-i18next';
import { IconButton, Link, Typography } from '@material-ui/core';
import { AppRoutes } from '../routes/AppRoutes';
import { useHistory } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { AppState } from '../store';
import { logoutUser } from '../store/user/actions';

const useStyle = makeStyles((theme: Theme) => ({
    page: {
        width: '100%',
    },
    header: {
        backgroundColor: theme.palette.background.paper,
        width: '100%',
        height: 45,
        borderBottomColor: '#EEE',
        borderBottomStyle: 'solid',
        borderBottomWidth: 2,
        display: 'flex',
        alignItems: 'center',
        marginBottom: 16,
    },
    contentWraper: {
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        width: '100%',
    },
    content: {
        width: 1080,
    },
    logo: {
        marginLeft: 65,
        cursor: 'pointer',
    },
    options: {
        marginLeft: 'auto',
        marginRight: 65,
        display: 'flex',
        alignItems: 'center',
    },
    optionItem: {
        marginLeft: 8,
        marginRight: 8,
    },
}));

interface LayoutProps {}

export const Layout: React.FC<LayoutProps> = (props) => {
    const {
        page,
        header,
        content,
        logo,
        options,
        optionItem,
        contentWraper,
    } = useStyle();
    const { user } = useSelector((state: AppState) => {
        return {
            user: state.UserStore.user,
        };
    });
    const dispatch = useDispatch();
    const { t } = useTranslation();
    const { push } = useHistory();
    const copy = React.useMemo(
        () => ({
            org: t('labels.org'),
            restaurants: t('labels.restaurants'),
            orders: t('labels.orders'),
        }),
        [t],
    );
    const goToHome = React.useCallback(() => {
        push(AppRoutes.Home);
    }, [push]);
    const logOut = React.useCallback(() => {
        dispatch(logoutUser());
    }, [dispatch]);
    return (
        <div className={page}>
            <div className={header}>
                <div className={logo}>
                    <Typography variant="h4" color="primary" onClick={goToHome}>
                        {copy.org}
                    </Typography>
                </div>
                <div className={options}>
                    <Link
                        component="button"
                        variant="body2"
                        onClick={(event: React.SyntheticEvent) => {
                            event.preventDefault();
                            push(AppRoutes.Restaurants);
                        }}
                        href={AppRoutes.Restaurants}
                        className={optionItem}
                    >
                        {copy.restaurants}
                    </Link>
                    <Link
                        component="button"
                        variant="body2"
                        onClick={(event: React.SyntheticEvent) => {
                            event.preventDefault();
                            push(AppRoutes.Orders);
                        }}
                        href={AppRoutes.Orders}
                        className={optionItem}
                    >
                        {copy.orders}
                    </Link>
                    <Typography
                        variant="h5"
                        color="primary"
                        className={optionItem}
                    >
                        {user?.name}
                    </Typography>
                    <IconButton
                        color="primary"
                        aria-label="log out"
                        component="span"
                        className={optionItem}
                        onClick={logOut}
                    >
                        <ExitToApp />
                    </IconButton>
                </div>
            </div>
            <div className={contentWraper}>
                <div className={content}>{props.children}</div>
            </div>
        </div>
    );
};
