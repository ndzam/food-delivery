import { makeStyles, Theme } from '@material-ui/core/styles';

export const SignUpPageStyles = makeStyles((theme: Theme) => ({
    page: {
        width: '100%',
        height: '100vh',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
    },
    container: {
        boxSizing: 'border-box',
        minWidth: 450,
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        padding: 32,
    },
    form: {
        width: '90%',
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        marginTop: 32,
        marginBottom: 16,
    },
    input: {
        marginBottom: 24,
    },
}));
