import React from 'react';
import {
    Button,
    IconButton,
    InputAdornment,
    Paper,
    TextField,
    Typography,
} from '@material-ui/core';
import { useTranslation } from 'react-i18next';
import { SignInPageStyles } from './styles';
import { EmailRounded, Visibility, VisibilityOff } from '@material-ui/icons';
import { AppRoutes } from '../../routes/AppRoutes';
import { useHistory } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { getAuthService } from '../../api/services/ServiceProvider';
import { Error } from '../../components/Error';
import { User } from '../../api/models/User';
import { useDispatch } from 'react-redux';
import { setUser } from '../../store/user/actions';

type SignInForm = {
    email: string;
    password: string;
};

export const SignInPage: React.FC = () => {
    const { page, container, form, input } = SignInPageStyles();
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            welcome: t('signIn.welcome'),
            tip: t('signIn.tip'),
            email: t('labels.email'),
            password: t('labels.password'),
            signIn: t('labels.signIn'),
            signUp: t('labels.signUp'),
        }),
        [t],
    );
    const { push } = useHistory();
    const dispatch = useDispatch();
    const { register, errors, handleSubmit } = useForm<SignInForm>();
    const [showPassword, setShowPassword] = React.useState(false);
    const [apiRequest, makeApiRequest] = useApiRequestHook<User>();
    const handleClickShowPassword = () => {
        setShowPassword(!showPassword);
    };
    const handleMouseDownPassword = (
        event: React.MouseEvent<HTMLButtonElement>,
    ) => {
        event.preventDefault();
    };
    const onSubmit = React.useCallback(
        (data: SignInForm) => {
            console.log(data);
            const authService = getAuthService();
            makeApiRequest(authService.signIn(data.email, data.password));
        },
        [makeApiRequest],
    );
    const onSignUpClick = React.useCallback(() => {
        push(AppRoutes.SignUp);
    }, [push]);
    React.useEffect(() => {
        if (apiRequest.state === 'success') {
            const userResponse = apiRequest.response.data;
            if (userResponse) {
                dispatch(setUser(userResponse));
            }
        }
    }, [apiRequest, dispatch]);
    return (
        <div className={page}>
            <Paper elevation={5} className={container}>
                <Typography color="primary" variant="h3">
                    {copy.welcome}
                </Typography>
                <Typography color="secondary" variant="h5">
                    {copy.tip}
                </Typography>
                <form className={form} onSubmit={handleSubmit(onSubmit)}>
                    <TextField
                        id="email"
                        name="email"
                        label={copy.email}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <EmailRounded color="primary" />
                                </InputAdornment>
                            ),
                        }}
                        fullWidth
                        variant="outlined"
                        inputMode="email"
                        className={input}
                        size="small"
                        inputRef={register({ required: true })}
                        error={!!errors.email}
                    />
                    <TextField
                        id="password"
                        name="password"
                        label={copy.password}
                        type={showPassword ? 'text' : 'password'}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <IconButton
                                        color="primary"
                                        aria-label="toggle password visibility"
                                        onClick={handleClickShowPassword}
                                        onMouseDown={handleMouseDownPassword}
                                        edge="end"
                                    >
                                        {showPassword ? (
                                            <Visibility />
                                        ) : (
                                            <VisibilityOff />
                                        )}
                                    </IconButton>
                                </InputAdornment>
                            ),
                        }}
                        variant="outlined"
                        fullWidth
                        className={input}
                        size="small"
                        inputRef={register({ required: true })}
                        error={!!errors.password}
                    />
                    {apiRequest.state === 'fail' && (
                        <Error errorCode={apiRequest.response.errorCode} />
                    )}
                    <Button
                        variant="contained"
                        color="primary"
                        type="submit"
                        size="large"
                        disabled={apiRequest.state === 'loading'}
                    >
                        {copy.signIn}
                    </Button>
                </form>
                <Button
                    variant="text"
                    color="primary"
                    size="small"
                    fullWidth
                    onClick={onSignUpClick}
                    disabled={apiRequest.state === 'loading'}
                >
                    {copy.signUp}
                </Button>
            </Paper>
        </div>
    );
};
