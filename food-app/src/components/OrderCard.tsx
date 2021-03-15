import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Typography } from '@material-ui/core';
import { useTranslation } from 'react-i18next';

const useStyle = makeStyles((theme: Theme) => ({
    card: {
        background: '#FFF',
        width: 250,
        height: 130,
        borderRadius: 4,
        borderStyle: 'solid',
        borderWidth: 0.5,
        borderColor: '#EEE',
        cursor: 'pointer',
        '&:hover': {
            borderWidth: 1,
            borderColor: theme.palette.primary.main,
        },
        padding: 8,
        boxSizing: 'border-box',
        overflow: 'hidden',
        textOverflow: 'ellipsis',
    },
    cardDescription: { marginTop: 4 },
}));

interface OrderCardProps {
    name: string;
    price: string;
    date: Date;
    status: string;
    closed?: boolean;
    onClick?: () => void;
}

export const OrderCard: React.FC<OrderCardProps> = (props) => {
    const { name, price, date, status, onClick, closed } = props;
    const { card, cardDescription } = useStyle();
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            date: t('labels.date'),
            status: t('labels.status'),
            price: t('labels.price'),
            from: t('labels.from'),
            closed: t('labels.closed'),
        }),
        [t],
    );
    return (
        <div className={card} onClick={onClick}>
            <Typography variant="h5" color="secondary" noWrap>
                {closed ? copy.closed : `${copy.from}: ${name}`}
            </Typography>
            <Typography variant="body1" className={cardDescription}>
                {`${copy.status}: ${status}`}
            </Typography>
            <Typography variant="body2" className={cardDescription}>
                {`${copy.price}: $ ${price}`}
            </Typography>
            <Typography variant="body2" className={cardDescription}>
                {`${copy.date}: ${date.toISOString()}`}
            </Typography>
        </div>
    );
};
