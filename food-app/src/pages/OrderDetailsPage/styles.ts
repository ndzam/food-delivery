import { makeStyles, Theme } from '@material-ui/core/styles';

export const OrderDetailsPageStyles = makeStyles((theme: Theme) => ({
    page: {
        width: '100%',
    },
    content: {
        display: 'flex',
    },
    details: {
        marginRight: 'auto',
        maxWidth: 600,
        overflow: 'hidden',
    },
    options: {
        marginLeft: 'auto',
    },
    editStyle: {
        marginLeft: 8,
        marginRight: 8,
    },
    deleteStyle: {
        marginLeft: 8,
        marginRight: 8,
    },
}));
