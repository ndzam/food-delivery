import React from 'react';
import { makeStyles, Theme } from '@material-ui/core/styles';
import {
    Delete,
    Edit,
    IndeterminateCheckBox,
    AddBox,
} from '@material-ui/icons';
import { IconButton, Typography } from '@material-ui/core';

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
    },
    cardDescription: { marginTop: 8 },
    content: {
        overflow: 'hidden',
    },
    options: {
        marginLeft: 'auto',
        flex: 1,
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'space-between',
        maxWidth: 'max-content',
        paddingLeft: 4,
        paddingRight: 4,
    },
    topButton: {
        marginBottom: 8,
        marginLeft: 'auto',
    },
    bottomButton: {
        topBottom: 8,
        marginLeft: 'auto',
    },
    countStyle: {
        fontFamily: 'Roboto',
        fontSize: 28,
        lineHeight: '28px',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        textAlign: 'center',
        color: theme.palette.primary.main,
    },
}));

interface FoodCardProps {
    title: string;
    description: string;
    price: number;
    onClick: () => void;
    onEdit?: () => void;
    onDelete?: () => void;
    onAdd?: () => void;
    onRemove?: () => void;
    count?: number;
    canEdit?: boolean;
}

export const FoodCard: React.FC<FoodCardProps> = (props) => {
    const {
        title,
        description,
        price,
        onEdit,
        onDelete,
        canEdit,
        onAdd,
        onRemove,
        count,
    } = props;
    const {
        card,
        cardDescription,
        content,
        options,
        topButton,
        bottomButton,
        countStyle,
    } = useStyle();
    return (
        <div className={card}>
            <div className={content}>
                <Typography variant="h5" color="secondary" noWrap>
                    {title}
                </Typography>
                <Typography variant="body1" className={cardDescription} noWrap>
                    {description}
                </Typography>
                <Typography variant="body1" className={cardDescription}>
                    {`$ ${price}`}
                </Typography>
            </div>
            {canEdit ? (
                <div className={options}>
                    <IconButton
                        aria-label="edit"
                        component="span"
                        size="small"
                        className={topButton}
                        onClick={onEdit}
                    >
                        <Edit htmlColor="#ffc107" />
                    </IconButton>
                    <IconButton
                        aria-label="delete"
                        component="span"
                        size="small"
                        className={bottomButton}
                        onClick={onDelete}
                    >
                        <Delete htmlColor="#dc3545" />
                    </IconButton>
                </div>
            ) : (
                <div className={options}>
                    <IconButton
                        aria-label="edit"
                        component="span"
                        size="small"
                        className={topButton}
                        onClick={onAdd}
                    >
                        <AddBox color="secondary" />
                    </IconButton>
                    <div className={countStyle}>{count}</div>
                    <IconButton
                        aria-label="delete"
                        component="span"
                        size="small"
                        className={bottomButton}
                        onClick={onRemove}
                    >
                        <IndeterminateCheckBox color="secondary" />
                    </IconButton>
                </div>
            )}
        </div>
    );
};
