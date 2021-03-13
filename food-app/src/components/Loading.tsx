import React from 'react';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
import { Error as ErrorIcon } from '@material-ui/icons';
import { useTranslation } from 'react-i18next';
import { LinearProgress } from '@material-ui/core';

const useStyles = makeStyles((theme: Theme) =>
    createStyles({
        root: {
            position: 'absolute',
            top: 0,
            left: 0,
            right: 0,
            width: '100%',
        },
    }),
);

export const Loading: React.FC = () => {
    const classes = useStyles();

    return (
        <div className={classes.root}>
            <LinearProgress color="primary" />
        </div>
    );
};
