import React from 'react';
import { createStyles, makeStyles, Theme } from '@material-ui/core/styles';
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
