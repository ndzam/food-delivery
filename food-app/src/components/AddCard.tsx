import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import { AddCircleOutline } from '@material-ui/icons';
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
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
    },
}));

interface AddCardProps {
    onClick?: () => void;
    label: string;
}

export const AddCard: React.FC<AddCardProps> = (props) => {
    const { onClick, label } = props;
    const { card } = useStyle();
    return (
        <div className={card} onClick={onClick}>
            <AddCircleOutline color="secondary" />
            <Typography variant="h5" color="secondary" noWrap>
                {label}
            </Typography>
        </div>
    );
};
