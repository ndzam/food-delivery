import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { useTranslation } from 'react-i18next';

const useStyle = makeStyles((theme: Theme) => ({
    page: {
        width: '100%',
        padding: 32,
        display: 'flex',
        justifyItems: 'center',
        alignItems: 'center',
        flexDirection: 'column',
        fontSize: 42,
        lineHeight: '42px',
        fontFamily: 'Roboto',
        color: theme.palette.primary.main,
        textAlign: 'center',
        maxWidth: 'max-content',
    },
    head: {
        display: 'flex',
        justifyItems: 'center',
        alignItems: 'center',
        fontSize: 96,
        lineHeight: '96px',
        fontFamily: 'Roboto',
        color: theme.palette.primary.main,
        textAlign: 'center',
        marginBottom: 32,
    },
    content: {
        width: '100%',
        display: 'flex',
        justifyItems: 'center',
        alignItems: 'center',
        fontSize: 72,
        lineHeight: '72px',
        fontFamily: 'Roboto',
        color: theme.palette.primary.main,
        textAlign: 'center',
    },
}));

export const Page404: React.FC = () => {
    const { page, head, content } = useStyle();
    const { t } = useTranslation();
    return (
        <div className={page}>
            <div className={head}>
                <div>404</div>
            </div>
            <div className={content}>{t('labels.notFound')}</div>
        </div>
    );
};
