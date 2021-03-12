import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Error as ErrorIcon } from '@material-ui/icons';
import { useTranslation } from 'react-i18next';

const useStyle = makeStyles((theme: Theme) => ({
    error: {
        width: '100%',
        padding: 4,
        marginBottom: 8,
        backgroundColor: theme.palette.error.light,
        color: theme.palette.error.contrastText,
        display: 'flex',
        justifyItems: 'center',
        alignItems: 'center',
        borderRadius: 2,
        fontSize: 14,
        lineHeight: '14px',
    },
    text: {
        marginLeft: 8,
    },
}));

interface ErrorProps {
    errorCode?: string | null;
}

export const Error: React.FC<ErrorProps> = (props) => {
    const { error, text } = useStyle();
    const { t } = useTranslation();
    if (!props.errorCode || props.errorCode === null) return null;
    return (
        <div className={error}>
            <ErrorIcon />
            <div className={text}>{t(`errorCodes.${props.errorCode}`)}</div>
        </div>
    );
};
