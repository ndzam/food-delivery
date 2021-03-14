import { makeStyles } from '@material-ui/core/styles';

export const OrdersPageStyles = makeStyles(() => ({
    page: {
        width: '100%',
    },
    footer: {
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        marginTop: 32,
    },
    title: {
        marginBottom: 16,
    },
}));
