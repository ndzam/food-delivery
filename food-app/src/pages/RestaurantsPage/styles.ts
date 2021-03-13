import { makeStyles, Theme } from '@material-ui/core/styles';

export const RestaurantsPageStyles = makeStyles((theme: Theme) => ({
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
    searchContainer: {
        width: '100%',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        marginBottom: 32,
    },
    search: {
        width: 400,
    },
}));
