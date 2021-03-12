import React from 'react';
import {
    Radio,
    Button,
    IconButton,
    InputAdornment,
    Paper,
    TextField,
    Typography,
    RadioGroup,
    FormControlLabel,
} from '@material-ui/core';
import { useTranslation } from 'react-i18next';
import { SignUpPageStyles } from './styles';
import {
    EmailRounded,
    Visibility,
    VisibilityOff,
    SupervisorAccountRounded,
} from '@material-ui/icons';
import { AppRoutes } from '../../routes/AppRoutes';
import { useHistory } from 'react-router-dom';
import { Controller, useForm } from 'react-hook-form';
import { getAuthService } from '../../api/services/ServiceProvider';

import { useApiRequestHook } from '../../api/hooks/useApiRequestHook';
import { Error } from '../../components/Error';
import { User } from '../../api/models/User';
import { setUser } from '../../store/user/actions';
import { useDispatch } from 'react-redux';

type SignUpForm = {
    email: string;
    name: string;
    password: string;
    confirmPassword: string;
    role: string;
};

export const SignUpPage: React.FC = () => {
    const { page, container, form, input } = SignUpPageStyles();
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            welcome: t('signUp.welcome'),
            tip: t('signUp.tip'),
            haveAccount: t('signUp.haveAccount'),
            email: t('labels.email'),
            name: t('labels.name'),
            password: t('labels.password'),
            confirmPassword: t('labels.confirmPassword'),
            signUp: t('labels.signUp'),
        }),
        [t],
    );
    const { push } = useHistory();
    const dispatch = useDispatch();
    const {
        register,
        errors,
        handleSubmit,
        watch,
        control,
    } = useForm<SignUpForm>();
    const [showPassword, setShowPassword] = React.useState(false);
    const password = watch('password');
    const handleClickShowPassword = () => {
        setShowPassword(!showPassword);
    };
    const handleMouseDownPassword = (
        event: React.MouseEvent<HTMLButtonElement>,
    ) => {
        event.preventDefault();
    };
    const [apiRequest, makeApiRequest] = useApiRequestHook<User>();
    const onSubmit = React.useCallback(
        (data: SignUpForm) => {
            console.log(data);
            const authService = getAuthService();
            makeApiRequest(
                authService.SignUp(
                    data.email,
                    data.name,
                    data.password,
                    data.confirmPassword,
                    'owner',
                ),
            );
        },
        [makeApiRequest],
    );
    const onSignInClick = React.useCallback(() => {
        push(AppRoutes.SignIn);
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
                        color="primary"
                        id="name"
                        name="name"
                        label={copy.name}
                        InputProps={{
                            endAdornment: (
                                <InputAdornment position="end">
                                    <SupervisorAccountRounded color="primary" />
                                </InputAdornment>
                            ),
                        }}
                        fullWidth
                        variant="outlined"
                        inputMode="text"
                        className={input}
                        size="small"
                        inputRef={register({ required: true })}
                        error={!!errors.name}
                    />
                    <TextField
                        id="password"
                        color="primary"
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
                    <TextField
                        id="confirmPassword"
                        name="confirmPassword"
                        label={copy.confirmPassword}
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
                        inputRef={register({
                            required: true,
                            validate: (value) =>
                                value === password ||
                                'The passwords do not match',
                        })}
                        error={!!errors.confirmPassword}
                    />
                    <Controller
                        as={
                            <RadioGroup row>
                                <FormControlLabel
                                    value="user"
                                    control={<Radio color="primary" />}
                                    label="User"
                                />
                                <FormControlLabel
                                    value="owner"
                                    control={<Radio color="primary" />}
                                    label="Owner"
                                />
                            </RadioGroup>
                        }
                        control={control}
                        name="role"
                        defaultValue="user"
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
                        {copy.signUp}
                    </Button>
                </form>
                <Button
                    variant="text"
                    size="small"
                    color="primary"
                    fullWidth
                    onClick={onSignInClick}
                    disabled={apiRequest.state === 'loading'}
                >
                    {copy.haveAccount}
                </Button>
            </Paper>
        </div>
    );
};
