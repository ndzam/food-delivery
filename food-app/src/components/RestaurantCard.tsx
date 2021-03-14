import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { Typography } from '@material-ui/core';

const useStyle = makeStyles((theme: Theme) => ({
    card: {
        background: '#FFF',
        width: 250,
        height: 120,
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
    cardDescription: { marginTop: 8 },
}));

interface RestaurantCardProps {
    title: string;
    description: string;
    onClick?: () => void;
}

export const RestaurantCard: React.FC<RestaurantCardProps> = (props) => {
    const { title, description, onClick } = props;
    const { card, cardDescription } = useStyle();
    return (
        <div className={card} onClick={onClick}>
            <Typography variant="h5" color="secondary" noWrap>
                {title}
            </Typography>
            <Typography variant="body1" className={cardDescription}>
                {description}
            </Typography>
        </div>
    );
};
