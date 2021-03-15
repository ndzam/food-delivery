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
    title: {
        marginBottom: 16,
    },
    tableHolder: {
        marginTop: 32,
    },
    table: {
        minWidth: 650,
    },
    options: {
        marginLeft: 'auto',
        maxWidth: 200,
    },
    optionItem: {
        margin: 6,
    },
}));
