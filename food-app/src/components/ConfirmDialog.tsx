import React from 'react';
import Button from '@material-ui/core/Button';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import { useTranslation } from 'react-i18next';
import { makeStyles } from '@material-ui/core';

const useStyle = makeStyles(() => ({
    root: {
        minWidth: 350,
        boxSizing: 'border-box',
    },
}));

interface ConfirmDialogProps {
    text?: string;
    confirmLabel?: string;
    open?: boolean;
    onCancel: () => void;
    onConfirm: () => void;
}

export const ConfirmDialog: React.FC<ConfirmDialogProps> = (props) => {
    const { open = false, onCancel, onConfirm, text, confirmLabel } = props;
    const { root } = useStyle();
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            cancel: t('labels.cancel'),
            confirm: t('labels.confirm'),
            confirmTitle: t('confirm.title'),
        }),
        [t],
    );
    return (
        <Dialog
            open={open}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            disableEnforceFocus
        >
            <DialogTitle id="alert-dialog-title">
                {copy.confirmTitle}
            </DialogTitle>
            <DialogContent>
                <DialogContentText
                    id="alert-dialog-description"
                    className={root}
                >
                    {text}
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={onCancel} color="primary">
                    {copy.cancel}
                </Button>
                <Button onClick={onConfirm} autoFocus>
                    {confirmLabel ? confirmLabel : copy.confirm}
                </Button>
            </DialogActions>
        </Dialog>
    );
};
