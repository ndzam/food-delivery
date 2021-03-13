import { makeStyles, Theme } from '@material-ui/core/styles';

export const RestaurantDetailsPageStyles = makeStyles((theme: Theme) => ({
    page: {
        width: '100%',
    },
    content: {
        display: 'flex',
    },
    details: {
        marginRight: 'auto',
    },
    options: {
        marginLeft: 'auto',
    },
    editStyle: {
        marginLeft: 8,
        marginRight: 8,
        color: theme.palette.warning.main,
    },
    deleteStyle: {
        marginLeft: 8,
        marginRight: 8,
        color: theme.palette.error.main,
    },
}));
