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
    open?: boolean;
    onCancel: () => void;
    onConfirm: () => void;
}

export const DeleteConfirmDialog: React.FC<ConfirmDialogProps> = (props) => {
    const { open = false, onCancel, onConfirm } = props;
    const { root } = useStyle();
    const { t } = useTranslation();
    const copy = React.useMemo(
        () => ({
            cancel: t('labels.cancel'),
            delete: t('labels.delete'),
            confirmTitle: t('confirm.title'),
            confirmTip: t('confirm.tip'),
        }),
        [t],
    );
    return (
        <Dialog
            open={open}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
        >
            <DialogTitle id="alert-dialog-title">
                {copy.confirmTitle}
            </DialogTitle>
            <DialogContent>
                <DialogContentText
                    id="alert-dialog-description"
                    className={root}
                >
                    {copy.confirmTip}
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={onCancel} color="primary">
                    {copy.cancel}
                </Button>
                <Button onClick={onConfirm} autoFocus>
                    {copy.delete}
                </Button>
            </DialogActions>
        </Dialog>
    );
};
